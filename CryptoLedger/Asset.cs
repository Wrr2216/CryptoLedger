namespace CryptoLedger
{
    public class Asset
    {
        public string Ticker { get; set; }
        public decimal Amount { get; set; }
        public decimal Invested { get; set; }
        public string Wallet { get; set; }
        public string isStaked { get; set; }
    }
}