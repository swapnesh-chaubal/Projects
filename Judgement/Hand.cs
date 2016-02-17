using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Judgement
{
    public class Hand
    {
        /// <summary>
        /// The list of cards in the hand
        /// </summary>
        List<Card> Cards { get; set; }

        /// <summary>
        /// The Trump in the list
        /// </summary>
        Suit Trump { get; set; }
    }
}
