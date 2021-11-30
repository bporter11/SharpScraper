# Cardboard Stonks: Price Check Web Scraper

[![Build](https://github.com/cs100/final-project-ncari002-mkulb002-bport020-pullm002/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/cs100/final-project-ncari002-mkulb002-bport020-pullm002/actions/workflows/build.yml)
[![Deploy](https://github.com/cs100/final-project-ncari002-mkulb002-bport020-pullm002/actions/workflows/deploy.yml/badge.svg?branch=master)](https://github.com/cs100/final-project-ncari002-mkulb002-bport020-pullm002/actions/workflows/deploy.yml)
[![Tests](https://github.com/cs100/final-project-ncari002-mkulb002-bport020-pullm002/actions/workflows/tests.yml/badge.svg?branch=master)](https://github.com/cs100/final-project-ncari002-mkulb002-bport020-pullm002/actions/workflows/tests.yml)

Authors:
* [Nathan Caridad](https://github.com/ncari002)
* [Maksim Kulbaev](https://github.com/unsafe4u)
* [Brian Porter](https://github.com/bporter11)
* [Paul Ullman](https://github.com/PaulU090)

## Project Description
* Web scrapping/data management is a useful skill to learn, so we're using this project to learn

* Web Scrape Target - website with large database of prices for items we're interested in. More websites are supported in the release info

* C# for web scraping

* Txt, csv, xml, and xlsx as output formats

* Input is a website link to a specific item or a .txt file with list of links to specific items

* Output is name, price, rarity, set code, set name, URL of the items processed
## Patterns Used
 * Null Object Pattern:
     * Provides default behavior in case data is not available. This is useful for our web scraper because different website might not have all the data we need, so certain functions are not implement exactly the same as others and instead return null values. An abstract class is first defined, and a class is defined for each website we use. Since the given websites contain different data, we can allow some return values to be assigned null if there is no data available, instead of just crashing our code.
 * Factory Pattern:
     * Provides the best method of creating object. The client need not know how creating the object works as all objects are referred to using a common interface that allows any object, despite it's differences, to be properly interacted with. This is great for our website scraper because the client only has to interact with the factory and the concrete object, the process of making the concrete object from the abstract object is abstracted away from the client/user.
 * Strategy Pattern:
     * Provides an effective way to implement different forms of output (text,xml,xlsx). The strategy pattern is implemented through the IExportBase class which provides a default template for exporting the parsed data. The specific strategies inherit from the IExportBase class and are implemented in the TextExportBase, XLSXExportBase and XMLExportBase classes. This design patterns provides an effective way to have different forms of output without having to alter the base interface.
 * Composite Pattern:
     * Provides an abstract interface for the objects in the composition. This pattern implements the default behavior through the ICardTactic class, which gets inherited by its common Tactic classes. The ICardTactic class acts as an interface for accessing and managing its child components. The CardRushTactic, PokemonWizardTactic, TrollAndToadTactic, TCGMPTactic and CardMarketTactic all act as leaf classes inheriting from the abstract interface class (ICardTactic).
* This allowed us to write better code by defining clear guidelines as to how each website is handled, managing different use cases per website, and allowing things to inheret from an abstract class rather than hardcoding functionality. This allowed expandability in our code rather than having to restructure if we had new websites or export types in the future.
 
 ## Screenshots
 ![input args](https://github.com/cs100/final-project-ncari002-mkulb002-bport020-pullm002/blob/master/screenshots/input%20args.png)
You can pass direct links to products
 ![input files](https://github.com/cs100/final-project-ncari002-mkulb002-bport020-pullm002/blob/master/screenshots/input%20files.png)
Or you can pass in one or more files that contains multiple links
 ## Installation/Usage
You must have [.Net 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) installed

You can download the latest release [here](https://github.com/cs100/final-project-ncari002-mkulb002-bport020-pullm002/releases)

Command line usage works as 

Windows:
    CardScraper.exe args [OUTPUT TYPE] [EXPORT PATH] URL [URL...]

Unix
    CardScraper args [OUTPUT TYPE] [EXPORT PATH] URL [URL...]

Or

Windows:
    CardScraper.exe file [OUTPUT TYPE] [EXPORT PATH] File [File...]

Unix
    CardScraper file [OUTPUT TYPE] [EXPORT PATH] File [File...]
 
# Options
    [OUTPUT TYPE]           TXT, CSV, XML, or XLSX. Must be in all caps
    [EXPORT PATH]           specifies where you want your output to be located. if just
                            the name of a file, then it will be placed in the same
                            directory from where the command was ran
    [URL...]                Multiple URLs can be given at the same time
    [File...]               Multiple Files can be given at the same time
 
## Testing
Project was tested by manually checking desired values from the website and checking those values against those acquired from the program itself. Since price can flucuate, these test cases may need to be updated if run at a later date. We used xUnit testing framework
