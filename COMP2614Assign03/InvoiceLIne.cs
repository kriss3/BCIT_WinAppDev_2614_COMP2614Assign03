using System;

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
		public bool IsTaxable { get; set; }
		public decimal LineTotal
		{
			get
			{
				return this.Quantity * this.Price;
			}
		}
	}
}
