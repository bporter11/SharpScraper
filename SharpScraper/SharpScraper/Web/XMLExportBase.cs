using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharpScraper.Web
{
	/// <summary>
	/// Implements <see cref="IExportBase"/> that operates on .xml extension files.
	/// </summary>
	public class XMLExportBase : IExportBase
    {
		/// <summary>
		/// Base class for serializing card data as XML.
		/// </summary>
        [XmlRoot("Card", IsNullable = false)]
        public class CardSerializer
        {
			/// <summary>
			/// Reference to <see cref="ICardTactic.Name"/>.
			/// </summary>
            [XmlAttribute]
            public string Name;

			/// <summary>
			/// Reference to <see cref="ICardTactic.Price"/>.
			/// </summary>
            [XmlAttribute]
            public double Price;

			/// <summary>
			/// Reference to <see cref="ICardTactic.Rarity"/>.
			/// </summary>
            [XmlAttribute]
            public string Rarity;

			/// <summary>
			/// Reference to <see cref="ICardTactic.SetCode"/>.
			/// </summary>
            [XmlAttribute]
            public string SetCode;

			/// <summary>
			/// Reference to <see cref="ICardTactic.SetName"/>.
			/// </summary>
            [XmlAttribute]
            public string SetName;

			/// <summary>
			/// Reference to URL of provided <see cref="ICardTactic"/>.
			/// </summary>
            [XmlText]
            public string URL;

			/// <summary>
			/// Constructs new instance of <see cref="CardSerializer"/>.
			/// </summary>
			public CardSerializer()
            {
                this.URL = String.Empty;
                this.Name = String.Empty;
                this.Price = 0.0;
                this.Rarity = String.Empty;
                this.SetCode = String.Empty;
                this.SetName = String.Empty;
            }

			/// <summary>
			/// Constructs new instance of <see cref="CardSerializer"/> with URL and <see cref="ICardTactic"/> provided.
			/// </summary>
			/// <param name="url">URL of <see cref="ICardTactic"/> as <see cref="String"/>.</param>
			/// <param name="card"><see cref="ICardTactic"/> card data.</param>
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

		/// <summary>
		/// Base class for serializing collection of <see cref="ICardTactic"/>.
		/// </summary>
        public class CardDatabase
        {
			/// <summary>
			/// <see cref="Array"/> of <see cref="CardSerializer"/> classes.
			/// </summary>
            [XmlArray]
            public CardSerializer[] Cards;

			/// <summary>
			/// Constructs new instance of <see cref="CardDatabase"/>.
			/// </summary>
			public CardDatabase()
            {
                this.Cards = Array.Empty<CardSerializer>();
            }

			/// <summary>
			/// Constructs new instance of <see cref="CardDatabase"/> with collection of <see cref="ICardTactic"/> provided.
			/// </summary>
			/// <param name="cards"><see cref="IDictionary{TKey, TValue}"/> of URLs and their corresponding <see cref="ICardTactic"/>.</param>
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

		/// <summary>
		/// String identifier for this <see cref="IExportBase"/>.
		/// </summary>
		public static readonly string Name = "XML";

		/// <inheritdoc/>
        public Task Export(Stream stream, IDictionary<string, ICardTactic> cards)
        {
            var database = new CardDatabase(cards);

            var serializer = new XmlSerializer(typeof(CardDatabase));

            serializer.Serialize(stream, database);

            return Task.CompletedTask;
        }
    }
}
