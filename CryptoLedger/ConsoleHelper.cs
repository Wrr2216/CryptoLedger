using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoLedger
{
    class ConsoleHelper
    {
        public void LogClear(string _data, string _type)
        {
            string _parsed = String.Format(">: {0}", _data);

            switch (_type.ToLower())
            {
                case "wl":
                    Console.Clear();
                    Console.WriteLine(_parsed);
                    break;
                case "w":
                    Console.Clear();
                    Console.Write(_parsed);
                    break;
                default:
                    break;
            }
        }

        public void Log(string _data, string _type)
        {
            string _parsed = String.Format("{0}", _data);

            switch (_type.ToLower())
            {
                case "wl":
                    Console.WriteLine(_parsed);
                    break;
                case "w":
                    Console.Write(_parsed);
                    break;
                default:
                    break;
            }
        }

        public void LogErr(string _data, int _prior)
        {
            string _parsed;

            switch (_prior)
            {
                case 0:
                    _parsed = String.Format(">:{0} -> {1}", "CRITICAL", _data);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Log(_parsed, "wl");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 1:
                    _parsed = String.Format(">:{0} -> {1}", "HIGH", _data);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Log(_parsed, "wl");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 2:
                    _parsed = String.Format(">:{0} -> {1}", "LOW", _data);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Log(_parsed, "wl");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    return;
            }
            
        }
    }
}
