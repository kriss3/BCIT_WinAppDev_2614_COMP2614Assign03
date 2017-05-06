using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Console;

namespace COMP2614Assign03
{
	/// <summary>
	/// Utility class, all methods static;
	/// </summary>
	class Helper
	{
		public static void Greetings()
		{
			WriteLine("Hello from assignment 3");
			WriteLine();
			WriteLine(new String('-', 60));
			
		}

		public static void PrintInvoiceHeader()
		{
			WriteLine("Invoice Listing");
			WriteLine(new String('-',60));
		}

		public static void PrintInvoiceLineHeader()
		{
			WriteLine(new String('-', 60));
			WriteLine($"{"Qty",-5}{"SKU",-10}{"Description",-15}{"Price",10}{"PST",-10}{"EXT",15}");
			WriteLine(new String('-', 60));
		}
	}
}
