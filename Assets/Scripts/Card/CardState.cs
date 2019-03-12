using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardState  
{
    protected Card mCardReference;

    private CardState() { }

    public CardState(Card _card)
    {
        mCardReference = _card;
    }

    public virtual void OnClick()
    {
        Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
    }

    public virtual void NewTurn()
    {
        //Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
    }
}
