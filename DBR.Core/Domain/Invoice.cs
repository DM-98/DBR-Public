using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBR.Core.Domain;

[Table("Invoices")]
public class Invoice : BaseEntity
{
	[StringLength(150)]
	public string InvoiceURL { get; set; } = null!;
}