using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Data;
using ServiceStack.Text;

namespace ServiceStack.OrmLite;

/// <summary>
/// Allow for mocking and unit testing by providing non-disposing 
/// connection factory with injectable IDbCommand and IDbTransaction proxies
/// </summary>
public class OrmLiteConnectionFactory : IDbConnectionFactoryExtended
{
    public OrmLiteConnectionFactory()
        : this(null, null, true) { }

    public OrmLiteConnectionFactory(string connectionString)
        : this(connectionString, null, true) { }

    public OrmLiteConnectionFactory(string connectionString, IOrmLiteDialectProvider dialectProvider)
        : this(connectionString, dialectProvider, true) { }

    public OrmLiteConnectionFactory(string connectionString, IOrmLiteDialectProvider dialectProvider, bool setGlobalDialectProvider)
    {
        if (connectionString == "DataSource=:memory:")
            connectionString = ":memory:";
        ConnectionString = connectionString;
        AutoDisposeConnection = connectionString != ":memory:";
        this.DialectProvider = dialectProvider ?? OrmLiteConfig.DialectProvider;

        if (setGlobalDialectProvider && dialectProvider != null)
        {
            OrmLiteConfig.DialectProvider = dialectProvider;
        }

        this.ConnectionFilter = x => x;

        JsConfig.InitStatics();
    }

    public IOrmLiteDialectProvider DialectProvider { get; set; }

    public string ConnectionString { get; set; }

    public bool AutoDisposeConnection { get; set; }

    public Func<IDbConnection, IDbConnection> ConnectionFilter { get; set; }

    /// <summary>
    /// Force the IDbConnection to always return this IDbCommand
    /// </summary>
    public IDbCommand AlwaysReturnCommand { get; set; }

    /// <summary>
    /// Force the IDbConnection to always return this IDbTransaction
    /// </summary>
    public IDbTransaction AlwaysReturnTransaction { get; set; }

    public Action<OrmLiteConnection> OnDispose { get; set; }

    private OrmLiteConnection ormLiteConnection;
    private OrmLiteConnection OrmLiteConnection => ormLiteConnection ??= DialectProvider != null 
        ? DialectProvider.CreateOrmLiteConnection(this) 
        : new OrmLiteConnection(this);

    public virtual IDbConnection CreateDbConnection()
    {
        if (this.ConnectionString == null)
            throw new ArgumentNullException("ConnectionString", "ConnectionString must be set");

        var connection = AutoDisposeConnection
            ? DialectProvider.CreateOrmLiteConnection(this)
            : OrmLiteConnection;

        return connection;
    }

    public virtual IDbConnection Use(IDbConnection connection, IDbTransaction trans = null)
    {
        return new OrmLiteConnection(this, connection, trans);
    }

    public static IDbConnection CreateDbConnection(string namedConnection)
    {
        if (namedConnection == null)
            throw new ArgumentNullException(nameof(namedConnection));
            
        if (!NamedConnections.TryGetValue(namedConnection, out var factory))
            throw new KeyNotFoundException("No factory registered is named " + namedConnection);

        IDbConnection connection = factory.AutoDisposeConnection
            ? factory.DialectProvider.CreateOrmLiteConnection(factory, namedConnection)
            : factory.OrmLiteConnection;
        return connection;
    }

    public DbConnection CreateDbWithWriteLock(string namedConnection=null)
    {
        var factory = this;
        if (namedConnection != null && !NamedConnections.TryGetValue(namedConnection, out factory))
            throw new KeyNotFoundException("No factory registered is named " + namedConnection);

        return new SingleWriterDbConnection(factory, Locks.GetDbLock(namedConnection));
    }

    public virtual IDbConnection OpenDbConnection()
    {
        var connection = CreateDbConnection();
        connection.Open();
        return connection;
    }
    public virtual IDbConnection OpenDbConnection(Action<IDbConnection> configure)
    {
        var connection = CreateDbConnection();
        configure?.Invoke(connection);
        connection.Open();
        return connection;
    }

    public virtual async Task<IDbConnection> OpenDbConnectionAsync(CancellationToken token = default)
    {
        var connection = CreateDbConnection();
        if (connection is OrmLiteConnection ormliteConn)
        {
            await ormliteConn.OpenAsync(token).ConfigAwait();
            return connection;
        }

        await DialectProvider.OpenAsync(connection, token).ConfigAwait();
        return connection;
    }
    public virtual async Task<IDbConnection> OpenDbConnectionAsync(Action<IDbConnection> configure, CancellationToken token = default)
    {
        var connection = CreateDbConnection();
        configure?.Invoke(connection);
        if (connection is OrmLiteConnection ormliteConn)
        {
            await ormliteConn.OpenAsync(token).ConfigAwait();
            return connection;
        }

        await DialectProvider.OpenAsync(connection, token).ConfigAwait();
        return connection;
    }

    public virtual async Task<IDbConnection> OpenDbConnectionAsync(string namedConnection, CancellationToken token = default)
    {
        var connection = CreateDbConnection(namedConnection);
        if (connection is OrmLiteConnection ormliteConn)
        {
            await ormliteConn.OpenAsync(token).ConfigAwait();
            return connection;
        }

        await DialectProvider.OpenAsync(connection, token).ConfigAwait();
        return connection;
    }

    public virtual async Task<IDbConnection> OpenDbConnectionAsync(string namedConnection, Action<IDbConnection> configure, CancellationToken token = default)
    {
        var connection = CreateDbConnection(namedConnection);
        configure?.Invoke(connection);
        if (connection is OrmLiteConnection ormliteConn)
        {
            await ormliteConn.OpenAsync(token).ConfigAwait();
            return connection;
        }

        await DialectProvider.OpenAsync(connection, token).ConfigAwait();
        return connection;
    }

    public virtual IDbConnection OpenDbConnectionString(string connectionString)
    {
        if (connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));

        var connection = DialectProvider.CreateOrmLiteConnection(this);
        connection.ConnectionString = connectionString;

        connection.Open();
        return connection;
    }

    public virtual IDbConnection OpenDbConnectionString(string connectionString, Action<IDbConnection> configure)
    {
        if (connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));

        var connection = DialectProvider.CreateOrmLiteConnection(this);
        connection.ConnectionString = connectionString;
        configure?.Invoke(connection);

        connection.Open();
        return connection;
    }

    public virtual async Task<IDbConnection> OpenDbConnectionStringAsync(string connectionString, CancellationToken token = default)
    {
        if (connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));

        var connection = DialectProvider.CreateOrmLiteConnection(this);
        connection.ConnectionString = connectionString;

        await connection.OpenAsync(token).ConfigAwait();
        return connection;
    }

    public virtual async Task<IDbConnection> OpenDbConnectionStringAsync(string connectionString, Action<IDbConnection> configure, CancellationToken token = default)
    {
        if (connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));

        var connection = DialectProvider.CreateOrmLiteConnection(this);
        connection.ConnectionString = connectionString;
        configure?.Invoke(connection);

        await connection.OpenAsync(token).ConfigAwait();
        return connection;
    }

    public virtual IDbConnection OpenDbConnectionString(string connectionString, string providerName)
    {
        if (connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));
        if (providerName == null)
            throw new ArgumentNullException(nameof(providerName));

        if (!DialectProviders.TryGetValue(providerName, out var dialectProvider))
            throw new ArgumentException($"{providerName} is not a registered DialectProvider");

        var dbFactory = new OrmLiteConnectionFactory(connectionString, dialectProvider, setGlobalDialectProvider:false);

        return dbFactory.OpenDbConnection();
    }

    public virtual IDbConnection OpenDbConnectionString(string connectionString, string providerName, Action<IDbConnection> configure)
    {
        if (connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));
        if (providerName == null)
            throw new ArgumentNullException(nameof(providerName));

        if (!DialectProviders.TryGetValue(providerName, out var dialectProvider))
            throw new ArgumentException($"{providerName} is not a registered DialectProvider");

        var dbFactory = new OrmLiteConnectionFactory(connectionString, dialectProvider, setGlobalDialectProvider:false);
        return dbFactory.OpenDbConnection(configure);
    }

    public virtual async Task<IDbConnection> OpenDbConnectionStringAsync(string connectionString, string providerName, CancellationToken token = default)
    {
        if (connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));
        if (providerName == null)
            throw new ArgumentNullException(nameof(providerName));

        if (!DialectProviders.TryGetValue(providerName, out var dialectProvider))
            throw new ArgumentException($"{providerName} is not a registered DialectProvider");

        var dbFactory = new OrmLiteConnectionFactory(connectionString, dialectProvider, setGlobalDialectProvider:false);

        return await dbFactory.OpenDbConnectionAsync(token).ConfigAwait();
    }
    public virtual async Task<IDbConnection> OpenDbConnectionStringAsync(string connectionString, string providerName, Action<IDbConnection> configure, CancellationToken token = default)
    {
        if (connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));
        if (providerName == null)
            throw new ArgumentNullException(nameof(providerName));

        if (!DialectProviders.TryGetValue(providerName, out var dialectProvider))
            throw new ArgumentException($"{providerName} is not a registered DialectProvider");

        var dbFactory = new OrmLiteConnectionFactory(connectionString, dialectProvider, setGlobalDialectProvider:false);

        return await dbFactory.OpenDbConnectionAsync(token).ConfigAwait();
    }

    public virtual IDbConnection OpenDbConnection(string namedConnection)
    {
        var connection = CreateDbConnection(namedConnection);
        connection.Open();
        return connection;
    }

    public virtual IDbConnection OpenDbConnection(string namedConnection, Action<IDbConnection> configure)
    {
        var connection = CreateDbConnection(namedConnection);
        configure?.Invoke(connection);
        connection.Open();
        return connection;
    }

    private static Dictionary<string, IOrmLiteDialectProvider> dialectProviders;
    public static Dictionary<string, IOrmLiteDialectProvider> DialectProviders => dialectProviders ??= new Dictionary<string, IOrmLiteDialectProvider>();

    public virtual void RegisterDialectProvider(string providerName, IOrmLiteDialectProvider dialectProvider)
    {
        DialectProviders[providerName] = dialectProvider;
    }

    private static Dictionary<string, OrmLiteConnectionFactory> namedConnections;
    public static Dictionary<string, OrmLiteConnectionFactory> NamedConnections => namedConnections ??= new Dictionary<string, OrmLiteConnectionFactory>();

    public virtual void RegisterConnection(string namedConnection, string connectionString, IOrmLiteDialectProvider dialectProvider)
    {
        RegisterConnection(namedConnection, new OrmLiteConnectionFactory(connectionString, dialectProvider, setGlobalDialectProvider: false));
    }

    public virtual void RegisterConnection(string namedConnection, OrmLiteConnectionFactory connectionFactory)
    {
        NamedConnections[namedConnection] = connectionFactory;
        Locks.AddLock(namedConnection);
    }
}

public static class OrmLiteConnectionFactoryExtensions
{
    /// <summary>
    /// Alias for <see cref="OpenDbConnection(ServiceStack.Data.IDbConnectionFactory,string)"/>
    /// </summary>
    public static IDbConnection Open(this IDbConnectionFactory connectionFactory)
    {
        return connectionFactory.OpenDbConnection();
    }
    public static IDbConnection Open(this IDbConnectionFactory connectionFactory, Action<IDbConnection> configure)
    {
        var db = connectionFactory.CreateDbConnection();
        configure?.Invoke(db);
        db.Open();
        return db;
    }

    /// <summary>
    /// Alias for OpenDbConnectionAsync
    /// </summary>
    public static Task<IDbConnection> OpenDbConnectionAsync(this IDbConnectionFactory connectionFactory, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionAsync(token);
    }
    public static Task<IDbConnection> OpenDbConnectionAsync(this IDbConnectionFactory connectionFactory, Action<IDbConnection> configure, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionAsync(configure, token);
    }
    public static Task<IDbConnection> OpenAsync(this IDbConnectionFactory connectionFactory, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionAsync(token);
    }
    public static Task<IDbConnection> OpenAsync(this IDbConnectionFactory connectionFactory, Action<IDbConnection> configure, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionAsync(configure, token);
    }

    /// <summary>
    /// Alias for OpenDbConnectionAsync
    /// </summary>
    public static Task<IDbConnection> OpenAsync(this IDbConnectionFactory connectionFactory, string namedConnection, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionAsync(namedConnection, token);
    }
    public static Task<IDbConnection> OpenAsync(this IDbConnectionFactory connectionFactory, string namedConnection, Action<IDbConnection> configure, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionAsync(namedConnection, configure, token);
    }

    /// <summary>
    /// Alias for OpenDbConnection
    /// </summary>
    public static IDbConnection Open(this IDbConnectionFactory connectionFactory, string namedConnection)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnection(namedConnection);
    }
    public static IDbConnection Open(this IDbConnectionFactory connectionFactory, string namedConnection, Action<IDbConnection> configure)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnection(namedConnection, configure);
    }

    /// <summary>
    /// Alias for OpenDbConnection
    /// </summary>
    public static IDbConnection OpenDbConnection(this IDbConnectionFactory connectionFactory, string namedConnection)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnection(namedConnection);
    }
    public static Task<IDbConnection> OpenDbConnectionAsync(this IDbConnectionFactory connectionFactory, string namedConnection, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionAsync(namedConnection, token);
    }
    public static Task<IDbConnection> OpenDbConnectionAsync(this IDbConnectionFactory connectionFactory, string namedConnection, Action<IDbConnection> configure, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionAsync(namedConnection, configure, token);
    }

    /// <summary>
    /// Alias for OpenDbConnection
    /// </summary>
    public static IDbConnection OpenDbConnectionString(this IDbConnectionFactory connectionFactory, string connectionString)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionString(connectionString);
    }
    public static IDbConnection OpenDbConnectionString(this IDbConnectionFactory connectionFactory, string connectionString, Action<IDbConnection> configure)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionString(connectionString,configure);
    }

    public static IDbConnection OpenDbConnectionString(this IDbConnectionFactory connectionFactory, string connectionString, string providerName)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionString(connectionString, providerName);
    }
    public static IDbConnection OpenDbConnectionString(this IDbConnectionFactory connectionFactory, string connectionString, string providerName, Action<IDbConnection> configure)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionString(connectionString, providerName, configure);
    }
    public static Task<IDbConnection> OpenDbConnectionStringAsync(this IDbConnectionFactory connectionFactory, string connectionString, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionStringAsync(connectionString, token);
    }
    public static Task<IDbConnection> OpenDbConnectionStringAsync(this IDbConnectionFactory connectionFactory, string connectionString, Action<IDbConnection> configure, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionStringAsync(connectionString, configure, token);
    }

    public static Task<IDbConnection> OpenDbConnectionStringAsync(this IDbConnectionFactory connectionFactory, string connectionString, string providerName, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionStringAsync(connectionString, providerName, token);
    }
    public static Task<IDbConnection> OpenDbConnectionStringAsync(this IDbConnectionFactory connectionFactory, string connectionString, string providerName, Action<IDbConnection> configure, CancellationToken token = default)
    {
        return ((OrmLiteConnectionFactory)connectionFactory).OpenDbConnectionStringAsync(connectionString, providerName, configure, token);
    }

    public static IOrmLiteDialectProvider GetDialectProvider(this IDbConnectionFactory connectionFactory, ConnectionInfo dbInfo)
    {
        return dbInfo != null
            ? GetDialectProvider(connectionFactory, providerName:dbInfo.ProviderName, namedConnection:dbInfo.NamedConnection)
            : ((OrmLiteConnectionFactory) connectionFactory).DialectProvider;
    }
        
    public static IOrmLiteDialectProvider GetDialectProvider(this IDbConnectionFactory connectionFactory,
        string providerName = null, string namedConnection = null)
    {
        var dbFactory = (OrmLiteConnectionFactory) connectionFactory;

        if (!string.IsNullOrEmpty(providerName))
            return OrmLiteConnectionFactory.DialectProviders.TryGetValue(providerName, out var provider)
                ? provider
                : throw new NotSupportedException($"Dialect provider is not registered '{providerName}'");
            
        if (!string.IsNullOrEmpty(namedConnection))
            return OrmLiteConnectionFactory.NamedConnections.TryGetValue(namedConnection, out var namedFactory)
                ? namedFactory.DialectProvider
                : throw new NotSupportedException($"Named connection is not registered '{namedConnection}'");
            
        return dbFactory.DialectProvider;
    }

    public static IDbConnection ToDbConnection(this IDbConnection db)
    {
        return db is IHasDbConnection hasDb
            ? hasDb.DbConnection.ToDbConnection()
            : db;
    }

    public static IDbCommand ToDbCommand(this IDbCommand dbCmd)
    {
        return dbCmd is IHasDbCommand hasDbCmd
            ? hasDbCmd.DbCommand.ToDbCommand()
            : dbCmd;
    }

    public static IDbTransaction ToDbTransaction(this IDbTransaction dbTrans)
    {
        return dbTrans is IHasDbTransaction hasDbTrans
            ? hasDbTrans.DbTransaction
            : dbTrans;
    }

    public static Guid GetConnectionId(this IDbConnection db) =>
        db is OrmLiteConnection conn ? conn.ConnectionId : Guid.Empty;

    public static Guid GetConnectionId(this IDbCommand dbCmd) =>
        dbCmd is OrmLiteCommand cmd ? cmd.ConnectionId : Guid.Empty;
        
    public static void RegisterConnection(this IDbConnectionFactory dbFactory, string namedConnection, string connectionString, IOrmLiteDialectProvider dialectProvider)
    {
        ((OrmLiteConnectionFactory)dbFactory).RegisterConnection(namedConnection, connectionString, dialectProvider);
    }

    public static void RegisterConnection(this IDbConnectionFactory dbFactory, string namedConnection, OrmLiteConnectionFactory connectionFactory)
    {
        ((OrmLiteConnectionFactory)dbFactory).RegisterConnection(namedConnection, connectionFactory);
    }
        
    public static IDbConnection OpenDbConnection(this IDbConnectionFactory dbFactory, ConnectionInfo connInfo, Action<IDbConnection> configure = null)
    {            
        if (dbFactory is IDbConnectionFactoryExtended dbFactoryExt && connInfo != null)
        {
            if (connInfo.ConnectionString != null)
            {
                return connInfo.ProviderName != null 
                    ? dbFactoryExt.OpenDbConnectionString(connInfo.ConnectionString, connInfo.ProviderName, configure) 
                    : dbFactoryExt.OpenDbConnectionString(connInfo.ConnectionString, configure);
            }

            if (connInfo.NamedConnection != null)
                return dbFactoryExt.OpenDbConnection(connInfo.NamedConnection, configure);
        }
        return dbFactory.Open(configure);
    }

    public static async Task<IDbConnection> OpenDbConnectionAsync(this IDbConnectionFactory dbFactory, ConnectionInfo connInfo, Action<IDbConnection> configure = null)
    {            
        if (dbFactory is IDbConnectionFactoryExtended dbFactoryExt && connInfo != null)
        {
            if (connInfo.ConnectionString != null)
            {
                return connInfo.ProviderName != null 
                    ? await dbFactoryExt.OpenDbConnectionStringAsync(connInfo.ConnectionString, connInfo.ProviderName, configure).ConfigAwait() 
                    : await dbFactoryExt.OpenDbConnectionStringAsync(connInfo.ConnectionString, configure).ConfigAwait();
            }

            if (connInfo.NamedConnection != null)
                return await dbFactoryExt.OpenDbConnectionAsync(connInfo.NamedConnection, configure).ConfigAwait();
        }
        return await dbFactory.OpenAsync(configure).ConfigAwait();
    }
        
    public static Dictionary<string, OrmLiteConnectionFactory> GetNamedConnections(this IDbConnectionFactory dbFactory) => 
        OrmLiteConnectionFactory.NamedConnections;

    public static IDbConnection Use(this IDbConnectionFactory dbFactory, IDbConnection connection, IDbTransaction trans=null)
    {
        return ((OrmLiteConnectionFactory)dbFactory).Use(connection, trans);
    }
}