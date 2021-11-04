using System;
using ConsoleTables;
using System.Collections.Generic;

namespace CryptoLedger
{
    class Program
    {

        static void Main(string[] args)
        {
            bool doMenu = true;
            while (doMenu)
            {
                doMenu = Menu();
            }
        }

        private static bool Menu()
        {
            Program p = new Program();
            ConsoleHelper ch = new ConsoleHelper();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            ch.Log("Choose an option:", "wl");
            Console.ForegroundColor = ConsoleColor.Green;
            ch.Log("1) Load Asset List", "wl");
            ch.Log("2) Add Asset", "wl");
            ch.Log("3) Remove Asset", "wl");
            Console.ForegroundColor = ConsoleColor.Red;
            ch.Log("4) Exit", "wl");
            Console.ForegroundColor = ConsoleColor.White;
            ch.Log("\r\nSelect an option>\\: ", "w");

            switch (Console.ReadLine())
            {
                case "1":
                    return p.listAssets();
                case "2":
                    return p.addAsset();
                case "3":
                    return p.remAsset();
                case "4":
                    return false;
                default:
                    return true;
            }
        }

		private bool addAsset()
		{
            ConsoleHelper ch = new ConsoleHelper();

            ch.LogClear("Ticker?: ", "w");
			string ticker = Console.ReadLine();
            ch.LogClear("Amount?: ", "w");
            decimal amount = Convert.ToDecimal(Console.ReadLine());
            ch.LogClear("Invested?: ", "w");
            decimal invested = Convert.ToDecimal(Console.ReadLine());
            ch.LogClear("Wallet?: ", "w");
            string wallet = Console.ReadLine();
            ch.LogClear("Is it staked? (y/n): ", "w");
            string staked;
			if (Console.ReadLine().ToLower() == "n")
			{
				staked = "No";
			}
			else if (Console.ReadLine().ToLower() == "y")
			{
				staked = "Yes";
			}
			else
			{
				staked = "No";
			}

			DataHelper data = new DataHelper();
			data.addAsset(ticker, amount, invested, wallet, staked);

			return true;
		}

        private bool listAssets()
        {
            ConsoleHelper ch = new ConsoleHelper();
            DataHelper data = new DataHelper();
            Console.Clear();

            var assetTable = new ConsoleTable("Asset", "Amount", "Invested", "Wallet", "Staked");
            List<Asset> _retData = data.getAssets();
            
            foreach (Asset _asset in _retData)
            {
                assetTable.AddRow(_asset.Ticker, _asset.Amount, _asset.Invested, _asset.Wallet, _asset.isStaked);
            }
            assetTable.Write();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            ch.Log("Choose an option:", "wl");
            Console.ForegroundColor = ConsoleColor.Green;
            ch.Log("1) Add Asset", "wl");
            ch.Log("2) Remove Asset", "wl");
            Console.ForegroundColor = ConsoleColor.Red;
            ch.Log("3) Exit", "wl");
            Console.ForegroundColor = ConsoleColor.White;
            ch.Log("\r\nSelect an option>\\: ", "w");

            switch (Console.ReadLine())
            {
                case "1":
                    return addAsset();
                case "2":
                    return remAsset();
                case "3":
                    return true;
                default:
                    return true;
            }
       }

        private bool remAsset()
        {
            ConsoleHelper ch = new ConsoleHelper();
            DataHelper data = new DataHelper();
            Console.Clear();

            string _ticker;
            ch.Log("Enter the Ticker for the asset to be removed: ", "w");
            _ticker = Console.ReadLine();
            ch.Log(String.Format("Re-enter the Ticker ({0}): ", _ticker), "wl");
            if (Console.ReadLine().ToLower() == _ticker.ToLower())
            {
                data.remAsset(_ticker);
                return true;
            }
            else {
                ch.LogClear("Try again. Values did not match", "wl");
                return true;
            }
        }
    }
}
