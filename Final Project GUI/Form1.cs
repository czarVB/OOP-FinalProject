using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using CardLib;
using MyCardBox;
using System.Collections.Generic;

namespace Final_Project_GUI
{
    public partial class frmBoard : Form
    {
        //initialize form
        public frmBoard()
        {
            InitializeComponent();
        }
        // The default card size
        static private Size cardSize = new Size(90, 151);
        // Default hand size
        static private int handSize = 6;
        // The amount, in points, that CardBox controls are enlarged when hovered over. 
        private const int POP = 25;
        // If we choose to implement drag and drop functionality (doesn't work alongside _Click)
        private CardBox dragCard;
        Deck myDeck = new Deck();
        Talon theTalon = new Talon();
        Hand playerHand = new Hand();
        Hand computerHand = new Hand();
        Card trumpCard = new Card();

        int deckSize = 36;
        int roundNumber = 0;

        // Variables for determining whose turn it is
        private bool playerTurn;
        private bool AIAttacking;
        private bool AIAlreadyAttacked = false;

        public void newGame()
        {
            Deck myDeck = new Deck();
            Talon theTalon = new Talon();
            Hand playerHand = new Hand();
            Hand computerHand = new Hand();

            int deckSize = 36;
            int handSize = 8;
            int roundNumber = 0;
        }

        private void frmBoard_Load(object sender, EventArgs e)
        {
            newGame();
            trumpCard = myDeck.DrawCard();
            pbTalon.Image = trumpCard.GetCardImage();
            trumpCard.FaceUp = true;
            pbTrumpSuit.Image = trumpCard.GetCardImage();

            playerHand.AddCards(myDeck, handSize);
            for (int i = 1; i <= handSize; i++)
            {
                playerHand[i].FaceUp = true;
                CardBox aCardBox = new CardBox(playerHand[i]);

                // Wire the apporpriate event handlers for each cardbox
                aCardBox.Click += CardBox_Click;

                // wire CardBox_MouseEnter for the "POP" visual effect
                aCardBox.MouseEnter += CardBox_MouseEnter;

                // wire CardBox_MouseLeave for the regular visual effect
                aCardBox.MouseLeave += CardBox_MouseLeave;

                pnlPlayerHand.Controls.Add(aCardBox);
                RealignCards(pnlPlayerHand);
            }

            computerHand.AddCards(myDeck, handSize);
            for (int i = 1; i <= handSize; i++)
            {
                computerHand[i].FaceUp = true;
                CardBox aCardBox = new CardBox(computerHand[i]);

                // Wire the apporpriate event handlers for each cardbox
                aCardBox.Click += CardBox_Click;

                // wire CardBox_MouseEnter for the "POP" visual effect
                aCardBox.MouseEnter += CardBox_MouseEnter;

                // wire CardBox_MouseLeave for the regular visual effect
                aCardBox.MouseLeave += CardBox_MouseLeave;

                pnlOpponentHand.Controls.Add(aCardBox);
                RealignCards(pnlOpponentHand);
            }

            // Establish a baseline card for each player
            int playerLowest = 0;
            int opponentLowest = 0;

            // Determine the lowest card of the proper suit - player
            foreach (CardBox cards in pnlPlayerHand.Controls)
            {
                if (cards.Card.Suit == trumpCard.Suit)
                {
                    if (playerLowest == 0)
                    {
                        playerLowest = (int)cards.Card.Rank;
                    }
                    else if ((int)cards.Card.Rank < playerLowest)
                    {
                        playerLowest = (int)cards.Card.Rank;
                    }
                }
            }

            // Determine the lowest card of the proper suit - AI
            foreach (CardBox cards in pnlOpponentHand.Controls)
            {
                if (cards.Card.Suit == trumpCard.Suit)
                {
                    if (opponentLowest == 0)
                    {
                        opponentLowest = (int)cards.Card.Rank;
                    }
                    else if ((int)cards.Card.Rank < playerLowest)
                    {
                        opponentLowest = (int)cards.Card.Rank;
                    }
                }
            }

            // Determine whose turn it is based on the lowest trump card
            // THIS DOESN'T ACCOUNT FOR TIES
            if (playerLowest < opponentLowest)
            {
                playerTurn = true;
                AIAttacking = false;
            }
            else
            {
                playerTurn = false;
                AIAttacking = true;
            }
        }



        /// <summary>
        /// When a CardBox is clicked, move to the opposite panel.
        /// </summary>
        void CardBox_Click(object sender, EventArgs e)
        {
            if (playerTurn)
            {
                // Convert sender to a CardBox
                CardBox aCardBox = sender as CardBox;

                // If the conversion worked
                if (aCardBox != null)
                {
                    while (playerTurn = true)
                    {
                        // If the card 
                        if (aCardBox.Parent == pnlPlayerHand)
                        {
                            if (theTalon.cardAttackValidation(aCardBox.Card))
                            {
                                // Remove the card from the home panel
                                pnlPlayerHand.Controls.Remove(aCardBox);
                                theTalon.AddCard(aCardBox.Card);
                                playerHand.Remove(aCardBox.Card);
                                // Resize the CardBox - not sure why this is needed
                                aCardBox.Size = cardSize;

                                // Add the control to the play panel
                                pnlPlayArea.Controls.Add(aCardBox);

                                // Call the opponent's method to start their turn
                                playerTurn = false;
                            }
                        }
                    }

                    // Realign the cards 
                    RealignCards(pnlPlayerHand);
                    RealignCards(pnlPlayArea);

                    // AI's response
                    //AITurn();
                }
            }
            else
            {
                // Convert sender to a CardBox
                CardBox aCardBox = sender as CardBox;

                // If the conversion worked
                if (aCardBox != null)
                {
                    bool defended = false;
                    while (defended = false)
                    {
                        // If the card 
                        if (aCardBox.Parent == pnlPlayerHand)
                        {
                            if (theTalon.cardDefendValidation(trumpCard, aCardBox.Card))
                            {
                                // Remove the card from the home panel
                                pnlPlayerHand.Controls.Remove(aCardBox);
                                theTalon.AddCard(aCardBox.Card);
                                playerHand.Remove(aCardBox.Card);
                                // Resize the CardBox - not sure why this is needed
                                aCardBox.Size = cardSize;

                                // Add the control to the play panel
                                pnlPlayArea.Controls.Add(aCardBox);

                                // Call the opponent's method to start their turn
                                defended = true;
                            }
                        }
                    }

                    // Realign the cards 
                    RealignCards(pnlPlayerHand);
                    RealignCards(pnlPlayArea);

                    // AI's response
                    //AITurn();
                }
            }
        }

        /// <summary>
        ///  CardBox controls grow in size when the mouse is over it.
        /// </summary>
        void CardBox_MouseEnter(object sender, EventArgs e)
        {
            // Convert sender to a CardBox
            CardBox aCardBox = sender as CardBox;

            // If the conversion worked and the card is in hand
            if (aCardBox != null)
            {
                if (aCardBox.Parent == pnlPlayerHand)
                {
                    // Enlarge the card for visual effect
                    aCardBox.Size = new Size(cardSize.Width + POP, cardSize.Height + POP);

                    // move the card to the top edge of the panel.
                    aCardBox.Top = 0;
                }
            }
        }
        /// <summary>
        /// CardBox control shrinks to regular size when the mouse leaves.
        /// </summary>
        void CardBox_MouseLeave(object sender, EventArgs e)
        {
            // Convert sender to a CardBox
            CardBox aCardBox = sender as CardBox;

            // If the conversion worked
            if (aCardBox != null)
            {
                if (aCardBox.Parent == pnlPlayerHand)
                {
                    // resize the card back to regular size
                    aCardBox.Size = cardSize;

                    // move the card down to accommodate for the smaller size.
                    aCardBox.Top = POP;
                }
            }
        }
        private void RealignCards(Panel panelHand)
        {
            // Determine the number of cards/controls in the panel.
            int myCount = panelHand.Controls.Count;

            // If there are any cards in the panel
            if (myCount > 0)
            {
                // Determine how wide one card/control is.
                int cardWidth = panelHand.Controls[0].Width;

                // Determine where the left-hand edge of a card/control placed in the middle of the panel should be  
                int startPoint = (panelHand.Width - cardWidth) / 2;

                // An offset for the remaining cards
                int offset = 0;

                // If there are more than one cards/controls in the panel
                if (myCount > 1)
                {
                    // Determine what the offset should be for each card based on the space available and the number of card/controls
                    offset = (panelHand.Width - cardWidth - 2 * 25) / (myCount - 1);  // Minus the offset on both sides

                    // If the offset is bigger than the card/control width, i.e. there is lots of room, set the offset to the card width. The cards/controls will not overlap at all.
                    if (offset > cardWidth)
                    {
                        offset = cardWidth;
                    }

                    // Determine width of all the cards/controls 
                    int allCardsWidth = (myCount - 1) * offset + cardWidth;

                    // Set the start point to where the left-hand edge of the "first" card should be.
                    startPoint = (panelHand.Width - allCardsWidth) / 2;
                }

                // Align the "first" card (which is the last control in the collection)
                panelHand.Controls[myCount - 1].Top = 25;
                System.Diagnostics.Debug.Write(panelHand.Controls[myCount - 1].Top.ToString() + "\n"); // Debugging
                panelHand.Controls[myCount - 1].Left = startPoint;

                // for each of the remaining controls, in reverse order.
                for (int index = myCount - 2; index >= 0; index--)
                {
                    // Align the current card
                    panelHand.Controls[index].Top = 25;
                    panelHand.Controls[index].Left = panelHand.Controls[index + 1].Left + offset;
                }
            }
        }

    }
}