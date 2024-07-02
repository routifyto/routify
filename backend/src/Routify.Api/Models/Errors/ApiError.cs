namespace Routify.Api.Models.Errors;

public enum ApiError
{
    Unknown = 0,
    
    //accounts
    EmailAlreadyExists = 1000,
    InvalidEmailOrPassword = 1001,
    GoogleAuthFailed = 1002,
    UserPendingActivation = 1003,
}