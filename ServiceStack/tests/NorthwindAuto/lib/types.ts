import { ApiResult } from './client';

/* Options:
Date: 2022-03-31 17:50:09
Version: 6.03
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: http://localhost:20000

//GlobalNamespace: 
//MakePropertiesOptional: False
//AddServiceStackTypes: True
//AddResponseStatus: False
//AddImplicitVersion: 
//AddDescriptionAsComments: True
//IncludeTypes: 
//ExcludeTypes: 
//DefaultImports: 
*/


export interface IReturn<T>
{
    createResponse(): T;
}

export interface IReturnVoid
{
    createResponse(): void;
}

export interface IHasSessionId
{
    sessionId: string;
}

export interface IHasBearerToken
{
    bearerToken: string;
}

export interface IPost
{
}

export interface IGet
{
}

export interface IPut
{
}

export interface IDelete
{
}

// @DataContract
export class AdminUserBase
{
    // @DataMember(Order=1)
    public userName: string;

    // @DataMember(Order=2)
    public firstName: string;

    // @DataMember(Order=3)
    public lastName: string;

    // @DataMember(Order=4)
    public displayName: string;

    // @DataMember(Order=5)
    public email: string;

    // @DataMember(Order=6)
    public password: string;

    // @DataMember(Order=7)
    public profileUrl: string;

    // @DataMember(Order=8)
    public userAuthProperties: { [index: string]: string; };

    // @DataMember(Order=9)
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<AdminUserBase>) { (Object as any).assign(this, init); }
}

// @DataContract
export class QueryBase
{
    // @DataMember(Order=1)
    public skip?: number;

    // @DataMember(Order=2)
    public take?: number;

    // @DataMember(Order=3)
    public orderBy: string;

    // @DataMember(Order=4)
    public orderByDesc: string;

    // @DataMember(Order=5)
    public include: string;

    // @DataMember(Order=6)
    public fields: string;

    // @DataMember(Order=7)
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<QueryBase>) { (Object as any).assign(this, init); }
}

export class QueryDb<T> extends QueryBase
{

    public constructor(init?: Partial<QueryDb<T>>) { super(init); (Object as any).assign(this, init); }
}

// @DataContract
export class CrudEvent
{
    // @DataMember(Order=1)
    public id: number;

    // @DataMember(Order=2)
    public eventType: string;

    // @DataMember(Order=3)
    public model: string;

    // @DataMember(Order=4)
    public modelId: string;

    // @DataMember(Order=5)
    public eventDate: string;

    // @DataMember(Order=6)
    public rowsUpdated?: number;

    // @DataMember(Order=7)
    public requestType: string;

    // @DataMember(Order=8)
    public requestBody: string;

    // @DataMember(Order=9)
    public userAuthId: string;

    // @DataMember(Order=10)
    public userAuthName: string;

    // @DataMember(Order=11)
    public remoteIp: string;

    // @DataMember(Order=12)
    public urn: string;

    // @DataMember(Order=13)
    public refId?: number;

    // @DataMember(Order=14)
    public refIdStr: string;

    // @DataMember(Order=15)
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<CrudEvent>) { (Object as any).assign(this, init); }
}

export class AppInfo
{
    public baseUrl: string;
    public serviceStackVersion: string;
    public serviceName: string;
    public serviceDescription: string;
    public serviceIconUrl: string;
    public brandUrl: string;
    public brandImageUrl: string;
    public textColor: string;
    public linkColor: string;
    public backgroundColor: string;
    public backgroundImageUrl: string;
    public iconUrl: string;
    public jsTextCase: string;
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<AppInfo>) { (Object as any).assign(this, init); }
}

export class ImageInfo
{
    public svg: string;
    public uri: string;
    public alt: string;
    public cls: string;

    public constructor(init?: Partial<ImageInfo>) { (Object as any).assign(this, init); }
}

export class LinkInfo
{
    public id: string;
    public href: string;
    public label: string;
    public icon: ImageInfo;

    public constructor(init?: Partial<LinkInfo>) { (Object as any).assign(this, init); }
}

export class ThemeInfo
{
    public form: string;
    public modelIcon: ImageInfo;

    public constructor(init?: Partial<ThemeInfo>) { (Object as any).assign(this, init); }
}

export class ApiCss
{
    public form: string;
    public fieldset: string;
    public field: string;

    public constructor(init?: Partial<ApiCss>) { (Object as any).assign(this, init); }
}

export class AppTags
{
    public default: string;
    public other: string;

    public constructor(init?: Partial<AppTags>) { (Object as any).assign(this, init); }
}

export class LocodeUi
{
    public css: ApiCss;
    public tags: AppTags;
    public maxFieldLength: number;
    public maxNestedFields: number;
    public maxNestedFieldLength: number;

    public constructor(init?: Partial<LocodeUi>) { (Object as any).assign(this, init); }
}

export class ExplorerUi
{
    public css: ApiCss;
    public tags: AppTags;

    public constructor(init?: Partial<ExplorerUi>) { (Object as any).assign(this, init); }
}

export class FormatInfo
{
    public method: string;
    public options: string;
    public locale: string;

    public constructor(init?: Partial<FormatInfo>) { (Object as any).assign(this, init); }
}

export class ApiFormat
{
    public locale: string;
    public assumeUtc: boolean;
    public number: FormatInfo;
    public date: FormatInfo;

    public constructor(init?: Partial<ApiFormat>) { (Object as any).assign(this, init); }
}

export class UiInfo
{
    public brandIcon: ImageInfo;
    public hideTags: string[];
    public modules: string[];
    public alwaysHideTags: string[];
    public adminLinks: LinkInfo[];
    public theme: ThemeInfo;
    public locode: LocodeUi;
    public explorer: ExplorerUi;
    public defaultFormats: ApiFormat;
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<UiInfo>) { (Object as any).assign(this, init); }
}

export class ConfigInfo
{
    public debugMode?: boolean;
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<ConfigInfo>) { (Object as any).assign(this, init); }
}

export class NavItem
{
    public label: string;
    public href: string;
    public exact?: boolean;
    public id: string;
    public className: string;
    public iconClass: string;
    public show: string;
    public hide: string;
    public children: NavItem[];
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<NavItem>) { (Object as any).assign(this, init); }
}

export class FieldCss
{
    public field: string;
    public input: string;
    public label: string;

    public constructor(init?: Partial<FieldCss>) { (Object as any).assign(this, init); }
}

export class InputInfo
{
    public id: string;
    public name: string;
    public type: string;
    public value: string;
    public placeholder: string;
    public help: string;
    public label: string;
    public title: string;
    public size: string;
    public pattern: string;
    public readOnly?: boolean;
    public required?: boolean;
    public disabled?: boolean;
    public autocomplete: string;
    public autofocus: string;
    public min: string;
    public max: string;
    public step?: number;
    public minLength?: number;
    public maxLength?: number;
    public allowableValues: string[];
    public allowableEntries: KeyValuePair<String,String>[];
    public options: string;
    public ignore?: boolean;
    public css: FieldCss;
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<InputInfo>) { (Object as any).assign(this, init); }
}

export class MetaAuthProvider
{
    public name: string;
    public label: string;
    public type: string;
    public navItem: NavItem;
    public icon: ImageInfo;
    public formLayout: InputInfo[];
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<MetaAuthProvider>) { (Object as any).assign(this, init); }
}

export class AuthInfo
{
    public hasAuthSecret?: boolean;
    public hasAuthRepository?: boolean;
    public includesRoles?: boolean;
    public includesOAuthTokens?: boolean;
    public htmlRedirect: string;
    public authProviders: MetaAuthProvider[];
    public roleLinks: { [index: string]: LinkInfo[]; };
    public serviceRoutes: { [index: string]: string[]; };
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<AuthInfo>) { (Object as any).assign(this, init); }
}

export class AutoQueryConvention
{
    public name: string;
    public value: string;
    public types: string;
    public valueType: string;

    public constructor(init?: Partial<AutoQueryConvention>) { (Object as any).assign(this, init); }
}

export class AutoQueryInfo
{
    public maxLimit?: number;
    public untypedQueries?: boolean;
    public rawSqlFilters?: boolean;
    public autoQueryViewer?: boolean;
    public async?: boolean;
    public orderByPrimaryKey?: boolean;
    public crudEvents?: boolean;
    public crudEventsServices?: boolean;
    public accessRole: string;
    public namedConnection: string;
    public viewerConventions: AutoQueryConvention[];
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<AutoQueryInfo>) { (Object as any).assign(this, init); }
}

export class ScriptMethodType
{
    public name: string;
    public paramNames: string[];
    public paramTypes: string[];
    public returnType: string;

    public constructor(init?: Partial<ScriptMethodType>) { (Object as any).assign(this, init); }
}

export class ValidationInfo
{
    public hasValidationSource?: boolean;
    public hasValidationSourceAdmin?: boolean;
    public serviceRoutes: { [index: string]: string[]; };
    public typeValidators: ScriptMethodType[];
    public propertyValidators: ScriptMethodType[];
    public accessRole: string;
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<ValidationInfo>) { (Object as any).assign(this, init); }
}

export class SharpPagesInfo
{
    public apiPath: string;
    public scriptAdminRole: string;
    public metadataDebugAdminRole: string;
    public metadataDebug?: boolean;
    public spaFallback?: boolean;
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<SharpPagesInfo>) { (Object as any).assign(this, init); }
}

export class RequestLogsInfo
{
    public requiredRoles: string[];
    public requestLogger: string;
    public serviceRoutes: { [index: string]: string[]; };
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<RequestLogsInfo>) { (Object as any).assign(this, init); }
}

export class FilesUploadLocation
{
    public name: string;
    public readAccessRole: string;
    public writeAccessRole: string;
    public allowExtensions: string[];
    public allowOperations: string;
    public maxFileCount?: number;
    public minFileBytes?: number;
    public maxFileBytes?: number;

    public constructor(init?: Partial<FilesUploadLocation>) { (Object as any).assign(this, init); }
}

export class FilesUploadInfo
{
    public basePath: string;
    public locations: FilesUploadLocation[];
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<FilesUploadInfo>) { (Object as any).assign(this, init); }
}

export class MetadataTypeName
{
    public name: string;
    public namespace: string;
    public genericArgs: string[];

    public constructor(init?: Partial<MetadataTypeName>) { (Object as any).assign(this, init); }
}

export class MetadataDataContract
{
    public name: string;
    public namespace: string;

    public constructor(init?: Partial<MetadataDataContract>) { (Object as any).assign(this, init); }
}

export class MetadataDataMember
{
    public name: string;
    public order?: number;
    public isRequired?: boolean;
    public emitDefaultValue?: boolean;

    public constructor(init?: Partial<MetadataDataMember>) { (Object as any).assign(this, init); }
}

export class MetadataAttribute
{
    public name: string;
    public constructorArgs: MetadataPropertyType[];
    public args: MetadataPropertyType[];

    public constructor(init?: Partial<MetadataAttribute>) { (Object as any).assign(this, init); }
}

export class RefInfo
{
    public model: string;
    public selfId: string;
    public refId: string;
    public refLabel: string;

    public constructor(init?: Partial<RefInfo>) { (Object as any).assign(this, init); }
}

export class MetadataPropertyType
{
    public name: string;
    public type: string;
    public namespace: string;
    public isValueType?: boolean;
    public isEnum?: boolean;
    public isPrimaryKey?: boolean;
    public genericArgs: string[];
    public value: string;
    public description: string;
    public dataMember: MetadataDataMember;
    public readOnly?: boolean;
    public paramType: string;
    public displayType: string;
    public isRequired?: boolean;
    public allowableValues: string[];
    public allowableMin?: number;
    public allowableMax?: number;
    public attributes: MetadataAttribute[];
    public input: InputInfo;
    public format: FormatInfo;
    public ref: RefInfo;

    public constructor(init?: Partial<MetadataPropertyType>) { (Object as any).assign(this, init); }
}

export class MetadataType
{
    public name: string;
    public namespace: string;
    public genericArgs: string[];
    public inherits: MetadataTypeName;
    public implements: MetadataTypeName[];
    public displayType: string;
    public description: string;
    public notes: string;
    public icon: ImageInfo;
    public isNested?: boolean;
    public isEnum?: boolean;
    public isEnumInt?: boolean;
    public isInterface?: boolean;
    public isAbstract?: boolean;
    public dataContract: MetadataDataContract;
    public properties: MetadataPropertyType[];
    public attributes: MetadataAttribute[];
    public innerTypes: MetadataTypeName[];
    public enumNames: string[];
    public enumValues: string[];
    public enumMemberValues: string[];
    public enumDescriptions: string[];
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<MetadataType>) { (Object as any).assign(this, init); }
}

export class MediaRule
{
    public size: string;
    public rule: string;
    public applyTo: string[];
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<MediaRule>) { (Object as any).assign(this, init); }
}

export class AdminUsersInfo
{
    public accessRole: string;
    public enabled: string[];
    public userAuth: MetadataType;
    public allRoles: string[];
    public allPermissions: string[];
    public queryUserAuthProperties: string[];
    public queryMediaRules: MediaRule[];
    public formLayout: InputInfo[];
    public css: ApiCss;
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<AdminUsersInfo>) { (Object as any).assign(this, init); }
}

export class PluginInfo
{
    public loaded: string[];
    public auth: AuthInfo;
    public autoQuery: AutoQueryInfo;
    public validation: ValidationInfo;
    public sharpPages: SharpPagesInfo;
    public requestLogs: RequestLogsInfo;
    public filesUpload: FilesUploadInfo;
    public adminUsers: AdminUsersInfo;
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<PluginInfo>) { (Object as any).assign(this, init); }
}

export class CustomPluginInfo
{
    public accessRole: string;
    public serviceRoutes: { [index: string]: string[]; };
    public enabled: string[];
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<CustomPluginInfo>) { (Object as any).assign(this, init); }
}

export class MetadataTypesConfig
{
    public baseUrl: string;
    public usePath: string;
    public makePartial: boolean;
    public makeVirtual: boolean;
    public makeInternal: boolean;
    public baseClass: string;
    public package: string;
    public addReturnMarker: boolean;
    public addDescriptionAsComments: boolean;
    public addDataContractAttributes: boolean;
    public addIndexesToDataMembers: boolean;
    public addGeneratedCodeAttributes: boolean;
    public addImplicitVersion?: number;
    public addResponseStatus: boolean;
    public addServiceStackTypes: boolean;
    public addModelExtensions: boolean;
    public addPropertyAccessors: boolean;
    public excludeGenericBaseTypes: boolean;
    public settersReturnThis: boolean;
    public makePropertiesOptional: boolean;
    public exportAsTypes: boolean;
    public excludeImplementedInterfaces: boolean;
    public addDefaultXmlNamespace: string;
    public makeDataContractsExtensible: boolean;
    public initializeCollections: boolean;
    public addNamespaces: string[];
    public defaultNamespaces: string[];
    public defaultImports: string[];
    public includeTypes: string[];
    public excludeTypes: string[];
    public treatTypesAsStrings: string[];
    public exportValueTypes: boolean;
    public globalNamespace: string;
    public excludeNamespace: boolean;
    public dataClass: string;
    public dataClassJson: string;
    public ignoreTypes: string[];
    public exportTypes: string[];
    public exportAttributes: string[];
    public ignoreTypesInNamespaces: string[];

    public constructor(init?: Partial<MetadataTypesConfig>) { (Object as any).assign(this, init); }
}

export class MetadataRoute
{
    public path: string;
    public verbs: string;
    public notes: string;
    public summary: string;

    public constructor(init?: Partial<MetadataRoute>) { (Object as any).assign(this, init); }
}

export class ApiUiInfo
{
    public locodeCss: ApiCss;
    public explorerCss: ApiCss;
    public formLayout: InputInfo[];
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<ApiUiInfo>) { (Object as any).assign(this, init); }
}

export class MetadataOperationType
{
    public request: MetadataType;
    public response: MetadataType;
    public actions: string[];
    public returnsVoid?: boolean;
    public method: string;
    public returnType: MetadataTypeName;
    public routes: MetadataRoute[];
    public dataModel: MetadataTypeName;
    public viewModel: MetadataTypeName;
    public requiresAuth?: boolean;
    public requiredRoles: string[];
    public requiresAnyRole: string[];
    public requiredPermissions: string[];
    public requiresAnyPermission: string[];
    public tags: string[];
    public ui: ApiUiInfo;

    public constructor(init?: Partial<MetadataOperationType>) { (Object as any).assign(this, init); }
}

export class MetadataTypes
{
    public config: MetadataTypesConfig;
    public namespaces: string[];
    public types: MetadataType[];
    public operations: MetadataOperationType[];

    public constructor(init?: Partial<MetadataTypes>) { (Object as any).assign(this, init); }
}

// @DataContract
export class ResponseError
{
    // @DataMember(Order=1)
    public errorCode: string;

    // @DataMember(Order=2)
    public fieldName: string;

    // @DataMember(Order=3)
    public message: string;

    // @DataMember(Order=4)
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<ResponseError>) { (Object as any).assign(this, init); }
}

// @DataContract
export class ResponseStatus
{
    // @DataMember(Order=1)
    public errorCode: string;

    // @DataMember(Order=2)
    public message: string;

    // @DataMember(Order=3)
    public stackTrace: string;

    // @DataMember(Order=4)
    public errors: ResponseError[];

    // @DataMember(Order=5)
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<ResponseStatus>) { (Object as any).assign(this, init); }
}

export class KeyValuePair<TKey, TValue>
{
    public key: TKey;
    public value: TValue;

    public constructor(init?: Partial<KeyValuePair<TKey, TValue>>) { (Object as any).assign(this, init); }
}

export class AppMetadata
{
    public app: AppInfo;
    public ui: UiInfo;
    public config: ConfigInfo;
    public contentTypeFormats: { [index: string]: string; };
    public httpHandlers: { [index: string]: string; };
    public plugins: PluginInfo;
    public customPlugins: { [index: string]: CustomPluginInfo; };
    public api: MetadataTypes;
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<AppMetadata>) { (Object as any).assign(this, init); }
}

// @DataContract
export class AuthenticateResponse implements IHasSessionId, IHasBearerToken
{
    // @DataMember(Order=1)
    public userId: string;

    // @DataMember(Order=2)
    public sessionId: string;

    // @DataMember(Order=3)
    public userName: string;

    // @DataMember(Order=4)
    public displayName: string;

    // @DataMember(Order=5)
    public referrerUrl: string;

    // @DataMember(Order=6)
    public bearerToken: string;

    // @DataMember(Order=7)
    public refreshToken: string;

    // @DataMember(Order=8)
    public profileUrl: string;

    // @DataMember(Order=9)
    public roles: string[];

    // @DataMember(Order=10)
    public permissions: string[];

    // @DataMember(Order=11)
    public responseStatus: ResponseStatus;

    // @DataMember(Order=12)
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<AuthenticateResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class AssignRolesResponse
{
    // @DataMember(Order=1)
    public allRoles: string[];

    // @DataMember(Order=2)
    public allPermissions: string[];

    // @DataMember(Order=3)
    public meta: { [index: string]: string; };

    // @DataMember(Order=4)
    public responseStatus: ResponseStatus;

    public constructor(init?: Partial<AssignRolesResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class UnAssignRolesResponse
{
    // @DataMember(Order=1)
    public allRoles: string[];

    // @DataMember(Order=2)
    public allPermissions: string[];

    // @DataMember(Order=3)
    public meta: { [index: string]: string; };

    // @DataMember(Order=4)
    public responseStatus: ResponseStatus;

    public constructor(init?: Partial<UnAssignRolesResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class AdminUserResponse
{
    // @DataMember(Order=1)
    public id: string;

    // @DataMember(Order=2)
    public result: { [index: string]: Object; };

    // @DataMember(Order=3)
    public details: { [index:string]: Object; }[];

    // @DataMember(Order=4)
    public responseStatus: ResponseStatus;

    public constructor(init?: Partial<AdminUserResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class AdminUsersResponse
{
    // @DataMember(Order=1)
    public results: { [index:string]: Object; }[];

    // @DataMember(Order=2)
    public responseStatus: ResponseStatus;

    public constructor(init?: Partial<AdminUsersResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class AdminDeleteUserResponse
{
    // @DataMember(Order=1)
    public id: string;

    // @DataMember(Order=2)
    public responseStatus: ResponseStatus;

    public constructor(init?: Partial<AdminDeleteUserResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class QueryResponse<T>
{
    // @DataMember(Order=1)
    public offset: number;

    // @DataMember(Order=2)
    public total: number;

    // @DataMember(Order=3)
    public results: T[];

    // @DataMember(Order=4)
    public meta: { [index: string]: string; };

    // @DataMember(Order=5)
    public responseStatus: ResponseStatus;

    public constructor(init?: Partial<QueryResponse<T>>) { (Object as any).assign(this, init); }
}

// @Route("/metadata/app")
// @DataContract
export class MetadataApp implements IReturn<AppMetadata>
{
    // @DataMember(Order=1)
    public view: string;

    public constructor(init?: Partial<MetadataApp>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'MetadataApp'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new AppMetadata(); }
}

/**
* Sign In
*/
// @Route("/auth", "OPTIONS,GET,POST,DELETE")
// @Route("/auth/{provider}", "OPTIONS,GET,POST,DELETE")
// @Api(Description="Sign In")
// @DataContract
export class Authenticate implements IReturn<AuthenticateResponse>, IPost
{
    /**
    * AuthProvider, e.g. credentials
    */
    // @DataMember(Order=1)
    public provider: string;

    // @DataMember(Order=2)
    public state: string;

    // @DataMember(Order=3)
    public oauth_token: string;

    // @DataMember(Order=4)
    public oauth_verifier: string;

    // @DataMember(Order=5)
    public userName: string;

    // @DataMember(Order=6)
    public password: string;

    // @DataMember(Order=7)
    public rememberMe?: boolean;

    // @DataMember(Order=9)
    public errorView: string;

    // @DataMember(Order=10)
    public nonce: string;

    // @DataMember(Order=11)
    public uri: string;

    // @DataMember(Order=12)
    public response: string;

    // @DataMember(Order=13)
    public qop: string;

    // @DataMember(Order=14)
    public nc: string;

    // @DataMember(Order=15)
    public cnonce: string;

    // @DataMember(Order=17)
    public accessToken: string;

    // @DataMember(Order=18)
    public accessTokenSecret: string;

    // @DataMember(Order=19)
    public scope: string;

    // @DataMember(Order=20)
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<Authenticate>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'Authenticate'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new AuthenticateResponse(); }
}

// @Route("/assignroles", "POST")
// @DataContract
export class AssignRoles implements IReturn<AssignRolesResponse>, IPost
{
    // @DataMember(Order=1)
    public userName: string;

    // @DataMember(Order=2)
    public permissions: string[];

    // @DataMember(Order=3)
    public roles: string[];

    // @DataMember(Order=4)
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<AssignRoles>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'AssignRoles'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new AssignRolesResponse(); }
}

// @Route("/unassignroles", "POST")
// @DataContract
export class UnAssignRoles implements IReturn<UnAssignRolesResponse>, IPost
{
    // @DataMember(Order=1)
    public userName: string;

    // @DataMember(Order=2)
    public permissions: string[];

    // @DataMember(Order=3)
    public roles: string[];

    // @DataMember(Order=4)
    public meta: { [index: string]: string; };

    public constructor(init?: Partial<UnAssignRoles>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'UnAssignRoles'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new UnAssignRolesResponse(); }
}

// @DataContract
export class AdminGetUser implements IReturn<AdminUserResponse>, IGet
{
    // @DataMember(Order=10)
    public id: string;

    public constructor(init?: Partial<AdminGetUser>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'AdminGetUser'; }
    public getMethod() { return 'GET'; }
    public createResponse() { return new AdminUserResponse(); }
}

// @DataContract
export class AdminQueryUsers implements IReturn<AdminUsersResponse>, IGet
{
    // @DataMember(Order=1)
    public query: string;

    // @DataMember(Order=2)
    public orderBy: string;

    // @DataMember(Order=3)
    public skip?: number;

    // @DataMember(Order=4)
    public take?: number;

    public constructor(init?: Partial<AdminQueryUsers>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'AdminQueryUsers'; }
    public getMethod() { return 'GET'; }
    public createResponse() { return new AdminUsersResponse(); }
}

// @DataContract
export class AdminCreateUser extends AdminUserBase implements IReturn<AdminUserResponse>, IPost
{
    // @DataMember(Order=10)
    public roles: string[];

    // @DataMember(Order=11)
    public permissions: string[];

    public constructor(init?: Partial<AdminCreateUser>) { super(init); (Object as any).assign(this, init); }
    public getTypeName() { return 'AdminCreateUser'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new AdminUserResponse(); }
}

// @DataContract
export class AdminUpdateUser extends AdminUserBase implements IReturn<AdminUserResponse>, IPut
{
    // @DataMember(Order=10)
    public id: string;

    // @DataMember(Order=11)
    public lockUser?: boolean;

    // @DataMember(Order=12)
    public unlockUser?: boolean;

    // @DataMember(Order=13)
    public addRoles: string[];

    // @DataMember(Order=14)
    public removeRoles: string[];

    // @DataMember(Order=15)
    public addPermissions: string[];

    // @DataMember(Order=16)
    public removePermissions: string[];

    public constructor(init?: Partial<AdminUpdateUser>) { super(init); (Object as any).assign(this, init); }
    public getTypeName() { return 'AdminUpdateUser'; }
    public getMethod() { return 'PUT'; }
    public createResponse() { return new AdminUserResponse(); }
}

// @DataContract
export class AdminDeleteUser implements IReturn<AdminDeleteUserResponse>, IDelete
{
    // @DataMember(Order=10)
    public id: string;

    public constructor(init?: Partial<AdminDeleteUser>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'AdminDeleteUser'; }
    public getMethod() { return 'DELETE'; }
    public createResponse() { return new AdminDeleteUserResponse(); }
}

// @Route("/crudevents/{Model}")
// @DataContract
export class GetCrudEvents extends QueryDb<CrudEvent> implements IReturn<QueryResponse<CrudEvent>>
{
    // @DataMember(Order=1)
    public authSecret: string;

    // @DataMember(Order=2)
    public model: string;

    // @DataMember(Order=3)
    public modelId: string;

    public constructor(init?: Partial<GetCrudEvents>) { super(init); (Object as any).assign(this, init); }
    public getTypeName() { return 'GetCrudEvents'; }
    public getMethod() { return 'GET'; }
    public createResponse() { return new QueryResponse<CrudEvent>(); }
}



/**
 * Server Metadata containing App capabilities and APIs used to dynamically render the UI  
 */
export let Server:AppMetadata;

/** Tailwind Responsive Breakpoints
 * { 2xl:1536, xl:1280, lg:1024, md:768, sm:640 } */
export type Breakpoints = Record<'2xl' | 'xl' | 'lg' | 'md' | 'sm', boolean>;

/** Return self or reactive proxy of self */
export type Identity = <T>(args: T) => T;

/** Invoke a Tailwind Definition Rule */
export type Transition = (prop:string,enter:boolean) => boolean;

/** Publish/Subscribe to App Events */
export type Bus = {
    subscribe: (type: string, callback: Function) => {
        unsubscribe: () => void;
    };
    publish: (eventType: string, arg: any) => void;
};

/** High-level API encapsulating client PetiteVue App */
export type App = {
    /** Publish/Subscript to App events */
    events: Bus;
    /** PetiteVue App instance */
    readonly petite: any;
    /** Register map of PetiteVue components using key as Components Name */
    components: (components: Record<string,Function|Object>) => void;
    /** Register single component 
     * @param {string} name
     * @param {string|Function} component Auto Component template HTML or Component Function */
    component: (name: string, component: string|Function) => void;
    /** Register Auto Component with $template contents
     * @param {string} name
     * @param {string} $template */
    template: (name: string, $template: string) => void;
    /** Register map of Auto Components using key as Components Name */
    templates: (templates: Record<string,string>) => void;
    /** Register PetiteVue directive */
    directive: (name: string, fn: Function) => void;
    /** Register (non-reactive) global App state property */
    prop: (name: string, val: any) => void;
    /** Register multiple (non-reactive) global App state props */
    props: (props:Record<string,any>) => void;
    /** Build PetiteVue App instance */
    build: (args: Record<string,any>) => any;
    /** Dynamically load external script src */
    import: (arg0: string) => Promise<any>;
    /** Register callback to invoke after App has started */
    onStart: (f: Function) => void;
    /** Start App instance */
    start: () => void;
    /** App function for unsubscribing 'sub' subscription in Component instance */
    unsubscribe: () => void;
    /** PetiteVue.createApp - create PetiteVue instance */
    createApp: (args: any) => any;
    /** PetiteVue.nextTick - register callback to be fired after next async loop */
    nextTick: (f: Function) => void;
    /** PetiteVue.reactive - create a reactive store */
    reactive: Identity;
};

/** Utility class for managing Forms UI and behavior */
export type Forms = {
    getId: (type: MetadataType, row: any) => any;
    getType: (typeRef: string | {
        namespace: string;
        name: string;
    }) => MetadataType;
    inputId: (input: {id:string}) => string;
    colClass: (fields: number) => string;
    inputProp: (prop: MetadataPropertyType) => {
        id: string;
        type: string;
        'data-type': string;
    };
    getPrimaryKey: (type: MetadataType) => MetadataPropertyType|null;
    typeProperties: (type: MetadataType) => MetadataPropertyType[];
    relativeTime: (val: string | number | Date, rtf?: Intl.RelativeTimeFormat) => string;
    relativeTimeFromMs: (elapsedMs: number, rtf?: Intl.RelativeTimeFormat) => string;
    relativeTimeFromDate: (d: Date, from?: Date) => string;
    Lookup: {};
    lookupLabel: (model: any, id: any, label: string) => any;
    refInfo: (row: any, prop: MetadataPropertyType, props: MetadataPropertyType[]) => {
        href: {
            op: string;
            skip: any;
            edit: any;
            new: any;
            $qs: {
                [x: string]: any;
            };
        };
        icon: any;
        html: any;
    };
    fetchLookupValues: (results: any[], props: MetadataPropertyType[], refreshFn: () => void) => void;
    theme: ThemeInfo;
    formClass: string;
    gridClass: string;
    opTitle(op: MetadataOperationType): string;
    forAutoForm(type: MetadataType): (field: any) => void;
    forCreate(type: MetadataType): (field: any) => void;
    forEdit(type: MetadataType): (field: any) => void;
    getFormProp(id: any, type: any): MetadataPropertyType;
    getGridInputs(formLayout: InputInfo[], f?: (args: {
        id: string;
        input: InputInfo;
        rowClass: string;
    }) => void): {
        id: string;
        input: InputInfo;
        rowClass: string;
    }[];
    getGridInput(input: InputInfo, f?: (args: {
        id: string;
        input: InputInfo;
        rowClass: string;
    }) => void): {
        id: string;
        input: InputInfo;
        rowClass: string;
    };
    getFieldError(error: any, id: any): string|null;
    kvpValues(input: any): {key,value}[];
    useLabel(input: any): string|null;
    usePlaceholder(input: any): string|null;
    isRequired(input: any): boolean;
    resolveFormLayout(op: MetadataOperationType): InputInfo[];
    formValues(form: any): Record<string,any>;
    formData(form: any, op: MetadataOperationType): FormData;
    groupTypes(allTypes: any): any[];
    complexProp(prop: any): boolean;
    supportsProp(prop: any): boolean;
    populateModel(model: any, formLayout: any): Record<string,any>;
    apiValue(o: any): any;
    format(o: any, prop: MetadataPropertyType): any;
}

/** Generic functionality around AppMetadata */
export type Meta = {
    /** Global Cache */
    CACHE: {};
    /** HTTP Errors specially handled by Locode */
    HttpErrors: Record<number, string>;
    /** Map of Request DTO names to `MetadataOperationType` */
    OpsMap: Record<string, MetadataOperationType>;
    /** Map of DTO names to `MetadataType` */
    TypesMap: Record<string, MetadataType>;
    /** Map of DTO namespace + names to `MetadataType` */
    FullTypesMap: Record<string, MetadataType>;
    /** Find `MetadataOperationType` by API name */
    getOp: (opName: string) => MetadataOperationType;
    /** Find `MetadataType` by DTO name */
    getType: (typeRef: ({ namespace?: string; name: string; } | string)) => null | MetadataType;
    /** Check whether a Type is an Enum */
    isEnum: (type: string) => boolean;
    /** Get Enum Values of an Enum Type */
    enumValues: (type: string) => { key: string; value: string; }[];
    /** Get API Icon */
    getIcon: (args: ({ op?: MetadataOperationType; type?: MetadataType; })) => { svg: string; };
};

/** Reactive store to manage page navigation state and sync with history.pushState */
export type Routes = {
    /** The arg name that's used to identify the page name */
    page: string;
    /** Populate Route state */
    set: (args: Record<string, any>) => void;
    /** Snapshot of the current route state */
    state: Record<string, any>;
    /** Navigate to new route state */
    to: (args: Record<string, any>) => void;
    /** Return URL of current route state */
    href: (args: Record<string, any>) => string;
}

/** APIs for resolving SVG icons and data URIs for different File Types */
interface Files {
    /** Get Icon SVG for .ext */
    extSvg(ext: string): string | null;
    /** Get Icon src for .ext */
    extSrc(ext: string): string | null;
    /** Return file extension (without '.; prefix) of path or URI */
    getExt(path: string): string | null;
    /** Encode SVG for embedding in Data URI */
    encodeSvg(s: string): string;
    /** Return whether path is a URI to a previewable image */
    canPreview(path: string): boolean;
    /** Return file name part of URI or file path */
    getFileName(path: string): string | null;
    /** Format bytes into human-readable file size */
    formatBytes(bytes: number, d?: number): string;
    /** Get the Icon src for a file path or URI, previewable resources will return self, otherwise returns SVG URI of .ext */
    filePathUri(path: string): string | null;
    /** Convert SVG to Data URI */
    svgToDataUri(svg: string): string;
    /** Return Image URI of INPUT file attachments */
    fileImageUri(file: File | MediaSource): string;
    /** Clear all remaining Image URIs of INPUT file attachments */
    flush(): void;
}
export let Files:Files

/** APIs to inspect .NET Types */
interface Types {
    /** Return well-known C# alias for its .NET Type name */
    alias(type: string): any;
    /** Return underlying Type if nullable */
    unwrap(type: string): string;
    /** Resolve well-known C# Type Name from Type Ref */
    typeName(metaType:{name:string,genericArgs:string[]}): string;
    /** Resolve well-known C# Type Name from Name and Generic Args */
    typeName2(name: string, genericArgs: string[]): string;
    /** Return true if .NET Type is numeric */
    isNumber(type: string): boolean;
    /** Return true if .NET Type is a string */
    isString(type: string): boolean;
    /** Return true if .NET Type is a collection */
    isArray(type: string): boolean;
    /** Return true if typeof is a scalar value (string|number|symbol|boolean) */
    isPrimitive(type: string): boolean;
    /** Return value suitable for human display */
    formatValue(type: string, value: any): any;
    /** Create a unique key string from a Type Ref */
    key(typeRef:{namespace:string,name:string}): string | null;
    /** Return true if both Type Refs are equivalent */
    equals(a:{namespace:string,name:string},b:{namespace:string,name:string}): boolean;
    /** Return true if Property has named Attribute */
    propHasAttr(p: MetadataPropertyType, attr: string): boolean;
    /** Return named property on Type (case-insensitive) */
    getProp(type: MetadataType, name: string): MetadataPropertyType;
    /** Return all properties of a Type, inc. base class properties  */
    typeProperties(TypesMap, type): MetadataPropertyType[];
}
export let Types:Types


///=== EXPLORER ===///

/** Custom route params used in API Explorer */
export type ExplorerRoutes = {
    op?: string;
    tab?: string;
    lang?: string;
    provider?: string;
    preview?: string;
    body?: string;
    doc?: string;
    detailSrc?: string;
    form?: string;
    response?: string;
};
/** Route methods used in API Explorer */
export type ExplorerRoutesExtend = {
    queryHref(): string;
};

/** App's primary reactive store maintaining global functionality for Admin UI */
export type ExplorerStore = {
    cachedFetch: (url: string) => Promise<string>;
    copied: boolean;
    readonly opTabs: { [p: string]: string; };
    sideNav: { expanded: boolean; operations: MetadataOperationType[]; tag: string; }[];
    auth: AuthenticateResponse;
    readonly displayName: string | null;
    loadLang: () => void;
    langCache: () => { op: string; lang: string; url: string; };
    login: (args: any, $on?: Function) => void;
    detailSrcResult: {};
    logout: () => void;
    readonly isServiceStackType: boolean;
    api: ApiResult<AuthenticateResponse>;
    init: () => void;
    readonly op: MetadataOperationType | null;
    debug: boolean;
    readonly filteredSideNav: { tag: string; operations: MetadataOperationType[]; expanded: boolean; }[];
    readonly authProfileUrl: string | null;
    previewResult: string | null;
    readonly activeLangSrc: string | null;
    readonly previewCache: { preview: string; url: string; lang: string; } | null;
    toggle: (tag: string) => void;
    getTypeUrl: (types: string) => string;
    readonly authRoles: string[];
    filter: string;
    loadDetailSrc: () => void;
    baseUrl: string;
    readonly activeDetailSrc: string;
    readonly authLinks: LinkInfo[];
    readonly opName: string;
    readonly previewSrc: string;
    SignIn: (opt: any) => Function;
    hasRole: (role: string) => boolean;
    loadPreview: () => void;
    readonly authPermissions: string[];
    readonly useLang: string;
    invalidAccess: () => string | null;
};

/** Method arguments of custom Doc Components */
export interface DocComponentArgs {
    store: ExplorerStore;
    routes: ExplorerRoutes & Routes;
    breakpoints: Breakpoints;
    op: () => MetadataOperationType;
}

/** Method Signature of custom Doc Components */
export declare type DocComponent = (args:DocComponentArgs) => Record<string,any>;

///=== LOCODE ===///

/** Custom route params used in Locode */
export type LocodeRoutes = {
    op?: string;
    tab?: string;
    provider?: string;
    preview?: string;
    body?: string;
    doc?: string;
    skip?: string;
    new?: string;
    edit?: string;
};
/** Route methods used in Locode */
export type LocodeRoutesExtend = {
    onEditChange(any: any): void;
    update(): void;
    uiHref(any: any): string;
};

/* App's primary reactive store maintaining global functionality for Locode Apps */
export type LocodeStore = {
    cachedFetch: (url: string) => Promise<string>;
    copied: boolean;
    sideNav: { expanded: boolean; operations: MetadataOperationType[]; tag: string; }[];
    auth: AuthenticateResponse;
    readonly displayName: string | null;
    login: (args: any, $on?: Function) => void;
    detailSrcResult: any;
    logout: () => void;
    readonly isServiceStackType: boolean;
    readonly opViewModel: string;
    api: ApiResult<AuthenticateResponse>;
    modalLookup: any | null;
    init: () => void;
    readonly op: MetadataOperationType;
    debug: boolean;
    readonly filteredSideNav: { tag: string; operations: MetadataOperationType[]; expanded: boolean; }[];
    readonly authProfileUrl: string | null;
    previewResult: string | null;
    readonly opDesc: string;
    toggle: (tag: string) => void;
    readonly opDataModel: string;
    readonly authRoles: string[];
    filter: string;
    baseUrl: string;
    readonly authLinks: LinkInfo[];
    readonly opName: string;
    SignIn: (opt: any) => Function;
    hasRole: (role: string) => boolean;
    readonly authPermissions: string[];
    readonly useLang: string;
    invalidAccess: () => string | null;
};

/** Manage users query & filter preferences in the Users browsers localStorage */
export type LocodeSettings = {
    op: (op: string) => any;
    lookup: (op: string) => any;
    saveOp: (op: string, fn: Function) => void;
    hasPrefs: (op: string) => boolean;
    saveOpProp: (op: string, name: string, fn: Function) => void;
    saveLookup: (op: string, fn: Function) => void;
    events: {
        op: (op: string) => string;
        lookup: (op: string) => string;
        opProp: (op: string, name: string) => string;
    };
    opProp: (op: string, name: string) => any;
    clearPrefs: (op: string) => void;
};

/** Create a new state for an API that encapsulates its invocation and execution */
export type ApiState = {
    op: MetadataOperationType;
    client: any;
    apiState: ApiState;
    formLayout: any;
    createModel: (args: any) => any;
    apiLoading: boolean;
    apiResult: any;
    readonly api: any;
    createRequest: (args: any) => any;
    model: any;
    title: string | null;
    readonly error: ResponseStatus;
    readonly errorSummary: string|null;
    fieldError(id: string): string|null;
    field(propName: string, f?: (args: { id: string; input: InputInfo; rowClass: string; }) => void): any;
    apiSend(dtoArgs: Record<string, any>, queryArgs?: Record<string, any>): any;
    apiForm(formData: FormData, queryArgs?: Record<string, any>): any;
};

/** All CRUD API States available for this operation */
export type CrudApisState = {
    opQuery:  MetadataOperationType | null;
    opCreate: MetadataOperationType | null;
    opPatch:  MetadataOperationType | null;
    opUpdate: MetadataOperationType | null;
    opDelete: MetadataOperationType | null;
    apiQuery:  ApiState | null;
    apiCreate: ApiState | null;
    apiPatch:  ApiState | null;
    apiUpdate: ApiState | null;
    apiDelete: ApiState | null;
};

export type CrudApisStateProp = CrudApisState & {
    prop: MetadataPropertyType;
    opName: string;
    dataModel: MetadataType;
    viewModel: MetadataType; 
    viewModelColumns: MetadataPropertyType[];
    callback:Function;
    createPrefs: () => any;
    selectedColumns: (prefs:any) => MetadataPropertyType[];
}

/** Method arguments of custom Create Form Components */
export interface CreateComponentArgs {
    store: LocodeStore;
    routes: LocodeRoutes & LocodeRoutesExtend & Routes;
    settings: LocodeSettings;
    state: () => CrudApisState;
    save: () => void;
    done: () => void;
}
/** Method Signature of custom Create Form Components */
export declare type CreateComponent = (args:CreateComponentArgs) => Record<string,any>;

/** Method arguments of custom Edit Form Components */
export interface EditComponentArgs {
    store: LocodeStore;
    routes: LocodeRoutes & LocodeRoutesExtend & Routes;
    settings: LocodeSettings;
    state: () => CrudApisState;
    save: () => void;
    done: () => void;
}
/** Method signature of custom Edit Form Components */
export declare type EditComponent = (args:EditComponentArgs) => Record<string,any>;

///=== ADMIN ===///

/** Route methods used in Admin UI */
export type AdminRoutes = {
    tab?: string;
    provider?: string;
    q?: string;
    page?: string;
    sort?: string;
    new?: string;
    edit?: string;
};

/** App's primary reactive store maintaining global functionality for Admin UI */
export type AdminStore = {
    adminLink(string: any): LinkInfo;
    init(): void;
    cachedFetch(string: any): Promise<unknown>;
    debug: boolean;
    copied: boolean;
    auth: AuthenticateResponse | null;
    readonly authProfileUrl: string | null;
    readonly displayName: null;
    readonly link: LinkInfo;
    readonly isAdmin: boolean;
    login(any: any): void;
    readonly adminUsers: AdminUsersInfo;
    readonly authRoles: string[];
    filter: string;
    baseUrl: string;
    logout(): void;
    readonly authLinks: LinkInfo[];
    SignIn(): Function;
    readonly adminLinks: LinkInfo[];
    api: ApiResult<AuthenticateResponse> | null;
    readonly authPermissions: any;
};
