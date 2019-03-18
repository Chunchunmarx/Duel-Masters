using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckState : CardState
{
    public DeckState(Card _card) : base(_card)
    {
        mCardState = CARD_STATE.DECK;
    }

}
