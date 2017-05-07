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
		public static void PrintInvoiceHeader(IEnumerable<InvoiceHeader> data)
		{
			WriteLine("Invoice Listing");
			GetDottedLine();
			WriteLine(); WriteLine();

			foreach (var header in data)
			{
				WriteLine($"{"Invoice Number:",-20}{header.InvoiceNumber,-15}");
				WriteLine($"{"Invoice Date:",-20}{header.InvoiceDate.ToString("MMM dd, yyyy"),-15}");
				WriteLine($"{"Discount Date:",-20}{(GetDiscountDate(header.InvoiceDate, header.Term).ToString("MMM dd, yyyy")),-15}");
				WriteLine($"{"Terms:",-20}{GetTerm(header.Term)}");

				PrintInvoiceLineHeader(header.InvoiceLines);
				GetDottedLine();
				WriteLine(); WriteLine();
			}

		}

		public static void PrintInvoiceLineHeader(IEnumerable<InvoiceLine> invLines)
		{
			GetDottedLine();
			WriteLine($"{"Qty",-5}{" "}{"SKU",-10}{"Description",-20}{"Price",10}{" "}{"PST",5}{"EXT",10}");
			GetDottedLine();
			foreach (var line in invLines)
			{
				WriteLine($"{line.Quantity,5}{" "}{line.SKU,-10}{line.Description,-20}{line.Price,10}{(line.Taxable ? "T" : "N"),5}{"EXT",10}");
			}



		}

		public static IEnumerable<InvoiceHeader> GetInvoices()
		{
			var file = GetFile();
			return GetData(file); ;
		}
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


		#region Private Methods

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

		private static string GetTerm(int term)
		{
			int[] results = ParseTerms(term);

			StringBuilder sb = new StringBuilder();
			var discountPerentage = String.Format("{0:0.00}% ", results[0]);
			sb.Append(discountPerentage);
			var days = string.Format("{0} days ",results[1]);
			sb.Append(days);
			sb.Append("ADI");

			return sb.ToString();
		}

		private static DateTime GetDiscountDate(DateTime invoiceDate, int term)
		{
			int[] result = ParseTerms(term);
			return invoiceDate.AddDays((double)result[1]);
		}

		private static int[] ParseTerms(int term)
		{
			var intArray = term.ToString().Select(c => Convert.ToInt32(c.ToString())).ToArray();

			var results = new int[2];

			for (int i = 0; i < intArray.Length; i++)
			{
				results[0] = intArray[0];
				results[1] = int.Parse(intArray[1].ToString() + intArray[2].ToString());
			}

			return results;
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

		private static void GetDottedLine()
		{
			var num = 62;
			WriteLine(new String('-', num));
		}
		#endregion Private Methods
	}
}
