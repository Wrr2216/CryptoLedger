using System;
using System.Collections.Generic;
using NoobsMuc.Coinmarketcap.Client;

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

        #region Add/Remove Asset
        public void addAsset(string dTicker, decimal dAmount, decimal dInvested, string dWallet, string dIsStaked)
        {
            if (dTicker.ToLower() == "test")
                return;
            decimal _marketVal = Convert.ToDecimal(this.getMarketValue(dTicker).Price);
            _assetDB.addAsset(dTicker, dAmount, dInvested, dWallet, dIsStaked, _marketVal);
        }

        public void removeAsset(string dTicker)
        {
            _assetDB.remAsset(dTicker);
        }
        #endregion

        #region Retrieve Assets
        public Asset getAsset(string dTicker)
        {
            return _assetDB.getAsset(dTicker);
        }

        public List<Asset> getAllAssets()
        {
            return _assetDB.getAssets();
        }
        #endregion

        #region Modify Values
        public void updateTicker(string dTicker, string _input)
        {
            _assetDB.updateTicker(dTicker, _input);
        }
        public void updateAmount(string dTicker, double _input)
        {
            _assetDB.updateAmount(dTicker, _input);
        }
        public void updateInvested(string dTicker, double _input)
        {
            _assetDB.updateInvested(dTicker, _input);
        }
        public void updateWallet(string dTicker, string _input)
        {
            _assetDB.updateWallet(dTicker, _input);
        }
        public void updateStaked(string dTicker, string _input)
        {
            _assetDB.updateStaked(dTicker, _input);
        }
        public void updateMarketVal(string dTicker, double _input)
        {
            _assetDB.updateMarketVal(dTicker, _input);
        }
        #endregion

        #region CoinMarketCap API Interactions
        public Currency getMarketValue()
        {
            ICoinmarketcapClient client = new CoinmarketcapClient("-");
            Currency currency = client.GetCurrencyBySymbol(this.Ticker);
            return currency;
        }

        public Currency getMarketValue(string dTicker)
        {
            ICoinmarketcapClient client = new CoinmarketcapClient("-");
            Currency currency = client.GetCurrencyBySymbol(dTicker);
            return currency;
        }
        #endregion
    }
}
