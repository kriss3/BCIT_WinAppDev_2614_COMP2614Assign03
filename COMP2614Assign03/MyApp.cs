using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			Helper.Greetings();
			Helper.PrintInvoiceHeader();
			Helper.PrintInvoiceLineHeader();
		}
	}
}
