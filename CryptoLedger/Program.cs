using System;
using System.Collections.Generic;
using System.IO;
using Spectre.Console;
using System.Threading;

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
            DBHelper db = new DBHelper();

            AnsiConsole.Status()
                .Start("Initializing Database...", ctx =>
                {
                    db.initializeDatabase();
                    Thread.Sleep(1500);

                    ctx.Status("Updating Database values...");
                });

            Console.Clear();

            var _menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select Menu[/]")
                    .PageSize(7)
                    .MoreChoicesText("[grey](Move up and down to select option)[/]")
                    .AddChoices(new[] {
                            "Portfolio",
                            "Lookup Ticker",
                            "Add Ticker",
                            "Remove Ticker",
                            "[red]Exit Application[/]"
                        })
                );

            switch ($"{_menu}")
            {
                case "Portfolio":
                    return p.ListAssets();
                case "Lookup Ticker":
                    return p.listAsset();
                case "Add Ticker":
                    return p.addAsset();
                case "Remove Ticker":
                    return p.remAsset();
                case "[red]Exit Application[/]":
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

            Asset _asset = new Asset();
            _asset.addAsset(
                ticker,
                amount,
                invested,
                wallet,
                staked
            );

            return true;
        }
        private bool remAsset()
        {
            ConsoleHelper ch = new ConsoleHelper();
            Console.Clear();

            string _ticker;
            ch.Log("Enter the Ticker for the asset to be removed: ", "w");
            _ticker = Console.ReadLine();
            ch.Log(String.Format("Re-enter the Ticker ({0}): ", _ticker), "wl");
            if (Console.ReadLine().ToLower() == _ticker.ToLower())
            {
                Asset _asset = new Asset();
                _asset.removeAsset(_ticker);
                return true;
            }
            else
            {
                ch.LogClear("Try again. Values did not match", "wl");
                return true;
            }
        }
        private bool listAsset()
        {
            ConsoleHelper ch = new ConsoleHelper();
            Console.Clear();

            ch.Log("Lookup Asset: ", "wl");

            Asset _asset = new Asset().getAsset(Console.ReadLine());
            var _table = new Table();
            _table.AddColumns("Asset", "Amount", "Invested", "Wallet", "Staked", "Value");
            _table.AddRow(
                            String.Format("[blue]{0}[/]", _asset.Ticker),
                            String.Format("{0}", _asset.Amount),
                            String.Format("[green]{0}[/]", _asset.Invested),
                            String.Format("{0}", _asset.Wallet),
                            String.Format("{0}", _asset.isStaked),
                            String.Format("[green]{0}[/]", (_asset.marketVal * _asset.Amount))
                        );

            return tinyMenu();
        }
        private bool ListAssets()
        {
            ConsoleHelper ch = new ConsoleHelper();
            Console.Clear();

            List<Asset> _retData;

            var _table = new Table();
            _table.AddColumns("Asset", "Amount", "Invested", "Wallet", "Staked", "Value");

            AnsiConsole.Status()
                .Start("Loading Portfolio...", ctx =>
                {
                    _retData = new Asset().getAllAssets();
                    Thread.Sleep(1000);

                    foreach (Asset _asset in _retData)
                    {

                        //asset.Ticker, _asset.Amount, _asset.Invested, _asset.Wallet, _asset.isStaked
                        _table.AddRow(
                            String.Format("[blue]{0}[/]", _asset.Ticker),
                            String.Format("{0}", _asset.Amount),
                            String.Format("[green]{0}[/]", _asset.Invested),
                            String.Format("{0}", _asset.Wallet),
                            String.Format("{0}", _asset.isStaked),
                            String.Format("[green]{0}[/]", (_asset.marketVal * _asset.Amount))
                        );
                    }
                });

            AnsiConsole.Write(_table);

            return tinyMenu();
        }
        private bool tinyMenu()
        {
            Program p = new Program();

            var _menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select Menu[/]")
                    .PageSize(7)
                    .MoreChoicesText("[grey](Move up and down to select option)[/]")
                    .AddChoices(new[] {
                            "Main Menu",
                            "Exit Application"
                    })
                );

            switch ($"{_menu}")
            {
                case "Main Menu":
                    return true;
                case "Exit Application":
                    return false;
                default:
                    return true;
            }
        }

    }
}
