using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace COMP2614Assign03
{
	/// <summary>
	/// Main application entry;
	/// Krzysztof Szczurowski
	/// Repo location: https://github.com/kriss3/BCIT_WinAppDev_2614_COMP2614Assign03.git
	/// </summary>
	class MyApp
	{
		static void Main(string[] args)
		{
			Run();
			ReadLine();
		}

		public static void Run()
		{
			IEnumerable<InvoiceHeader> data = null;
			try
			{
				data = Helper.GetInvoices();
			}
			catch (DirectoryNotFoundException ex)
			{
				Helper.PrintErrorMessage($"Cannot find Invoice file. Please check configuration file.\n\n{ex.Message}");
			}
			catch (Exception ex)
			{
				Helper.PrintErrorMessage(ex.Message);
			}

			if (data != null)
			{
				Helper.PrintInvoiceHeader(data);
				SetCursorPosition(0,0);
			}
		}
	}
}
