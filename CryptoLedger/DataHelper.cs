using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace CryptoLedger
{
    class DataHelper
    {
        private string _database = @"Data Source=C:\Users\Logan\Documents\Databases\assets.db";

        public void addAsset(string dTicker, decimal dAmount, decimal dInvested, string dWallet, string disStaked)
        {
            ConsoleHelper ch = new ConsoleHelper();

            using var _connection = new SQLiteConnection(_database);
            try
            {
                _connection.Open();

                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "INSERT INTO assets(ticker, amount, invested, wallet, staked) VALUES(@dTicker, @dAmount, @dInvested, @dWallet, @disStaked)";

                cmd.Parameters.AddWithValue("@dTicker", dTicker);
                cmd.Parameters.AddWithValue("@dAmount", dAmount);
                cmd.Parameters.AddWithValue("@dInvested", dInvested);
                cmd.Parameters.AddWithValue("@dWallet", dWallet);
                cmd.Parameters.AddWithValue("@disStaked", disStaked);

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

        public List<Asset> getAssets()
        {
            ConsoleHelper ch = new ConsoleHelper();
            List<Asset> _constructedList = new List<Asset>();
            using var _connection = new SQLiteConnection(_database);

            try
            {
                _connection.Open();
                string query = "SELECT * FROM assets";
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
                        isStaked = Convert.ToString(_reader.GetValue(4))
                    });
                }
            }
            catch (Exception ex)
            {
                ch.LogErr(ex.Message, 0);
            }
            finally {
                _connection.Close();
            }

            return _constructedList;
        }
    }
}