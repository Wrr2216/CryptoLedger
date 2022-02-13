using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading;

namespace CryptoLedger
{
    class DBHelper
    {
        private readonly string dbconfig = @".\database.config";
        private readonly string database = @"Data Source=.\Databases\assets.db; Version=3;";

        public void SaveData(string data) => File.WriteAllText("data_" + DateTime.Now, data);

        public void InitializeDatabase()
        {
            var ch = new ConsoleHelper();

            var dir = @".\Databases";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!File.Exists(@".\Databases\assets.db"))
            {
                using var connection = new SQLiteConnection(this.database);
                if (connection == null)
                {
                    throw new ArgumentNullException(nameof(connection));
                }

                try
                {
                    connection.Open();

                    using var cmd = new SQLiteCommand(connection);
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
                    connection.Close();
                }
            }

            Program.HasInit = true;

        }
        #region Update Values
        public void UpdateDbMarket()
        {
            var ch = new ConsoleHelper();
            var listAsset = new List<Asset>();
            if (listAsset == null)
            {
                throw new ArgumentNullException(nameof(listAsset));
            }

            using var connection = new SQLiteConnection(this.database);
            try
            {
                connection.Open();

                using var cmd = new SQLiteCommand(connection);
                cmd.CommandText = "SELECT * FROM assets";
                cmd.Prepare();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetValue(0) == DBNull.Value)
                    {
                        continue;
                    }

                    var tempAsset = new Asset()
                    {
                        Ticker = Convert.ToString(reader.GetValue(0)),
                        Amount = Convert.ToDecimal(reader.GetValue(1)),
                        Invested = Convert.ToDecimal(reader.GetValue(2)),
                        Wallet = Convert.ToString(reader.GetValue(3)),
                        isStaked = Convert.ToString(reader.GetValue(4)),
                        marketVal = Convert.ToDecimal(reader.GetValue(5))
                    };
                    listAsset.Add(tempAsset);
                }
            }
            catch (Exception ex)
            {
                ch.LogErr("Try 1 " + ex.Message, 0);
            }
            finally
            {
                connection.Close();
            }

            using var connection2 = new SQLiteConnection(this.database);

            try
            {
                connection2.Open();

                using var cmd = new SQLiteCommand(connection2);
                foreach (var entry in listAsset)
                {
                    Console.Write(string.Format("Updated: {0}", entry.Ticker));
                    cmd.CommandText = "UPDATE assets SET cv = @dMarketValue WHERE (ticker = @dTicker)";
                    cmd.Parameters.AddWithValue("@dMarketValue", Convert.ToDecimal(entry.getMarketValue().Price));
                    cmd.Parameters.AddWithValue("@dTicker", entry.Ticker.ToString());

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                ch.LogErr("6", 0);
            }
            catch (Exception ex)
            {
                ch.LogErr("Try 2 " + ex.Message, 0);
            }
            finally
            {
                connection2.Close();
            }
        }


        #region Modify Values
        public void updateTicker(string dTicker, string _input)
        {
            ConsoleHelper ch = new ConsoleHelper();
            Asset _asset = new Asset().getAsset(dTicker);

            using var _connection = new SQLiteConnection(this.database);
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

            using var _connection = new SQLiteConnection(this.database);
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

            using var _connection = new SQLiteConnection(this.database);
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

            using var _connection = new SQLiteConnection(this.database);
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

            using var _connection = new SQLiteConnection(this.database);
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

            using var _connection = new SQLiteConnection(this.database);
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

            using var _connection = new SQLiteConnection(this.database);
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

            using var _connection = new SQLiteConnection(this.database);
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
            using var _connection = new SQLiteConnection(this.database);
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
            using var _connection = new SQLiteConnection(this.database);

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
