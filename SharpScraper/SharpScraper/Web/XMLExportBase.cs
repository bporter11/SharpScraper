using System;

using System.Collections.Generic;

using System.IO;

using System.Threading.Tasks;

using System.Xml.Serialization;



namespace SharpScraper.Web

{

    public class XMLExportBase : IExportBase

    {

        [XmlRoot("Card", IsNullable = false)]

        public class CardSerializer

        {

            [XmlAttribute]

            public string Name;



            [XmlAttribute]

            public double Price;



            [XmlAttribute]

            public string Rarity;



            [XmlAttribute]

            public string SetCode;



            [XmlAttribute]

            public string SetName;



            [XmlText]

            public string URL;



            public CardSerializer()

            {

                this.URL = String.Empty;

                this.Name = String.Empty;

                this.Price = 0.0;

                this.Rarity = String.Empty;

                this.SetCode = String.Empty;

                this.SetName = String.Empty;

            }



            public CardSerializer(string url, ICardTactic card)

            {

                this.URL = url;

                this.Name = card.Name;

                this.Price = card.Price;

                this.Rarity = card.Rarity;

                this.SetCode = card.SetCode;

                this.SetName = card.SetName;

            }

        }



        public class CardDatabase

        {

            [XmlArray]

            public CardSerializer[] Cards;



            public CardDatabase()

            {

                this.Cards = Array.Empty<CardSerializer>();

            }



            public CardDatabase(IDictionary<string, ICardTactic> cards)

            {

                this.Cards = new CardSerializer[cards.Count];



                int counter = 0;



                foreach (var pair in cards)

                {

                    this.Cards[counter++] = new CardSerializer(pair.Key, pair.Value);

                }

            }

        }



        public static readonly string Name = "XML";



        public Task Export(Stream stream, IDictionary<string, ICardTactic> cards)

        {

            var database = new CardDatabase(cards);



            var serializer = new XmlSerializer(typeof(CardDatabase));



            serializer.Serialize(stream, database);



            return Task.CompletedTask;

        }

    }

}


