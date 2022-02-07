# CryptoLedger
## Small project to list crypto assets and update you on their metrics.

### Required Libraries
* Spectre.Console
* RestSharp
* Newtonsoft.Json

### Docs

The main menu as of intial commit:
![Main Menu](https://www.wirr.space/u/Q8GSAy.png)

The listed assets page:
![Assets Page](https://www.wirr.space/u/hp79so.png)

Main Features:
* Add and Remove assets on the fly.
* Query CoinMarketCap for live information on assets.
* Export your data and makes hourly backups while running.
* Stores assets in a local SQLite Database to limit I/O usage.

TO DO:
* Website integration to query the information on an online database so it can be viewed remotely.

Current Commands:
* Portfolio - Displays your portfolio.
* Lookup Ticker - Lookup info on the specific Ticker.
* Add Ticker - Add Ticker to the portfolio.
* Remove Ticker - Remove Ticker from the portfolio.
* Update Ticker - Update information on Ticker in the portfolio.
* Export Data - Exports current portfolio to a CSV file.
* Exit - Closes the Application.
