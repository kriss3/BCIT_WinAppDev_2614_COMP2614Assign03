using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP2614Assign03
{
	/// <summary>
	/// Basic data class for Invoice lines;
	/// Adding InvoiceId to relate Invoice Header and Invoice Lines
	/// </summary>
	class InvoiceLine
	{
		public int Quantity { get; set; }
		public string SKU { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public bool Taxable { get; set; }
		public Guid InvoiceId { get; set; }
	}
}
