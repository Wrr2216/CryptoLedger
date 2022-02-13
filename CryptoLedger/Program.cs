namespace CryptoLedger
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Spectre.Console;

    internal class Program
    {
        public static bool HasInit { get; set; } = false;

        private static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var doMenu = true;
            while (doMenu)
            {
                doMenu = Menu();
            }
        }

        private static bool Menu()
        {
            var p = new Program();
            var ch = new ConsoleHelper();
            var db = new DBHelper();

            if (!HasInit)
            {
                //var inp = AnsiConsole.Ask<string>("Perform Database Update? (y/n) ");


                AnsiConsole.Status()
                    .Start("Initializing... ", ctx =>
                    {
                        db.InitializeDatabase();
                        db.UpdateDbMarket();

                        /* if (inp.ToLower() == "y")
                         {
                             db.UpdateDbMarket();
                             _ = ctx.Status("Updating Database values...");
                         }*/
                    });
            }

            Console.Clear();
            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select Menu[/]")
                    .PageSize(7)
                    .MoreChoicesText("[grey](Move up and down to select option)[/]")
                    .AddChoices("Portfolio", "Lookup Ticker", "Add Ticker", "Remove Ticker", "Update Ticker",
                        "Export Data", "[red]Exit Application[/]")
            );

            return $"{menu}" switch
            {
                "Portfolio" => p.ListAssets(),
                "Lookup Ticker" => p.ListAsset(),
                "Add Ticker" => p.AddAsset(),
                "Remove Ticker" => p.RemAsset(),
                "Update Ticker" => p.UpdateAsset(),
                "Export Data" => p.exportData(),
                "[red]Exit Application[/]" => false,
                _ => true
            };
        }

        private bool AddAsset()
        {
            var ch = new ConsoleHelper();

            ch.LogClear("Ticker?: ", "w");
            var ticker = Console.ReadLine();

            // Check exists ticker
            // if (exists(ticker))
            // {
            //    ch.LogClear("Already Exists", "w");
            //     return true;
            // }

            ch.LogClear("Amount?: ", "w");
            var amount = Convert.ToDecimal(Console.ReadLine());
            ch.LogClear("Invested?: ", "w");
            var invested = Convert.ToDecimal(Console.ReadLine());
            ch.LogClear("Wallet?: ", "w");
            var wallet = Console.ReadLine();
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

            var asset = new Asset();
            asset.addAsset(
                ticker,
                amount,
                invested,
                wallet,
                staked
            );

            return true;
        }

        private bool RemAsset()
        {
            var ch = new ConsoleHelper();
            Console.Clear();

            string ticker;
            ch.Log("Enter the Ticker for the asset to be removed: ", "w");
            ticker = Console.ReadLine();
            ch.Log(string.Format("Re-enter the Ticker ({0}): ", ticker), "wl");
            if (Console.ReadLine().ToLower() == ticker.ToLower())
            {
                var _asset = new Asset();
                _asset.removeAsset(ticker);
                return true;
            }

            ch.LogClear("Try again. Values did not match", "wl");
            return true;
        }

        private bool UpdateAsset()
        {
            var ch = new ConsoleHelper();
            Console.Clear();

            var _tempAssetList = new Asset().getAllAssets();
            var _initTable = new Table();
            _initTable.AddColumns("Asset");

            foreach (var _asset in _tempAssetList)
            {
                _initTable.AddRow(
                    string.Format("[blue]{0}[/]", _asset.Ticker)
                );
            }

            AnsiConsole.Write(_initTable);

            var _ticker = AnsiConsole.Ask<string>("Enter the ticker to modify: ");
            var _tempAsset = new Asset().getAsset(_ticker.ToUpper());
            var _table = new Table();
            _table.AddColumns("Asset", "Amount", "Invested", "Wallet", "Staked", "Value");
            _table.AddRow(
                string.Format("[red](1)[/] [blue]{0}[/]", _tempAsset.Ticker),
                string.Format("[red](2)[/] {0}", _tempAsset.Amount),
                string.Format("[red](3)[/] [green]{0}[/]", _tempAsset.Invested),
                string.Format("[red](4)[/] {0}", _tempAsset.Wallet),
                string.Format("[red](5)[/] {0}", _tempAsset.isStaked),
                string.Format("[red](6)[/] [green]{0}[/]", _tempAsset.marketVal * _tempAsset.Amount)
            );
            _table.AddRow("(7) EXIT", "", "", "", "", "");

            AnsiConsole.Write(_table);

            var _sel = AnsiConsole.Ask<int>("Select option to modify [red](1-6)[/]: ");

            switch (_sel)
            {
                case 1:
                    var _amt1 = AnsiConsole.Ask<string>("Enter new value: ");
                    _tempAsset.updateTicker(_tempAsset.Ticker, _amt1);
                    return this.tinyMenu();
                case 2:
                    var _amt2 = AnsiConsole.Ask<double>("Enter new value: ");
                    _tempAsset.updateAmount(_tempAsset.Ticker, _amt2);
                    return this.tinyMenu();
                case 3:
                    var _amt3 = AnsiConsole.Ask<double>("Enter new value: ");
                    _tempAsset.updateInvested(_tempAsset.Ticker, _amt3);
                    return this.tinyMenu();
                case 4:
                    var _amt4 = AnsiConsole.Ask<string>("Enter new value: ");
                    _tempAsset.updateWallet(_tempAsset.Ticker, _amt4);
                    return this.tinyMenu();
                case 5:
                    var _amt5 = AnsiConsole.Ask<string>("Enter new value: ");
                    _tempAsset.updateStaked(_tempAsset.Ticker, _amt5);
                    return this.tinyMenu();
                case 6:
                    var _amt6 = AnsiConsole.Ask<double>("Enter new value: ");
                    _tempAsset.updateMarketVal(_tempAsset.Ticker, _amt6);
                    return this.tinyMenu();
                case 7:
                    return false;
                default:
                    return false;
            }
        }

        

        private bool ListAsset()
        {
            var ch = new ConsoleHelper();
            Console.Clear();

            ch.Log("Lookup Asset: ", "wl");

            var asset = new Asset().getAsset(Console.ReadLine());
            var table = new Table();
            table.AddColumns("Asset", "Amount", "Invested", "Wallet", "Staked", "Value");
            table.AddRow(
                string.Format("[blue]{0}[/]", asset.Ticker),
                string.Format("{0}", asset.Amount),
                string.Format("[green]{0}[/]", asset.Invested),
                string.Format("{0}", asset.Wallet),
                string.Format("{0}", asset.isStaked),
                string.Format("[green]{0}[/]", asset.marketVal * asset.Amount)
            );

            return this.tinyMenu();
        }

        private bool ListAssets()
        {
            var ch = new ConsoleHelper();
            Console.Clear();

            double totalInvest = 0;
            double totalValue = 0;
            double totalProfit = 0;

            List<Asset> retData;
            var items = new List<(string Label, double Value)>();

            var table = new Table();
            table.AddColumns("Asset", "Amount", "Invested", "Wallet", "Staked", "Value", "P/L");

            AnsiConsole.Status()
                .Start("Loading Portfolio...", ctx =>
                {
                    retData = new Asset().getAllAssets();


                    var csv = new StringBuilder();
                    var valColor = "[white]";
                    foreach (var asset in retData)
                    {
                        var csvAmt = asset.Amount;
                        var csvTicker = asset.Ticker;
                        var csvInvested = asset.Invested;
                        var csvWallet = asset.Wallet;
                        var csvStaked = asset.isStaked;
                        var csvMarketVal = asset.marketVal * asset.Amount;

                        var csvEntry = string.Format("{0},{1},{2},{3},{4},{5}", csvTicker, csvAmt, csvInvested,
                            csvWallet, csvStaked, csvMarketVal);
                        csv.AppendLine(csvEntry);
                        if (asset.marketVal * asset.Amount < asset.Invested)
                        {
                            valColor = "[red]";
                        }
                        else if (asset.marketVal * asset.Amount >= asset.Invested)
                        {
                            valColor = "[green]";
                        }
                        else
                        {
                            valColor = "[white]";
                        }

                        var plCol = "";
                        var plAmt = (asset.marketVal * asset.Amount) - asset.Invested;

                        if (plAmt > 0)
                        {
                            plCol = "[green]";
                        }
                        else
                        {
                            plCol = "[red]";
                        }


                        //asset.Ticker, _asset.Amount, _asset.Invested, _asset.Wallet, _asset.isStaked
                        table.AddRow(
                            string.Format("[blue]{0}[/] ({1})", asset.Ticker, asset.marketVal.ToString("C3")),
                            string.Format("{0}", asset.Amount.ToString()),
                            string.Format("[green]{0}[/]", asset.Invested.ToString("C3")),
                            string.Format("{0}", asset.Wallet),
                            string.Format("{0}", asset.isStaked),
                            string.Format("{0}{1}[/]", valColor, (asset.marketVal * asset.Amount).ToString("C3")),
                            string.Format("{0}${1}[/]", plCol, plAmt.ToString("C3"))
                        );
                        totalInvest = totalInvest + Convert.ToDouble(asset.Invested);
                        totalValue = totalValue + Convert.ToDouble(asset.marketVal * asset.Amount);
                        totalProfit = totalValue - totalInvest;
                    }

                    // Get all time stats
                    this.WriteToAllTimeCsv();
                    //Save to CSV here
                    this.WriteToCsv(csv.ToString(), false);

                    if (totalValue > totalInvest)
                    {
                        _ = table.AddRow("TOTALS", "", string.Format("[green]{0}[/]", totalInvest.ToString("C3")), "",
                            "", string.Format("[green]{0}[/]", totalValue.ToString("")),
                            string.Format("PROFIT: {0}", totalProfit.ToString("")));
                    }
                    else
                    {
                        _ = table.AddRow("TOTALS", "", string.Format("[red]{0}[/]", totalInvest.ToString("C3")), "", "",
                            string.Format("[red]{0}[/]", totalValue.ToString()),
                            string.Format("PROFIT: {0}", totalProfit.ToString()));
                    }
                });


            table.Border = TableBorder.Heavy;
            table.Centered();
            table.BorderColor(Color.Red);

            AnsiConsole.Write(table);

            return this.tinyMenu();
        }

        private void WriteToCsv(string data, bool bypass)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException($"'{nameof(data)}' cannot be null or empty.", nameof(data));
            }

            var dateNow = DateTime.Now.ToString("MM-dd-yyyy--HH");
            var tempPath = string.Format(@".\Data\data_{0}.csv", dateNow);

            if (!File.Exists(tempPath) || bypass)
            {
                File.WriteAllText(tempPath, data);
            }
        }

        private void WriteToAllTimeCsv()
        {
            var dateNow = DateTime.Now.ToString("MM-dd-yyyy");
            var tempPath = @".\Data\overall.csv";

            double csvtotalInvest = 0;
            double csvtotalValue = 0;
            List<Asset> retData;
            retData = new Asset().getAllAssets();

            foreach (var _asset in retData)
            {
                csvtotalInvest = csvtotalInvest + Convert.ToDouble(_asset.Invested);
                csvtotalValue = csvtotalValue + Convert.ToDouble(_asset.marketVal * _asset.Amount);
            }

            var _modifiedData = string.Format("{0},{1},{2},{3}", dateNow, csvtotalInvest.ToString(),
                csvtotalValue.ToString(), csvtotalValue - csvtotalInvest);

            if (!File.Exists(tempPath))
            {
                File.Create(tempPath);
            }

            if (!File.ReadAllText(tempPath).Contains(dateNow))
            {
                File.AppendAllText(tempPath, _modifiedData);
            }
        }

        private bool exportData()
        {
            double _csvtotalInvest = 0;
            double _csvtotalValue = 0;
            List<Asset> _retData;
            _retData = new Asset().getAllAssets();

            var csv = new StringBuilder();
            var csvHeader = "Ticker,Amount,Invested,Wallet,Staked,Market Value";

            csv.AppendLine(csvHeader);

            foreach (var _asset in _retData)
            {
                var csvAmt = _asset.Amount;
                var csvTicker = _asset.Ticker;
                var csvInvested = _asset.Invested;
                var csvWallet = _asset.Wallet;
                var csvStaked = _asset.isStaked;
                var csvMarketVal = _asset.marketVal * _asset.Amount;
                _csvtotalInvest = _csvtotalInvest + Convert.ToDouble(_asset.Invested);
                _csvtotalValue = _csvtotalValue + Convert.ToDouble(_asset.marketVal * _asset.Amount);

                var csvTotalInv = string.Format("Total Invested: {0}", _csvtotalInvest);
                var csvTotalVal = string.Format("Total Value: {0}", _csvtotalValue);
                var csvEntry = string.Format("{0},{1},{2},{3},{4},{5}", csvTicker, csvAmt, csvInvested, csvWallet,
                    csvStaked, csvMarketVal);


                csv.AppendLine(csvEntry);
            }

            csv.AppendLine("Total Invested: " + _csvtotalInvest);
            csv.AppendLine("Total Value: " + _csvtotalValue);

            //Save to CSV here
            this.WriteToCsv(csv.ToString(), true);

            return true;
        }

        private bool tinyMenu()
        {
            var p = new Program();

            var _menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select Menu[/]")
                    .PageSize(7)
                    .MoreChoicesText("[grey](Move up and down to select option)[/]")
                    .AddChoices("Main Menu", "Exit Application")
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
