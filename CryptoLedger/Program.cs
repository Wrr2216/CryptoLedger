using System;
using System.Collections.Generic;
using System.IO;
using Spectre.Console;
using System.Threading;

namespace CryptoLedger
{
    class Program
    {
        public static bool hasInit = false;

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

            if (!hasInit) {string _inp = AnsiConsole.Ask<string>("Perform Database Update? (y/n) ");


                AnsiConsole.Status()
                .Start("Initializing Database...", ctx =>
                {

                    db.initializeDatabase();


                    if (_inp.ToLower() == "y")
                    {
                        db.updateDBMarket();
                        ctx.Status("Updating Database values...");
                    }


                }); 
            }

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
                            "Update Ticker",
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
                case "Update Ticker":
                    return p.updateAsset();
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

        private bool updateAsset()
        {
            ConsoleHelper ch = new ConsoleHelper();
            Console.Clear();

            List<Asset> _tempAssetList = new Asset().getAllAssets();
            var _initTable = new Table();
            _initTable.AddColumns("Asset");

            foreach (Asset _asset in _tempAssetList)
            {
                _initTable.AddRow(
                    String.Format("[blue]{0}[/]", _asset.Ticker)
                );
            }

            AnsiConsole.Write(_initTable);

            string _ticker = AnsiConsole.Ask<string>("Enter the ticker to modify: ");
            Asset _tempAsset = new Asset().getAsset(_ticker.ToUpper());
            var _table = new Table();
            _table.AddColumns("Asset", "Amount", "Invested", "Wallet", "Staked", "Value");
            _table.AddRow(
                            String.Format("[red](1)[/] [blue]{0}[/]", _tempAsset.Ticker),
                            String.Format("[red](2)[/] {0}", _tempAsset.Amount),
                            String.Format("[red](3)[/] [green]{0}[/]", _tempAsset.Invested),
                            String.Format("[red](4)[/] {0}", _tempAsset.Wallet),
                            String.Format("[red](5)[/] {0}", _tempAsset.isStaked),
                            String.Format("[red](6)[/] [green]{0}[/]", (_tempAsset.marketVal * _tempAsset.Amount))
                        );
            _table.AddRow("(7) EXIT", "", "", "", "", "");

            AnsiConsole.Write(_table);

            int _sel = AnsiConsole.Ask<int>("Select option to modify [red](1-6)[/]: ");

            switch (_sel)
            {
                case 1:
                    string _amt1 = AnsiConsole.Ask<string>("Enter new value: ");
                    _tempAsset.updateTicker(_tempAsset.Ticker, _amt1);
                    return tinyMenu();
                case 2:
                    double _amt2 = AnsiConsole.Ask<double>("Enter new value: ");
                    _tempAsset.updateAmount(_tempAsset.Ticker, _amt2);
                    return tinyMenu();
                case 3:
                    double _amt3 = AnsiConsole.Ask<double>("Enter new value: ");
                    _tempAsset.updateInvested(_tempAsset.Ticker, _amt3);
                    return tinyMenu();
                case 4:
                    string _amt4 = AnsiConsole.Ask<string>("Enter new value: ");
                    _tempAsset.updateWallet(_tempAsset.Ticker, _amt4);
                    return tinyMenu();
                case 5:
                    string _amt5 = AnsiConsole.Ask<string>("Enter new value: ");
                    _tempAsset.updateStaked(_tempAsset.Ticker, _amt5);
                    return tinyMenu();
                case 6:
                    double _amt6 = AnsiConsole.Ask<double>("Enter new value: ");
                    _tempAsset.updateMarketVal(_tempAsset.Ticker, _amt6);
                    return tinyMenu();
                case 7:
                    return false;
                default:
                    return false;
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

            double _totalInvest = 0;
            double _totalValue = 0;

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
                        _totalInvest = _totalInvest + Convert.ToDouble(_asset.Invested);
                        _totalValue = _totalValue + Convert.ToDouble((_asset.marketVal * _asset.Amount));
                    }
                    _table.AddRow("TOTALS", "", String.Format("[green]${0}[/]", _totalInvest), "", "", String.Format("[green]${0}[/]", _totalValue));
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
