using DBR.Core.Domain;

namespace DBR.Core.DTOs.Outputs;

public class InvoiceDTO : BaseEntity
{
	public string InvoiceURL { get; set; } = null!;
}