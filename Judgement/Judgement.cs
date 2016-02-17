using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Text;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Judgement
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Judgement : Microsoft.Xna.Framework.Game
    {
        const int BOARD_WIDTH = 962;
        const int BOARD_HEIGHT = 722;

        //const int BOARD_WIDTH = 1280;
        //const int BOARD_HEIGHT = 1024;

        // The card width to height ratio is always 4/5
        const int CARD_WIDTH = 120;
        const int CARD_HEIGHT = 160;

        // distance from the sides or top and bottom of the cards
        const int OFFSET = 10;
        const int CARD_PLACE_OFFSET = 40;
        int MidX, MidY;
        RoundState roundState;
        GameState gameState;
        bool gameStarted;
        bool buttonPressed;
        bool EnterPressed;
        int SummaryLineIndex;
        string Rounds;

        List<string> listOfNames;
        GraphicsDeviceManager graphics;
        List<string> PlayerNames;
        List<int> ColumnXValues;
        List<int> ColumnYValues;
        List<string> ColumnHeaders;
        List<int> HandsStated;
        List<string> HandSummary;
        bool Clicked;
        Microsoft.Xna.Framework.Input.Keys PreviousKey;

        #region Fonts
        
        SpriteBatch spriteBatch;
        SpriteFont roundFont;
        SpriteFont menuFont;
        SpriteFont InstructionsFont;
        SpriteFont titleFont;

        #endregion
        Suit trump;
        List<Card> cardsOnTableList;

        Deck deck;
        bool downKeyPressed;
        bool upKeyPressed;
        bool AreScoresCalculated;
        bool SummaryShown;
        int UpKeyPressedCount, DownKeyPressedCount;
        int OldHands, NewHands;
        Suit OldTrump, NewTrump;

        Player player2;
        Player player3;
        Player player4;
        //Dictionary<Card, PlayerIndex> cardsOnTable;
        OrderedDictionary cardsOnTable;
        //OrderedDictionary d;
        
        Texture2D backSide;
        PlayerIndex playerToPlay;
        int timeSinceLastFrame;
        Menu theMenu;

        PlayerIndex winningPlayer;
        const int CARD_SPEED = 50;
        int cardsToDeal;
        int roundNumber;
        string name;
        bool cardsDistributed;

         int optionSelected;
        int optionAnimationCounter;
        int StartYInstructions;

        SpriteFont AboutFont;
        PlayerIndex playerToStart;
        PlayerState playerState;
        int HandKeystroke;
        int titleCounter;
        int incrementValueForTitle;
        int incrementValueForMenu;
        List<int> Scores;
        int TotalScore;
        Card ClickedCard;
        // the number of hands should not be equal to the number of cards in the deck
        // this flag is used to indicate the same
        bool AreHandsValid;
        bool IsEligibleForTrumpChange;
        int framesElapsed;
        int YourTurnDisplayCount;
        bool GamePaused;

        public Judgement()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = BOARD_WIDTH;
            graphics.PreferredBackBufferHeight = BOARD_HEIGHT;
            graphics.IsFullScreen = false;

            Clicked = false;
            trump = Suit.Unspecified;
            deck = new Deck();
            deck.DistanceBetweenCards = 25;
            // turn on the mouse cursor
            this.IsMouseVisible = true;

            Rounds = "13";
            name = string.Empty;
            HandsStated = new List<int>();
            HandSummary = new List<string>();
            Content.RootDirectory = "Content";
            MidX = (BOARD_WIDTH / 2) - (CARD_WIDTH / 2);
            MidY = (BOARD_HEIGHT / 2) - (CARD_HEIGHT / 2);

            GamePaused = false;
            YourTurnDisplayCount = 0;
            UpKeyPressedCount = 0;
            DownKeyPressedCount = 0;
            SummaryLineIndex = 0;
            ClickedCard = null;
            cardsOnTableList = new List<Card>();
            //cardsOnTable = new Dictionary<Card, PlayerIndex>();
            cardsOnTable = new OrderedDictionary();
            downKeyPressed = false;

            gameStarted = true;
            playerToPlay = PlayerIndex.Two;
            roundState = RoundState.HandOver;
            timeSinceLastFrame = 0;
            AreHandsValid = true;
            cardsDistributed = false;
            //playerToStart = PlayerIndex.Four;
            IsEligibleForTrumpChange = false;
            SummaryShown = false;

            StartYInstructions = 20;
            AreScoresCalculated = false;
            
            optionSelected = 0;
            optionAnimationCounter = 0;
            PlayerNames = new List<string>();
            Scores = new List<int>();
            TotalScore = 0; // set the total score to 0
            listOfNames = new List<string>();
            listOfNames.Add("Ashwani");
            listOfNames.Add("Rahul");
            listOfNames.Add("Bhakti");
            listOfNames.Add("Akshay");
            listOfNames.Add("Sanhita");
            listOfNames.Add("Nishij");
            listOfNames.Add("Amol");
            listOfNames.Add("Prajyot");
            listOfNames.Add("Prachiti");
            listOfNames.Add("Ajit");
            listOfNames.Add("Ravi");
            listOfNames.Add("Sonia");
            listOfNames.Add("Sayli");
            listOfNames.Add("Aman");
            listOfNames.Add("Sonal");
            listOfNames.Add("Anita");
            listOfNames.Add("Prasad");
            listOfNames.Add("Vikram");
            listOfNames.Add("Sagar");
            listOfNames.Add("Sachin");
            listOfNames.Add("Shweta");
        }

        #region Deck related functions

        private void DistributeCards(int count)
        {
            List<Card> tempList = deck.Cards;
            deck.Cards = new List<Card>();

            // Cards to Player 1
            for (int index = 0; index < count; index += 4)
            {
                deck.Cards.Add(tempList[index]);
            }

            // Cards to Player 2
            for (int index = 1; index < count; index += 4)
            {
                player2.Cards.Add(tempList[index]);
            }

            // Cards to Player 3
            for (int index = 2; index < count; index += 4)
            {
                player3.Cards.Add(tempList[index]);
            }

            // Cards to Player 4
            for (int index = 3; index < count; index += 4)
            {
                player4.Cards.Add(tempList[index]);
            }
        }

        private void FilterDeck()
        {
            // filter the cards below 7. 
            List<Card> tempList = new List<Card>();
            foreach (Card card in deck.Cards)
            {
                if (((int)card.RankOfCard) > 7)
                {
                    tempList.Add(card);
                }
                else if (card.RankOfCard == Rank.Ace)
                {
                    tempList.Add(card);
                }
                // Only the 7 of hearts and 7 of spades are kept
                //else if (((int)card.RankOfCard) == 7)
                //{
                //    if (card.CardSuite == Suit.Spades || card.CardSuite == Suit.Hearts)
                //    {
                //        tempList.Add(card);
                //    }
                //}
                
            }
            deck.Cards = tempList;
        }

        private void ShuffleCards()
        {
            Card temp;
            Random randomize = new Random();
            for (int i = deck.Cards.Count - 1; i > 0; i--)
            {
                int n = randomize.Next(i + 1);

                // swap the cards
                temp = deck.Cards[i];
                deck.Cards[i] = deck.Cards[n];
                deck.Cards[n] = temp;
            }
        }

        private void FillCompleteDeck()
        {
            Card tempCard;

            List<Card> tempList = new List<Card>();
            for (int rankIndex = 0; rankIndex < 13; rankIndex++)
            {
                for (int suitIndex = 0; suitIndex < 4; suitIndex++)
                {
                    tempCard = new Card();
                    tempCard.CardSuite = (Suit)suitIndex;
                    tempCard.RankOfCard = (Rank)rankIndex;
                    tempList.Add(tempCard);
                }
            }
            deck.Cards = tempList;
        }

        #endregion

        #region Common XNA functions

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();


            titleCounter = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            roundFont = Content.Load<SpriteFont>("Palatino Linotype");
            menuFont = Content.Load<SpriteFont>("Old English Text MT");
            titleFont = Content.Load<SpriteFont>("Broadway");
            menuFont = Content.Load<SpriteFont>("Bookman Old Style");
            InstructionsFont = Content.Load<SpriteFont>("Broadway");
            //menuBackground = Content.Load<Texture2D>(@"Images/Menus/BackGround");
            AboutFont = Content.Load<SpriteFont>("Matura");
            Random random = new Random();
            theMenu = new Menu(titleFont, menuFont);
            
            
            string name;

            while (PlayerNames.Count < 3)
            {
                name = listOfNames[random.Next(listOfNames.Count - 1)];
                if (!PlayerNames.Contains(name))
                {
                    PlayerNames.Add(name);
                }
            }

            player2 = new Player();
            player2.PlayerNumber = PlayerIndex.Two;
            player2.Name = PlayerNames[0];

            player3 = new Player();
            player3.PlayerNumber = PlayerIndex.Three;
            player3.Name = PlayerNames[1];

            player4 = new Player();
            player4.PlayerNumber = PlayerIndex.Four;
            player4.Name = PlayerNames[2];

            CalculatePlayerCardPositions();

            StartNewGame();

            StringBuilder key = new StringBuilder();
            // add items to the menu
            for (int index = 0; index < 15; index++)
            {
                for (int indexString = 0; indexString < theMenu.MenuStrings.Count; indexString++)
                {
                    key = key.Remove(0, key.Length);
                    key.Append(theMenu.MenuStrings[indexString]);
                    key.Append(index);
                    theMenu.MenuScreens.Add(key.ToString(), Content.Load<Texture2D>(@"Images/Menus/" + key));
                }
            }

            Texture2D backgroundWood = Content.Load<Texture2D>(@"Images/BackgroundWood2");
            theMenu.BackgroundWood = backgroundWood;
            theMenu.MainScreen = Content.Load<Texture2D>(@"Images/Menus/MainScreen");
            theMenu.BackgroundRed = Content.Load<Texture2D>(@"Images/BackgroundRed");
            theMenu.BackgroundOrange = Content.Load<Texture2D>(@"Images/BackgroundGrayScale");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();
            System.Windows.Forms.Cursor.Current = Cursors.Arrow;
            KeyboardState currentKeyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();



            if (gameStarted)
            {
                if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    theMenu.PreviousMenuState = theMenu.MenuState;
                    theMenu.MenuState = MenuState.MainMenu;
                    theMenu.PreviousGameState = gameState;
                    GamePaused = true;
                }
                if (playerState == PlayerState.WaitingToStateHands)
                {
                    //SummaryShown = true;
                    if ((HandsStated.Count > 0) && (SummaryShown == false))
                    {
                        if (HandSummary.Count > 0)
                        {
                            SummaryShown = true;
                            theMenu.MenuState = MenuState.ShowSummary;
                            gameState = GameState.ShowMenu;
                            theMenu.PreviousGameState = GameState.RoundInProgress;
                        }

                    }
                }
                if (HandsStated.Count == 4)
                {
                    //if (SummaryShown == false)
                    //{
                        if (playerState == PlayerState.StatedHandsAndTrump)
                        {
                            // the summary is only shown if player one has also stated the trump
                            // else the last step will be missed out
                            SummaryShown = true;
                            theMenu.MenuState = MenuState.ShowSummary;
                            gameState = GameState.ShowMenu;
                            theMenu.PreviousGameState = GameState.RoundInProgress;
                            
                      //  }
                            HandsStated.Clear();
                    }
                }
                //if (GamePaused)
                //{
                //    theMenu.MenuStrings[0] = "Resume";
                //}
                //else
                //{
                //    theMenu.MenuStrings[0] = "Start";
                //}
                  
                if (gameState == GameState.RoundStarting)
                {
                    StartNewRound();
                }
                else if (gameState == GameState.RoundInProgress)
                {
                    if (roundState == RoundState.HandOver)
                    {
                        gameState = GameState.RoundInProgress;
                        // the round has begun again
                        roundState = RoundState.HandInProgress;
                        // clear the cards on the table
                        cardsOnTable.Clear();
                        cardsOnTableList.Clear();
                        roundState = RoundState.HandInProgress;
                        if (deck.Cards.Count == 0)
                        {
                            HandSummary.Clear();
                            gameState = GameState.ShowMenu;
                            theMenu.MenuState = MenuState.ShowScore;
                        }
                    }
                    else if (roundState == RoundState.HandInProgress)
                    {
                        if (cardsOnTable.Count < 4)
                        {
                            if (playerToPlay == PlayerIndex.One)
                            {
                                // its the human's turn, we dont need to do anything
                            }
                            else
                            {

                                switch (playerToPlay)
                                {
                                    case PlayerIndex.Four:
                                        PlaceCard(player4);
                                        playerToPlay = PlayerIndex.One;
                                        break;
                                    case PlayerIndex.Two:
                                        PlaceCard(player2);
                                        playerToPlay = PlayerIndex.Three;
                                        break;
                                    case PlayerIndex.Three:
                                        PlaceCard(player3);
                                        playerToPlay = PlayerIndex.Four;
                                        break;
                                    default:
                                        break;
                                }

                            }
                        }
                        else
                        {
                            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                            if (timeSinceLastFrame >= 1000)
                            {
                                timeSinceLastFrame = 0;
                                roundState = RoundState.CardsPlaced;
                                winningPlayer = FindPlayerToGetTheHand();
                                playerToPlay = winningPlayer;
                                switch (winningPlayer)
                                {
                                    case PlayerIndex.Four:
                                        UpdatePlayerHands(player4);
                                        break;
                                    case PlayerIndex.One:
                                        deck.HandsMade++;
                                        break;
                                    case PlayerIndex.Three:
                                        UpdatePlayerHands(player3);
                                        break;
                                    case PlayerIndex.Two:
                                        UpdatePlayerHands(player2);
                                        break;
                                    default:
                                        break;
                                }

                            }
                        }
                    }
                }
                else if (gameState == GameState.RoundOver)
                {
                    HandKeystroke = 0;
                    deck.Trump = Suit.Unspecified;
                    
                    cardsDistributed = false;

                    deck.HandsMade = 0;
                    deck.HandsStated = 0;
                    player2.HandsMade = 0;
                    player2.HandsStated = 0;

                    player3.HandsMade = 0;
                    player3.HandsStated = 0;

                    player4.HandsMade = 0;
                    player4.HandsStated = 0;

                    gameState = GameState.RoundStarting;
                }
                else if (gameState == GameState.GameOver)
                {
                    
                    if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                    {
                        if (EnterPressed)
                        {
                            return;
                        }
                        EnterPressed = true;
                        gameState = GameState.ShowMenu;
                        theMenu.MenuState = MenuState.ShowAbout;
                    }
                    else
                    {
                        EnterPressed = false;
                    }

                    if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (buttonPressed)
                        {
                            return;
                        }
                        gameState = GameState.ShowMenu;
                        theMenu.MenuState = MenuState.ShowAbout;
                    }
                    else
                    {
                        buttonPressed = false;
                        
                    }
                }
                if (gameState == GameState.RoundInProgress)
                {

                    // only if its the player's turn to play
                    if (playerToPlay == PlayerIndex.One)
                    {
                        MouseState currentMouseState = Mouse.GetState();

                        if (currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            // each time this look executes, if a button press is detected, the car flikers
                            // since it is picked up and kept down again. The buttonPressed variable is 
                            // used to handle this case. If buttonPressed is true, the card position is not
                            // updated, this takes care of flickering if the button is kept pressed
                            if (!buttonPressed)
                            {
                                Card cardToRemove = null;
                                buttonPressed = true;

                                int posX, posY;
                                // the x position of the placed 
                                posX = MidX;
                                posY = MidY + CARD_PLACE_OFFSET;
                                bool cardPresent = false;
                                Card playingCard ;
                                bool isClickOnCard;
                                for (int index = 0; index < deck.Cards.Count; index++)
                                {
                                    playingCard = deck.Cards[index];
                                    isClickOnCard = playingCard.IsClickOnCard(currentMouseState.X, currentMouseState.Y, deck.DistanceBetweenCards);
                                    if (!isClickOnCard)
                                    {
                                        // check for the last card
                                        if (index == deck.Cards.Count - 1)
                                        {
                                            // This is the last card (Right Most), we need to check 
                                            // click on the entire card and not jst the difference
                                            // between the cards

                                            if (currentMouseState.X >= playingCard.Position.X && currentMouseState.X <= (playingCard.Position.X + CARD_WIDTH))
                                            {
                                                if (currentMouseState.Y >= playingCard.Position.Y && currentMouseState.Y <= (playingCard.Position.Y + playingCard.Image.Height))
                                                {
                                                    isClickOnCard = true;
                                                }
                                            }
                                            else
                                            {
                                                isClickOnCard = false;
                                            }

                                            
                                        }
                                    }
                                    if (isClickOnCard)
                                    {
                                        // Check if the card is of the same suit as the 1st card
                                        if (cardsOnTable.Count > 0)
                                        {
                                            Card card = cardsOnTableList[0];
                                            if (playingCard.CardSuite != card.CardSuite)
                                            {
                                                // check if no card exists in the player's deck of the suit 
                                                // with which the hand has started
                                                foreach (Card cardToCheck in deck.Cards)
                                                {
                                                    if (card.CardSuite == cardToCheck.CardSuite)
                                                    {
                                                        cardPresent = true;
                                                        break;
                                                    }
                                                }
                                                if (cardPresent)
                                                {
                                                    playingCard.IsMarkedBlack = true;
                                                    playingCard.IsClicked = true;
                                                    ClickedCard = playingCard;
                                                }
                                                else
                                                {
                                                    // Place card
                                                    playingCard.Position = new Vector2(posX, posY);
                                                    // set the card to remove
                                                    cardToRemove = playingCard;
                                                    deck.PlacedCard = cardToRemove;
                                                    cardsOnTable.Add(cardToRemove, PlayerIndex.One);
                                                    cardsOnTableList.Add(cardToRemove);
                                                    playerToPlay = PlayerIndex.Two;

                                                }
                                            }
                                            else
                                            {
                                                // Place card
                                                playingCard.Position = new Vector2(posX, posY);
                                                // set the card to remove
                                                cardToRemove = playingCard;
                                                deck.PlacedCard = cardToRemove;
                                                cardsOnTable.Add(cardToRemove, PlayerIndex.One);
                                                cardsOnTableList.Add(cardToRemove);
                                                playerToPlay = PlayerIndex.Two;
                                            }
                                        }
                                        else
                                        {
                                            // Place card
                                            playingCard.Position = new Vector2(posX, posY);
                                            // set the card to remove
                                            cardToRemove = playingCard;
                                            deck.PlacedCard = cardToRemove;
                                            cardsOnTable.Add(cardToRemove, PlayerIndex.One);
                                            cardsOnTableList.Add(cardToRemove);
                                            playerToPlay = PlayerIndex.Two;
                                        }
                                        break;
                                    }
                                }

                                if (cardToRemove != null)
                                {
                                    deck.Cards.Remove(cardToRemove);
                                    playerToPlay = PlayerIndex.Two;
                                }
                            }
                        }
                        else
                        {
                            buttonPressed = false;
                            if (ClickedCard != null)
                            {
                                ClickedCard.IsClicked = false;
                            }
                        }
                    }
                    

                }

                if (gameState == GameState.ShowMenu)
                {
                    if (theMenu.MenuState == MenuState.MainMenu)
                    {
                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                        {
                            if (downKeyPressed)
                            {
                                return;
                            }

                            downKeyPressed = true;

                            optionSelected++;
                            optionAnimationCounter = 0;

                            if (optionSelected > 3)
                            {
                                optionSelected = 0;
                            }
                        }
                        else
                        {
                            downKeyPressed = false;
                        }
                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                        {
                            if (upKeyPressed)
                            {
                                return;
                            }

                            upKeyPressed = true;

                            optionSelected--;
                            optionAnimationCounter = 0;

                            if (optionSelected < 0)
                            {
                                optionSelected = 3;
                            }
                        }
                        else
                        {
                            upKeyPressed = false;
                        }

                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                        {
                            if (EnterPressed)
                            {
                                return;
                            }
                            EnterPressed = true;
                            switch (optionSelected)
                            {
                                case 0:
                                    //gameState = GameState.RoundStarting;
                                    //if (string.Compare(theMenu.MenuStrings[0],"Resume") == 0)
                                    //{
                                    //    //resume the game
                                    //    theMenu.MenuState = theMenu.PreviousMenuState;
                                    //    gameState = theMenu.PreviousGameState;
                                    //    GamePaused = false;
                                    //}
                                    //else
                                    //{
                                    //    theMenu.MenuState = MenuState.AskName;
                                    //}
                                    theMenu.MenuState = MenuState.AskName;
                                    gameStarted = true;
                                    break;
                                case 1:
                                    // Instructions
                                    theMenu.MenuState = MenuState.ShowInstructions;
                                    //optionSelected = 0;
                                    break;
                                case 2:
                                    // About
                                    theMenu.MenuState = MenuState.ShowAbout;
                                    //optionSelected = 0;
                                    break;
                                case 3:
                                    this.Exit();
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            EnterPressed = false;
                        }
                        Rectangle rectCoOrdinates;
                        if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {

                            rectCoOrdinates = theMenu.MenuStringRectangles[optionSelected];

                            if (rectCoOrdinates.X <= mouseState.X && (rectCoOrdinates.X + rectCoOrdinates.Width) >= (mouseState.X))
                            {
                                if (rectCoOrdinates.Y <= mouseState.Y && (rectCoOrdinates.Y + rectCoOrdinates.Height) >= (mouseState.Y))
                                {

                                    switch (optionSelected)
                                    {
                                        case 0:
                                            //gameState = GameState.RoundStarting;
                                            //if (string.Compare(theMenu.MenuStrings[0],"Resume") == 0)
                                            //{
                                            //    //resume the game
                                            //    theMenu.MenuState = theMenu.PreviousMenuState;
                                            //    gameState = theMenu.PreviousGameState;
                                            //    GamePaused = false;
                                            //}
                                            //else
                                            //{
                                            //    theMenu.MenuState = MenuState.AskName;
                                            //}
                                            theMenu.MenuState = MenuState.AskName;
                                            gameStarted = true;
                                            break;
                                        case 1:
                                            // Instructions
                                            theMenu.MenuState = MenuState.ShowInstructions;
                                            //optionSelected = 0;
                                            break;
                                        case 2:
                                            // About
                                            theMenu.MenuState = MenuState.ShowAbout;
                                            //optionSelected = 0;
                                            break;
                                        case 3:
                                            this.Exit();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            
                        }
                        // Check for the mouse
                        for (int index = 0; index < theMenu.MenuStringRectangles.Count; index++)
                        {
                            rectCoOrdinates = theMenu.MenuStringRectangles[index];

                            if (rectCoOrdinates.X <= mouseState.X && (rectCoOrdinates.X + rectCoOrdinates.Width) >= (mouseState.X))
                            {
                                if (rectCoOrdinates.Y <= mouseState.Y && (rectCoOrdinates.Y + rectCoOrdinates.Height) >= (mouseState.Y))
                                {
                                    optionSelected = index;
                                    System.Windows.Forms.Cursor.Current = Cursors.Hand;
                                    break;
                                }
                            }
 
                        }
                    }
                    else if (theMenu.MenuState == MenuState.ShowScore)
                    {
                        // if the count is same as the round number, the scores have already been 
                        // calculated
                        if (!AreScoresCalculated)
                        {
                            int score = 0;
                            AreScoresCalculated = true;
                            // Calculate score for player 2
                            player2.CalculateScore();

                            // Calculate score for player 3
                            player3.CalculateScore();

                            // Calculate score for player 4
                            player4.CalculateScore();

                            // // Calculate score for player 1
                            if (deck.HandsMade == deck.HandsStated)
                            {
                                TotalScore += deck.HandsStated * 10;
                                Scores.Add(deck.HandsStated * 10);
                            }
                            else
                            {
                                //int score;
                                if (deck.HandsStated > deck.HandsMade)
                                {
                                    score = deck.HandsMade - deck.HandsStated;
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

                        if (currentKeyboardState.GetPressedKeys().Length != 0)
                        {
                            AreScoresCalculated = false;
                            gameState = GameState.RoundInProgress;
                            // the round has begun again
                            roundState = RoundState.HandInProgress;
                            // clear the cards on the table
                            cardsOnTable.Clear();
                            cardsOnTableList.Clear();
                            roundState = RoundState.HandInProgress;
                            if (deck.Cards.Count == 0)
                            {
                                gameState = GameState.RoundOver;
                            }
                        }

                        if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            if (buttonPressed)
                            {
                                Clicked = false;
                            }

                            AreScoresCalculated = false;
                            gameState = GameState.RoundInProgress;
                            // the round has begun again
                            roundState = RoundState.HandInProgress;
                            // clear the cards on the table
                            cardsOnTable.Clear();
                            cardsOnTableList.Clear();
                            roundState = RoundState.HandInProgress;
                            if (deck.Cards.Count == 0)
                            {
                                gameState = GameState.RoundOver;
                            }
                        }
                        else
                        {
                            buttonPressed = false;
                        }

                        
                    }
                    else if (theMenu.MenuState == MenuState.ShowAbout)
                    {
                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                        {
                            if (EnterPressed)
                            {
                                return;
                            }
                            EnterPressed = true;

                            theMenu.MenuState = MenuState.MainMenu;

                        }
                        else
                        {
                            EnterPressed = false;
                        }

                        if (Clicked)
                        {
                            theMenu.MenuState = MenuState.MainMenu;
                        }

                    }
                    else if (theMenu.MenuState == MenuState.ShowInstructions)
                    {
                        int speed = 10;
                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                        {
                            if (EnterPressed)
                            {
                                return;
                            }
                            EnterPressed = true;

                            theMenu.MenuState = MenuState.MainMenu;

                        }
                        else
                        {
                            EnterPressed = false;
                        }

                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                        {
                            if (downKeyPressed)
                            {
                                DownKeyPressedCount++;
                                if (DownKeyPressedCount <= speed)
                                {
                                    return;
                                }
                                if (DownKeyPressedCount > speed)
                                {
                                    DownKeyPressedCount = 0;
                                }

                            }

                            downKeyPressed = true;

                            StartYInstructions -= 20;
                        }
                        else
                        {
                            downKeyPressed = false;
                            DownKeyPressedCount = 0;
                        }

                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                        {
                            if (upKeyPressed)
                            {
                                UpKeyPressedCount++;
                                if (UpKeyPressedCount <= speed)
                                {
                                    return;
                                }
                                // the count of 20 is to gradually scroll the screen
                                if (UpKeyPressedCount > speed)
                                {
                                    UpKeyPressedCount = 0;
                                }

                            }

                            upKeyPressed = true;
                            StartYInstructions += 20;
                        }
                        else
                        {
                            UpKeyPressedCount = 0;
                            upKeyPressed = false;
                        }
                    }
                    else if (theMenu.MenuState == MenuState.AskName)
                    {
                        GetKeyPressesName(currentKeyboardState);
                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                        {
                            if (EnterPressed)
                            {
                                return;
                            }
                            if (name != string.Empty)
                            {
                                EnterPressed = true;
                                CalculateGridForScore();
                                theMenu.MenuState = MenuState.AskRounds;
                            }
                        }
                        else
                        {
                            EnterPressed = false;
                        }
                    }
                    else if (theMenu.MenuState == MenuState.AskRounds)
                    {
                        GetKeyPressesRounds(currentKeyboardState);
                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                        {
                            if (EnterPressed)
                            {
                                return;
                            }
                            EnterPressed = true;
                            try
                            {
                                int rounds = Convert.ToInt32(Rounds);
                                if (rounds <= 13 && rounds > 0)
                                {
                                    gameState = GameState.RoundStarting;
                                    gameStarted = true;
                                    roundNumber = 13 - rounds;
                                }
                            }
                            catch (Exception)
                            {
                                // do nothing for now
                            }
  
                        }
                        else
                        {
                            EnterPressed = false;
                        }
                    }
                    else if (theMenu.MenuState == MenuState.AskTrump)
                    {
                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                        {
                            if (downKeyPressed)
                            {
                                return;
                            }

                            downKeyPressed = true;

                            optionSelected++;

                            if (optionSelected > 3)
                            {
                                optionSelected = 0;
                            }
                        }
                        else
                        {
                            downKeyPressed = false;
                        }

                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                        {
                            if (upKeyPressed)
                            {
                                return;
                            }

                            upKeyPressed = true;

                            optionSelected--;
                            if (optionSelected < 0)
                            {
                                optionSelected = 3;
                            }
                        }
                        else
                        {
                            upKeyPressed = false;
                        }

                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D1)
                            || currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad1))
                        {
                            optionSelected = 0;
                        }

                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D2)
                            || currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad1))
                        {
                            optionSelected = 1;
                        }

                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D3)
                            || currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad3))
                        {
                            optionSelected = 2;
                        }

                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D4)
                            || currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad4))
                        {
                            optionSelected = 3;
                        }

                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                        {
                            if (EnterPressed)
                            {
                                return;
                            }

                            EnterPressed = true;
                            if (optionSelected != -1)
                            {
                                playerState = PlayerState.StatedHandsAndTrump;
                                gameState = GameState.RoundStarting;
                                switch (optionSelected)
                                {
                                    case 0:
                                        trump = Suit.Hearts;
                                        HandSummary.Add(name + " stated the maximum hands.");
                                        HandSummary.Add("Hence, " + name + " chose the trump which is Hearts.");
                                        break;
                                    case 1:
                                        trump = Suit.Diamonds;
                                        HandSummary.Add(name + " stated the maximum hands.");
                                        HandSummary.Add("Hence, " + name + " chose the trump which is Diamonds.");
                                        break;
                                    case 2:
                                        trump = Suit.Clubs;
                                        HandSummary.Add(name + " stated the maximum hands.");
                                        HandSummary.Add("Hence, " + name + " chose the trump which is Clubs.");
                                        break;
                                    case 3:
                                        trump = Suit.Spades;
                                        HandSummary.Add(name + " stated the maximum hands.");
                                        HandSummary.Add("Hence, " + name + " chose the trump which is Spades.");
                                        break;
                                    default:
                                        break;
                                }
                            }



                        }
                        else
                        {
                            EnterPressed = false;
                        }
                        Rectangle rectCoOrdinates;
                        for (int index = 0; index < theMenu.TrumpStringRectangles.Count; index++)
                        {
                            rectCoOrdinates = theMenu.TrumpStringRectangles[index];

                            if (rectCoOrdinates.X <= mouseState.X && (rectCoOrdinates.X + rectCoOrdinates.Width) >= (mouseState.X))
                            {
                                if (rectCoOrdinates.Y <= mouseState.Y && (rectCoOrdinates.Y + rectCoOrdinates.Height) >= (mouseState.Y))
                                {
                                    optionSelected = index;
                                    System.Windows.Forms.Cursor.Current = Cursors.Hand;
                                    break;
                                }
                            }

                        }

                        if (Clicked)
                        {
                            rectCoOrdinates = theMenu.TrumpStringRectangles[optionSelected];

                            if (rectCoOrdinates.X <= mouseState.X && (rectCoOrdinates.X + rectCoOrdinates.Width) >= (mouseState.X))
                            {
                                if (rectCoOrdinates.Y <= mouseState.Y && (rectCoOrdinates.Y + rectCoOrdinates.Height) >= (mouseState.Y))
                                {
                                    playerState = PlayerState.StatedHandsAndTrump;
                                    gameState = GameState.RoundStarting;
                                    switch (optionSelected)
                                    {
                                        case 0:
                                            trump = Suit.Hearts;
                                            HandSummary.Add(name + " stated the maximum hands.");
                                            HandSummary.Add("Hence, " + name + " chose the trump which is Hearts.");
                                            break;
                                        case 1:
                                            trump = Suit.Diamonds;
                                            HandSummary.Add(name + " stated the maximum hands.");
                                            HandSummary.Add("Hence, " + name + " chose the trump which is Diamonds.");
                                            break;
                                        case 2:
                                            trump = Suit.Clubs;
                                            HandSummary.Add(name + " stated the maximum hands.");
                                            HandSummary.Add("Hence, " + name + " chose the trump which is Clubs.");
                                            break;
                                        case 3:
                                            trump = Suit.Spades;
                                            HandSummary.Add(name + " stated the maximum hands.");
                                            HandSummary.Add("Hence, " + name + " chose the trump which is Spades.");
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else if (theMenu.MenuState == MenuState.AskHands)
                    {
                        if (currentKeyboardState.GetPressedKeys().Length != 0)
                        {
                            switch (currentKeyboardState.GetPressedKeys()[0])
                            {
                                case Microsoft.Xna.Framework.Input.Keys.NumPad0:
                                case Microsoft.Xna.Framework.Input.Keys.D0:
                                    HandKeystroke = 0;
                                    break;
                                case Microsoft.Xna.Framework.Input.Keys.NumPad1:
                                case Microsoft.Xna.Framework.Input.Keys.D1:
                                    HandKeystroke = 1;
                                    break;
                                case Microsoft.Xna.Framework.Input.Keys.NumPad2:
                                case Microsoft.Xna.Framework.Input.Keys.D2:
                                    HandKeystroke = 2;
                                    break;
                                case Microsoft.Xna.Framework.Input.Keys.NumPad3:
                                case Microsoft.Xna.Framework.Input.Keys.D3:
                                    HandKeystroke = 3;
                                    break;
                                case Microsoft.Xna.Framework.Input.Keys.NumPad4:
                                case Microsoft.Xna.Framework.Input.Keys.D4:
                                    HandKeystroke = 4;
                                    break;
                                case Microsoft.Xna.Framework.Input.Keys.NumPad5:
                                case Microsoft.Xna.Framework.Input.Keys.D5:
                                    HandKeystroke = 5;
                                    break;
                                case Microsoft.Xna.Framework.Input.Keys.NumPad6:
                                case Microsoft.Xna.Framework.Input.Keys.D6:
                                    HandKeystroke = 6;
                                    break;
                                case Microsoft.Xna.Framework.Input.Keys.NumPad7:
                                case Microsoft.Xna.Framework.Input.Keys.D7:
                                    HandKeystroke = 7;
                                    break;
                                case Microsoft.Xna.Framework.Input.Keys.NumPad8:
                                case Microsoft.Xna.Framework.Input.Keys.D8:
                                    HandKeystroke = 8;
                                    break;
                                case Microsoft.Xna.Framework.Input.Keys.NumPad9:
                                case Microsoft.Xna.Framework.Input.Keys.D9:
                                    HandKeystroke = 9;
                                    break;
                                //case Keys.C:
                                //    if (CheckIfEligibleForTrumpChange(HandKeystroke))
                                //    {
                                //        theMenu.MenuState = MenuState.AskTrump;
                                //    }

                                    //break;
                                default:
                                    break;
                            }

                            if (playerToStart == PlayerIndex.Two)
                            {
                                // we only need to check for the total hands and number of cards
                                // condition if the player is the last one to state the hands

                                int totalHands = player2.HandsStated + player3.HandsStated +
                                    player4.HandsStated + HandKeystroke;

                                if (totalHands == deck.Cards.Count)
                                {
                                    AreHandsValid = false;
                                }
                                else
                                {
                                    AreHandsValid = true;
                                }

                            }

                            if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                            {
                                if (EnterPressed)
                                {
                                    return;
                                }
                                EnterPressed = true;

                                if (AreHandsValid)
                                {
                                    gameState = GameState.RoundStarting;
                                    playerState = PlayerState.StatedHands;
                                    deck.HandsStated = HandKeystroke;
                                    HandSummary.Add(name + " stated " + HandKeystroke + " hands.");
                                    HandsStated.Add(HandKeystroke);
                                    SummaryShown = false;
                                }

                            }
                            else
                            {
                                EnterPressed = false;
                            }

                            if (Clicked)
                            {
                                if (AreHandsValid)
                                {
                                    gameState = GameState.RoundStarting;
                                    playerState = PlayerState.StatedHands;
                                    deck.HandsStated = HandKeystroke;
                                    HandSummary.Add(name + " stated " + HandKeystroke + " hands.");
                                    HandsStated.Add(HandKeystroke);
                                    SummaryShown = false;
                                }

                            }

                        }
                    }
                    else if (theMenu.MenuState == MenuState.ShowSummary)
                    {
                        if (currentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                        {
                            if (EnterPressed)
                            {
                                return;
                            }
                            EnterPressed = true;
                            if (theMenu.PreviousGameState == GameState.RoundInProgress)
                            {
                                gameState = GameState.RoundStarting;
                            }
                            else
                            {
                                theMenu.MenuState = theMenu.PreviousMenuState;
                            }


                        }
                        else
                        {
                            EnterPressed = false;
                        }

                        if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            if (buttonPressed)
                            {
                                Clicked = false;
                            }

                            if (AreHandsValid)
                            {
                                if (theMenu.PreviousGameState == GameState.RoundInProgress)
                                {
                                    gameState = GameState.RoundStarting;
                                }
                                else
                                {
                                    theMenu.MenuState = theMenu.PreviousMenuState;
                                }
                            }
                        }
                        else
                        {
                            buttonPressed = false;
                        }
                        
                    }
                }

                base.Update(gameTime);
            }
        }

        private bool CheckIfEligibleForTrumpChange(int playerHands)
        {
            IsEligibleForTrumpChange = true;

            foreach (int hands in HandsStated)
            {
                if (playerHands < hands)
                {
                    IsEligibleForTrumpChange = false;
                    break;
                }
            }

            return IsEligibleForTrumpChange;

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            if (gameStarted)
            {
                spriteBatch.Begin();
                Rectangle destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);
                spriteBatch.Draw(theMenu.BackgroundWood, destinationRectBkgrnd, Color.White);
                spriteBatch.End();

                if (gameState == GameState.RoundStarting)
                {
                    if (cardsDistributed)
                    {
                        DrawPlayer1Cards();
                        DrawAllCards();
                        DrawCardsOnTable();
                    }
                }
                else if (gameState == GameState.RoundOver)
                {

                }
                else if (gameState == GameState.GameOver)
                {
                    Player playerToWin = GetPlayerWithMaxScore();
                    string winnerName = string.Empty;
                    string output;
                    int startX = 20;
                    // Draw Hello World
                  
                    Color textColor = Color.Azure;

                    Vector2 FontOrigin;
                    Vector2 FontPos;
                    int startY = 20;
                    //int startX;
                    destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);
                    // Draw the string
                    spriteBatch.Begin();
                    spriteBatch.Draw(theMenu.BackgroundRed, destinationRectBkgrnd, Color.DarkGray);
                    //string output;

                    if (playerToWin == null)
                    {
                        output = "CONGRATULATIONS!!!";
                        winnerName = "You have ";
                        FontOrigin = InstructionsFont.MeasureString(output) / 2;
                        startX = (int)(BOARD_WIDTH / 2 - FontOrigin.X * 0.5f);
                        FontPos = new Vector2(startX, startY);
                        spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
                    0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
                    }
                    else
                    {
                        winnerName = playerToWin.Name + " has";
                    }


                    output = winnerName + "got the maximum score.";
                    FontOrigin = AboutFont.MeasureString(output);
                    startY = (int)((BOARD_HEIGHT / 2 - FontOrigin.Y * 0.6) - 75);
                    FontPos = new Vector2(BOARD_WIDTH / 2 - FontOrigin.X * 0.6f, startY);
                    spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
        0, Vector2.Zero, 1, SpriteEffects.None, 0.5f);

                    output = winnerName + "won the game.";
                    FontPos = new Vector2(40, 150);


                    spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
0, Vector2.Zero, 1, SpriteEffects.None, 0.5f);

                    spriteBatch.End();
                }
                else if (gameState == GameState.RoundInProgress)
                {
                    DrawPlayer1Cards();
                    DrawAllCards();
                    DrawCardsOnTable();

                    if (roundState == RoundState.CardsPlaced)
                    {
                        float prevX = 0, prevY = 0;
                        switch (winningPlayer)
                        {
                            case PlayerIndex.Two:

                                foreach (Card card in cardsOnTable.Keys)
                                {
                                    prevX = card.Position.X;
                                    prevY = card.Position.Y;
                                    if (prevX > BOARD_WIDTH)
                                    {
                                        roundState = RoundState.HandOver;
                                        break;
                                    }
                                    card.Position = new Vector2(prevX + CARD_SPEED, prevY);
                                    card.Draw(spriteBatch);
                                }


                                break;
                            case PlayerIndex.One:
                                foreach (Card card in cardsOnTable.Keys)
                                {
                                    prevX = card.Position.X;
                                    prevY = card.Position.Y;
                                    if (prevY > BOARD_HEIGHT)
                                    {
                                        roundState = RoundState.HandOver;
                                        break;
                                    }
                                    card.Position = new Vector2(prevX, prevY + CARD_SPEED);
                                    card.Draw(spriteBatch);
                                }
                                break;
                            case PlayerIndex.Three:
                                foreach (Card card in cardsOnTable.Keys)
                                {
                                    prevX = card.Position.X;
                                    prevY = card.Position.Y;
                                    if (prevY < 0)
                                    {
                                        roundState = RoundState.HandOver;
                                        break;
                                    }
                                    card.Position = new Vector2(prevX, prevY - CARD_SPEED);
                                    card.Draw(spriteBatch);
                                }
                                break;
                            case PlayerIndex.Four:
                                foreach (Card card in cardsOnTable.Keys)
                                {
                                    prevX = card.Position.X;
                                    prevY = card.Position.Y;
                                    if (prevX < 0)
                                    {
                                        roundState = RoundState.HandOver;
                                        break;
                                    }
                                    card.Position = new Vector2(prevX - CARD_SPEED, prevY);
                                    card.Draw(spriteBatch);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (gameState == GameState.ShowMenu)
                {
                    if (theMenu.MenuState == MenuState.MainMenu)
                    {
                        ShowMenu();
                    }
                    else if (theMenu.MenuState == MenuState.AskTrump)
                    {
                        AskTrump();
                    }
                    else if (theMenu.MenuState == MenuState.AskHands)
                    {
                        AskHands();
                    }
                    else if (theMenu.MenuState == MenuState.ShowScore)
                    {
                        DrawScoreSheet();
                    }
                    else if (theMenu.MenuState == MenuState.ShowSummary)
                    {
                        DrawSummary();
                    }
                    else if (theMenu.MenuState == MenuState.ShowInstructions)
                    {
                        ShowInstructions(StartYInstructions);
                    }
                    else if (theMenu.MenuState == MenuState.ShowAbout)
                    {
                        ShowAbout();
                    }
                    else if (theMenu.MenuState == MenuState.AskName)
                    {
                        AskName();
                    }
                    else if (theMenu.MenuState == MenuState.AskRounds)
                    {
                        AskRounds();
                    }
                }
            }

            base.Draw(gameTime);
        }

        #endregion

        #region Game related functions

        #region Draw related functions

        private void DrawSummary()
        {
            framesElapsed++;
            Rectangle destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);

            spriteBatch.Begin();
            spriteBatch.Draw(theMenu.BackgroundRed, destinationRectBkgrnd, Color.DarkGray);
            string output = "Summary";

            Vector2 FontOrigin = menuFont.MeasureString(output);
            Vector2 FontPos = new Vector2((BOARD_WIDTH / 2) - (FontOrigin.X / 2), 50 - FontOrigin.Y / 2);
            spriteBatch.DrawString(menuFont, output, FontPos, Color.Silver,
                0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            int linesToShow = SummaryLineIndex;


            // 1 sec passed
            Color textColor = Color.White;

            try
            {
                for (int index = 0; index < linesToShow; index++)
                {
                    //if (framesElapsed == 120)
                    //{
                        framesElapsed = 0;
                        output = HandSummary[index];
                        FontOrigin = menuFont.MeasureString(output);
                        // the font is half of its original size, hence we divide by 4
                        float x = (float)(BOARD_WIDTH / 2) - (float)((FontOrigin.X * 0.55) / 2);
                        float y = 100 + index * 70;
                        FontPos = new Vector2(x, y);
                        spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                            0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);
                    //}
                }
                SummaryLineIndex++;
            }
            catch (ArgumentOutOfRangeException)
            {
                // Do nothing
            }

            output = "Press any key to continue...";

            FontOrigin = menuFont.MeasureString(output);
            // the font is half of its original size, hence we divide by 4
            FontPos = new Vector2(BOARD_WIDTH - FontOrigin.X - 10, BOARD_HEIGHT - FontOrigin.Y - 10);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        private void DrawScoreSheet()
        {
            Rectangle destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);

            string output;

            Color textColor = Color.White;
            // Find the center of the string
            Vector2 FontOrigin;
            Vector2 FontPos;

            // Draw the string
            spriteBatch.Begin();
            spriteBatch.Draw(theMenu.BackgroundRed, destinationRectBkgrnd, Color.DarkGray);
            output = "Score Sheet";

            FontOrigin = menuFont.MeasureString(output);
            FontPos = new Vector2((BOARD_WIDTH / 2) - (FontOrigin.X / 2), 50 - FontOrigin.Y / 2);
            spriteBatch.DrawString(menuFont, output, FontPos, Color.Silver,
                0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

            textColor = Color.White; 

            for (int index = 0; index < ColumnXValues.Count; index++)
            {
                output = ColumnHeaders[index];
                FontOrigin = menuFont.MeasureString(output);
                // the font is half of its original size, hence we divide by 4
                FontPos = new Vector2(ColumnXValues[index] - (FontOrigin.X / 4), ColumnYValues[0] - (FontOrigin.Y / 2));
                spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                    0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);
            }
            int xIndex;
            int scoreIndex;
            textColor = Color.SkyBlue; 
            for (scoreIndex = 0; scoreIndex < Scores.Count; scoreIndex++)
            {
                xIndex = 0;
                
                // Round number
                output = (scoreIndex + 1).ToString();
                FontOrigin = menuFont.MeasureString(output);
                // the font is half of its original size, hence we divide by 4
                FontPos = new Vector2(ColumnXValues[xIndex] - (FontOrigin.X / 4), ColumnYValues[scoreIndex + 1] - (FontOrigin.Y / 2));
                spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                    0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

                xIndex++;
                // Player 1
                output = (Scores[scoreIndex]).ToString();
                FontOrigin = menuFont.MeasureString(output);
                // the font is half of its original size, hence we divide by 4
                FontPos = new Vector2(ColumnXValues[xIndex] - (FontOrigin.X / 4), ColumnYValues[scoreIndex + 1] - (FontOrigin.Y / 2));
                spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                    0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

                xIndex++;
                // Player 2
                output = (player2.Scores[scoreIndex]).ToString();
                FontOrigin = menuFont.MeasureString(output);
                // the font is half of its original size, hence we divide by 4
                FontPos = new Vector2(ColumnXValues[xIndex] - (FontOrigin.X / 4), ColumnYValues[scoreIndex + 1] - (FontOrigin.Y / 2));
                spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                    0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

                xIndex++;
                // Player 3
                output = (player3.Scores[scoreIndex]).ToString();
                FontOrigin = menuFont.MeasureString(output);
                // the font is half of its original size, hence we divide by 4
                FontPos = new Vector2(ColumnXValues[xIndex] - (FontOrigin.X / 4), ColumnYValues[scoreIndex + 1] - (FontOrigin.Y / 2));
                spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                    0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

                xIndex++;
                // Player 4
                output = (player4.Scores[scoreIndex]).ToString();
                FontOrigin = menuFont.MeasureString(output);
                // the font is half of its original size, hence we divide by 4
                FontPos = new Vector2(ColumnXValues[xIndex] - (FontOrigin.X / 4), ColumnYValues[scoreIndex + 1] - (FontOrigin.Y / 2));
                spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                    0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);
            }

            // show the total scores

            xIndex = 0;
            textColor = Color.White;
            // Round number
            output = "Total:";
            FontOrigin = menuFont.MeasureString(output);
            // the font is half of its original size, hence we divide by 4
            FontPos = new Vector2(ColumnXValues[xIndex] - (FontOrigin.X / 4), ColumnYValues[scoreIndex + 1] - (FontOrigin.Y / 2));
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

            xIndex++;
            // Player 1
            output = TotalScore.ToString();
            FontOrigin = menuFont.MeasureString(output);
            // the font is half of its original size, hence we divide by 4
            FontPos = new Vector2(ColumnXValues[xIndex] - (FontOrigin.X / 4), ColumnYValues[scoreIndex + 1] - (FontOrigin.Y / 2));
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

            xIndex++;
            // Player 2
            output = player2.TotalScore.ToString();
            FontOrigin = menuFont.MeasureString(output);
            // the font is half of its original size, hence we divide by 4
            FontPos = new Vector2(ColumnXValues[xIndex] - (FontOrigin.X / 4), ColumnYValues[scoreIndex + 1] - (FontOrigin.Y / 2));
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

            xIndex++;
            // Player 3
            output = player3.TotalScore.ToString();
            FontOrigin = menuFont.MeasureString(output);
            // the font is half of its original size, hence we divide by 4
            FontPos = new Vector2(ColumnXValues[xIndex] - (FontOrigin.X / 4), ColumnYValues[scoreIndex + 1] - (FontOrigin.Y / 2));
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

            xIndex++;
            // Player 4
            output = player4.TotalScore.ToString();
            FontOrigin = menuFont.MeasureString(output);
            // the font is half of its original size, hence we divide by 4
            FontPos = new Vector2(ColumnXValues[xIndex] - (FontOrigin.X / 4), ColumnYValues[scoreIndex + 1] - (FontOrigin.Y / 2));
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

            output = "Press any key to continue...";

            FontOrigin = menuFont.MeasureString(output);
            // the font is half of its original size, hence we divide by 4
            FontPos = new Vector2(BOARD_WIDTH - FontOrigin.X - 10, BOARD_HEIGHT - FontOrigin.Y - 10);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                0, Vector2.Zero, 0.55f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        private void DrawPlayer1Cards()
        {
            float posX, posY;

            // PLAYER 1 Calculations
            // Calculate the LEFT, this is from where the cards will be placed for player 1 and 3
            // LeftPlayer1 = (BOARD_WIDTH / 2) - Total width of the complete deck
            // Total width of deck = Distance between the cards * number of cards 
            //                      + width of the last card
            float temp;
            posX = (BOARD_WIDTH / 2) -
                (deck.DistanceBetweenCards * deck.Cards.Count + CARD_WIDTH - deck.DistanceBetweenCards) / 2;
            // store the value in a temp variable so that we don't have top calculate it again
            temp = posX;
            posY = BOARD_HEIGHT - CARD_HEIGHT - OFFSET;

            if (playerToPlay == PlayerIndex.One)
            {
                YourTurnDisplayCount++;
                // we need the 'Your Turn' string to blink
                if (YourTurnDisplayCount > 40 && YourTurnDisplayCount < 80)
                {
                    // Find the center of the string
                    Vector2 FontOrigin;
                    Vector2 FontPos;

                    // Draw the string
                    string output = "Your Turn!!!";

                    FontOrigin = menuFont.MeasureString(output);
                    FontPos = new Vector2((BOARD_WIDTH / 2) - (FontOrigin.X * 0.4f * 0.5f), posY - FontOrigin.Y / 2);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(menuFont, output, FontPos, Color.Firebrick,
                        0, Vector2.Zero, 0.4f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                }

                if (YourTurnDisplayCount > 80)
                {
                    YourTurnDisplayCount = 0;
                }
            }
            for (int index = 0; index < deck.Cards.Count; index++)
            {
                posX += deck.DistanceBetweenCards;
                deck.Cards[index].Position = new Vector2(posX, posY);
                //x += index * 31;
                deck.Cards[index].Draw(spriteBatch);
            }
        }

        private void DrawAllCards()
        {
            DrawPlayer1Cards();

            float posX, posY;
            float temp;
            int lengthOfSuit;

            posX = (BOARD_WIDTH / 2) -
                (deck.DistanceBetweenCards * deck.Cards.Count + CARD_WIDTH - deck.DistanceBetweenCards) / 2;
            // store the value in a temp variable so that we don't have top calculate it again
            temp = posX;
            posY = BOARD_HEIGHT - CARD_HEIGHT - OFFSET;
            Color textColor = Color.Black;

            spriteBatch.Begin();
            string statistics = name.ToString() +": " + deck.HandsMade.ToString() + " / " + deck.HandsStated.ToString();

            Vector2 FontOrigin = roundFont.MeasureString(statistics);
            // Draw the string
            spriteBatch.DrawString(roundFont, statistics, new Vector2(temp - FontOrigin.X, BOARD_HEIGHT - 25), textColor,
                0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            // State the trump
            statistics = "Trump: " + deck.Trump;

            FontOrigin = roundFont.MeasureString(statistics);
            // Draw the string
            spriteBatch.DrawString(roundFont, statistics, new Vector2(BOARD_WIDTH - FontOrigin.X, BOARD_HEIGHT - 25), textColor,
                0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            // Player 2
            posX = BOARD_WIDTH - CARD_WIDTH - OFFSET; ;
            lengthOfSuit = (deck.Cards.Count * deck.DistanceBetweenCards) + (int)CARD_HEIGHT - deck.DistanceBetweenCards;
            posY = (BOARD_HEIGHT / 2) - (lengthOfSuit / 2);

            statistics = player2.Name + ": " + player2.HandsMade.ToString() + " / " + player2.HandsStated.ToString();

            FontOrigin = roundFont.MeasureString(statistics);
            // Draw the string
            spriteBatch.DrawString(roundFont, statistics, new Vector2(posX, posY - 10), textColor,
                0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            Rectangle destinationRectangle;
            for (int index = 0; index < player2.Cards.Count; index++)
            {

                posY += deck.DistanceBetweenCards;
                //x += index * 31;
                destinationRectangle = new Rectangle(Convert.ToInt32(posX),
                                    Convert.ToInt32(posY),
                                    Convert.ToInt32(CARD_WIDTH),
                                    Convert.ToInt32(CARD_HEIGHT));

                spriteBatch.Draw(backSide, destinationRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }


            // PLAYER 3 Calculations

            // Draw the cards of all the other players
            posX = temp;
            posY = OFFSET;

            statistics = player3.Name + ": " + player3.HandsMade.ToString() + " / " + player3.HandsStated.ToString();

            // Draw the string
            spriteBatch.DrawString(roundFont, statistics, new Vector2(posX - FontOrigin.X, 10), textColor,
                0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            for (int index = 0; index < player3.Cards.Count; index++)
            {
                posX += deck.DistanceBetweenCards;
                destinationRectangle = new Rectangle(Convert.ToInt32(posX),
                                    Convert.ToInt32(posY),
                                    Convert.ToInt32(CARD_WIDTH),
                                    Convert.ToInt32(CARD_HEIGHT));

                spriteBatch.Draw(backSide, destinationRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }



            // Player 4
            posX = OFFSET;
            posY = (BOARD_HEIGHT / 2) -
                (deck.DistanceBetweenCards * deck.Cards.Count + CARD_HEIGHT - deck.DistanceBetweenCards) / 2;
            // store the value in a temp variable so that we don't have top calculate it again
            temp = posY;

            statistics = player4.Name + ": " + player4.HandsMade.ToString() + " / " + player4.HandsStated.ToString();

            // Draw the string
            spriteBatch.DrawString(roundFont, statistics, new Vector2(posX, posY - 10), textColor,
                0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            for (int index = 0; index < player4.Cards.Count; index++)
            {
                posY += deck.DistanceBetweenCards;
                //x += index * 31;
                destinationRectangle = new Rectangle(Convert.ToInt32(posX),
                                    Convert.ToInt32(posY),
                                    Convert.ToInt32(CARD_WIDTH),
                                    Convert.ToInt32(CARD_HEIGHT));

                spriteBatch.Draw(backSide, destinationRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }


            spriteBatch.End();
        }

        /// <summary>
        /// Function to Draw the cards placed on the table
        /// </summary>
        private void DrawCardsOnTable()
        {

            foreach (Card card in cardsOnTable.Keys)
            {
                if (card.Image == null)
                {
                    string path = @"Images/Cards/" + card.CardSuite + @"/" + card.RankOfCard;
                    card.Image = Content.Load<Texture2D>(path);
                }

                card.Draw(spriteBatch);
            }
        }

        #endregion

        #region Menu related functions

        private void AskTrump()
        {
            int startX = 20, startY = 20, increment = 75;
            Rectangle destinationRectBkgrnd = new Rectangle();
            destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);

            // Draw Hello World
            string output = "Hi " + name + ", you've stated the maximum hands.";
            
            Color textColor = Color.White;

            // Find the center of the string
            Vector2 FontOrigin = menuFont.MeasureString(output) / 2;
            Vector2 FontPos = new Vector2(startX, startY);
            // Draw the string
            spriteBatch.Begin();
            spriteBatch.Draw(theMenu.BackgroundRed, destinationRectBkgrnd, Color.DarkGray);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
    0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.5f);
            
            startY += increment;
            output = "Please select the Trump.";
            FontPos = new Vector2(startX, startY);
                        spriteBatch.DrawString(menuFont, output, FontPos, textColor,
    0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.5f);

    //        output = "Trump.";

    //        FontPos = new Vector2(startX, startY);
    //        spriteBatch.DrawString(menuFont, output, FontPos, textColor,
    //0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            output = "Please select one suit:";
            startY += increment;
            //startY += increment;

            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
    0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.5f);

            for (int index = 0; index < theMenu.SuitStrings.Count; index++)
            {
                output = theMenu.SuitStrings[index];

                startY += increment;
                if (optionSelected == index)
                {
                    FontPos = new Vector2(startX, startY);
                    spriteBatch.DrawString(menuFont, output, FontPos, Color.Yellow,
                    0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.5f);
                }
                else
                {
                    FontPos = new Vector2(startX, startY);
                    spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                    0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.5f);
                }

            }

            spriteBatch.End();

            // Show cards to the player
            float posX = (BOARD_WIDTH / 2) -
            (deck.DistanceBetweenCards * deck.Cards.Count + CARD_WIDTH - deck.DistanceBetweenCards) / 2;
            // store the value in a temp variable so that we don't have top calculate it again
            float posY = BOARD_HEIGHT - CARD_HEIGHT - OFFSET;

            for (int index = 0; index < deck.Cards.Count; index++)
            {
                posX += deck.DistanceBetweenCards;
                deck.Cards[index].Position = new Vector2(posX, posY);
                //x += index * 31;
                deck.Cards[index].Draw(spriteBatch);
            }
        }

        private void AskHands()
        {
            int startX = 20, startY = 20, increment = 65;
            // Draw Hello World
            string output = "Please state the number of hands you want to make.";
            Color textColor = Color.White;
            float fontSize = 0.6f;

            // Find the center of the string
            Vector2 FontOrigin = menuFont.MeasureString(output) / 2;
            Vector2 FontPos = new Vector2(startX, startY);
            Rectangle destinationRectBkgrnd = new Rectangle();
            destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);
            // Draw the string
            spriteBatch.Begin();
            spriteBatch.Draw(theMenu.BackgroundRed, destinationRectBkgrnd, Color.DarkGray);

            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "Following is the summary: ";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = name + ": " + HandKeystroke.ToString();

            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);
            if (!AreHandsValid)
            {
                FontPos = new Vector2(startX + menuFont.MeasureString(output).X * 0.7f ,startY + 5);
                output = "(Number of hands should not equal the number of cards)";

                spriteBatch.DrawString(menuFont, output, FontPos, Color.SkyBlue,
                    0, Vector2.Zero, (fontSize - 0.2f), SpriteEffects.None, 0.5f);
            }
            startY += (increment - 10);
            

            if (player2.HandsStated == 0 && player2.PlayerState == PlayerState.WaitingToStateHands)
            {
                output = player2.Name + ": (To be stated)";
            }
            else
            {
                output = player2.Name + ": " + player2.HandsStated.ToString();
            }

            startY += 10;

            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
                0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);


            if (player3.HandsStated == 0 && player3.PlayerState == PlayerState.WaitingToStateHands)
            {
                output = player3.Name + ": (To be stated)";
            }
            else
            {
                output = player3.Name + ": " + player3.HandsStated.ToString();
            }

            startY += increment;

            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);


            if (player4.HandsStated == 0 && player4.PlayerState == PlayerState.WaitingToStateHands)
            {
                output = player4.Name + ": (To be stated)";
            }
            else
            {
                output = player4.Name + ": " + player4.HandsStated.ToString();
            }

            startY += increment;

            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            startY += increment;

            YourTurnDisplayCount++;
            // we need the message string to blink
            if (YourTurnDisplayCount > 40 && YourTurnDisplayCount < 80)
            {
                // Find the center of the string
                // Draw the string
                output = "Press the respective number key to state the hands and Enter to continue)";

                FontOrigin = menuFont.MeasureString(output);
                FontPos = new Vector2((BOARD_WIDTH / 2) - (FontOrigin.X * 0.4f * 0.5f), BOARD_HEIGHT - CARD_HEIGHT - OFFSET - FontOrigin.Y / 2);
                spriteBatch.DrawString(menuFont, output, FontPos, Color.Silver,
                    0, Vector2.Zero, 0.4f, SpriteEffects.None, 0.5f);
            }

            if (YourTurnDisplayCount > 80)
            {
                YourTurnDisplayCount = 0;
            }

    //        output = "(Press the respective number key to state the hands)";
    //        FontPos = new Vector2(startX, startY);
    //        spriteBatch.DrawString(menuFont, output, FontPos, textColor,
    //0, Vector2.Zero, (fontSize - 0.1f), SpriteEffects.None, 0.5f);

            spriteBatch.End();

            // Show cards to the player
            float posX = (BOARD_WIDTH / 2) -
            (deck.DistanceBetweenCards * deck.Cards.Count + CARD_WIDTH - deck.DistanceBetweenCards) / 2;
            // store the value in a temp variable so that we don't have top calculate it again
            float posY = BOARD_HEIGHT - CARD_HEIGHT - OFFSET;

            for (int index = 0; index < deck.Cards.Count; index++)
            {
                posX += deck.DistanceBetweenCards;
                deck.Cards[index].Position = new Vector2(posX, posY);
                //x += index * 31;
                deck.Cards[index].Draw(spriteBatch);
            }


        }

        private void GetKeyPressesName(KeyboardState keyboardState)
        {
            if (keyboardState.GetPressedKeys().Length == 0)
            {
                PreviousKey = Microsoft.Xna.Framework.Input.Keys.Zoom;
                return;
            }
            // if a key is kept pressed. we only need to take its 1st instance

            if (keyboardState.IsKeyUp(PreviousKey))
            {
                PreviousKey = Microsoft.Xna.Framework.Input.Keys.Zoom;
            }

            if (keyboardState.IsKeyDown(PreviousKey))
            {
                return;
            }

            PreviousKey = keyboardState.GetPressedKeys()[0];
             switch (keyboardState.GetPressedKeys()[0])
             {
                 case Microsoft.Xna.Framework.Input.Keys.A:
                     name += "a";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.B:
                     name += "b";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.C:
                     name += "c";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D:
                     name += "d";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D0:
                     name += "0";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D1:
                     name += "1";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D2:
                     name += "2";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D3:
                     name += "3";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D4:
                     name += "4";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D5:
                     name += "5";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D6:
                     name += "6";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D7:
                     name += "7";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D8:
                     name += "8";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.D9:
                     name += "9";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.E:
                     name += "e";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.Space:
                     name += " ";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.Back:
                     if (name.Length > 0)
                     {
                         name = name.Substring(0, name.Length - 1);
                     }
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.F:
                     name += "f";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.G:
                     name += "g";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.H:
                     name += "h";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.I:
                     name += "i";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.J:
                     name += "j";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.K:
                     name += "k";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.L:
                     name += "l";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.M:
                     name += "m";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.N:
                     name += "n";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.NumPad0:
                     name += "0";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.NumPad1:
                     name += "1";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.NumPad2:
                     name += "2";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.NumPad3:
                     name += "3";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.NumPad4:
                     name += "4";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.NumPad5:
                     name += "5";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.NumPad6:
                     name += "6";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.NumPad7:
                     name += "7";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.NumPad8:
                     name += "8";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.NumPad9:
                     name += "9";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.O:
                     name += "o";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.P:
                     name += "p";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.Q:
                     name += "q";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.R:
                     name += "r";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.S:
                     name += "s";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.T:
                     name += "t";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.U:
                     name += "u";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.V:
                     name += "v";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.W:
                     name += "w";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.X:
                     name += "x";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.Y:
                     name += "y";
                     break;
                 case Microsoft.Xna.Framework.Input.Keys.Z:
                     name += "z";
                     break;
                 default:
                     break;
             }


            if (name.Length == 1)
            {
                name = name.ToUpper();
            }
            else if(name.Length > 1)
            {
                if (name[name.Length - 2] == ' ')
                {
                    //name[name.Length - 1] = (name[name.Length - 1].ToString().ToUpper())[0];
                    string temp1 = name.Substring(0, name.Length - 1);
                    //string temp2 = name[name.Length - 1].ToString();;
                    name = temp1 + (name[name.Length - 1].ToString().ToUpper())[0];
                }
            }
        }

        private void GetKeyPressesRounds(KeyboardState keyboardState)
        {
            if (keyboardState.GetPressedKeys().Length == 0)
            {
                PreviousKey = Microsoft.Xna.Framework.Input.Keys.Zoom;
                return;
            }
            // if a key is kept pressed. we only need to take its 1st instance

            if (keyboardState.IsKeyUp(PreviousKey))
            {
                PreviousKey = Microsoft.Xna.Framework.Input.Keys.Zoom;
            }

            if (keyboardState.IsKeyDown(PreviousKey))
            {
                return;
            }

            PreviousKey = keyboardState.GetPressedKeys()[0];
            switch (keyboardState.GetPressedKeys()[0])
            {
                case Microsoft.Xna.Framework.Input.Keys.D0:
                    Rounds += "0";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.D1:
                    Rounds += "1";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.D2:
                    Rounds += "2";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.D3:
                    Rounds += "3";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.D4:
                    Rounds += "4";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.D5:
                    Rounds += "5";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.D6:
                    Rounds += "6";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.D7:
                    Rounds += "7";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.D8:
                    Rounds += "8";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.D9:
                    Rounds += "9";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Back:
                    if (Rounds.Length > 0)
                    {
                        Rounds = Rounds.Substring(0, Rounds.Length - 1);
                    }
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad0:
                    Rounds += "0";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad1:
                    Rounds += "1";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad2:
                    Rounds += "2";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad3:
                    Rounds += "3";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad4:
                    Rounds += "4";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad5:
                    Rounds += "5";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad6:
                    Rounds += "6";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad7:
                    Rounds += "7";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad8:
                    Rounds += "8";
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad9:
                    Rounds += "9";
                    break;
                default:
                    break;
            }

        }

        private void AskName()
        {
            int startX = 20, startY = 20, increment = 65;
            // Draw Hello World
            string output = "Hello player, please enter your name:";
            Color textColor = Color.White;
            float fontSize = 0.6f;

            // Find the center of the string
            Vector2 FontOrigin = menuFont.MeasureString(output) / 2;
            Vector2 FontPos = new Vector2(startX, startY);
            Rectangle destinationRectBkgrnd = new Rectangle();
            destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);
            // Draw the string
            spriteBatch.Begin();
            spriteBatch.Draw(theMenu.BackgroundRed, destinationRectBkgrnd, Color.DarkGray);
            startY = (int)((BOARD_HEIGHT / 2 - FontOrigin.Y * 0.6) - increment);
            FontPos = new Vector2(BOARD_WIDTH / 2 - FontOrigin.X * 0.6f, startY);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            //startY += increment;
            output = name;
            FontOrigin = menuFont.MeasureString(output) / 2;
            float nameWidth = FontOrigin.X;
            FontPos = new Vector2(BOARD_WIDTH / 2 - FontOrigin.X / 2, BOARD_HEIGHT/2 - FontOrigin.Y);
            spriteBatch.DrawString(menuFont, output, FontPos, Color.Yellow,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            YourTurnDisplayCount++;
            // we need the 'blink' string to blink
            if (YourTurnDisplayCount > 40 && YourTurnDisplayCount < 80)
            {
                // Find the center of the string
                // Draw the string
                output = "|";

                FontOrigin = menuFont.MeasureString(output);
                FontPos = new Vector2(BOARD_WIDTH / 2 + nameWidth / 2, BOARD_HEIGHT / 2 - FontOrigin.Y / 2);
                spriteBatch.DrawString(menuFont, output, FontPos, Color.Yellow,
                    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);
            }

            if (YourTurnDisplayCount > 80)
            {
                YourTurnDisplayCount = 0;
            }

            spriteBatch.End();

        }

        private void AskRounds()
        {
            int startX = 20, startY = 20, increment = 65;
            // Draw Hello World
            string output = "Hello " + name + ", Please enter the number of rounds";
            Color textColor = Color.White;
            float fontSize = 0.6f;

            // Find the center of the string
            Vector2 FontOrigin = menuFont.MeasureString(output) / 2;
            Vector2 FontPos = new Vector2(startX, startY);
            Rectangle destinationRectBkgrnd = new Rectangle();
            destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);
            // Draw the string
            spriteBatch.Begin();
            spriteBatch.Draw(theMenu.BackgroundRed, destinationRectBkgrnd, Color.DarkGray);
            startY = (int)((BOARD_HEIGHT / 2 - FontOrigin.Y * 0.6) - increment * 3);
            FontPos = new Vector2(BOARD_WIDTH / 2 - FontOrigin.X * 0.6f, startY);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);
            
            output = "you want in the game";
            FontOrigin = menuFont.MeasureString(output) / 2;

            startY = (int)((BOARD_HEIGHT / 2 - FontOrigin.Y * 0.6) - increment * 2);
            FontPos = new Vector2(BOARD_WIDTH / 2 - FontOrigin.X * 0.6f, startY);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            output = "(13 is the maximum in which all the cards will be used)";
            FontOrigin = menuFont.MeasureString(output) / 2;

            startY = (int)((BOARD_HEIGHT / 2 - FontOrigin.Y * 0.6) - increment);
            FontPos = new Vector2(BOARD_WIDTH / 2 - FontOrigin.X * 0.6f, startY);
            spriteBatch.DrawString(menuFont, output, FontPos, textColor,
0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            //startY += increment;
            output = Rounds.ToString();
            FontOrigin = menuFont.MeasureString(output) / 2;
            float roundWidth = FontOrigin.X;
            FontPos = new Vector2(BOARD_WIDTH / 2 - FontOrigin.X / 2, BOARD_HEIGHT / 2 - FontOrigin.Y);
            spriteBatch.DrawString(menuFont, output, FontPos, Color.Yellow,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            YourTurnDisplayCount++;
            // we need the 'blink' string to blink
            if (YourTurnDisplayCount > 40 && YourTurnDisplayCount < 80)
            {
                // Find the center of the string
                // Draw the string
                output = "|";

                FontOrigin = menuFont.MeasureString(output);
                FontPos = new Vector2(BOARD_WIDTH / 2 + roundWidth / 2, BOARD_HEIGHT / 2 - FontOrigin.Y / 2);
                spriteBatch.DrawString(menuFont, output, FontPos, Color.Yellow,
                    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);
            }

            if (YourTurnDisplayCount > 80)
            {
                YourTurnDisplayCount = 0;
            }
            spriteBatch.End();

        }

        private void ShowAbout()
        {
            int startY = 20, startX = 20, increment = 65;
            // Draw Hello World
            string output = " Hey folks, my name is Swapnesh. We usd to play";
            Color textColor = Color.Azure;
            float fontSize = 0.8f;

            // Find the center of the string
            Vector2 FontOrigin = AboutFont.MeasureString(output) / 2;
            Vector2 FontPos = new Vector2(startX, startY);
            Rectangle destinationRectBkgrnd = new Rectangle();
            destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);
            // Draw the string
            spriteBatch.Begin();
            spriteBatch.Draw(theMenu.BackgroundRed, destinationRectBkgrnd, Color.DarkGray);

            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = " Judgement during our engineering days and it was ";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = " always fun. Hence, I thought i should port this game";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = " on the PC and XBox 360.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = " The game will be soon available on the XBox";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = " and XBox Live Marketplace";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "-----------------------------------------------------------------------";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            fontSize -= 0.1f;
            startY += increment;
            output = " Please feel free to mail me your suggestions.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = " Name: Swapnesh Chaubal";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = " E-mail: swapnesh.chaubal@gmail.com";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            fontSize += 0.1f;
            startY += increment;
            output = "-----------------------------------------------------------------------";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(AboutFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        private void ShowInstructions(int startY)
        {
            int startX = 20, increment = 65;
            // Draw Hello World
            string output = "Judgement as the name states, tests your accuracy to guess the";
            Color textColor = Color.Azure;
            float fontSize = 0.26f;
            
            // Find the center of the string
            Vector2 FontOrigin = InstructionsFont.MeasureString(output) / 2;
            Vector2 FontPos = new Vector2(startX, startY);
            Rectangle destinationRectBkgrnd = new Rectangle();
            destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);
            // Draw the string
            spriteBatch.Begin();
            spriteBatch.Draw(theMenu.BackgroundRed, destinationRectBkgrnd, Color.DarkGray);

            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "number of hands that you can make.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            startY += increment;
            output = "Rules:";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "1. When the cards are distributed, each player decides a trump";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    in his/her mind and states the number of hands that he/she";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);


            startY += increment;
            output = "    can make with that trump.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "2. When all the players have stated their hands, the player who";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    has stated the maximum hands, gets to state the trump.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "3. After the game starts, a player is free to play any card.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    However, if a card has already been placed on the table,";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    a player needs to play a card of the same suit with which";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    the hand has started (i.e. suit of the first card in the ";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    hand). If a card of that suit does not exist, the player";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    is free to play any card. If he wants to take the hand,";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    he can do so using a trump card. However, if some other";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    player uses a higher order trump, the hand will go to that";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    player. If he plays a card other than the trump, he will not";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    get the hand, no matter what the rank of the card.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "4. A round is over when all the distributed cards have";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "    been used up. After each round, the score sheet is shown.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            startY += increment;
            output = "Scoring:";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "  The score is calculated as follows:";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "      i. For a player whose HandsStated is equal to HandsMade";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "          Score = HandsMade * 10";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "     ii. For a player whose HandsStated is not equal to HandsMade";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "          Score = Difference between the hands * (-10)";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "  The total score is calculated at the end of each round.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "  The player with the maximum score at the end of all rounds wins.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            startY += increment;
            output = "Rounds:";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "  1. The player gets to decide how many rounds should the game";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "      have. There can be a maximum of 13 rounds since there are";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "      4 players and 52 cards.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "  2. The number of cards to be distributed depends on the number";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "      of rounds the player selects i.e. Rounds * 4";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "  3. The number of cards distributed to each player decreases by";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "      1 after each round, untill no cards are to be distributed.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "      That marks the end of the game.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            startY += increment;
            output = "Controls:";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "  1. On the screen to state hands, use the Numpad or number keys";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "       to state the number of hands and Press Enter.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "  2. On the screen to select trump, use the arrow keys (Up/Down)";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "       to scroll Press Enter to select.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "  3. Use the mouse to select the card which you want to place";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f);

            startY += increment;
            output = "       to scroll Press Enter to select.";
            FontPos = new Vector2(startX, startY);
            spriteBatch.DrawString(InstructionsFont, output, FontPos, textColor,
    0, Vector2.Zero, fontSize, SpriteEffects.None, 0.5f); 

            spriteBatch.End();
        }

        private void ShowMenu()
        {
            gameState = GameState.ShowMenu;

            float posX = 100;
            float posY = 200;

            Rectangle destinationRectBkgrnd = new Rectangle();

            string output;

            spriteBatch.Begin();

            destinationRectBkgrnd = new Rectangle(0, 0, BOARD_WIDTH, BOARD_HEIGHT);
            
            // each selected menu item animates in 14 steps, if the count is 14, it has reached
            // its maximum size and needs to be now reduced. When the size is 0, it is 
            // increased again
            if (optionAnimationCounter >= 14)
            {
                incrementValueForMenu = -1;
            }
            if (optionAnimationCounter <= 0)
            {
                incrementValueForMenu = 1;
            }
            string key;
            // form the key with the menu item name and the number
            if ((optionSelected == 0) &&(string.Compare(theMenu.MenuStrings[0],"Resume") == 0) )
            {
                key = "Start" + (optionAnimationCounter).ToString();
            }
            else
            {
                key = theMenu.MenuStrings[optionSelected] + (optionAnimationCounter).ToString();
            }
            

            spriteBatch.Draw(theMenu.MenuScreens[key], destinationRectBkgrnd, Color.White);

            optionAnimationCounter += incrementValueForMenu;

            // we need the animate the title
            output = "JUDGEMENT";
            Vector2 FontOrigin;
            FontOrigin = titleFont.MeasureString(output);
            posX = (BOARD_WIDTH / 2) - (FontOrigin.X) / 2;
            posY = 20;
            
            if (titleCounter >= 80)
            {
                incrementValueForTitle = -1;
            }
            if (titleCounter <= 0)
            {
                incrementValueForTitle = 1;
            }
            titleCounter += incrementValueForTitle;

            int tempStringCounter;
            // we'll animate each letter in the title for 10 frames
            if (titleCounter <= 10 && titleCounter >0)
            {
                tempStringCounter = 0;
            }
            else
            {
                tempStringCounter = Convert.ToInt32(Math.Ceiling(titleCounter / 10.0));
            }
            // split the title into three parts, the left, the letter to animate and the right
            string leftSplit = output.Substring(0, tempStringCounter);
            string rightSplit = output.Substring(tempStringCounter + 1, output.Length - tempStringCounter - 1);
            string mid = output[tempStringCounter].ToString();
            posX -= 10;

            Vector2 FontPos = new Vector2(posX, posY);
            // draw each of the strings
            spriteBatch.DrawString(titleFont, leftSplit, FontPos, Color.Red,
    0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            posX += titleFont.MeasureString(leftSplit).X;
            posX += 10;
            posY -= 10;
            FontPos = new Vector2(posX, posY);

            spriteBatch.DrawString(titleFont, mid, FontPos, Color.Red,
0, Vector2.Zero, 1.3f, SpriteEffects.None, 0.5f);
            posX += titleFont.MeasureString(mid).X;
            posX += 10;
            posY += 10;
            FontPos = new Vector2(posX, posY);
            spriteBatch.DrawString(titleFont, rightSplit, FontPos, Color.Red,
0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);


            output = theMenu.MenuStrings[optionSelected];
            FontOrigin = titleFont.MeasureString(output);

            // Calculate the x and y depending on the string to be displayed
            posX = BOARD_WIDTH / 2 - FontOrigin.X / 4 - optionAnimationCounter * 2;
            Rectangle coOrdinates;
            switch (output)
            {
                case "Start":
                case "Resume":
                    posY = 216 - FontOrigin.Y / 4;
                    coOrdinates = new Rectangle((int)posX, (int)posY, (int)FontOrigin.X, (int)FontOrigin.Y / 2);
                    theMenu.MenuStringRectangles[0] = coOrdinates;
                    break;
                case "Instructions":
                    posY = 344 - FontOrigin.Y / 4;
                    posX -= optionAnimationCounter;
                    coOrdinates = new Rectangle((int)posX, (int)posY, (int)FontOrigin.X, (int)FontOrigin.Y / 2);
                    theMenu.MenuStringRectangles[1] = coOrdinates;
                    break;
                case "About":
                    posY = 471 - FontOrigin.Y / 4;
                    coOrdinates = new Rectangle((int)posX, (int)posY, (int)FontOrigin.X, (int)FontOrigin.Y / 2);
                    theMenu.MenuStringRectangles[2] = coOrdinates;
                    break;
                case "Exit":
                    posY = 608 - FontOrigin.Y / 4;
                    posX += optionAnimationCounter;
                    coOrdinates = new Rectangle((int)posX, (int)posY, (int)FontOrigin.X, (int)FontOrigin.Y / 2);
                    theMenu.MenuStringRectangles[3] = coOrdinates;
                    break;
                default:
                    break;
            }

            FontPos = new Vector2(posX, posY);
            float size = (float)(0.5 +  (double)optionAnimationCounter/100);
            spriteBatch.DrawString(titleFont, output, FontPos, Color.WhiteSmoke,
                0, Vector2.Zero, size, SpriteEffects.None, 0.5f);
            
            spriteBatch.End();
        }

        #endregion

        #region Round starting functins

        private void StartNewGame()
        {
            // flag to indicate that the game has started
            gameStarted = true;
            roundNumber = 13;
            gameState = GameState.ShowMenu;
            theMenu.MenuState = MenuState.MainMenu;
        }

        private void CalculateHandsAndTrump(Player player, Suit trump)
        {
            OldTrump = trump;
            player.StateHandsAndTrump(ref OldTrump, ref NewTrump, ref OldHands, ref NewHands);
            // check if new hands is greater than old hands
            // if it is we need to check with the other players hands
            bool changeTrump = false;
            if (NewHands > OldHands)
            {
                changeTrump = true;

                for (int index = 0; index < HandsStated.Count; index++)
                {
                    if (NewHands < HandsStated[index])
                    {
                        changeTrump = false;
                    }
                }
            }

            if (changeTrump)
            {
                // the trump can be changed now
                HandSummary.Add(player.Name + " changed the trump to " + NewTrump + ".");
                deck.Trump = NewTrump;
                HandSummary.Add(player.Name + " stated " + NewHands + " hands.");
                player.SetTrumpAndInitialize(NewTrump);
                player.HandsStated = NewHands;
                HandsStated.Add(NewHands);
            }
            else
            {
                deck.Trump = OldTrump;
                HandSummary.Add(player.Name + " stated " + OldHands + " hands.");
                player.SetTrumpAndInitialize(OldTrump);
                player.HandsStated = OldHands;
                HandsStated.Add(OldHands);
            }
            SummaryShown = false;
        }
        private void StartNewRound()
        {
            
            PlayerIndex playerIndexWithMaxHands = PlayerIndex.Four;
            if (!cardsDistributed)
            {
                cardsDistributed = true;
                deck.Trump = Suit.Unspecified;
                if (roundNumber == 13)
                {
                    gameState = GameState.GameOver;
                    return;
                }
                roundNumber++;

                int playerNumber = roundNumber;
                while (playerNumber > 3)
                {
                    playerNumber -= 3;
                }

                playerToStart = (PlayerIndex)(playerNumber - 1);// enum starts from 0

                // Fill the deck again
                gameState = GameState.RoundStarting;
                FillCompleteDeck();
                ShuffleCards();

                // 13 is the max number of rounds that the game can have (+1 is to add a min
                // of 4 cards in the 13th round ) and their are four suits
                cardsToDeal = (14 - roundNumber) * 4;
                DistributeCards(cardsToDeal);
                cardsDistributed = true;

                // initially all players state their hands
                playerState = PlayerState.WaitingToStateHands;
                player2.PlayerState = PlayerState.WaitingToStateHands;
                player3.PlayerState = PlayerState.WaitingToStateHands;
                player4.PlayerState = PlayerState.WaitingToStateHands;

                List<Card> cards = deck.Cards;
                string path;
                // TODO: use this.Content to load your game content here

                foreach (Card playingCard in cards)
                {
                    path = @"Images/Cards/" + playingCard.CardSuite + @"/" + playingCard.RankOfCard;
                    playingCard.Image = Content.Load<Texture2D>(path);
                }
                backSide = Content.Load<Texture2D>(@"Images/Cards/CardBackSide1");

            }

            //trump = deck.Trump;
            player2.TotalCards = deck.Cards.Count;
            player3.TotalCards = deck.Cards.Count;
            player4.TotalCards = deck.Cards.Count;
            int hands;
            if (playerToStart == PlayerIndex.One)
            {
                if (playerState == PlayerState.StatedHands)
                {
                    if (player2.PlayerState != PlayerState.StatedHandsAndTrump)
                    {
                        player2.HandsStated = player2.SelectTrumpAndInitialize();
                        HandsStated.Add(player2.HandsStated);
                        HandSummary.Add(player2.Name + " stated " + player2.HandsStated + " hands.");
                        player2.PlayerState = PlayerState.StatedHandsAndTrump;      
                    }

                    if (player3.PlayerState != PlayerState.StatedHandsAndTrump)
                    {
                        player3.HandsStated = player3.SelectTrumpAndInitialize();
                        HandsStated.Add(player3.HandsStated);
                        HandSummary.Add(player3.Name + " stated " + player3.HandsStated + " hands.");
                        player3.PlayerState = PlayerState.StatedHandsAndTrump;
                    }

                    if (player4.PlayerState != PlayerState.StatedHandsAndTrump)
                    {
                        hands = player4.SelectTrumpAndInitialize();

                        if (deck.HandsStated + player2.HandsStated + player3.HandsStated + hands
                            == deck.Cards.Count)
                        {
                            if (hands == 0)
                            {
                                hands++;
                            }
                            else
                            {
                                hands--;
                            }
                        }
                        player4.HandsStated  = hands;
                        HandsStated.Add(player4.HandsStated);
                        HandSummary.Add(player4.Name + " stated " + player4.HandsStated + " hands.");
                        player4.PlayerState = PlayerState.StatedHandsAndTrump;
                    }
                }
            }
            else if (playerToStart == PlayerIndex.Two)
            {

                if (player2.PlayerState != PlayerState.StatedHandsAndTrump)
                {
                    player2.HandsStated = player2.SelectTrumpAndInitialize();
                    HandsStated.Add(player2.HandsStated);
                    HandSummary.Add(player2.Name + " stated " + player2.HandsStated + " hands.");
                    player2.PlayerState = PlayerState.StatedHandsAndTrump;
                }

                if (player3.PlayerState != PlayerState.StatedHandsAndTrump)
                {
                    player3.HandsStated = player3.SelectTrumpAndInitialize();
                    HandsStated.Add(player3.HandsStated);
                    HandSummary.Add(player3.Name + " stated " + player3.HandsStated + " hands.");
                    player3.PlayerState = PlayerState.StatedHandsAndTrump;
                }

                if (player4.PlayerState != PlayerState.StatedHandsAndTrump)
                {
                    player4.HandsStated = player4.SelectTrumpAndInitialize();

                    HandsStated.Add(player4.HandsStated);
                    HandSummary.Add(player4.Name + " stated " + player4.HandsStated + " hands.");
                    player4.PlayerState = PlayerState.StatedHandsAndTrump;
                }

            }
            else if (playerToStart == PlayerIndex.Three)
            {
                if (player3.PlayerState != PlayerState.StatedHandsAndTrump)
                {
                    player3.HandsStated = player3.SelectTrumpAndInitialize();
                    HandsStated.Add(player3.HandsStated);
                    HandSummary.Add(player3.Name + " stated " + player3.HandsStated + " hands.");
                    player3.PlayerState = PlayerState.StatedHandsAndTrump;
                }

                if (player4.PlayerState != PlayerState.StatedHandsAndTrump)
                {
                    player4.HandsStated = player4.SelectTrumpAndInitialize();
                    HandsStated.Add(player4.HandsStated);
                    HandSummary.Add(player4.Name + " stated " + player4.HandsStated + " hands.");
                    player4.PlayerState = PlayerState.StatedHandsAndTrump;
                }

                if (playerState == PlayerState.StatedHands)
                {
                    if (player2.PlayerState != PlayerState.StatedHandsAndTrump)
                    {
                        hands = player2.SelectTrumpAndInitialize();
                        // the total hands cannot equal the number of cards
                        if (deck.HandsStated + hands + player3.HandsStated + player4.HandsStated
                            == deck.Cards.Count)
                        {
                            if (hands == 0)
                            {
                                hands++;
                            }
                            else
                            {
                                hands--;
                            }
                        }
                        player2.HandsStated = hands;
                        HandsStated.Add(player2.HandsStated);
                        HandSummary.Add(player2.Name + " stated " + player2.HandsStated + " hands.");
                        player2.PlayerState = PlayerState.StatedHandsAndTrump;
                    }
                }

            }
            else if (playerToStart == PlayerIndex.Four)
            {
                if (player4.PlayerState != PlayerState.StatedHandsAndTrump)
                {
                    player4.HandsStated = player4.SelectTrumpAndInitialize();
                    HandsStated.Add(player4.HandsStated);
                    HandSummary.Add(player4.Name + " stated " + player4.HandsStated + " hands.");
                    player4.PlayerState = PlayerState.StatedHandsAndTrump;
                }

                if (playerState == PlayerState.StatedHands)
                {
                    if (player2.PlayerState != PlayerState.StatedHandsAndTrump)
                    {
                        player2.HandsStated = player2.SelectTrumpAndInitialize();
                        HandsStated.Add(player2.HandsStated);
                        HandSummary.Add(player2.Name + " stated " + player2.HandsStated + " hands.");
                        player2.PlayerState = PlayerState.StatedHandsAndTrump;
                    }

                    if (player3.PlayerState != PlayerState.StatedHandsAndTrump)
                    {
                        hands = player3.SelectTrumpAndInitialize();

                        if (deck.HandsStated + player2.HandsStated + hands + player4.HandsStated
                            == deck.Cards.Count)
                        {
                            if (hands == 0)
                            {
                                hands++;
                            }
                            else
                            {
                                hands--;
                            }
                        }
                        player3.HandsStated = hands;

                        HandsStated.Add(player3.HandsStated);
                        HandSummary.Add(player3.Name + " stated " + player3.HandsStated + " hands.");
                        player3.PlayerState = PlayerState.StatedHandsAndTrump;
                    }
                }
            }
            if (playerState == PlayerState.WaitingToStateTrump)
            {
                gameState = GameState.ShowMenu;
                theMenu.MenuState = MenuState.AskTrump;
                optionSelected = -1;
            }

            if (playerState == PlayerState.WaitingToStateHands)
            {
                gameState = GameState.ShowMenu;
                //theMenu.MenuState = MenuState.ShowSummary;
                theMenu.MenuState = MenuState.AskHands;
            }

            if ((playerState == PlayerState.StatedHands) &&
                (player2.PlayerState == PlayerState.StatedHandsAndTrump) &&
                (player3.PlayerState == PlayerState.StatedHandsAndTrump) &&
                (player4.PlayerState == PlayerState.StatedHandsAndTrump))
            {
                // compare hands
                Player playerWithMaxHands = GetPlayerWithMaximumHands();
                if (playerWithMaxHands == null)
                {
                    // player 1 has max hands
                    playerState = PlayerState.WaitingToStateTrump;
                    trump = deck.Trump;
                    playerIndexWithMaxHands = PlayerIndex.One;

                }
                else
                {
                    playerState = PlayerState.StatedHandsAndTrump;
                    trump = playerWithMaxHands.Trump;
                    playerIndexWithMaxHands = playerWithMaxHands.PlayerNumber;
                    HandSummary.Add(playerWithMaxHands.Name + " stated the maximum hands i.e. " + playerWithMaxHands.HandsStated + " hands.");
                    HandSummary.Add("Hence, " + playerWithMaxHands.Name + " chose the trump which is " + trump + ".");
                }
            }

            if (playerState == PlayerState.WaitingToStateTrump)
            {
                gameState = GameState.ShowMenu;
                theMenu.MenuState = MenuState.AskTrump;
            }

            if ((playerState == PlayerState.StatedHandsAndTrump) &&
                (player2.PlayerState == PlayerState.StatedHandsAndTrump) &&
                (player3.PlayerState == PlayerState.StatedHandsAndTrump) &&
                (player4.PlayerState == PlayerState.StatedHandsAndTrump))
            {
                if (playerIndexWithMaxHands == PlayerIndex.One)
                {
                    trump = deck.Trump;
                    HandSummary.Add(name + " stated the maximum hands i.e. " + deck.HandsStated + " hands.");
                    HandSummary.Add("Hence, " + name + " chose the trump which is " + trump + ".");
                }

                deck.Trump = trump;
                player2.Trump = trump;
                player3.Trump = trump;
                player4.Trump = trump;
                player2.InitializePlayer();
                player3.InitializePlayer();
                player4.InitializePlayer();
                gameState = GameState.RoundInProgress;
                
            }
        }

        #endregion

        #region Other supporting functions

        private Player GetPlayerWithMaximumHands()
        {
            Player[] players = { player2, player3, player4 };

            Player maxHands = new Player();
            maxHands.HandsStated = -1;

            // we need to find the player with the max hands so that we can select his
            // trump
            for (int index = 0; index < players.Length; index++)
            {
                if (players[index].HandsStated > maxHands.HandsStated)
                {
                    maxHands = players[index];
                }
            }

            if (deck.HandsStated > maxHands.HandsStated)
            {
                return null; // null will refer to player 1
            }
            else
            {
                // we now check if the multiple players have stated the same hands
                List<PlayerIndex> playersWithEqualHands = new List<PlayerIndex>();

                // we add all players to the list who have stated equal hands
                if (deck.HandsStated == maxHands.HandsStated)
                {
                    playersWithEqualHands.Add(PlayerIndex.One);
                }

                if (player2.HandsStated == maxHands.HandsStated)
                {
                    playersWithEqualHands.Add(PlayerIndex.Two);
                }
                if (player3.HandsStated == maxHands.HandsStated)
                {
                    playersWithEqualHands.Add(PlayerIndex.Three);
                }
                if (player4.HandsStated == maxHands.HandsStated)
                {
                    playersWithEqualHands.Add(PlayerIndex.Four);
                }
                int firstPlayer = (int)playerToStart;
                PlayerIndex playerToState = playerToStart;
                bool playerFound = false;
                while (true)
                {
                    // we now find the first player who stated the maximum hands
                    // the player number will be incremented starting with 'playerToStart'
                    for (int index = 0; index < playersWithEqualHands.Count; index++)
                    {
                        if (firstPlayer == (int)playersWithEqualHands[index])
                        {
                            playerToState = playersWithEqualHands[index];
                            playerFound = true;
                            break;
                        }
                    }
                    if (playerFound)
                    {
                        break;
                    }
                    firstPlayer++;
                    if (firstPlayer > 3)
                    {
                        firstPlayer = 0;
                    }
                }

                switch (playerToState)
                {
                    case PlayerIndex.Four:
                        maxHands = player4;
                        break;
                    case PlayerIndex.One:
                        maxHands = null;
                        break;
                    case PlayerIndex.Three:
                        maxHands = player3;
                        break;
                    case PlayerIndex.Two:
                        maxHands = player2;
                        break;
                    default:
                        break;
                }
                playerToStart = playerToState;
                return maxHands;
            }
        }

        private Player GetPlayerWithMaxScore()
        {
            Player[] players = { player2, player3, player4 };

            Player maxScore = new Player();
            maxScore.TotalScore = -1;

            // we need to find the player with the max hands so that we can select his
            // trump
            for (int index = 0; index < players.Length; index++)
            {
                if (players[index].TotalScore > maxScore.TotalScore)
                {
                    maxScore = players[index];
                }
            }

            if (TotalScore > maxScore.TotalScore)
            {
                return null; // null will refer to player 1
            }
            else
            {
                return maxScore;
            }
        }

        private void CompareHandsAndSetTrump()
        {
            Player[] players = { player2, player3, player4 };
            
            Player maxHands = new Player();
            maxHands.HandsStated = -1;

            for (int index = 0; index < players.Length; index++)
            {
                if (players[index].HandsStated > maxHands.HandsStated)
                {
                    maxHands = players[index];
                }
            }
            Suit trumpSuit = Suit.Unspecified;

            if (deck.HandsStated > maxHands.HandsStated)
            {
                trumpSuit = deck.Trump;
                HandSummary.Add(name + " stated the maximum hands i.e. " + deck.HandsStated + " hands.");
                HandSummary.Add("Hence, " + name + " chose the trump which is " + trumpSuit + ".");
            }
            else
            {
                trumpSuit = maxHands.Trump;
                HandSummary.Add(maxHands.Name + " stated the maximum hands i.e. " + maxHands.HandsStated + " hands.");
                HandSummary.Add("Hence, " + maxHands.Name + " chose the trump which is " + trumpSuit + ".");
            }

            deck.Trump = trumpSuit;
            player2.Trump = trumpSuit;
            player3.Trump = trumpSuit;
            player4.Trump = trumpSuit;
        }

        private void UpdatePlayerHands(Player player)
        {
            player.HandsMade++;
            foreach (Card card in cardsOnTable.Keys)
            {
                player.TotalUsedCards.Add(card);
            }

        }

        private PlayerIndex FindPlayerToGetTheHand()
        {
            List<Card> trumpCards = new List<Card>();
            List<Card> cardsThatMatter = new List<Card>();
            Card winningCard;
            //Suit suitToCheck =  (cardsOnTable.Keys.ToList<Card>()[0]).CardSuite;
            Suit suitToCheck = cardsOnTableList[0].CardSuite;
            foreach (Card card in cardsOnTable.Keys)
            {
                // If the card suit contains trumps, the highest trump among them is the biggest
                // card in the deck
                if (card.CardSuite == deck.Trump)
                {
                    trumpCards.Add(card);
                }
                if (card.CardSuite == suitToCheck)
                {
                    cardsThatMatter.Add(card);
                }
            }

            if (trumpCards.Count > 0)
            {
                winningCard = trumpCards[0];
                if (trumpCards.Count > 1)
                {
                    for (int index = 1; index < trumpCards.Count; index++)
                    {
                        if (trumpCards[index].RankOfCard > winningCard.RankOfCard)
                        {
                            winningCard = trumpCards[index];
                        }
                    }
                }
            }
            else
            {
                winningCard = cardsThatMatter[0];
                if (cardsThatMatter.Count > 1)
                {
                    for (int index = 1; index < cardsThatMatter.Count; index++)
                    {
                        if (cardsThatMatter[index].RankOfCard > winningCard.RankOfCard)
                        {
                            winningCard = cardsThatMatter[index];
                        }
                    }
                }
            }
            return (PlayerIndex)cardsOnTable[winningCard];
        }

        private void CalculatePlayerCardPositions()
        {
            int posX, posY;
            // the x position of the placed 

            // Player2
            posX = MidX + CARD_PLACE_OFFSET;
            posY = MidY;
            player2.CardPosition = new Vector2(posX, posY);

            // Player 3
            posX = MidX;
            posY = MidY - CARD_PLACE_OFFSET;
            player3.CardPosition = new Vector2(posX, posY);

            // Player 4
            posX = MidX - CARD_PLACE_OFFSET;
            posY = MidY;
            player4.CardPosition = new Vector2(posX, posY);
        }

        private void CalculateGridForScore()
        {
            // Divide the surface into grid. There will be 5 columns
            // Round, Player names (4 players)
            int numberOfColumns = 5;

            ColumnXValues = new List<int>();
            ColumnYValues = new List<int>();
            int colWidth = BOARD_WIDTH / numberOfColumns;
            int startX = 0;
            int colMid;
            for (int index = 0; index < numberOfColumns; index++)
            {
                colMid = startX + (colWidth / 2);
                ColumnXValues.Add(colMid);
                startX += colWidth;
            }

            int numberOfRows = 15;

            // 50 is to show the heading
            int spaceForHeading = 100;
            int rowHeight = (BOARD_HEIGHT - spaceForHeading) / numberOfRows;
            int startY = spaceForHeading;
            int rowMid;
            for (int index = 0; index < numberOfRows; index++)
            {
                rowMid = startY + (rowHeight / 2);
                ColumnYValues.Add(rowMid);
                startY += rowHeight;
            }

            ColumnHeaders = new List<string>();
            ColumnHeaders.Add("Round");
            ColumnHeaders.Add(name);
            ColumnHeaders.Add(player2.Name);
            ColumnHeaders.Add(player3.Name);
            ColumnHeaders.Add(player4.Name);
        }
        private Card PlaceCard(Player player)
        {
            Card card;
            player.CardsOnTable.Clear();
            foreach (Card cardKey in cardsOnTable.Keys)
            {
                player.CardsOnTable.Add(cardKey);
            }
            card = player.PlaceCard();
            card.Position = new Vector2(player.CardPosition.X, player.CardPosition.Y);
            cardsOnTable.Add(card, player.PlayerNumber);
            cardsOnTableList.Add(card);
            return card;
        }

        #endregion

        #endregion
    }
}