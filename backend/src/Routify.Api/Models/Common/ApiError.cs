namespace Routify.Api.Models.Common;

public enum ApiError
{
    Unknown = 0,
    
    Unauthorized = 401,
    
    //accounts
    EmailAlreadyExists = 1000,
    InvalidEmailOrPassword = 1001,
    GoogleAuthFailed = 1002,
    UserPendingActivation = 1003,
    InvalidEmail = 1004,
    
    //apps
    AppNotFound = 2000,
    NoAppAccess = 2001,
    CannotManageApp = 2002,
    
    //routes
    RouteNotFound = 3000,
    CannotManageRoutes = 3001,
    
    //app providers
    AppProviderNotFound = 4000,
    CannotManageAppProviders = 4001,
    
    //app users
    AppUserNotFound = 5000,
    CannotAddOwner = 5002,
    CannotAddAdmin = 5003,
    CannotAddMember = 5004,
    EmailListEmpty = 5005,
    CannotRemoveOwner = 5006,
    CannotRemoveAdmin = 5007,
    CannotRemoveMember = 5008,
    CannotRemoveLastOwner = 5009,
    
    
    //api keys
    ApiKeyNotFound = 6000,
    CannotManageApiKeys = 6001,
    
    //logs
    CompletionLogNotFound = 7000,
    
    //consumers
    ConsumerNotFound = 8000,
    CannotManageConsumers = 8001,
}