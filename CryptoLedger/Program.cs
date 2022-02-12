﻿using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

            if (!hasInit)
            {
                string _inp = AnsiConsole.Ask<string>("Perform Database Update? (y/n) ");


                AnsiConsole.Status()
                .Start("Initializing... ", ctx =>
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
                            "Export Data",
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
                case "Export Data":
                    return p.exportData();
                case "[red]Exit Application[/]":
                    return false;
                default:
                    return true;
            }
        }

        private bool exists(string tick)
        {
            Asset _tempAsset = new Asset().getAsset(tick.ToUpper());
            if (_tempAsset != null)
                return true;
            return false;
        }

        private bool addAsset()
        {
            ConsoleHelper ch = new ConsoleHelper();

            ch.LogClear("Ticker?: ", "w");
            string ticker = Console.ReadLine();

            // Check exists ticker
            // if (exists(ticker))
            // {
            //    ch.LogClear("Already Exists", "w");
            //     return true;
            // }

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
            double _totalProfit = 0;
            double _totalLoss = 0;

            List<Asset> _retData;
            List<(string Label, double Value)> items = new List<(string Label, double Value)>();

            var _table = new Table();
            _table.AddColumns("Asset", "Amount", "Invested", "Wallet", "Staked", "Value", "P/L");

            AnsiConsole.Status()
      .Start("Loading Portfolio...", ctx =>
      {
          _retData = new Asset().getAllAssets();



          var csv = new StringBuilder();
          string _valColor = "[white]";
          foreach (Asset _asset in _retData)
          {
              var csvAmt = _asset.Amount;
              var csvTicker = _asset.Ticker;
              var csvInvested = _asset.Invested;
              var csvWallet = _asset.Wallet;
              var csvStaked = _asset.isStaked;
              var csvMarketVal = (_asset.marketVal * _asset.Amount);

              var csvEntry = string.Format("{0},{1},{2},{3},{4},{5}", csvTicker, csvAmt, csvInvested, csvWallet, csvStaked, csvMarketVal);
              csv.AppendLine(csvEntry);
              if ((_asset.marketVal * _asset.Amount) < _asset.Invested)
              {
                  _valColor = "[red]";
              }
              else if ((_asset.marketVal * _asset.Amount) >= _asset.Invested)
              {
                  _valColor = "[green]";
              }
              else
              {
                  _valColor = "[white]";

              }

              string plCol = "";
              var plAmt = (_asset.marketVal * _asset.Amount) - _asset.Invested;

              if (plAmt > 0)
              { plCol = "[green]"; }
              else
              {
                  plCol = "[red]";
              }


              //asset.Ticker, _asset.Amount, _asset.Invested, _asset.Wallet, _asset.isStaked
              _table.AddRow(
                  String.Format("[blue]{0}[/] ({1})", _asset.Ticker, _asset.marketVal.ToString("C3")),
                  String.Format("{0}", _asset.Amount.ToString("C3")),
                  String.Format("[green]{0}[/]", _asset.Invested.ToString("C3")),
                  String.Format("{0}", _asset.Wallet),
                  String.Format("{0}", _asset.isStaked),
                  String.Format("{0}{1}[/]", _valColor, (_asset.marketVal * _asset.Amount).ToString("C3")),
                  String.Format("{0}${1}[/]", plCol, plAmt.ToString("C3"))
              );
              _totalInvest = _totalInvest + Convert.ToDouble(_asset.Invested);
              _totalValue = _totalValue + Convert.ToDouble((_asset.marketVal * _asset.Amount));
              _totalProfit = _totalValue - _totalInvest;

          }

          // Get all time stats
          writeToAllTimeCsv();
          //Save to CSV here
          writeToCsv(csv.ToString(), false);

          if (_totalValue > _totalInvest)
              _ = _table.AddRow("TOTALS", "", String.Format("[green]{0}[/]", _totalInvest.ToString("C3")), "", "", String.Format("[green]{0}[/]", _totalValue.ToString("C3")), String.Format("PROFIT: {0}", _totalProfit.ToString("C3")));
          else
              _ = _table.AddRow("TOTALS", "", String.Format("[red]{0}[/]", _totalInvest.ToString("C3")), "", "", String.Format("[red]{0}[/]", _totalValue.ToString("C3")), String.Format("PROFIT: {0}", _totalProfit.ToString("C3")));

      });


            _table.Border = TableBorder.Heavy;
            _table.Centered();
            _table.BorderColor(Color.Red);

            AnsiConsole.Write(_table);

            return tinyMenu();
        }

        private void writeToCsv(string _data, bool _bypass)
        {
            string dateNow = DateTime.Now.ToString("MM-dd-yyyy--HH");
            string tempPath = string.Format(@".\Data\data_{0}.csv", dateNow);

            if (!File.Exists(tempPath) || _bypass == true)
                File.WriteAllText(tempPath, _data);
        }

        private void writeToAllTimeCsv()
        {
            string dateNow = DateTime.Now.ToString("MM-dd-yyyy");
            string tempPath = string.Format(@".\Data\overall.csv");

            double _csvtotalInvest = 0;
            double _csvtotalValue = 0;
            List<Asset> _retData;
            _retData = new Asset().getAllAssets();

            foreach (Asset _asset in _retData)
            {
                _csvtotalInvest = _csvtotalInvest + Convert.ToDouble(_asset.Invested);
                _csvtotalValue = _csvtotalValue + Convert.ToDouble((_asset.marketVal * _asset.Amount));
            }

            var _modifiedData = string.Format("{0},{1},{2},{3}", dateNow, _csvtotalInvest.ToString(), _csvtotalValue.ToString(), (_csvtotalValue - _csvtotalInvest));

            if (!File.Exists(tempPath))
                File.Create(tempPath);

            if (!File.ReadAllText(tempPath).Contains(dateNow))
                File.AppendAllText(tempPath, _modifiedData);
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

            foreach (Asset _asset in _retData)
            {
                var csvAmt = _asset.Amount;
                var csvTicker = _asset.Ticker;
                var csvInvested = _asset.Invested;
                var csvWallet = _asset.Wallet;
                var csvStaked = _asset.isStaked;
                var csvMarketVal = (_asset.marketVal * _asset.Amount);
                _csvtotalInvest = _csvtotalInvest + Convert.ToDouble(_asset.Invested);
                _csvtotalValue = _csvtotalValue + Convert.ToDouble((_asset.marketVal * _asset.Amount));

                var csvTotalInv = string.Format("Total Invested: {0}", _csvtotalInvest);
                var csvTotalVal = string.Format("Total Value: {0}", _csvtotalValue);
                var csvEntry = string.Format("{0},{1},{2},{3},{4},{5}", csvTicker, csvAmt, csvInvested, csvWallet, csvStaked, csvMarketVal);


                csv.AppendLine(csvEntry);
            }

            csv.AppendLine("Total Invested: " + _csvtotalInvest.ToString());
            csv.AppendLine("Total Value: " + _csvtotalValue.ToString());

            //Save to CSV here
            writeToCsv(csv.ToString(), true);

            return true;
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
