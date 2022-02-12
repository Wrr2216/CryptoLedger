using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading;

namespace CryptoLedger
{
    class DBHelper
    {
        private string _database = @"Data Source=.\Databases\assets.db; Version=3;";

        public void saveData(string data)
        {
            File.WriteAllText("data_" + System.DateTime.Now.ToString(), data);
        }

        public void initializeDatabase()
        {
            ConsoleHelper ch = new ConsoleHelper();

            var _dir = @".\Databases";

            if (!Directory.Exists(_dir))
                Directory.CreateDirectory(_dir);

            if (!File.Exists(@".\Databases\assets.db"))
            {
                using var _connection = new SQLiteConnection(_database);
                try
                {
                    _connection.Open();

                    using var cmd = new SQLiteCommand(_connection);
                    cmd.CommandText = "create table assets(id int, ticker text, amount decimal, invested decimal, wallet text, staked text, cv decimal)";
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ch.LogErr(ex.Message, 0);
                }
                finally
                {
                    _connection.Close();
                }
            }

            Program.hasInit = true;

        }
        #region Update Values
        public void updateDBMarket()
        {
            ConsoleHelper ch = new ConsoleHelper();
            List<Asset> _listAsset = new List<Asset>();

            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();

                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "SELECT * FROM assets";
                cmd.Prepare();
                SQLiteDataReader _reader = cmd.ExecuteReader();
                while (_reader.Read())
                {
                    if (_reader.GetValue(0) != DBNull.Value)
                    {
                        Asset _tempAsset = new Asset()
                        {
                            Ticker = Convert.ToString(_reader.GetValue(0)),
                            Amount = Convert.ToDecimal(_reader.GetValue(1)),
                            Invested = Convert.ToDecimal(_reader.GetValue(2)),
                            Wallet = Convert.ToString(_reader.GetValue(3)),
                            isStaked = Convert.ToString(_reader.GetValue(4)),
                            marketVal = Convert.ToDecimal(_reader.GetValue(5))
                        };
                        _listAsset.Add(_tempAsset);
                    }
                }
            }
            catch (Exception ex)
            {
                ch.LogErr("Try 1 " + ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }

            using var _connection2 = new SQLiteConnection(_database);

            try
            {
                _connection2.Open();

                using var cmd = new SQLiteCommand(_connection2);
                foreach (Asset _entry in _listAsset)
                {
                    Console.Write(String.Format("Updated: {0}", _entry.Ticker));
                    cmd.CommandText = "UPDATE assets SET cv = @dMarketValue WHERE (ticker = @dTicker)";
                    cmd.Parameters.AddWithValue("@dMarketValue", Convert.ToDecimal(_entry.getMarketValue().Price));
                    cmd.Parameters.AddWithValue("@dTicker", _entry.Ticker.ToString());

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    Thread.Sleep(1000);
                }
                ch.LogErr("6", 0);
            }
            catch (Exception ex)
            {
                ch.LogErr("Try 2 " + ex.Message, 0);
            }
            finally
            {
                _connection2.Close();
            }
        }


        #region Modify Values
        public void updateTicker(string dTicker, string _input)
        {
            ConsoleHelper ch = new ConsoleHelper();
            Asset _asset = new Asset().getAsset(dTicker);

            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();

                using var cmd = new SQLiteCommand(_connection);
                Console.WriteLine(String.Format("Updating {0}...", _asset.Ticker));
                cmd.CommandText = "UPDATE assets SET ticker = @input WHERE (ticker = @dTicker)";
                cmd.Parameters.AddWithValue("@dTicker", _asset.Ticker.ToString());
                cmd.Parameters.AddWithValue("@input", _input);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }
        }
        public void updateAmount(string dTicker, double _input)
        {
            ConsoleHelper ch = new ConsoleHelper();
            Asset _asset = new Asset().getAsset(dTicker);

            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();

                using var cmd = new SQLiteCommand(_connection);
                Console.WriteLine(String.Format("Updating {0}...", _asset.Ticker));
                cmd.CommandText = "UPDATE assets SET amount = @dAmount WHERE (ticker = @dTicker)";
                cmd.Parameters.AddWithValue("@dTicker", _asset.Ticker.ToString());
                cmd.Parameters.AddWithValue("@dAmount", _input);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }
        }
        public void updateInvested(string dTicker, double _input)
        {
            ConsoleHelper ch = new ConsoleHelper();
            Asset _asset = new Asset().getAsset(dTicker);

            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();

                using var cmd = new SQLiteCommand(_connection);
                Console.WriteLine(String.Format("Updating {0}...", _asset.Ticker));
                cmd.CommandText = "UPDATE assets SET invested = @input WHERE (ticker = @dTicker)";
                cmd.Parameters.AddWithValue("@dTicker", _asset.Ticker.ToString());
                cmd.Parameters.AddWithValue("@input", _input);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }
        }
        public void updateWallet(string dTicker, string _input)
        {
            ConsoleHelper ch = new ConsoleHelper();
            Asset _asset = new Asset().getAsset(dTicker);

            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();

                using var cmd = new SQLiteCommand(_connection);
                Console.WriteLine(String.Format("Updating {0}...", _asset.Ticker));
                cmd.CommandText = "UPDATE assets SET wallet = @input WHERE (ticker = @dTicker)";
                cmd.Parameters.AddWithValue("@dTicker", _asset.Ticker.ToString());
                cmd.Parameters.AddWithValue("@input", _input);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }
        }
        public void updateStaked(string dTicker, string _input)
        {
            ConsoleHelper ch = new ConsoleHelper();
            Asset _asset = new Asset().getAsset(dTicker);

            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();

                using var cmd = new SQLiteCommand(_connection);
                Console.WriteLine(String.Format("Updating {0}...", _asset.Ticker));
                cmd.CommandText = "UPDATE assets SET staked = @input WHERE (ticker = @dTicker)";
                cmd.Parameters.AddWithValue("@dTicker", _asset.Ticker.ToString());
                cmd.Parameters.AddWithValue("@input", _input);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }
        }
        public void updateMarketVal(string dTicker, double _input)
        {
            ConsoleHelper ch = new ConsoleHelper();
            Asset _asset = new Asset().getAsset(dTicker);

            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();

                using var cmd = new SQLiteCommand(_connection);
                Console.WriteLine(String.Format("Updating {0}...", _asset.Ticker));
                cmd.CommandText = "UPDATE assets SET cv = @input WHERE (ticker = @dTicker)";
                cmd.Parameters.AddWithValue("@dTicker", _asset.Ticker.ToString());
                cmd.Parameters.AddWithValue("@input", _input);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }
        }
        #endregion
        #endregion

        #region Add / Remove Asset
        public void addAsset(string dTicker, decimal dAmount, decimal dInvested, string dWallet, string disStaked, decimal dCv)
        {
            ConsoleHelper ch = new ConsoleHelper();

            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();

                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "INSERT INTO assets(ticker, amount, invested, wallet, staked, cv) VALUES(@dTicker, @dAmount, @dInvested, @dWallet, @disStaked, @dCv)";

                cmd.Parameters.AddWithValue("@dTicker", dTicker);
                cmd.Parameters.AddWithValue("@dAmount", dAmount);
                cmd.Parameters.AddWithValue("@dInvested", dInvested);
                cmd.Parameters.AddWithValue("@dWallet", dWallet);
                cmd.Parameters.AddWithValue("@disStaked", disStaked);
                cmd.Parameters.AddWithValue("@dCv", dCv);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }
        }

        public void remAsset(string dTicker)
        {
            ConsoleHelper ch = new ConsoleHelper();

            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();
                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "DELETE FROM assets WHERE (ticker = @dTicker)";
                cmd.Parameters.AddWithValue("@dTicker", dTicker);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }
        }
        #endregion

        #region Retrieve Asset(s)
        public Asset getAsset(string dTicker)
        {
            ConsoleHelper ch = new ConsoleHelper();
            Asset _tempAsset = new Asset();
            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();

                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "SELECT * FROM assets WHERE (ticker = @dTicker)";
                cmd.Parameters.AddWithValue("@dTicker", dTicker);
                cmd.Prepare();
                SQLiteDataReader _reader = cmd.ExecuteReader();
                while (_reader.Read())
                {
                    _tempAsset = new Asset()
                    {
                        Ticker = Convert.ToString(_reader.GetValue(0)),
                        Amount = Convert.ToDecimal(_reader.GetValue(1)),
                        Invested = Convert.ToDecimal(_reader.GetValue(2)),
                        Wallet = Convert.ToString(_reader.GetValue(3)),
                        isStaked = Convert.ToString(_reader.GetValue(4))
                    };
                }

                return _tempAsset;
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }

            return _tempAsset;
        }

        public List<Asset> getAssets()
        {
            ConsoleHelper ch = new ConsoleHelper();
            List<Asset> _constructedList = new List<Asset>();
            using var _connection = new SQLiteConnection(_database);

            try
            {
                _connection.Open();
                string query = "SELECT * FROM assets ORDER BY invested DESC";
                using var cmd = new SQLiteCommand(query, _connection);
                using SQLiteDataReader _reader = cmd.ExecuteReader();

                while (_reader.Read())
                {
                    _constructedList.Add(new Asset()
                    {
                        Ticker = Convert.ToString(_reader.GetValue(0)),
                        Amount = Convert.ToDecimal(_reader.GetValue(1)),
                        Invested = Convert.ToDecimal(_reader.GetValue(2)),
                        Wallet = Convert.ToString(_reader.GetValue(3)),
                        isStaked = Convert.ToString(_reader.GetValue(4)),
                        marketVal = Convert.ToDecimal(_reader.GetValue(5))
                    });
                }
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally
            {
                _connection.Close();
            }

            return _constructedList;
        }
        #endregion
    }
}