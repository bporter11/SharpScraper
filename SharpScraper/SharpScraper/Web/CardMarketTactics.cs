using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
    public class CardMarketTactic : ICardTactic
    {
        private enum QualityType : int
        {
            MT, // mint
            NM, // near-mint
            EX, // excelent
            GD, // good
            LP, // lightly-played
            PL, // played
            PO, // poor
        }

        private const string kNameRegEx = @"^(?<name>[^()]*)[(]?(?<setcode>[^()]*)[)]?$";
        private const string kPriceRegEx = @"^(?<price>[\d,.]+).*$";

        private static readonly CultureInfo ms_cultureInfo = new CultureInfo("en")
        {
            NumberFormat = new NumberFormatInfo()
            {
                NumberDecimalSeparator = ",",
                NumberGroupSeparator = ".",
            },
        };

        private string m_name;
        private double m_price;
        private string m_rarity;
        private string m_setCode;
        private string m_setName;

        public static readonly string Domain = "www.cardmarket.com";

        public string Name => this.m_name;
        public double Price => this.m_price;
        public string Rarity => this.m_rarity;
        public string SetCode => this.m_setCode;
        public string SetName => this.m_setName;

        public CardMarketTactic()
        {
            this.m_name = String.Empty;
            this.m_rarity = String.Empty;
            this.m_setCode = String.Empty;
            this.m_setName = String.Empty;
        }

        public Task Parse(HtmlDocument document)
        {
            var main = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "div", "class", "flex-grow-1");
            var code = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "span", "class", "h4 text-muted font-weight-normal font-italic");
            var trow = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "div", "class", "row");
            var name = WebUtils.GetHtmlNodeContentByPath(main, "h1");

            if (code is not null)
            {
                this.m_setName = code.InnerText.Trim();
            }

            if (name is not null)
            {
                var substr = name.Substring(0, name.Length - this.m_setName.Length);
                var tokens = new Regex(CardMarketTactic.kNameRegEx).Match(substr);

                this.m_name = tokens.Groups["name"].ToString().Trim();
                this.m_setCode = tokens.Groups["setcode"].ToString().Trim();
            }

            if (trow is not null)
            {
                var rarity = WebUtils.FindHtmlNodeWithAttributeRecursive(trow, "span", "class", "icon");

                if (rarity is not null)
                {
                    this.m_rarity = rarity.GetAttributeValue("title", String.Empty);
                }
            }

            var table = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "div", "class", "table-body");

            if (table is null)
            {
                return Task.CompletedTask;
            }

            var mapper = new List<(string quality, double price)>();

            foreach (var child in table.ChildNodes)
            {
                var attributes = WebUtils.FindHtmlNodeWithAttributeRecursive(child, "div", "class", "product-attributes col");

                if (attributes is null)
                {
                    continue;
                }

                var href = WebUtils.FindHtmlNodeWithAttributeRecursive(attributes, "span", "class", "badge ");

                if (href is not null)
                {
                    var quality = href.InnerText.Trim();

                    var priceRow = WebUtils.FindHtmlNodeWithAttributeRecursive(child, "div", "class", "mobile-offer-container d-flex d-md-none justify-content-end col");

                    if (priceRow is null)
                    {
                        continue;
                    }

                    var priceData = WebUtils.FindHtmlNodeWithAttributeRecursive(child, "span", "class", "font-weight-bold color-primary small text-right text-nowrap");

                    if (priceData is null)
                    {
                        continue;
                    }

                    var tokens = new Regex(CardMarketTactic.kPriceRegEx).Match(priceData.InnerText);
                    var strstr = tokens.Groups["price"].ToString();

                    if (Double.TryParse(strstr, NumberStyles.Any, CardMarketTactic.ms_cultureInfo, out var price))
                    {
                        mapper.Add((quality, price));
                    }
                }
            }

            if (mapper.Count == 0)
            {
                return Task.CompletedTask;
            }

            mapper.Sort((x, y) =>
            {
                if (!Enum.TryParse(x.quality, out QualityType qualityX))
                {
                    if (!Enum.TryParse<QualityType>(y.quality, out _))
                    {
                        return x.price.CompareTo(y.price);
                    }

                    return 1;
                }

                if (!Enum.TryParse(y.quality, out QualityType qualityY))
                {
                    return -1;
                }

                var comparer = qualityX.CompareTo(qualityY);

                return comparer == 0 ? x.price.CompareTo(y.price) : comparer;
            });

            this.m_price = mapper[0].price;

            return Task.CompletedTask;
        }
    }
}
