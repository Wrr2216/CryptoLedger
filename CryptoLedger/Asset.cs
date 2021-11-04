using System.Collections.Generic;

namespace CryptoLedger
{
    public class Asset
    {
        DBHelper _assetDB = new DBHelper();
        public string Ticker { get; set; }
        public decimal Amount { get; set; }
        public decimal Invested { get; set; }
        public string Wallet { get; set; }
        public string isStaked { get; set; }

        public void addAsset(string dTicker, decimal dAmount, decimal dInvested, string dWallet, string dIsStaked)
        {
            _assetDB.addAsset(dTicker, dAmount, dInvested, dWallet, dIsStaked);
        }
        public Asset getAsset(string dTicker)
        {
            return _assetDB.getAsset(dTicker);
        }

        public List<Asset> getAllAssets()
        {
            return _assetDB.getAssets();
        }

        public void removeAsset(string dTicker)
        {
            _assetDB.remAsset(dTicker);
        }
    }
}