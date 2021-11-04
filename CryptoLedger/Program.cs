using System;
using ConsoleTables;
using System.Globalization;
using System.Collections.Generic;

/* CREDITS 
 * CSharp Examples - https://www.csharp-console-examples.com/general/reading-excel-file-in-c-console-application/
 * 
 */

// TODO:
/*
 * 1). Fix Read/Write to SQLite Database
 * 2). Modify SQlite DB connections to allow Async connections
 * 3). LOW PRIOR: Add Coinmarketcap API interaction.
 * 
 */

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

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Choose an option:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1) Load Asset List");
            Console.WriteLine("2) Add Asset");
            Console.WriteLine("3) Remove Asset");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("4) Exit");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\r\nSelect an option>\\: ");

            switch (Console.ReadLine())
            {
                case "1":
                    return p.listAssets();
                case "2":
                    Console.WriteLine("Add");
                    return p.addAsset();
                case "3":
                    Console.WriteLine("Remove");
                    return true;
                case "4":
                    Console.WriteLine("Exit");
                    return false;
                default:
                    return true;
            }
        }

        private bool addAsset()
        {
            Console.Clear();
            Console.Write("Ticker? : ");
            string ticker = Console.ReadLine();
            Console.Clear();
            Console.Write("Amount?: ");
            decimal amount = Convert.ToDecimal(Console.ReadLine());
            Console.Clear();
            Console.Write("Invested?: ");
            decimal invested = Convert.ToDecimal(Console.ReadLine());
            Console.Clear();
            Console.Write("Wallet?: ");
            string wallet = Console.ReadLine();
            Console.Clear();
            Console.Write("Is it staked? (y/n): ");
            bool staked;
            if (Console.ReadLine() == "n")
            {
                staked = false;
            }
            else if (Console.ReadLine() == "y")
            {
                staked = true;
            }
            else
            {
                staked = false;
            }

            DataHelper data = new DataHelper();
            data.addAsset(ticker, amount, invested, wallet, staked);

            return true;
        }

        private bool listAssets()
        {
            DataHelper data = new DataHelper();
            Console.Clear();

            //Construct table
            var assetTable = new ConsoleTable("Asset", "Amount", "Invested", "Wallet", "Staked");

            //Retrive Json data from data file to populate the table.

            data.getAssets();
            
            /*foreach (Asset item in asset)
            {
                assetTable.AddRow(item.Ticker, item.Amount, item.Invested, item.Wallet, item.isStaked);
            }
            assetTable.Write();
            */
            Console.WriteLine();
            Console.Write("Type exit to return to the Main Menu: ");
            if (Console.ReadLine() == "exit")
                return true;
            return true;
        }

        private bool testConsole()
        {
            Console.Clear();
            var table = new ConsoleTable("Asset", "Amount", "Invested", "Wallet", "Staked");
            table.AddRow("BTC", "1", "10000", "CoinBase", "No");
            table.AddRow("BTC", "1", "10000", "CoinBase", "No");
            table.AddRow("BTC", "1", "10000", "CoinBase", "No");
            table.Write();

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Type exit to return to the Main Menu: ");
            if (Console.ReadLine() == "exit")
                return true;
            return true;
        }



    }
}
