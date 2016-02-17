using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Judgement
{
    public class Menu
    {
        const int BOARD_WIDTH = 962;
        const int BOARD_HEIGHT = 722;

        public List<string> MenuStrings { get; set; }
        public MenuState MenuState { get; set; }
        public MenuState PreviousMenuState { get; set; }
        public GameState PreviousGameState { get; set; }
        public List<string> SuitStrings { get; set; }
        public Texture2D BackgroundWood { get; set; }
        public Texture2D BackgroundRed { get; set; }
        public Texture2D BackgroundOrange { get; set; }
        public Dictionary<string, Texture2D> MenuScreens { get; set; }
        public Texture2D MainScreen { get; set; }
        
        public List<Rectangle> MenuStringRectangles { get; set; }
        public List<Rectangle> TrumpStringRectangles { get; set; }

        #region Constructor

        public Menu(SpriteFont TitleFont, SpriteFont menuFont)
        {
            MenuStrings = new List<string>();
            SuitStrings = new List<string>();
            MenuScreens = new Dictionary<string, Texture2D>();

            // add the menu strings
            MenuStrings.Add("Start");
            MenuStrings.Add("Instructions");
            MenuStrings.Add("About");
            MenuStrings.Add("Exit");

            SuitStrings.Add("1. Hearts");
            SuitStrings.Add("2. Diamonds");
            SuitStrings.Add("3. Clubs");
            SuitStrings.Add("4. Spades");

            // add 4 items for now
            MenuStringRectangles = new List<Rectangle>();
            Vector2 FontOrigin;

            string output = "Start";
            FontOrigin = TitleFont.MeasureString(output);
            int optionAnimationCounter = 3;
            float  posX = BOARD_WIDTH / 2 - FontOrigin.X / 4 - optionAnimationCounter * 2;
            float posY = 216 - FontOrigin.Y / 4;
            FontOrigin = TitleFont.MeasureString(output);
            MenuStringRectangles.Add(new Rectangle((int)posX, (int)posY, (int)FontOrigin.X, (int)FontOrigin.Y / 2));

            output = "Instructions";
            FontOrigin = TitleFont.MeasureString(output);
            posY = 344 - FontOrigin.Y / 4;
            posY = 344 - FontOrigin.Y / 4;
            posX -= optionAnimationCounter;
            MenuStringRectangles.Add(new Rectangle((int)posX, (int)posY, (int)FontOrigin.X, (int)FontOrigin.Y / 2));

            output = "About";
            FontOrigin = TitleFont.MeasureString(output);
            posY = 471 - FontOrigin.Y / 4;
            MenuStringRectangles.Add(new Rectangle((int)posX, (int)posY, (int)FontOrigin.X, (int)FontOrigin.Y / 2));

            output = "Exit";
            FontOrigin = TitleFont.MeasureString(output);
            posY = 608 - FontOrigin.Y / 4;
            posX -= optionAnimationCounter * 10;
            MenuStringRectangles.Add(new Rectangle((int)posX, (int)posY, (int)FontOrigin.X + optionAnimationCounter * 10, (int)FontOrigin.Y / 2));

            int startY = 170;
            int startX = 20;
            int increment = 75;
            TrumpStringRectangles = new List<Rectangle>();
            for (int index = 0; index < SuitStrings.Count; index++)
            {
                output = SuitStrings[index];
                FontOrigin = menuFont.MeasureString(output);
                startY += increment;
                TrumpStringRectangles.Add(new Rectangle(startX, startY, (int)(FontOrigin.X * 0.7), (int)(FontOrigin.Y * 0.7)));
            }
        }

        #endregion
    }
}
