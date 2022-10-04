using DBR.Core.Enums;

namespace DBR.Core.DTOs.Outputs;

public class ResponseDTO<TOUT> where TOUT : class
{
	public bool Success { get; set; }

	public string? ErrorMessage { get; set; }

	public string? ExceptionMessage { get; set; }

	public string? InnerExceptionMessage { get; set; }

	public ErrorType ErrorType { get; set; }

	public TOUT? Content { get; set; }
}