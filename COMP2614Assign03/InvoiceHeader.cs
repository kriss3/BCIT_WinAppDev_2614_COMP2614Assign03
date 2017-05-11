using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP2614Assign03
{
	/// <summary>
	/// Class to handle Invoice Header;
	/// Adding InvoiceId to create foreign-key relationship with Invoice Line;
	/// Invoice Header can have multiple Invoice Lines;
	/// </summary>
	class InvoiceHeader
	{
		public int InvoiceNumber { get; set; }
		public DateTime InvoiceDate { get; set; }
		public int Term { get; set; }
		public List<InvoiceLine> InvoiceLines { get; set; }
		public Guid InvoiceId { get; set; }

		/// <summary>
		/// Method to split integer into Array of two elements: [0] - discount [1] - number of days; 
		/// Using Lambda expression to get 3 digit integer code into array of int;
		/// </summary>
		/// <returns>Array of Integers</returns>
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
