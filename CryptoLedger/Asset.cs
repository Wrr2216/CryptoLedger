using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CryptoLedger
{
    public class Asset
    {
        public string Ticker { get; set; }
        public decimal Amount { get; set; }
        public decimal Invested { get; set; }
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