using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Cards.cs - A class defining the Cards object. Inherits
 * from CollectionBase and implements ICloneable.
 *
 * Author: Spence McComb - 100426427
 * Since: 2020/02/20
 * See: Beginning Visual C#® 2012 Programming
 */

namespace CardLib
{
    public class Cards : List<Card>, ICloneable
    {
        // Returns a deep copy of the object
        public object Clone()
        {
            Cards newCards = new Cards();
            foreach (Card sourceCard in this)
            {
                newCards.Add((Card)sourceCard.Clone());
            }
            return newCards;
        }

        /// <summary>
        /// Utility method for copying card instances into another Cards instance
        /// - used in Deck.Shuffle(). This implementation assumes that the source
        /// and target collections are of the same size.
        /// </summary>
        /// <param name="targetCards"></param>
        public void CopyTo(Cards targetCards)
        {
            for (int index = 0; index < this.Count; index++)
            {
                targetCards[index] = this[index];
            }
        }
    }


}
