using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Judgement
{
    /// <summary>
    /// Enumeration for Suit of the card
    /// </summary>
    public enum Suit
    {
        /// <summary>
        /// Spades
        /// </summary>
        Spades,

        /// <summary>
        /// Hearts
        /// </summary>
        Hearts,

        /// <summary>
        /// Diamonds
        /// </summary>
        Diamonds,

        /// <summary>
        /// Clubs
        /// </summary>
        Clubs,

        /// <summary>
        /// The suit/trump hasn't been stated yet
        /// </summary>
        Unspecified
    }

    /// <summary>
    /// Enumeration for Rank of the card
    /// </summary>
    public enum Rank
    {
        /// <summary>
        /// Two
        /// </summary>
        Two,

        /// <summary>
        /// Three
        /// </summary>
        Three,

        /// <summary>
        /// Four
        /// </summary>
        Four,

        /// <summary>
        /// Five
        /// </summary>
        Five,

        /// <summary>
        /// Six
        /// </summary>
        Six,

        /// <summary>
        /// Seven
        /// </summary>
        Seven,

        /// <summary>
        /// Eight
        /// </summary>
        Eight,

        /// <summary>
        /// Nine
        /// </summary>
        Nine,

        /// <summary>
        /// Ten
        /// </summary>
        Ten,

        /// <summary>
        /// Jack
        /// </summary>
        Jack,

        /// <summary>
        /// Queen
        /// </summary>
        Queen,
        
        /// <summary>
        /// King
        /// </summary>
        King,

        /// <summary>
        /// Ace
        /// </summary>
        Ace,
    }

    public enum RoundState
    {
        HandInProgress,

        CardsMoving,

        HandOver,

        CardsPlaced
    }

    public enum GameState
    {
        RoundInProgress,

        RoundOver,

        RoundStarting,

        ShowMenu,

        GameOver,
        
        Paused
    }

    public enum PlayerState
    {
        PlayedCard,

        WaitingToPlay,

        WaitingToStateHands,

        WaitingToStateTrump,

        StatedHandsAndTrump,

        StatedHands,

        StatedTrump
    }

    public enum MenuState
    {
        MainMenu,

        AskTrump,

        AskHands,

        AskName,

        AskRounds,

        ShowScore,

        ShowSummary,

        ShowInstructions,

        ShowAbout
    }
}
