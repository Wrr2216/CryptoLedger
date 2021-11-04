using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CryptoLedger
{
    class DataHelper
    {
        private string _database = @"Data Source=C:\Users\Logan\Documents\Databases\assets.db";

        // Serialize Object
        // Use Asset asset = JsonConvert.DeserializeObject<Asset>(data) to deserialize
        public void addAsset(string dTicker, decimal dAmount, decimal dInvested, string dWallet, bool disStaked)
        {
            using var _connection = new SQLiteConnection(_database);
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
            Console.WriteLine("Added asset");
        }

        public List<Asset> getAssets()
        {
            List<Asset> _constructedList = new List<Asset>();
            using var _connection = new SQLiteConnection(_database);
            _connection.Open();

            string query = "SELECT * FROM assets";

            using var cmd = new SQLiteCommand(query, _connection);
            using SQLiteDataReader _reader = cmd.ExecuteReader();

            while (_reader.Read())
            {
                _constructedList.Add(new Asset()
                {
                    Ticker = _reader.GetString(1),
                    Amount = _reader.GetInt32(2),
                    Invested = _reader.GetDecimal(3),
                    Wallet = _reader.GetString(4),
                    isStaked = _reader.GetBoolean(5)
                });
                Console.WriteLine("Added Asset" + _reader.GetString(1));
            }

            return _constructedList;
        }

        private bool fileCheck(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
                Console.WriteLine("File path not found; Creating...");
                return true;
            }
            else if (File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}


/*
 string Ticker
int Amount
int Invested
string Wallet
bool isStaked
 */