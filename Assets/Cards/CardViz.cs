using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SA
{
    public class CardViz : MonoBehaviour
    {
        public Text attack;
        public Text defense;
        public Text armor;
        public Text morale;

        public Card card;

        private void Start()
        {
            LoadCard(card);
        }

        public void LoadCard(Card c)
        {
            if (c == null)
            { return; }

            card = c;

            attack.text = c.attack;
            defense.text = c.defense;
            armor.text = c.armor;
            morale.text = c.morale;
        }


    }
}