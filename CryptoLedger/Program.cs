using System;
using Newtonsoft.Json;
using ConsoleTables;

/* CREDITS 
 * CSharp Examples - https://www.csharp-console-examples.com/general/reading-excel-file-in-c-console-application/
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
                    p.testConsole();
                    return false;
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
            float amount = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.Write("Invested?: ");
            int invested = Convert.ToInt32(Console.ReadLine());
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

        private void testConsole()
        {
            var table = new ConsoleTable("Asset", "Amount", "Invested", "Wallet", "Staked");
            table.AddRow("BTC", "1", "10000", "CoinBase", "No");
            table.AddRow("BTC", "1", "10000", "CoinBase", "No");
            table.AddRow("BTC", "1", "10000", "CoinBase", "No");
            table.Write();
        }



    }
}
