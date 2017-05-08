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

		public int[] ParseTerm()
		{
			var intArray = Term.ToString().Select(c => Convert.ToInt32(c.ToString())).ToArray();

			var results = new int[2];

			for (int i = 0; i < intArray.Length; i++)
			{
				results[0] = intArray[0];
				results[1] = int.Parse(intArray[1].ToString() + intArray[2].ToString());
			}

			return results;
		}
	}
}
