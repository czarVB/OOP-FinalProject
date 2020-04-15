using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLib;
using System.Windows.Forms;

namespace Final_Project_GUI
{
    public abstract class Player
    {

        private int myStance;
        private bool myTurn;
        private Cards myHand;
        private Panel myPanel;

        public int Stance
        {
            get
            {
                return myStance;
            }
            set
            {
                myStance = value;
            }
        }
        public bool Turn
        {
            get
            {
                return myTurn;
            }
        }

        public void giveCard(Card card)
        {
            myHand.Add(card);
        }

        public void removeCard(Card card)
        {
            myHand.Remove(card);
        }

        public Cards getCards()
        {
            return myHand;
        }

        public int handSize()
        {
            return myHand.Count;
        }

        public abstract void enableTurn();

    }
}
