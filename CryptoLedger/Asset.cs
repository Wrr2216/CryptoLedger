using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoLedger
{
    public class Asset
    {
        public string Ticker { get; set; }
        public float Amount { get; set; }
        public int Invested { get; set; }
        public string Wallet { get; set; }
        public bool isStaked { get; set; }
    }
}


/*
 string Ticker
int Amount
int Invested
string Wallet
bool isStaked
 */