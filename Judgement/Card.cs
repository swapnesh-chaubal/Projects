using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Judgement
{
    public class Card
    {
        #region Constants

        // The distance from the top. This is where the cards of player 1 will be placed
        const int TOP = 550;
        const int LEFT = 10;
        double SizeRatio = 4 / 5; // Width / Height ratio
        Rectangle destinationRectangle;
        
        #endregion

        #region Properties

        public Rank RankOfCard { get; set; }
        public Suit CardSuite { get; set; }
        public Texture2D Image { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public bool IsCardPicked { get; set; }
        public bool IsMarkedBlack { get; set; }
        public bool IsClicked { get; set; }
        
        #endregion

        public Card()
        {
            Position = new Vector2(LEFT, TOP);
            Size = new Vector2(3 * 40, 4 * 40);
            //destinationRectangle = new Rectangle(Convert.ToInt32(Position.X),
            //                                    Convert.ToInt32(Position.Y),
            //                                    Convert.ToInt32(Size.X),
            //                                    Convert.ToInt32(Size.Y));
            IsCardPicked = false;

        }
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCardText()
        {
            StringBuilder builder = new StringBuilder();

            switch (RankOfCard)
            {
                case Rank.Ace:
                    builder.Append("Ace of ");
                    break;
                case Rank.Two:
                    builder.Append("Two of ");
                    break;
                case Rank.Three:
                    builder.Append("Three of ");
                    break;
                case Rank.Four:
                    builder.Append("Four of ");
                    break;
                case Rank.Five:
                    builder.Append("Five of ");
                    break;
                case Rank.Six:
                    builder.Append("Six of ");
                    break;
                case Rank.Seven:
                    builder.Append("Seven of ");
                    break;
                case Rank.Eight:
                    builder.Append("Eight of ");
                    break;
                case Rank.Nine:
                    builder.Append("Nine of ");
                    break;
                case Rank.Ten:
                    builder.Append("Ten of ");
                    break;
                case Rank.Jack:
                    builder.Append("Jack of ");
                    break;
                case Rank.Queen:
                    builder.Append("Queen of ");
                    break;
                case Rank.King:
                    builder.Append("King of ");
                    break;
                default:
                    break;
            }

            switch (CardSuite)
            {
                case Suit.Spades:
                    builder.Append("Spades");
                    break;
                case Suit.Hearts:
                    builder.Append("Hearts");
                    break;
                case Suit.Diamonds:
                    builder.Append("Diamonds");
                    break;
                case Suit.Clubs:
                    builder.Append("Clubs");
                    break;
                default:
                    break;
            }

            return builder.ToString();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Image,
            //        new Vector2(Position.X, Position.Y),

            //        Color.White);

            destinationRectangle = new Rectangle(Convert.ToInt32(Position.X),
                                                Convert.ToInt32(Position.Y),
                                                Convert.ToInt32(Size.X),
                                                Convert.ToInt32(Size.Y));

            spriteBatch.Begin();
            if (IsMarkedBlack && IsClicked)
            {
                spriteBatch.Draw(Image, destinationRectangle, null, Color.DarkSlateGray, 0, Vector2.Zero, SpriteEffects.None, 0);    
            }
            else
            {
                spriteBatch.Draw(Image, destinationRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);    
            }
            
            spriteBatch.End();
        }

        public bool UpdateCardPosition(int x, int y, int DistanceBetweenCards)
        {
            if (x >= Position.X && x <= (Position.X + DistanceBetweenCards))
            {
                if (y >= Position.Y && y <= (Position.Y + Image.Height))
                {
                    if (IsCardPicked)
                    {
                        // If card is already picked, put it down again
                        Position = new Vector2(Position.X, Position.Y + 50);
                        IsCardPicked = false;
                    }
                    else
                    {
                        Position = new Vector2(Position.X, Position.Y - 50);
                        IsCardPicked = true;
                    }
                    
                    return true;
                }
            }
            return false;
        }


        public bool IsClickOnCard(int x, int y, int DistanceBetweenCards)
        {
            if (x >= Position.X && x <= (Position.X + DistanceBetweenCards))
            {
                if (y >= Position.Y && y <= (Position.Y + Image.Height))
                {
                    return true;
                }
            }
            
            return false;
        }
        #endregion

    }
}
