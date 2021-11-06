using NoobsMuc.Coinmarketcap.Client;
using System;
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
        public decimal marketVal { get; set; }

        public void addAsset(string dTicker, decimal dAmount, decimal dInvested, string dWallet, string dIsStaked)
        {
            decimal _marketVal = Convert.ToDecimal(this.getMarketValue(dTicker).Price);
            _assetDB.addAsset(dTicker, dAmount, dInvested, dWallet, dIsStaked, _marketVal);
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

        public Currency getMarketValue()
        {
            ICoinmarketcapClient client = new CoinmarketcapClient("[REDACTED]");
            Currency currency = client.GetCurrencyBySymbol(this.Ticker);
            return currency;
        }

        public Currency getMarketValue(string dTicker)
        {
            ICoinmarketcapClient client = new CoinmarketcapClient("[REDACTED]");
            Currency currency = client.GetCurrencyBySymbol(dTicker);
            return currency;
        }
    }
}
