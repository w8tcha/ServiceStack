﻿// Copyright (c) ServiceStack, Inc. All Rights Reserved.
// License: https://raw.github.com/ServiceStack/ServiceStack/master/license.txt

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace ServiceStack;

[Tag(TagNames.Auth), Api("Sign In")]
[DataContract]
public class Authenticate : IPost, IReturn<AuthenticateResponse>, IMeta
{
    public Authenticate() {}
    public Authenticate(string provider) => this.provider = provider;

    [Description("AuthProvider, e.g. credentials")]
    [DataMember(Order = 1)] public string provider { get; set; }
    [DataMember(Order = 2)] public string UserName { get; set; }
    [DataMember(Order = 3)] public string Password { get; set; }
    [DataMember(Order = 4)] public bool? RememberMe { get; set; }

    [DataMember(Order = 5)] public string AccessToken { get; set; }
    [DataMember(Order = 6)] public string AccessTokenSecret { get; set; }
    [DataMember(Order = 7)] public string ReturnUrl { get; set; }

    [DataMember(Order = 8)] public string ErrorView { get; set; }
    [DataMember(Order = 9)] public Dictionary<string, string> Meta { get; set; }
}

[DataContract]
public class AuthenticateResponse : IMeta, IHasResponseStatus, IHasSessionId, IHasBearerToken, IHasRefreshTokenExpiry
{
    [DataMember(Order = 1)] public string UserId { get; set; }
    [DataMember(Order = 2)] public string SessionId { get; set; }
    [DataMember(Order = 3)] public string UserName { get; set; }
    [DataMember(Order = 4)] public string DisplayName { get; set; }
    [DataMember(Order = 5)] public string ReferrerUrl { get; set; }
    [DataMember(Order = 6)] public string BearerToken { get; set; }
    [DataMember(Order = 7)] public string RefreshToken { get; set; }
    [DataMember(Order = 8)] public DateTime? RefreshTokenExpiry { get; set; }
    [DataMember(Order = 9)] public string ProfileUrl { get; set; }
    [DataMember(Order = 10)] public List<string> Roles { get; set; } 
    [DataMember(Order = 11)] public List<string> Permissions { get; set; } 
    [DataMember(Order = 12)] public string AuthProvider { get; set; }

    [DataMember(Order = 13)] public ResponseStatus ResponseStatus { get; set; }
    [DataMember(Order = 14)] public Dictionary<string, string> Meta { get; set; }
}

[ExcludeMetadata]
[Tag(TagNames.Auth), Api("Sign Out")]
[DataContract]
[Route("/auth/logout", "GET,POST")]
public class AuthenticateLogout : IPost, IReturn<AuthenticateResponse>
{
    [DataMember(Order = 1)] public string ReturnUrl { get; set; }
}

[Tag(TagNames.Auth), Api("Sign Up")]
[DataContract]
public class Register : IPost, IReturn<RegisterResponse>, IMeta
{
    [DataMember(Order = 1)] public string UserName { get; set; }
    [DataMember(Order = 2)] public string FirstName { get; set; }
    [DataMember(Order = 3)] public string LastName { get; set; }
    [DataMember(Order = 4)] public string DisplayName { get; set; }
    [DataMember(Order = 5)] public string Email { get; set; }
    [DataMember(Order = 6)] public string Password { get; set; }
    [DataMember(Order = 7)] public string ConfirmPassword { get; set; }
    [DataMember(Order = 8)] public bool? AutoLogin { get; set; }
    // For Web Requests only can use ?continue or ?returnUrl
    // [DataMember(Order = 9)] public string Continue { get; set; }
    [DataMember(Order = 10)] public string ErrorView { get; set; }
    [DataMember(Order = 11)] public Dictionary<string, string> Meta { get; set; }
}

[DataContract]
public class RegisterResponse : IMeta, IHasResponseStatus, IHasSessionId, IHasBearerToken, IHasRefreshTokenExpiry
{
    [DataMember(Order = 1)] public string UserId { get; set; }
    [DataMember(Order = 2)] public string SessionId { get; set; }
    [DataMember(Order = 3)] public string UserName { get; set; }
    [DataMember(Order = 4)] public string ReferrerUrl { get; set; }
    [DataMember(Order = 5)] public string BearerToken { get; set; }
    [DataMember(Order = 6)] public string RefreshToken { get; set; }
    [DataMember(Order = 7)] public DateTime? RefreshTokenExpiry { get; set; }
    [DataMember(Order = 8)] public List<string> Roles { get; set; } 
    [DataMember(Order = 9)] public List<string> Permissions { get; set; } 
    [DataMember(Order = 10)] public string RedirectUrl { get; set; } 

    [DataMember(Order = 11)] public ResponseStatus ResponseStatus { get; set; }
    [DataMember(Order = 12)] public Dictionary<string, string> Meta { get; set; }
}

[Tag(TagNames.Auth)]
[DataContract]
public class AssignRoles : IPost, IReturn<AssignRolesResponse>, IMeta
{
    [DataMember(Order = 1)]
    public string UserName { get; set; }

    [DataMember(Order = 2)]
    public List<string> Permissions { get; set; } = [];

    [DataMember(Order = 3)]
    public List<string> Roles { get; set; } = [];

    [DataMember(Order = 4)] public Dictionary<string, string> Meta { get; set; }
}

[DataContract]
public class AssignRolesResponse : IHasResponseStatus, IMeta
{
    [DataMember(Order = 1)]
    public List<string> AllRoles { get; set; } = [];

    [DataMember(Order = 2)]
    public List<string> AllPermissions { get; set; } = [];

    [DataMember(Order = 3)] public Dictionary<string, string> Meta { get; set; }

    [DataMember(Order = 4)]
    public ResponseStatus ResponseStatus { get; set; }
}

[Tag(TagNames.Auth)]
[DataContract]
public class UnAssignRoles : IPost, IReturn<UnAssignRolesResponse>, IMeta
{
    public UnAssignRoles()
    {
        this.Roles = new List<string>();
        this.Permissions = new List<string>();
    }

    [DataMember(Order = 1)]
    public string UserName { get; set; }

    [DataMember(Order = 2)]
    public List<string> Permissions { get; set; }

    [DataMember(Order = 3)]
    public List<string> Roles { get; set; }

    [DataMember(Order = 4)] public Dictionary<string, string> Meta { get; set; }
}

[DataContract]
public class UnAssignRolesResponse : IHasResponseStatus, IMeta
{
    public UnAssignRolesResponse()
    {
        this.AllRoles = new List<string>();
    }

    [DataMember(Order = 1)]
    public List<string> AllRoles { get; set; }

    [DataMember(Order = 2)]
    public List<string> AllPermissions { get; set; }

    [DataMember(Order = 3)] public Dictionary<string, string> Meta { get; set; }

    [DataMember(Order = 4)]
    public ResponseStatus ResponseStatus { get; set; }
}

[DataContract]
public class CancelRequest : IPost, IReturn<CancelRequestResponse>, IMeta
{
    [DataMember(Order = 1)]
    public string Tag { get; set; }

    [DataMember(Order = 2)] public Dictionary<string, string> Meta { get; set; }
}

[DataContract]
public class CancelRequestResponse : IHasResponseStatus, IMeta
{
    [DataMember(Order = 1)]
    public string Tag { get; set; }

    [DataMember(Order = 2)]
    public TimeSpan Elapsed { get; set; }

    [DataMember(Order = 3)] public Dictionary<string, string> Meta { get; set; }
        
    [DataMember(Order = 4)]
    public ResponseStatus ResponseStatus { get; set; }
}

[ExcludeMetadata]
[DataContract]
[Route("/event-subscribers/{Id}")]
public class UpdateEventSubscriber : IPost, IReturn<UpdateEventSubscriberResponse>
{
    [DataMember(Order = 1)]
    public string Id { get; set; }
    [DataMember(Order = 2)]
    public string[] SubscribeChannels { get; set; }
    [DataMember(Order = 3)]
    public string[] UnsubscribeChannels { get; set; }
}

[DataContract]
public class UpdateEventSubscriberResponse
{
    [DataMember(Order = 1)]
    public ResponseStatus ResponseStatus { get; set; }
}

[ExcludeMetadata]
public class GetEventSubscribers : IGet, IReturn<List<Dictionary<string, string>>>
{
    public string[] Channels { get; set; }
}

[Tag(TagNames.Auth)]
[DataContract]
public class GetApiKeys : IGet, IReturn<GetApiKeysResponse>, IMeta
{
    [DataMember(Order = 1)] public string Environment { get; set; }
    [DataMember(Order = 2)] public Dictionary<string, string> Meta { get; set; }
}

[DataContract]
public class GetApiKeysResponse : IHasResponseStatus, IMeta
{
    [DataMember(Order = 1)] public List<UserApiKey> Results { get; set; }

    [DataMember(Order = 2)] public Dictionary<string, string> Meta { get; set; }
    [DataMember(Order = 3)] public ResponseStatus ResponseStatus { get; set; }
}

[Tag(TagNames.Auth)]
[DataContract]
public class RegenerateApiKeys : IPost, IReturn<RegenerateApiKeysResponse>, IMeta
{
    [DataMember(Order = 1)] public string Environment { get; set; }
    [DataMember(Order = 2)] public Dictionary<string, string> Meta { get; set; }
}

[DataContract]
public class RegenerateApiKeysResponse : IHasResponseStatus, IMeta
{
    [DataMember(Order = 1)] public List<UserApiKey> Results { get; set; }

    [DataMember(Order = 2)] public Dictionary<string, string> Meta { get; set; }
    [DataMember(Order = 3)] public ResponseStatus ResponseStatus { get; set; }
}

[DataContract]
public class UserApiKey : IMeta
{
    [DataMember(Order = 1)] public string Key { get; set; }
    [DataMember(Order = 2)] public string KeyType { get; set; }
    [DataMember(Order = 3)] public DateTime? ExpiryDate { get; set; }
    [DataMember(Order = 4)] public Dictionary<string, string> Meta { get; set; }
}

[DataContract, ExcludeMetadata]
public partial class ConvertSessionToToken : IPost, IReturn<ConvertSessionToTokenResponse>, IMeta
{
    [DataMember(Order = 1)]
    public bool PreserveSession { get; set; }
    [DataMember(Order = 2)] public Dictionary<string, string> Meta { get; set; }
}

[DataContract]
public class ConvertSessionToTokenResponse : IMeta
{
    [DataMember(Order = 1)]
    public Dictionary<string, string> Meta { get; set; }

    [DataMember(Order = 2)]
    public string AccessToken { get; set; }

    [DataMember(Order = 3)]
    public string RefreshToken { get; set; }

    [DataMember(Order = 4)]
    public ResponseStatus ResponseStatus { get; set; }
}
    
[Tag(TagNames.Auth)]
[DataContract]
public partial class GetAccessToken : IPost, IReturn<GetAccessTokenResponse>, IMeta
{
    [DataMember(Order = 1)]
    public string RefreshToken { get; set; }
    [DataMember(Order = 2)] public Dictionary<string, string> Meta { get; set; }
}

[DataContract]
public class GetAccessTokenResponse : IHasResponseStatus, IMeta
{
    [DataMember(Order = 1)]
    public string AccessToken { get; set; }

    [DataMember(Order = 2)] public Dictionary<string, string> Meta { get; set; }
    [DataMember(Order = 3)] public ResponseStatus ResponseStatus { get; set; }
}

[DataContract, ExcludeMetadata]
public partial class GetNavItems : IReturn<GetNavItemsResponse>
{
    [DataMember(Order = 1)]
    public string Name { get; set; }
}

[DataContract]
public class GetNavItemsResponse : IMeta
{
    [DataMember(Order = 1)]
    public string BaseUrl { get; set; }
    [DataMember(Order = 2)]
    public List<NavItem> Results { get; set; }
    [DataMember(Order = 3)]
    public Dictionary<string, List<NavItem>> NavItemsMap { get; set; }
    [DataMember(Order = 4)]
    public Dictionary<string, string> Meta { get; set; }
    [DataMember(Order = 5)]
    public ResponseStatus ResponseStatus { get; set; }
}

[DataContract, ExcludeMetadata]
public partial class MetadataApp : IGet, IReturn<AppMetadata>
{
    [DataMember(Order = 1)]
    public string View { get; set; }
    [DataMember(Order = 2)]
    public List<string> IncludeTypes { get; set; }
}

[DataContract]
public class GetFile : IReturn<FileContent>, IGet
{
    [DataMember(Order = 1)]
    public string Path { get; set; }
}

[DataContract]
public class FileContent
{
    [DataMember(Order = 1)]
    public string Name { get; set; }
        
    [DataMember(Order = 2)]
    public string Type { get; set; }
        
    [DataMember(Order = 3)]
    public int Length { get; set; }
        
    [DataMember(Order = 4)]
    public byte[] Body { get; set; }
        
    [DataMember(Order = 5)]
    public ResponseStatus ResponseStatus { get; set; }
}

[DataContract]
public class StreamFiles : IReturn<FileContent>
{
    [DataMember(Order = 1)]
    public List<string> Paths { get; set; }
}

[DataContract]
public class StreamServerEvents : IReturn<StreamServerEventsResponse>
{
    [DataMember(Order = 1)]
    public string[] Channels { get; set; }
}

[DataContract]
public class StreamServerEventsResponse
{
    //ServerEventMessage
    [DataMember(Order = 1)]
    public long EventId { get; set; }
    [DataMember(Order = 2)]
    public string Channel { get; set; }
//        [DataMember(Order = 3)] //ignore returning Data body
    public string Data { get; set; }
    [DataMember(Order = 4)]
    public string Selector { get; set; }
    [DataMember(Order = 5)]
    public string Json { get; set; }
    [DataMember(Order = 6)]
    public string Op { get; set; }
    [DataMember(Order = 7)]
    public string Target { get; set; }
    [DataMember(Order = 8)]
    public string CssSelector { get; set; }
    [DataMember(Order = 9)]
    public Dictionary<string, string> Meta { get; set; }

    //ServerEventCommand
    [DataMember(Order = 10)]
    public string UserId { get; set; }
    [DataMember(Order = 11)]
    public string DisplayName { get; set; }
    [DataMember(Order = 12)]
    public string ProfileUrl { get; set; }
    [DataMember(Order = 13)]
    public bool IsAuthenticated { get; set; }
    [DataMember(Order = 14)]
    public string[] Channels { get; set; }
    [DataMember(Order = 15)]
    public long CreatedAt { get; set; }
        
    //ServerEventConnect
    [DataMember(Order = 21)]
    public string Id { get; set; }
    [DataMember(Order = 22)]
    public string UnRegisterUrl { get; set; }
    [DataMember(Order = 23)]
    public string UpdateSubscriberUrl { get; set; }
    [DataMember(Order = 24)]
    public string HeartbeatUrl { get; set; }
    [DataMember(Order = 25)]
    public long HeartbeatIntervalMs { get; set; }
    [DataMember(Order = 26)]
    public long IdleTimeoutMs { get; set; }
        
    [DataMember(Order = 30)]
    public ResponseStatus ResponseStatus { get; set; }
}

[DataContract]
public class DynamicRequest
{
    [DataMember(Order = 1)]
    public Dictionary<string, string> Params { get; set; }
}
    
//Validation Rules
[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public class GetValidationRules : IGet, IReturn<GetValidationRulesResponse>
{
    [DataMember(Order = 1)]
    public string AuthSecret { get; set; }
    [DataMember(Order = 2)]
    public string Type { get; set; }
}
[DataContract]
public class GetValidationRulesResponse
{
    [DataMember(Order = 1)]
    public List<ValidationRule> Results { get; set; }
    [DataMember(Order = 2)]
    public ResponseStatus ResponseStatus { get; set; }
}
[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public class ModifyValidationRules : IReturnVoid
{
    [DataMember(Order = 1)]
    public string AuthSecret { get; set; }
    [DataMember(Order = 2)]
    public List<ValidationRule> SaveRules { get; set; }

    [DataMember(Order = 3)]
    public int[] DeleteRuleIds { get; set; }

    [DataMember(Order = 4)]
    public int[] SuspendRuleIds { get; set; }

    [DataMember(Order = 5)]
    public int[] UnsuspendRuleIds { get; set; }
        
    [DataMember(Order = 6)]
    public bool? ClearCache { get; set; }
}
    
//CrudEvents
[DataContract, ExcludeMetadata, Tag("locode")]
public partial class GetCrudEvents : QueryDb<CrudEvent>
{
    [DataMember(Order = 1)]
    public string AuthSecret { get; set; }
    [DataMember(Order = 2)]
    public string Model { get; set; }
    [DataMember(Order = 3)]
    public string ModelId { get; set; }
}

[DataContract, ExcludeMetadata]
public partial class CheckCrudEvents : IGet, IReturn<CheckCrudEventsResponse>
{
    [DataMember(Order = 1)]
    public string AuthSecret { get; set; }
    [DataMember(Order = 2)]
    public string Model { get; set; }
    [DataMember(Order = 3)]
    public List<string> Ids { get; set; }
}
    
[DataContract]
public class CheckCrudEventsResponse : IHasResponseStatus
{
    [DataMember(Order = 1)]
    public List<string> Results { get; set; }

    [DataMember(Order = 2)]
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// Capture a CRUD Event
/// </summary>
[DataContract]
public class CrudEvent : IMeta
{
    [AutoIncrement]
    [DataMember(Order = 1)]
    public long Id { get; set; }
    /// <summary>
    /// AutoCrudOperation, e.g. Create, Update, Patch, Delete, Save
    /// </summary>
    [DataMember(Order = 2)]
    public string EventType { get; set; }
    /// <summary>
    /// DB Model
    /// </summary>
    [Index]
    [DataMember(Order = 3)]
    public string Model { get; set; }
    /// <summary>
    /// Primary Key of DB Model
    /// </summary>
    [Index]
    [DataMember(Order = 4)]
    public string ModelId { get; set; }
    /// <summary>
    /// Date of Event (UTC)
    /// </summary>
    [DataMember(Order = 5)]
    public DateTime EventDate { get; set; }
    /// <summary>
    /// Rows Updated if available
    /// </summary>
    [DataMember(Order = 6)]
    public long? RowsUpdated { get; set; }
    /// <summary>
    /// Request DTO Type
    /// </summary>
    [DataMember(Order = 7)]
    public string RequestType { get; set; }
    /// <summary>
    /// Serialized Request Body
    /// </summary>
    [DataMember(Order = 8)]
    [StringLength(StringLengthAttribute.MaxText)]
    public string RequestBody { get; set; }
    /// <summary>
    /// UserAuthId if Authenticated
    /// </summary>
    [DataMember(Order = 9)]
    public string UserAuthId { get; set; }
    /// <summary>
    /// UserName or unique User Identifier
    /// </summary>
    [DataMember(Order = 10)]
    public string UserAuthName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [DataMember(Order = 11)]
    public string RemoteIp { get; set; }
    /// <summary>
    /// URN format: urn:{requesttype}:{ModelId}
    /// </summary>
    [DataMember(Order = 12)]
    public string Urn { get; set; }

    /// <summary>
    /// Custom Reference Data with integer Primary Key
    /// </summary>
    [DataMember(Order = 13)]
    public int? RefId { get; set; }
    /// <summary>
    /// Custom Reference Data with non-integer Primary Key
    /// </summary>
    [DataMember(Order = 14)]
    public string RefIdStr { get; set; }
    /// <summary>
    /// Custom Metadata to attach to this event
    /// </summary>
    [DataMember(Order = 15)]
    public Dictionary<string, string> Meta { get; set; }
}

[DataContract]
public abstract class AdminUserBase : IMeta
{
    [DataMember(Order = 1)] public string UserName { get; set; }
    [DataMember(Order = 2)] public string FirstName { get; set; }
    [DataMember(Order = 3)] public string LastName { get; set; }
    [DataMember(Order = 4)] public string DisplayName { get; set; }
    [DataMember(Order = 5)] public string Email { get; set; }
    [DataMember(Order = 6)] public string Password { get; set; }
    [DataMember(Order = 7)] public string ProfileUrl { get; set; }
    [DataMember(Order = 8)] public string PhoneNumber { get; set; }
    [DataMember(Order = 9)] public Dictionary<string, string> UserAuthProperties { get; set; } = [];
    [DataMember(Order = 10)] public Dictionary<string, string> Meta { get; set; }

    public string GetUserProperty(string name) => 
        UserAuthProperties.TryGetValue(name, out var value) ? value : null;
    public T GetUserProperty<T>(string name) => 
        UserAuthProperties.TryGetValue(name, out var value) ? value.ConvertTo<T>() : default;
}

[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminGetUser : IGet, IReturn<AdminUserResponse>
{
    [DataMember(Order = 10)] public string Id { get; set; }
}

#if NET8_0_OR_GREATER
[SystemJson(UseSystemJson.Response)]
#endif
[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminCreateUser : AdminUserBase, IPost, IReturn<AdminUserResponse>
{
    [DataMember(Order = 10)] public List<string> Roles { get; set; }
    [DataMember(Order = 11)] public List<string> Permissions { get; set; }
}
    
#if NET8_0_OR_GREATER
[SystemJson(UseSystemJson.Response)]
#endif
[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminUpdateUser : AdminUserBase, IPut, IReturn<AdminUserResponse>
{
    [DataMember(Order = 10)] public string Id { get; set; }
    [DataMember(Order = 11)] public bool? LockUser { get; set; }
    [DataMember(Order = 12)] public bool? UnlockUser { get; set; }
    [DataMember(Order = 13)] public DateTimeOffset? LockUserUntil { get; set; }
    [DataMember(Order = 14)] public List<string> AddRoles { get; set; }
    [DataMember(Order = 15)] public List<string> RemoveRoles { get; set; }
    [DataMember(Order = 16)] public List<string> AddPermissions { get; set; }
    [DataMember(Order = 17)] public List<string> RemovePermissions { get; set; }
    [DataMember(Order = 18)] public List<Property> AddClaims { get; set; }
    [DataMember(Order = 19)] public List<Property> RemoveClaims { get; set; }
}
    
[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminDeleteUser : IDelete, IReturn<AdminDeleteUserResponse>
{
    [DataMember(Order = 10)] public string Id { get; set; }
}

[DataContract]
public class AdminDeleteUserResponse : IHasResponseStatus
{
    [DataMember(Order = 1)] public string Id { get; set; }
    [DataMember(Order = 2)] public ResponseStatus ResponseStatus { get; set; }
}

[DataContract]
public partial class AdminUserResponse : IHasResponseStatus
{
    [DataMember(Order = 1)] public string Id { get; set; }
    [DataMember(Order = 2)] public Dictionary<string,object> Result { get; set; }
    [DataMember(Order = 3)] public List<Dictionary<string,object>> Details { get; set; }
    [DataMember(Order = 4)] public List<Property> Claims { get; set; }
    [DataMember(Order = 5)] public ResponseStatus ResponseStatus { get; set; }
}
    
[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminQueryUsers : IGet, IReturn<AdminUsersResponse>
{
    [DataMember(Order = 1)] public string Query { get; set; }
    [DataMember(Order = 2)] public string OrderBy { get; set; }
    [DataMember(Order = 3)] public int? Skip { get; set; }
    [DataMember(Order = 4)] public int? Take { get; set; }
}

[DataContract]
public partial class AdminUsersResponse : IHasResponseStatus
{
    [DataMember(Order = 1)] public List<Dictionary<string,object>> Results { get; set; }
    [DataMember(Order = 2)] public ResponseStatus ResponseStatus { get; set; }
}

[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminGetRoles : IGet, IReturn<AdminGetRolesResponse> {}
[DataContract]
public partial class AdminGetRolesResponse : IHasResponseStatus
{
    [DataMember(Order = 1)] public List<AdminRole> Results { get; set; }
    [DataMember(Order = 2)] public ResponseStatus ResponseStatus { get; set; }
}
[DataContract]
public class AdminRole
{
    public string Id { get; set; }
    public string Name { get; set; }
}

[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminGetRole : IGet, IReturn<AdminGetRoleResponse>
{
    [DataMember(Order = 1)] public string Id { get; set; }
}
[DataContract]
public partial class AdminGetRoleResponse : IHasResponseStatus
{
    [DataMember(Order = 1)] public AdminRole Result { get; set; }
    [DataMember(Order = 2)] public List<Property> Claims { get; set; }
    [DataMember(Order = 3)] public ResponseStatus ResponseStatus { get; set; }
}

[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminCreateRole : IPost, IReturn<IdResponse>
{
    [DataMember(Order = 1)] public string Name { get; set; }
}

#if NET8_0_OR_GREATER
[SystemJson(UseSystemJson.Response)]
#endif
[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminUpdateRole : IPost, IReturn<IdResponse>
{
    [Input(Type="text", ReadOnly = true)]
    [DataMember(Order = 1)] public string Id { get; set; }
    [DataMember(Order = 2)] public string Name { get; set; }
    [DataMember(Order = 3)] public List<Property> AddClaims { get; set; }
    [DataMember(Order = 4)] public List<Property> RemoveClaims { get; set; }
    [DataMember(Order = 5)] public ResponseStatus ResponseStatus { get; set; }
}

[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminDeleteRole : IDelete, IReturnVoid
{
    [DataMember(Order = 1)] public string Id { get; set; }
}

[DataContract, ValidateIsAuthenticated, ExcludeMetadata]
public class QueryUserApiKeys : IGet, IReturn<UserApiKeysResponse>
{
    [DataMember(Order = 1)] public int? Id { get; set; }
    [DataMember(Order = 2)] public string Search { get; set; }
    [DataMember(Order = 3)] public string OrderBy { get; set; }
    [DataMember(Order = 4)] public int? Skip { get; set; }
    [DataMember(Order = 5)] public int? Take { get; set; }
}
public partial class UserApiKeysResponse : IHasResponseStatus
{
    [DataMember(Order = 1)] public List<PartialApiKey> Results { get; set; }
    [DataMember(Order = 2)] public ResponseStatus ResponseStatus { get; set; }
}
[DataContract, ValidateIsAuthenticated, ExcludeMetadata]
public partial class CreateUserApiKey : IPost, IReturn<UserApiKeyResponse>
{
    [DataMember(Order = 1)] public string Name { get; set; }
    [DataMember(Order = 2)] public List<string> Scopes { get; set; } = [];
    [DataMember(Order = 3)] public List<string> Features { get; set; } = [];
    [DataMember(Order = 4)] public List<string> RestrictTo { get; set; } = [];
    [DataMember(Order = 5)] public DateTime? ExpiryDate { get; set; }
    [DataMember(Order = 6)] public string Notes { get; set; }
    [DataMember(Order = 7)] public int? RefId { get; set; }
    [DataMember(Order = 8)] public string RefIdStr { get; set; }
    [DataMember(Order = 9)] public Dictionary<string, string> Meta { get; set; }
}
[DataContract]
public partial class UserApiKeyResponse : IHasResponseStatus
{
    [DataMember(Order = 1)] public string Result { get; set; }
    [DataMember(Order = 2)] public ResponseStatus ResponseStatus { get; set; }
}
#if NET8_0_OR_GREATER
[SystemJson(UseSystemJson.Response)]
#endif
[DataContract, ValidateIsAuthenticated, ExcludeMetadata]
public partial class UpdateUserApiKey : IPatch, IReturn<EmptyResponse>
{
    [ValidateGreaterThan(0)]
    [DataMember(Order = 1)] public int Id { get; set; }
    [DataMember(Order = 2)] public string Name { get; set; }
    [DataMember(Order = 5)] public List<string> Scopes { get; set; } = [];
    [DataMember(Order = 6)] public List<string> Features { get; set; } = [];
    [DataMember(Order = 7)] public List<string> RestrictTo { get; set; } = [];
    [DataMember(Order = 8)] public DateTime? ExpiryDate { get; set; }
    [DataMember(Order = 9)] public DateTime? CancelledDate { get; set; }
    [DataMember(Order = 10)] public string Notes { get; set; }
    [DataMember(Order = 11)] public int? RefId { get; set; }
    [DataMember(Order = 12)] public string RefIdStr { get; set; }
    [DataMember(Order = 13)] public Dictionary<string, string> Meta { get; set; }
    [DataMember(Order = 14)] public List<string> Reset { get; set; }
}
[DataContract, ValidateIsAuthenticated, ExcludeMetadata]
public partial class DeleteUserApiKey : IDelete, IReturn<EmptyResponse>
{
    [ValidateGreaterThan(0)]
    [DataMember(Order = 1)] public int? Id { get; set; }
}

[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminQueryApiKeys : IGet, IReturn<AdminApiKeysResponse>
{
    [DataMember(Order = 1)] public int? Id { get; set; }
    [DataMember(Order = 2)] public string ApiKey { get; set; }
    [DataMember(Order = 3)] public string Search { get; set; }
    [DataMember(Order = 4)] public string UserId { get; set; }
    [DataMember(Order = 5)] public string UserName { get; set; }
    [DataMember(Order = 6)] public string OrderBy { get; set; }
    [DataMember(Order = 7)] public int? Skip { get; set; }
    [DataMember(Order = 8)] public int? Take { get; set; }
}
[DataContract]
public partial class AdminApiKeysResponse : IHasResponseStatus
{
    [DataMember(Order = 1)] public List<PartialApiKey> Results { get; set; }
    [DataMember(Order = 2)] public ResponseStatus ResponseStatus { get; set; }
}

[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminCreateApiKey : IPost, IReturn<AdminApiKeyResponse>
{
    [DataMember(Order = 1)] public string Name { get; set; }
    [DataMember(Order = 2)] public string UserId { get; set; }
    [DataMember(Order = 3)] public string UserName { get; set; }
    [DataMember(Order = 4)] public List<string> Scopes { get; set; } = [];
    [DataMember(Order = 5)] public List<string> Features { get; set; } = [];
    [DataMember(Order = 6)] public List<string> RestrictTo { get; set; } = [];
    [DataMember(Order = 7)] public DateTime? ExpiryDate { get; set; }
    [DataMember(Order = 8)] public string Notes { get; set; }
    [DataMember(Order = 9)] public int? RefId { get; set; }
    [DataMember(Order = 10)] public string RefIdStr { get; set; }
    [DataMember(Order = 11)] public Dictionary<string, string> Meta { get; set; }
}
[DataContract]
public partial class AdminApiKeyResponse : IHasResponseStatus
{
    [DataMember(Order = 1)] public string Result { get; set; }
    [DataMember(Order = 2)] public ResponseStatus ResponseStatus { get; set; }
}

#if NET8_0_OR_GREATER
[SystemJson(UseSystemJson.Response)]
#endif
[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminUpdateApiKey : IPatch, IReturn<EmptyResponse>
{
    [ValidateGreaterThan(0)]
    [DataMember(Order = 1)] public int Id { get; set; }
    [DataMember(Order = 2)] public string Name { get; set; }
    [DataMember(Order = 3)] public string UserId { get; set; }
    [DataMember(Order = 4)] public string UserName { get; set; }
    [DataMember(Order = 5)] public List<string> Scopes { get; set; } = [];
    [DataMember(Order = 6)] public List<string> Features { get; set; } = [];
    [DataMember(Order = 7)] public List<string> RestrictTo { get; set; } = [];
    [DataMember(Order = 8)] public DateTime? ExpiryDate { get; set; }
    [DataMember(Order = 9)] public DateTime? CancelledDate { get; set; }
    [DataMember(Order = 10)] public string Notes { get; set; }
    [DataMember(Order = 11)] public int? RefId { get; set; }
    [DataMember(Order = 12)] public string RefIdStr { get; set; }
    [DataMember(Order = 13)] public Dictionary<string, string> Meta { get; set; }
    [DataMember(Order = 14)] public List<string> Reset { get; set; }
}
[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public partial class AdminDeleteApiKey : IDelete, IReturn<EmptyResponse>
{
    [ValidateGreaterThan(0)]
    [DataMember(Order = 1)] public int? Id { get; set; }
}
[DataContract]
public class PartialApiKey : IMeta
{
    [DataMember(Order = 1)] public int Id { get; set; }
    [DataMember(Order = 2)] public string Name { get; set; }
    [DataMember(Order = 3)] public string UserId { get; set; }
    [DataMember(Order = 4)] public string UserName { get; set; }
    [DataMember(Order = 5)] public string VisibleKey { get; set; }
    [DataMember(Order = 6)] public string Environment { get; set; }
    [DataMember(Order = 7)] public DateTime CreatedDate { get; set; }
    [DataMember(Order = 8)] public DateTime? ExpiryDate { get; set; }
    [DataMember(Order = 9)] public DateTime? CancelledDate { get; set; }
    [DataMember(Order = 10)] public DateTime? LastUsedDate { get; set; }
    [DataMember(Order = 11)] public List<string> Scopes { get; set; } = [];
    [DataMember(Order = 12)] public List<string> Features { get; set; } = [];
    [DataMember(Order = 13)] public List<string> RestrictTo { get; set; } = [];
    [DataMember(Order = 14)] public string Notes { get; set; }
    [DataMember(Order = 15)] public int? RefId { get; set; }
    [DataMember(Order = 16)] public string RefIdStr { get; set; }
    [DataMember(Order = 17)] public Dictionary<string, string> Meta { get; set; }
    [DataMember(Order = 18)] public bool Active { get; set; }
}

/// <summary>
/// DTO to capture file uploaded using [UploadTo] 
/// </summary>
[DataContract]
public class UploadedFile
{
    [DataMember(Order = 1)]
    public string FileName { get; set; }
    [DataMember(Order = 2)]
    public string FilePath { get; set; }
    [DataMember(Order = 3)]
    public string ContentType { get; set; }
    [DataMember(Order = 4)]
    public long ContentLength { get; set; }
}

/// <summary>
/// Upload a file to the specified managed location
/// </summary>
[DataContract, ExcludeMetadata]
public partial class StoreFileUpload : IReturn<StoreFileUploadResponse>, IHasBearerToken, IPost
{
    [DataMember(Order = 1)]
    public string Name { get; set; }
    [DataMember(Order = 2)]
    public string BearerToken { get; set; }
}
[DataContract]
public partial class StoreFileUploadResponse
{
    [DataMember(Order = 1)]
    public List<string> Results { get; set; }
    [DataMember(Order = 2)]
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// Download file from the specified managed location
/// </summary>
[DataContract, ExcludeMetadata]
public partial class GetFileUpload : IReturn<byte[]>, IHasBearerToken, IGet
{
    [DataMember(Order = 1)]
    public string Name { get; set; }
    [DataMember(Order = 2)]
    public string Path { get; set; }
    [DataMember(Order = 3)]
    public string BearerToken { get; set; }
    [DataMember(Order = 4)]
    public bool? Attachment { get; set; }
}

/// <summary>
/// Overwrite file at the specified managed location
/// </summary>
[DataContract, ExcludeMetadata]
public partial class ReplaceFileUpload : IReturn<ReplaceFileUploadResponse>, IHasBearerToken, IPut
{
    [DataMember(Order = 1)]
    public string Name { get; set; }
    [DataMember(Order = 2)]
    public string Path { get; set; }
    [DataMember(Order = 3)]
    public string BearerToken { get; set; }
}
[DataContract]
public partial class ReplaceFileUploadResponse
{
    [DataMember(Order = 1)]
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// Delete file at the specified managed location
/// </summary>
[DataContract, ExcludeMetadata]
public partial class DeleteFileUpload : IReturn<DeleteFileUploadResponse>, IHasBearerToken, IDelete
{
    [DataMember(Order = 1)]
    public string Name { get; set; }
    [DataMember(Order = 2)]
    public string Path { get; set; }
    [DataMember(Order = 3)]
    public string BearerToken { get; set; }
}
[DataContract]
public partial class DeleteFileUploadResponse
{
    [DataMember(Order = 1)]
    public bool Result { get; set; }
    [DataMember(Order = 2)]
    public ResponseStatus ResponseStatus { get; set; }
}
