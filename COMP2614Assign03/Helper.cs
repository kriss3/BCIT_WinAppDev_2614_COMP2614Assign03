using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using static System.Console;

namespace COMP2614Assign03
{
	/// <summary>
	/// Utility class, all methods static;
	/// </summary>
	class Helper
	{
		public const int GST = 5;
		public const int PST = 7;


		#region Public Methods
		public static IEnumerable<InvoiceHeader> GetInvoices()
		{
			var file = GetFile();
			return GetData(file); ;
		}
		public static void PrintInvoiceHeader(IEnumerable<InvoiceHeader> data)
		{
			PrintDottedLine();
			WriteLine("Invoice Listing");
			PrintDottedLine();
			WriteLine();

			foreach (var header in data)
			{
				WriteLine($"{"Invoice Number:",-20}{header.InvoiceNumber,-15}");
				WriteLine($"{"Invoice Date:",-20}{header.InvoiceDate.ToString("MMM dd, yyyy"),-15}");
				WriteLine($"{"Discount Date:",-20}{(GetDiscountDate(header.InvoiceDate, header.Term).ToString("MMM dd, yyyy")),-15}");
				WriteLine($"{"Terms:",-20}{GetTerm(header.ParseTerm())}");

				PrintInvoiceLineHeader(header.InvoiceLines);
				PrintDottedLine();
				PrintCalculations(header, header.InvoiceLines);
				WriteLine();WriteLine();
			}

		}
		public static void PrintInvoiceLineHeader(IEnumerable<InvoiceLine> invLines)
		{
			PrintDottedLine();
			WriteLine($"{"Qty",-3}{" "}{"SKU",-10}{"Description",-20}{"Price",10}{"  "}{"PST",3}{"EXT",9}");
			PrintDottedLine();
			foreach (var line in invLines)
			{
				WriteLine($"{line.Quantity,3}{" "}{line.SKU,-10}{line.Description,-20}{line.Price,10}{" "}{(line.Taxable ? "Y" : "N"),3}{line.Quantity * line.Price,10:N}");
			}
		}
		public static void PrintCalculations(InvoiceHeader header, List<InvoiceLine> lines)
		{
			double subtotal = 0.0;
			double gst = 0.0;
			double pst = 0.0;
			double total = 0.0;

			foreach (var line in lines)
			{
				subtotal += ((double)line.Price * line.Quantity);
				gst += GetTax(((double)line.Price * line.Quantity),false);
				if (line.Taxable)
				{
					pst += GetTax(((double)line.Price * line.Quantity),true);
				}
			}
			WriteLine($"{"",14}{"Subtotal:"}{subtotal,35:N}");
			WriteLine($"{"",14}{"GST:"}{gst,40:N2}");
			if (pst > 0)
			{
				WriteLine($"{"",14}{"PST:"}{pst,40:N2}");
			}

			PrintDottedLine();
			total = subtotal + gst + pst;
			WriteLine($"{"",14}{"Total:"}{total,38:N2}");
			WriteLine();
			WriteLine($"{"",14}{"Discount:"}{GetDiscount(total, header.ParseTerm()[0]),35:N2}");
		}
		public static void PrintErrorMessage(string message)
		{
			Clear();
			ForegroundColor = ConsoleColor.Red;
			PrintDottedLine();
			WriteLine(message);
			PrintDottedLine();
			ForegroundColor = ConsoleColor.Gray;
		}

		#endregion Public Methods

		#region Private Methods

		private static string GetFile()
		{
			var fileFolder = ConfigurationManager.AppSettings["filePath"];
			string filter = "*.txt";
			return Directory.GetFiles(fileFolder, filter)[0];
		}
		private static List<InvoiceLine> GetLineDetails(string[] line, Guid guid)
		{
			List<InvoiceLine> iLines = new List<InvoiceLine>();
			for (int i = 1; i < line.Length; i++)
			{
				var lineDetails = line[i].Split(':');

				iLines.Add(new InvoiceLine
				{
					Quantity = int.Parse(lineDetails[0]),
					SKU = lineDetails[1],
					Description = lineDetails[2],
					Price = Decimal.Parse(lineDetails[3]),
					Taxable = IsTrue(lineDetails[4]),
					InvoiceId = guid
				});
			}

			return iLines;
		}
		private static InvoiceHeader GetHeader(string fileHeader, List<InvoiceLine> lines, Guid guid)
		{
			var header = fileHeader.Split(':');
			var invHeader = new InvoiceHeader
			{
				InvoiceNumber = int.Parse(header[0]),
				InvoiceDate = DateTime.Parse(header[1]),
				Term = int.Parse(header[2]),
				InvoiceLines = lines,
				InvoiceId = guid,
			};

			return invHeader;
		}
		/// <summary>
		/// Boolean "extension" method, incase we get variance of "True" we will check it
		/// </summary>
		/// <param name="value">takes whatever comes in the input file</param>
		/// <returns>value boolean True or Falses</returns>
		private static IEnumerable<InvoiceHeader> GetData(string sourceFile)
		{
			List<InvoiceHeader> iHeaders = new List<InvoiceHeader>();

			using (StreamReader sr = new StreamReader(sourceFile))
			{
				while (sr.Peek() > 0)
				{
					var line = sr.ReadLine().Split('|');
					Guid guid = Guid.NewGuid();
					var lineDetails = GetLineDetails(line, guid);
					var header = GetHeader(line[0], lineDetails, guid);
					iHeaders.Add(header);
				}
			}
			return iHeaders;
		}
		private static double GetTax(double price, bool isPst)
		{
			if (isPst)
			{
				return (price * PST) / 100;
			}
			else
			{
				return (price * GST) / 100;
			}
		}
		private static double GetDiscount(double total, double discountPercentage)
		{
			return (total * discountPercentage) / 100;
		}
		private static string GetTerm(int[] term)
		{
			StringBuilder sb = new StringBuilder();
			var discountPerentage = String.Format("{0:0.00}% ", term[0]);
			sb.Append(discountPerentage);
			var days = string.Format("{0} days ", term[1]);
			sb.Append(days);
			sb.Append("ADI");

			return sb.ToString();
		}
		private static DateTime GetDiscountDate(DateTime invoiceDate, int term)
		{
			return invoiceDate.AddDays((double)term);
		}
		private static bool IsTrue(string value)
		{
			try
			{
				if (value == null)
				{
					return false;
				}

				value = value.Trim();
				value = value.ToLower();

				if (value == "true")
				{
					return true;
				}

				if (value == "t")
				{
					return true;
				}

				if (value == "1")
				{
					return true;
				}

				// 7
				// Check for word yes
				if (value == "yes")
				{
					return true;
				}

				if (value == "y")
				{
					return true;
				}

				return false;
			}
			catch
			{
				return false;
			}
		}
		private static void PrintDottedLine()
		{
			var num = 58;
			WriteLine(new String('-', num));
		}
		
		#endregion Private Methods
	}
}
