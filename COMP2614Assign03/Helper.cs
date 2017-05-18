using System;
using System.Collections.Generic;
using System.Text;
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

		/// <summary>
		/// Wrapper around private method for fetching Data
		/// </summary>
		/// <param name="args">Command Line input</param>
		/// <returns>Generic Collection of InvoiceHeaders</returns>
		public static IList<InvoiceHeader> GetInvoices(string args)
		{
			return GetData(args);
		}
		/// <summary>
		/// Processing Invoice Headers and calling method to generate Invoice Line per each Invoice Header
		/// </summary>
		/// <param name="data">Date from a source file</param>
		public static void PrintInvoiceHeader(IList<InvoiceHeader> data)
		{
			PrintTitle();
			ForegroundColor = ConsoleColor.Blue;
			PrintDottedLine();
			WriteLine("Invoice Listing");
			PrintDottedLine();
			ForegroundColor = ConsoleColor.Gray;
			WriteLine();

			foreach (var header in data)
			{
				WriteLine($"{"Invoice Number:",-18}{header.InvoiceNumber,-10}");
				WriteLine($"{"Invoice Date:",-18}{header.InvoiceDate.ToString("MMM dd, yyyy"),-10}");
				WriteLine($"{"Discount Date:",-18}{(GetDiscountDate(header.InvoiceDate, header.Term).ToString("MMM dd, yyyy")),-10}");
				WriteLine($"{"Terms:",-18}{GetTerm(header.ParseTerm())}");

				PrintInvoiceLineHeader(header.InvoiceLines);
				PrintDottedLine();
				PrintCalculations(header, header.InvoiceLines);
				WriteLine();WriteLine();
			}
		}
		/// <summary>
		/// Helper method for Error handling
		/// </summary>
		/// <param name="message">String with custom message</param>
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

		private static string GetFileFromConfig()
		{
			var fileFolder = ConfigurationManager.AppSettings["filePath"];
			string filter = "*.txt";
			return Directory.GetFiles(fileFolder, filter)[0];
		}
		private static List<InvoiceLine> GetLineDetails(string[] line)
		{
			List<InvoiceLine> iLines = new List<InvoiceLine>();
			for (int i = 1; i < line.Length; i++)
			{
				var lineDetails = line[i].Split(':');

				iLines.Add(new InvoiceLine
				{
					Quantity = int.Parse(lineDetails[0]), //could use bool temp = int.TryParse(value, out myValue);
					SKU = lineDetails[1],
					Description = lineDetails[2],
					Price = Decimal.Parse(lineDetails[3]), //could use bool temp = int.TryParse(value, out myValue);
					IsTaxable = IsTrue(lineDetails[4])
				});
			}

			return iLines;
		}
		private static InvoiceHeader GetHeader(string fileHeader, IList<InvoiceLine> lines)
		{
			var header = fileHeader.Split(':');
			var invHeader = new InvoiceHeader
			{
				InvoiceNumber = int.Parse(header[0]),
				InvoiceDate = DateTime.Parse(header[1]),
				Term = int.Parse(header[2]),
				InvoiceLines = (List<InvoiceLine>)lines
			};

			return invHeader;
		}
		/// <summary>
		/// Boolean "extension" method, incase we get variance of "True" we will check it
		/// </summary>
		/// <param name="value">takes whatever comes in the input file</param>
		/// <returns>value boolean True or Falses</returns>
		private static IList<InvoiceHeader> GetData(string sourceFile)
		{
			List<InvoiceHeader> iHeaders = new List<InvoiceHeader>();

			using (StreamReader sr = new StreamReader(sourceFile))
			{
				while (sr.Peek() > 0)
				{
					var line = sr.ReadLine().Split('|');
					var lineDetails = GetLineDetails(line);
					var header = GetHeader(line[0], lineDetails);
					iHeaders.Add(header);
				}
			}
			return iHeaders;
		}
		private static void PrintInvoiceLineHeader(IList<InvoiceLine> invLines)
		{
			PrintDottedLine();
			WriteLine($"{"Qty",-3}{" "}{"SKU",-14}{"Description",-20}{"Price",10}{"  "}{"PST",3}{"EXT",11}");
			PrintDottedLine();
			foreach (var line in invLines)
			{
				WriteLine($"{line.Quantity,3}{" "}{line.SKU,-14}{line.Description,-20}{line.Price,10}{" "}{(line.IsTaxable ? "Y" : "N"),3}{line.LineTotal,12:N}");
			}
		}
		private static void PrintCalculations(InvoiceHeader header, IList<InvoiceLine> lines)
		{
			decimal subtotal, gst, pst, total;
			subtotal = gst = pst = total = 0.0m;

			foreach (var line in lines)
			{
				subtotal += line.LineTotal;
				gst += GetTax(line.LineTotal, false);
				if (line.IsTaxable)
				{
					pst += GetTax(line.LineTotal, true);
				}
			}
			WriteLine($"{"",18}{"Subtotal:"}{subtotal,37:N}");
			WriteLine($"{"",18}{"GST:"}{gst,42:N2}");
			if (pst > 0)
			{
				WriteLine($"{"",18}{"PST:"}{pst,42:N2}");
			}

			PrintDottedLine();
			total = subtotal + gst + pst;
			WriteLine($"{"",18}{"Total:"}{total,40:N2}");
			WriteLine();
			WriteLine($"{"",18}{"Discount:"}{GetDiscount(total, header.ParseTerm()[0]),37:N2}");
		}
		private static decimal GetTax(decimal price, bool isPst)
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
		private static decimal GetDiscount(decimal total, int discountPercentage)
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
			var num = 64;
			WriteLine(new String('-', num));
		}
		private static void PrintTitle()
		{
			Title = $"Welcome, {Environment.UserName}. Assignment 3 - Printing Invoices from dedicated source.";
		}

		#endregion Private Methods
	}
}
