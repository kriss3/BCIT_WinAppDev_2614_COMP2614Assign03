﻿using System;
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
		public static string input { get; private set; }
		static void Main(string[] args)
		{
			if (args.Length <= 0)
			{
				Helper.PrintErrorMessage($"Cannot find Invoice file. Please correct command line arguments.");
				return;
			}

			input = args[0];
			Run();
			ReadLine();
		}

		public static void Run()
		{
			IList<InvoiceHeader> data = null;
			try
			{
				data = Helper.GetInvoices(input);
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
