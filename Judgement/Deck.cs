using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Judgement
{
    public class Deck
    {
        /// <summary>
        /// The list of cards in the player's deck
        /// </summary>
        public List<Card> Cards { get; set; }

        /// <summary>
        /// The Trump
        /// </summary>
        public Suit Trump { get; set; }

        /// <summary>
        /// The list of hands made
        /// </summary>
        public List<Hand> Hands { get; set; }

        /// <summary>
        /// The number of hands made/captured
        /// </summary>
        public int HandsMade { get; set; }

        /// <summary>
        /// The number of hands stated
        /// </summary>
        public int HandsStated { get; set; }

        /// <summary>
        /// The distance between two cards
        /// </summary>
        public int DistanceBetweenCards { get; set; }

        /// <summary>
        /// The card placed by the player on the table
        /// </summary>
        public Card PlacedCard { get; set; }
    }
}
