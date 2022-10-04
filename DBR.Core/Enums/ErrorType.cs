namespace DBR.Core.Enums;

public enum ErrorType
{
	Unhandled = 0,
	InvalidInput = 1,
	EntityDuplicate = 2,
	EntityNotFound = 3,
	TokenNotFound = 4,
	ClaimsPrincipalNotFoundFromAccessToken = 5,
	UserNotFoundWithClaimsPrincipal = 6,
	OptimisticConcurrency = 7,
	RefreshTokenInvalidOrExpired = 8,
	UnableToRevokeMember = 9,
	CancellationTokenRequested = 10,
	UserManagerError = 11,
	HttpRequestError = 12,
	InvalidAppAccessToken = 13,
	TooManyLoginAttempts = 14
}