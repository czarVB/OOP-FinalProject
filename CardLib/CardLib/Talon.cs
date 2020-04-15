using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{
    class Talon : ICloneable
    {
        Cards theTalon = new Cards();
        private int riverCardsRemaning = 0;


        //default constructor
        public Talon()
        {

        }

        //parameriterized constructor sets new talon
        private Talon(Cards newDeck)
        {
            theTalon = newDeck;
        }


        //addcardtoriver method, will add a card to talon
        public void AddCard(Card card)
        {
            theTalon.Add(card);
            riverCardsRemaning = theTalon.Count();
        }

        //removecardfromriver method, will remove a card from the talon
        public void RemoveCard(Card card)
        {
            theTalon.Remove(card);
            riverCardsRemaning = theTalon.Count();
        }

        //shows the length of the talon
        public int length()
        {
            return theTalon.Count();
        }

        //get card based on int number
        public Card GetCard(int cardNumber)
        {
            if (cardNumber >= 0 && cardNumber <= 51)
                return theTalon[cardNumber];
            else
                throw (new System.ArgumentOutOfRangeException("cardNumber", cardNumber, "The deck is between 0 and 51 cards long, how do you screw this up?."));
        }

        //clones the talon cards
        public object Clone()
        {
            Talon newTalon = new Talon(theTalon.Clone() as Cards);
            return newTalon;
        }

        //clears the talon
        public void Clear()
        {
            theTalon.Clear();
        }

        //will compare the cards enetered into the river 
        public bool cardDefendValidation(Card trumpCard, Card card)
        {
            bool defended = false;

            if (theTalon.Count % 2 == 0)
            {
                if (card.Suit == theTalon[theTalon.Count - 1].Suit | theTalon[1].Suit == trumpCard.Suit)
                {
                    if (card > theTalon[theTalon.Count - 1] | theTalon[theTalon.Count].Suit == trumpCard.Suit)
                        defended = true;
                }
                return defended;
            }
            return defended;
        }

        //Attacking phase for human players
        public bool cardAttackValidation(Card attackingCard)
        {
            bool attack = false;

            if (theTalon.Count == 0)
            {
                attack = true;
            }

            if (theTalon.Count % 2 != 0)
            {
                if (attackingCard.Rank == theTalon[theTalon.Count].Rank | attackingCard.Rank == theTalon[theTalon.Count - 1].Rank)
                {
                    attack = true;
                }
            }
            return attack;
        }

        //shuffle method, randomizes the order of the cardlist, 
        //then uses the copy to method in cardlist to copy over the cards to a new cardlist deck. 
        public void Shuffle(int deckSize)
        {
            Cards newDeck = new Cards();
            bool[] assigned = new bool[deckSize];
            Random sourceGen = new Random();
            for (int i = 0; i < deckSize; i++)
            {
                int sourceCard = 0;
                bool foundCard = false;
                while (foundCard == false)
                {
                    sourceCard = sourceGen.Next(deckSize);
                    if (assigned[sourceCard] == false)
                        foundCard = true;
                }
                assigned[sourceCard] = true;
                newDeck.Add(theTalon[sourceCard]);
            }
            newDeck.CopyTo(theTalon);
        }
    }
}