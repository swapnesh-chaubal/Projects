using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Judgement
{
    /// <summary>
    /// Class for player
    /// </summary>
    public class Player
    {
        #region Properties

        #region Public

        /// <summary>
        /// the name of the player
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// the trump for the game
        /// </summary>
        public Suit Trump { get; set; }

        /// <summary>
        /// The no. of cards the game started with
        /// </summary>
        public int TotalCards { get; set; }

        /// <summary>
        /// the cards with the player
        /// </summary>
        public List<Card> Cards { get; set; }

        /// <summary>
        /// the cards that have been placed on the table as a part of the hand
        /// </summary>
        public List<Card> CardsOnTable { get; set; }

        /// <summary>
        /// The number of hands made/captured
        /// </summary>
        public int HandsMade { get; set; }

        /// <summary>
        /// The number of hands stated
        /// </summary>
        public int HandsStated { get; set; }

        /// <summary>
        /// The list of total used cards in the game
        /// </summary>
        public List<Card> TotalUsedCards { get; set; }

        /// <summary>
        /// The position where the card will be placed
        /// </summary>
        public Vector2 CardPosition { get; set; }

        /// <summary>
        /// The number of the player
        /// </summary>
        public PlayerIndex PlayerNumber { get; set; }

        /// <summary>
        /// The state of the player's action
        /// </summary>
        public PlayerState PlayerState { get; set; }

        /// <summary>
        /// Round-wise scores of the player
        /// </summary>
        public List<int> Scores { get; set; }

        /// <summary>
        /// The total score of the player
        /// </summary>
        public int TotalScore { get; set; }

        #endregion

        #region Private

        /// <summary>
        /// the trump cards in the suit
        /// </summary>
        List<Card> TrumpCards { get; set; }

        /// <summary>
        /// the heart in the suit
        /// </summary>
        List<Card> Hearts { get; set; }

        /// <summary>
        /// the spades in the suit
        /// </summary>
        List<Card> Spades { get; set; }

        /// <summary>
        /// the diamonds in the suit
        /// </summary>
        List<Card> Diamonds { get; set; }

        /// <summary>
        /// the clubs in the suit
        /// </summary>
        List<Card> Clubs { get; set; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Player()
        {
            CardsOnTable = new List<Card>();
            Cards = new List<Card>();
            TotalUsedCards = new List<Card>();
            Spades = new List<Card>();
            Hearts = new List<Card>();
            Clubs = new List<Card>();
            Diamonds = new List<Card>();
            TrumpCards = new List<Card>();
            Trump = Suit.Unspecified;
            Scores = new List<int>();
        }

        #endregion

        #region Initialization functions

        /// <summary>
        /// select the trumpcards
        /// </summary>
        private void GetTrumpAndOtherCards()
        {
            if (TrumpCards == null)
            {
                TrumpCards = new List<Card>();
            }

            TrumpCards.Clear();

            if (Clubs == null)
            {
                Clubs = new List<Card>();
            }
            Clubs.Clear();

            if (Diamonds == null)
            {
                Diamonds = new List<Card>();
            }
            Diamonds.Clear();

            if (Hearts == null)
            {
                Hearts = new List<Card>();
            }
            Hearts.Clear();

            if (Spades == null)
            {
                Spades = new List<Card>();
            }
            Spades.Clear();

            foreach (Card card in Cards)
            {
                if (card.CardSuite == Trump)
                {
                    TrumpCards.Add(card);
                }
                else if (card.CardSuite == Suit.Clubs)
                {

                    Clubs.Add(card);
                }
                else if (card.CardSuite == Suit.Diamonds)
                {

                    Diamonds.Add(card);
                }
                else if (card.CardSuite == Suit.Hearts)
                {

                    Hearts.Add(card);
                }
                else if (card.CardSuite == Suit.Spades)
                {

                    Spades.Add(card);
                }
            }
            // Sort all the sets of cards
            InsertionSort(TrumpCards);
            if (Clubs != null)
            {
                if (Clubs.Count != 0)
                {
                    InsertionSort(Clubs);
                }
            }

            if (Diamonds != null)
            {
                if (Diamonds.Count != 0)
                {
                    InsertionSort(Diamonds);
                }
            }

            if (Hearts != null)
            {
                if (Hearts.Count != 0)
                {
                    InsertionSort(Hearts);
                }
            }

            if (Spades != null)
            {
                if (Spades.Count != 0)
                {
                    InsertionSort(Spades);
                }
            }
        }

        /// <summary>
        /// Function to initialize the player attributes
        /// </summary>
        public void InitializePlayer()
        {
            GetTrumpAndOtherCards();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sort the cards in descending order
        /// </summary>
        /// <param name="array">the array to sort</param>
        private void InsertionSort(List<Card> array)
        {
            int key;
            Card cardKey;
            Card temp;

            for (int j = 1; j < array.Count; j++)
            {
                key = (int)array[j].RankOfCard;
                cardKey = array[j];
                int i = j - 1;
                while (i >= 0 && (int)array[i].RankOfCard > key)
                {
                    temp = array[i + 1];
                    array[i + 1] = array[i];
                    array[i] = temp;
                    i--;
                }

                array[i + 1] = cardKey;
            }
            array.Reverse();
        }

        /// <summary>
        /// Function to check whether the 1st card placed on the table is present in the suit
        /// </summary>
        /// <returns>true if present, false otherwise</returns>
        private bool IsCardPresent()
        {
            bool cardNotPresent = false;
            // we now check if the player contains the card of the same suit
            // as of the one placed on the table
            try
            {
                switch (CardsOnTable[0].CardSuite)
                {
                    case Suit.Spades:
                        if (Spades.Count == 0)
                        {
                            cardNotPresent = true;
                        }
                        break;
                    case Suit.Hearts:
                        if (Hearts.Count == 0)
                        {
                            cardNotPresent = true;
                        }
                        break;
                    case Suit.Diamonds:
                        if (Diamonds.Count == 0)
                        {
                            cardNotPresent = true;
                        }
                        break;
                    case Suit.Clubs:
                        if (Clubs.Count == 0)
                        {
                            cardNotPresent = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (NullReferenceException)
            {
                // check for the trump cards
                if (TrumpCards.Count == 0)
                {
                    cardNotPresent = true;
                }
            }

            // if card is nt present in the other suits, we need to check the trump cards suit
            if (cardNotPresent)
            {
                if (CardsOnTable[0].CardSuite == Trump)
                {
                    if (TrumpCards.Count == 0)
                    {
                        cardNotPresent = true;
                    }
                    else
                    {
                        cardNotPresent = false;
                    }
                }
            }
            return cardNotPresent;
        }

        /// <summary>
        /// Function to find the minumum card from all the suits
        /// </summary>
        /// <param name="IncludeTrump">true if the search should include trump cards, 
        /// false otherwise</param>
        /// <returns>Minimum card</returns>
        private Card FindMinCard(bool IncludeTrump)
        {
            Card minCard = null;
            if (Diamonds != null && Diamonds.Count != 0)
            {
                minCard = Diamonds[Diamonds.Count - 1];
            }
            if (Hearts != null && Hearts.Count != 0)
            {
                if (minCard != null)
                {
                    if ((int)minCard.RankOfCard > (int)Hearts[Hearts.Count - 1].RankOfCard)
                    {
                        minCard = (Card)Hearts[Hearts.Count - 1];
                    }
                }
                else
                {
                    minCard = Hearts[Hearts.Count - 1];
                }
            }
            if (Spades != null && Spades.Count != 0)
            {
                if (minCard != null)
                {
                    if ((int)minCard.RankOfCard > (int)Spades[Spades.Count - 1].RankOfCard)
                    {
                        minCard = (Card)Spades[Spades.Count - 1];
                    }
                }
                else
                {
                    minCard = Spades[Spades.Count - 1];
                }
            }
            if (Clubs != null && Clubs.Count != 0)
            {
                if (minCard != null)
                {
                    if ((int)minCard.RankOfCard > (int)Clubs[Clubs.Count - 1].RankOfCard)
                    {
                        minCard = (Card)Clubs[Clubs.Count - 1];
                    }
                }
                else
                {
                    minCard = Clubs[Clubs.Count - 1];
                }
            }

            if (minCard == null)
            {
                if (TrumpCards.Count != 0)
                {
                    minCard = TrumpCards[TrumpCards.Count - 1];
                    return minCard;
                }
            }
            else if (IncludeTrump)
            {
                if (TrumpCards.Count != 0)
                {
                    if (TrumpCards[TrumpCards.Count - 1].RankOfCard < minCard.RankOfCard)
                    {
                        minCard = TrumpCards[TrumpCards.Count - 1];
                        return minCard;
                    }
                }

            }
            return minCard;
        }

        /// <summary>
        /// Function to find the maximun card from all the suits
        /// </summary>
        /// <param name="IncludeTrump">true if the search should include trump cards, 
        /// false otherwise</param>
        /// <returns>Maximun card</returns>
        private Card FindMaxCard(bool IncludeTrump)
        {
            Card maxCard = null;
            if (Diamonds != null && Diamonds.Count != 0)
            {
                maxCard = Diamonds[0];
            }
            if (Hearts != null && Hearts.Count != 0)
            {
                if (maxCard != null)
                {
                    if ((int)maxCard.RankOfCard < (int)Hearts[0].RankOfCard)
                    {
                        maxCard = (Card)Hearts[0];
                    }
                }
                else
                {
                    maxCard = Hearts[0];
                }
            }
            if (Spades != null && Spades.Count != 0)
            {
                if (maxCard != null)
                {
                    if ((int)maxCard.RankOfCard < (int)Spades[0].RankOfCard)
                    {
                        maxCard = (Card)Spades[0];
                    }
                }
                else
                {
                    maxCard = Spades[0];
                }
            }
            if (Clubs != null && Clubs.Count != 0)
            {
                if (maxCard != null)
                {
                    if ((int)maxCard.RankOfCard < (int)Clubs[0].RankOfCard)
                    {
                        maxCard = (Card)Clubs[0];
                    }
                }
                else
                {
                    maxCard = Clubs[0];
                }
            }

            if (maxCard == null)
            {
                if (TrumpCards.Count != 0)
                {
                    maxCard = TrumpCards[0];
                    return maxCard;
                }
            }
            else if (IncludeTrump)
            {
                if (TrumpCards.Count != 0)
                {
                    if (TrumpCards[0].RankOfCard > maxCard.RankOfCard)
                    {
                        maxCard = TrumpCards[0];
                        return maxCard;
                    }
                }

            }
            return maxCard;
        }

        /// <summary>
        /// Function to check whether the given card exists
        /// </summary>
        /// <param name="deck">deck of cards</param>
        /// <param name="suit">suit to check</param>
        /// <param name="rank">rank to check</param>
        /// <returns>true if found, false otherwise</returns>
        private bool CheckCard(List<Card> deck, Suit suit, Rank rank)
        {
            if (TotalCards > 10)
            {
                // if the cards are 10 or less, theres some chance that the greater card has been
                // removed. Hence, there is no need to check anything
                foreach (Card card in deck)
                {
                    if (card.RankOfCard == rank && card.CardSuite == suit)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private List<Card> GetListOfSuit(Suit suit)
        {
            // card of the same suit is present
            List<Card> tempList = null;


            if (suit == Trump)
            {
                tempList = TrumpCards;
            }
            else
            {
                switch (suit)
                {
                    case Suit.Spades:
                        tempList = Spades;
                        break;
                    case Suit.Hearts:
                        tempList = Hearts;
                        break;
                    case Suit.Diamonds:
                        tempList = Diamonds;
                        break;
                    case Suit.Clubs:
                        tempList = Clubs;
                        break;
                    default:
                        break;
                }
            }

            return tempList;
        }

        /// <summary>
        /// Function to place the card if card of the required suit IS present
        /// </summary>
        /// <returns>card to place</returns>
        private Card PlaceCardIfPresent()
        {
            Card cardToPlace;
            // card of the same suit is present
            List<Card> tempList = null;

            tempList = GetListOfSuit(CardsOnTable[0].CardSuite);

            bool isGreatest = false;
            // iterate over all the cards on the table and check if this card is greater
            foreach (Card card in CardsOnTable)
            {
                // We only need to check the cards of the same suit
                if (tempList[0].CardSuite == card.CardSuite)
                {
                    if (tempList[0].RankOfCard > card.RankOfCard)
                    {
                        isGreatest = true;
                    }
                    else
                    {
                        isGreatest = false;
                        break;
                    }
                }
                else
                {
                    if (card.CardSuite == Trump)
                    {
                        // the hand has already been taken by a trump
                        isGreatest = false;
                    }
                }

            }
            // if we have a card which is greater than all cards on the table, put our biggest
            // card. Else put the smallest card of the suit
            if (isGreatest)
            {
                cardToPlace = tempList[0];
            }
            else
            {
                cardToPlace = tempList[tempList.Count - 1];
            }

            return cardToPlace;
        }

        /// <summary>
        /// Function to place the card if card of the required suit IS NOT present
        /// </summary>
        /// <returns>card to place</returns>
        private Card PlaceCardIfNotPresent()
        {
            Card cardToPlace;

            // player doesn't have the card of that suit
            // check the hands remaining to be made. Depending upon this, we'll put a card
            int handsRemaining = HandsStated - HandsMade;
            int handsSure = 0;
            // if there are still hands remaning to be made, we need to figure out
            // which card to place
            if (handsRemaining > 0)
            {
                // we start with searching the trump cards
                // we calculate the sure hands, depending upon higher order
                // trump cards
                if (TrumpCards.Count > 0)
                {
                    foreach (Card trumpCard in TrumpCards)
                    {
                        if (trumpCard.RankOfCard == Rank.Ace)
                        {
                            handsSure++;
                        }
                        if (trumpCard.RankOfCard == Rank.King)
                        {
                            handsSure++;
                        }
                        if (trumpCard.RankOfCard == Rank.Queen)
                        {
                            handsSure++;
                        }
                        if (trumpCard.RankOfCard == Rank.Jack)
                        {
                            handsSure++;
                        }
                    }
                }

                if (handsRemaining == handsSure)
                {
                    // if the no. of sure hands is equal to the hands remanining to be
                    // made. Throw the max trump card
                    cardToPlace = TrumpCards[0];
                }
                else
                {
                    // throw the lowest trump card to take the hand

                    int maxCardIndex = TrumpCards.Count - 1;
                    if (maxCardIndex > -1)
                    {
                        foreach (Card card in CardsOnTable)
                        {
                            // if someone has already put a trump to take the hand we need to
                            // put a bigger trump than his
                            if (card.CardSuite == Trump)
                            {
                                if (TrumpCards[maxCardIndex].RankOfCard < card.RankOfCard)
                                {
                                    do
                                    {
                                        // go to the greater trump card
                                        maxCardIndex--;

                                        if (maxCardIndex == -1)
                                        {
                                            break;
                                        }
                                    } while (TrumpCards[maxCardIndex].RankOfCard > card.RankOfCard);

                                }
                            }
                        }
                    }
                    if (maxCardIndex < 0)
                    {
                        // dont take the card
                        cardToPlace = FindMinCard(false);
                    }
                    else
                    {
                        cardToPlace = TrumpCards[maxCardIndex];
                    }

                }
            }
            else
            {
                // if we dont need to make any more hands, find the highest card of some other
                // suit
                cardToPlace = FindMinCard(false);
            }

            return cardToPlace;
        }

        /// <summary>
        /// Function to check if King, Queen and Ace are present (Not Used)
        /// </summary>
        /// <param name="cards">list of cards</param>
        /// <returns>true if present, palse otherwise</returns>
        private Card CheckKingAceQueen(List<Card> cards)
        {
            if (cards[0].RankOfCard == Rank.Ace)
            {
                return cards[0];
            }
            else if (cards[0].RankOfCard == Rank.King)
            {
                return cards[0];
            }
            else if (cards[0].RankOfCard == Rank.Queen)
            {
                return cards[0];
            }

            return null;
        }

        private void RemoveCard(Card cardToRemove)
        {
            if (cardToRemove.CardSuite == Trump)
            {
                TrumpCards.Remove(cardToRemove);
            }
            else
            {
                List<Card> tempList = null;
                switch (cardToRemove.CardSuite)
                {
                    case Suit.Spades:
                        tempList = Spades;
                        break;
                    case Suit.Hearts:
                        tempList = Hearts;
                        break;
                    case Suit.Diamonds:
                        tempList = Diamonds;
                        break;
                    case Suit.Clubs:
                        tempList = Clubs;
                        break;
                    default:
                        break;
                }
                if (tempList != null)
                {
                    tempList.Remove(cardToRemove);
                }
            }

            Cards.Remove(cardToRemove);
        }

        private Card FindAce(List<Card> cards)
        {
            Card cardToPlace = null;

            if (cards != null && cards.Count != 0)
            {
                if (cardToPlace == null)
                {
                    if (cards[0].RankOfCard == Rank.Ace)
                    {
                        cardToPlace = cards[0];
                    }
                }
            }

            return cardToPlace;
        }

        private Card FindOtherCardsAndCheckForGreaterCard(List<Card> cards, Rank rank)
        {
            Card cardToPlace = null;

            if (cards != null && cards.Count != 0)
            {
                if (cardToPlace == null)
                {
                    if (cards[0].RankOfCard == rank)
                    {
                        // we have a card  check if the higher order card has been used up
                        // if not we can place the card
                        if (CheckCard(TotalUsedCards, cards[0].CardSuite, (Rank)(rank + 1)))
                        {
                            cardToPlace = cards[0];
                        }
                    }
                }
            }

            return cardToPlace;
        }

        /// <summary>
        /// Calculates the hands based on the rank
        /// </summary>
        /// <param name="cards">the list of cards to check</param>
        /// <param name="higherOrderRank">the rank of the higher card</param>
        /// <param name="lowerOrderRank">the rank of the lower card</param>
        /// <returns>number of hands/good cards</returns>
        private int CalculateGoodCards(List<Card> cards, Rank higherOrderRank, Rank lowerOrderRank)
        {
            int numberOfCards = 0;
            int rankCounter;

            foreach (Card card in cards)
            {
                for (rankCounter = (int)higherOrderRank; (int)rankCounter >= (int)lowerOrderRank; rankCounter--)
                {
                    if (card.RankOfCard == (Rank)rankCounter)
                    {
                        numberOfCards++;
                    }
                }
            }

            return numberOfCards;
        }

        private int CalculateCardsInSequence(List<Card> cards)
        {
            int numberOfCards = 0;
            try
            {
                if (cards[0].RankOfCard == Rank.Ace)
                {
                    numberOfCards++;
                    if (cards[1].RankOfCard == Rank.King)
                    {
                        numberOfCards++;
                        if (cards[2].RankOfCard == Rank.Queen)
                        {
                            numberOfCards++;
                            if (cards[3].RankOfCard == Rank.Jack)
                            {
                                numberOfCards++;
                            }
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // do nothing, we are only checking the number of cards in sequence
            }

            return numberOfCards;
        }

        private int StateHands(Suit trump)
        {
            int numberOfHands = 0;
            int minTrumpWeight = (int)Rank.Eight;

             //if there are more than 11 cards each, theres a chance other players have
             //good cards. Hence, we need to calculate cards in sequence
            if (TotalCards > 10)
            {
                //if (Spades.Count != 0)
                //{
                //    numberOfHands += CalculateCardsInSequence(Spades);
                //}

                //if (Hearts.Count != 0)
                //{
                //    numberOfHands += CalculateCardsInSequence(Hearts);

                //}

                //if (Diamonds.Count != 0)
                //{
                //    numberOfHands += CalculateCardsInSequence(Diamonds);
                //}

                //if (Clubs.Count != 0)
                //{
                //    numberOfHands += CalculateCardsInSequence(Clubs);
                //}

                //foreach (Card card in TrumpCards)
                //{
                //    if ((int)card.RankOfCard >= minTrumpWeight)
                //    {
                //        numberOfHands++;
                //    }
                //}

                if (Spades.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Spades, Rank.Ace, Rank.Queen);
                }

                if (Hearts.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Hearts, Rank.Ace, Rank.Queen);
                }

                if (Diamonds.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Diamonds, Rank.Ace, Rank.Queen);
                }

                if (Clubs.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Clubs, Rank.Ace, Rank.Queen);
                }

                minTrumpWeight = (int)Rank.Ten;
                foreach (Card card in TrumpCards)
                {
                    if ((int)card.RankOfCard >= minTrumpWeight)
                    {
                        numberOfHands++;
                    }
                }
            }
            else if (TotalCards > 7)
            {
                if (Spades.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Spades, Rank.Ace, Rank.Ten);
                }

                if (Hearts.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Hearts, Rank.Ace, Rank.Ten);
                }

                if (Diamonds.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Diamonds, Rank.Ace, Rank.Ten);
                }

                if (Clubs.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Clubs, Rank.Ace, Rank.Ten);
                }

                minTrumpWeight = (int)Rank.Eight;
                foreach (Card card in TrumpCards)
                {
                    if ((int)card.RankOfCard >= minTrumpWeight)
                    {
                        numberOfHands++;
                    }
                }

            }
            else if (TotalCards > 4)
            {

                if (Spades.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Spades, Rank.Ace, Rank.Nine);
                }

                if (Hearts.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Hearts, Rank.Ace, Rank.Nine);
                }

                if (Diamonds.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Diamonds, Rank.Ace, Rank.Nine);
                }

                if (Clubs.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Clubs, Rank.Ace, Rank.Nine);
                }

                minTrumpWeight = (int)Rank.Eight;
                foreach (Card card in TrumpCards)
                {
                    if ((int)card.RankOfCard >= minTrumpWeight)
                    {
                        numberOfHands++;
                    }
                } 
            }
            else
            {
                if (Spades.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Spades, Rank.Ace, Rank.Eight);
                }

                if (Hearts.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Hearts, Rank.Ace, Rank.Eight);
                }

                if (Diamonds.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Diamonds, Rank.Ace, Rank.Eight);
                }

                if (Clubs.Count != 0)
                {
                    numberOfHands += CalculateGoodCards(Clubs, Rank.Ace, Rank.Eight);
                }

                minTrumpWeight = (int)Rank.Six;
                foreach (Card card in TrumpCards)
                {
                    if ((int)card.RankOfCard >= minTrumpWeight)
                    {
                        numberOfHands++;
                    }
                }
            }

            if (numberOfHands > 6)
            {
                numberOfHands = 6;
            }
            return numberOfHands;
        }

        private Suit StateTrump()
        {
            // check the count of all suits and total size of all suits
            // if the number of cards is the same, the suit will be decided by the weight of
            // each suit
            int spades = 0, hearts = 0, clubs = 0, diamonds = 0;
            int spadesWeight = 0, heartsWeight = 0, clubsWeight = 0, diamondsWeight = 0;

            foreach (Card card in Cards)
            {
                switch (card.CardSuite)
                {
                    case Suit.Spades:
                        spades++;
                        spadesWeight += (int)card.RankOfCard;
                        break;
                    case Suit.Hearts:
                        hearts++;
                        heartsWeight += (int)card.RankOfCard;
                        break;
                    case Suit.Diamonds:
                        diamonds++;
                        diamondsWeight += (int)card.RankOfCard;
                        break;
                    case Suit.Clubs:
                        clubs++;
                        clubsWeight += (int)card.RankOfCard;
                        break;
                    default:
                        break;
                }
            }


            Suit[] suits = { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades };
            int[] numbers = { clubs, diamonds, hearts, spades };
            int[] weights = { clubsWeight, diamondsWeight, heartsWeight, spadesWeight };

            int tempNum;
            Suit tempSuite;

            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    if (numbers[i] > numbers[j])
                    {
                        // swap
                        tempNum = numbers[i];
                        numbers[i] = numbers[j];
                        numbers[j] = tempNum;

                        tempNum = weights[i];
                        weights[i] = weights[j];
                        weights[j] = tempNum;

                        tempSuite = suits[i];
                        suits[i] = suits[j];
                        suits[j] = tempSuite;
                    }
                }
            }

            int suitNumber;
            if (numbers[3] == numbers[2])
            {
                // 2 suits have the same number of cards
                // hence we check the suit weight
                if (weights[3] > weights[2])
                {
                    suitNumber = 3;
                }
                else
                {
                    suitNumber = 2;
                }

                if (numbers[2] == numbers[1])
                {
                    // 3 suits have the same number of cards
                    if (weights[suitNumber] < weights[1])
                    {
                        suitNumber = 1;
                    }

                    if (numbers[1] == numbers[0])
                    {
                        // 4 suits have the same number of cards
                        if (weights[suitNumber] < weights[0])
                        {
                            suitNumber = 0;
                        }
                    }
                }
            }
            else
            {
                if ((numbers[3] - numbers[2]) < 2)
                {
                    if (weights[3] > weights[2])
                    {
                        suitNumber = 3;
                    }
                    else
                    {
                        suitNumber = 2;
                    }
                }
                suitNumber = 3;
            }

            return suits[suitNumber];
        }

        #endregion

        #region Public methods

        /// <summary>
        /// AI to place the appropriate card
        /// </summary>
        /// <returns>Card to place</returns>
        public Card PlaceCard()
        {
            Card cardToPlace = null;

            // Note: We've sorted the cards in descending order. Therefore, the 0th card is always 
            // the highest of that suit

            // Check the cards placed on the table. A count of zero indicates that no card has
            // been placed on the table yet and its our turn to place a card
            if (CardsOnTable.Count == 0)
            {
                // check for any Aces. Since, Ace is the card of the maximum value,
                // we dont need to worry about someone using a bigger card, unless he/she
                // uses the trump. If we find an Ace, place it
                // First we'll check the non trump cards

                cardToPlace = FindAce(Clubs);

                if (cardToPlace == null)
                {
                    cardToPlace = FindAce(Diamonds);
                }

                if (cardToPlace == null)
                {
                    cardToPlace = FindAce(Hearts);
                }

                if (cardToPlace == null)
                {
                    cardToPlace = FindAce(Spades);
                }


                // Now we'll check for Kings. However, before we place the king, we need to check
                // if the Ace for that color has been placed, otherwise, we'll lose the hand
                // since some player is bound to put the Ace to capture the hand.

                if (cardToPlace == null)
                {
                    cardToPlace = FindOtherCardsAndCheckForGreaterCard(Clubs, Rank.King);
                }

                if (cardToPlace == null)
                {
                    cardToPlace = FindOtherCardsAndCheckForGreaterCard(Diamonds, Rank.King);
                }

                if (cardToPlace == null)
                {
                    cardToPlace = FindOtherCardsAndCheckForGreaterCard(Hearts, Rank.King);
                }

                if (cardToPlace == null)
                {
                    cardToPlace = FindOtherCardsAndCheckForGreaterCard(Spades, Rank.King);
                }

                // We'll now check for Queens. Similar to the King and Ace case, before we place 
                // the Queen, we need to check if the King for that color has been placed, 
                // otherwise, we'll lose the hand since some player is bound to put the 
                // King to capture the hand.
                if (cardToPlace == null)
                {
                    cardToPlace = FindOtherCardsAndCheckForGreaterCard(Clubs, Rank.Queen);
                }

                if (cardToPlace == null)
                {
                    cardToPlace = FindOtherCardsAndCheckForGreaterCard(Diamonds, Rank.Queen);
                }

                if (cardToPlace == null)
                {
                    cardToPlace = FindOtherCardsAndCheckForGreaterCard(Hearts, Rank.Queen);
                }

                if (cardToPlace == null)
                {
                    cardToPlace = FindOtherCardsAndCheckForGreaterCard(Spades, Rank.Queen);
                }

                // We've used up the Ace, King and Queen of all the suits except the Trump
                // Now we go to the trump sequence
                if (cardToPlace == null)
                {
                    // no worthwhile card was found in other suits. Lets now search for trumps

                    cardToPlace = FindAce(TrumpCards);
                }
                if (cardToPlace == null)
                {
                    cardToPlace = FindOtherCardsAndCheckForGreaterCard(TrumpCards, Rank.King);
                }

                if (cardToPlace == null)
                {
                    cardToPlace = FindOtherCardsAndCheckForGreaterCard(TrumpCards, Rank.Queen);
                }

                if (TotalCards > 11)
                {
                    // if we are using the full deck or almost the full deck
                    // and if not more than two cards have been used
                    if (TotalCards - Cards.Count  >=2)
                    {
                        // No good card was found, lets give up this hand
                        // Find the least card in all the cards.
                        cardToPlace = FindMinCard(false);
                    }
                    else
                    {
                        // we put the max card out of the suit
                        cardToPlace = FindMaxCard(false);
                    }
                }
                else if (TotalCards < 5)
                {
                    // we put the max card out of the suit. The search includes the trump
                    cardToPlace = FindMaxCard(true);
                }
                else
                {
                    // we put the max card out of the suit
                    cardToPlace = FindMaxCard(false);
                }
                

            }
            else
            {
                // There is one card on the table
                bool cardNotPresent = IsCardPresent();

                if (cardNotPresent)
                {
                    cardToPlace = PlaceCardIfNotPresent();
                }
                else
                {
                    cardToPlace = PlaceCardIfPresent();
                }
            }
            if (cardToPlace == null)
            {
                cardToPlace = Cards[0];
            }

            RemoveCard(cardToPlace);

            return cardToPlace;

        }


        public void StateHandsAndTrump(ref Suit oldTrump, 
                                        ref Suit newTrump, 
                                        ref int handsWithOldTrump, 
                                        ref int handsWithNewTrump)
        {
            // set the old trump first to calculate the hands that can be made with that
            Trump = oldTrump;
            if (Trump == Suit.Unspecified)
            {
                Trump = StateTrump();
            }

            GetTrumpAndOtherCards();
            handsWithOldTrump = StateHands(Trump);

            if (oldTrump != Suit.Unspecified)
            {
                // repeat the same with the new trump
                newTrump = StateTrump();
                Trump = newTrump;
                GetTrumpAndOtherCards();
                handsWithNewTrump = StateHands(newTrump);
            }
            else
            {
                handsWithNewTrump = 0;
                oldTrump = Trump;
            }
        }


        public int SelectTrumpAndInitialize()
        {
            // select the trump
            int handsWithTrump;
            Trump = StateTrump();

            GetTrumpAndOtherCards();
            handsWithTrump = StateHands(Trump);
            return handsWithTrump;
        }

        public void SetTrumpAndInitialize(Suit trump)
        {
            Trump = trump;
            GetTrumpAndOtherCards();
        }

        public void CalculateScore()
        {
            int score;
            if (HandsMade == HandsStated)
            {
                TotalScore += HandsStated * 10;
                Scores.Add(HandsStated * 10);
            }
            else
            {
                if (HandsStated > HandsMade)
                {
                    score = HandsMade - HandsStated;
                    score *= 5;
                }
                else
                {
                    score = 0;
                }
                
                TotalScore += (score);
                Scores.Add(score);
            }
        }

        #endregion

    }
}
