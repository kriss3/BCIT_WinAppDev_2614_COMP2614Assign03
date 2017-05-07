using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP2614Assign03
{
	class InvoiceHeader
	{
		public int InvoiceNumber { get; set; }
		public DateTime InvoiceDate { get; set; }
		public int Term { get; set; }
		public List<InvoiceLine> InvoiceLines { get; set; }
		public Guid InvoiceId { get; set; }
	}
}
