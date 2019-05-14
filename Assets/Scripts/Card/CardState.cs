using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CARD_STATE
{
    INVALID,
    DECK,
    HAND,
    BATTLEZONE,
    AIR,
    MANAZONE,
    GRAVEYARD,
    TARGETING,
    NO_TARGETING
};

public class CardState
{
    protected Card mCardReference;
    protected CARD_STATE mCardState = CARD_STATE.INVALID;

    private CardState() { }

    public CardState(Card _card)
    {
        mCardReference = _card; //it works?
        mCardState = CARD_STATE.INVALID;
        mCardReference.SetIsInAir(false);
    }

    public virtual void OnClick()
    {
        Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
    }

    public virtual void OnMouseUp()
    {
        //Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
    }

    public virtual void NewTurn()
    {
        //Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
    }

    public virtual bool IsTapped()
    {
        //Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
        return false;
    }

    public virtual void LockTap()
    {
        //Debug.LogWarning("Am apelat OnClick() din abstractie!!!");
    }

    public void ToGraveyard()
    {
        LeaveState();
		mCardReference.DestroyCard();
    }

    public void ToHand()
    {
        LeaveState();
        GameManager.instance.GetMyHandManager(mCardReference).AddCardToHand(mCardReference);
    }

    public void ToMana()
    {

    }

    public CARD_STATE GetState()
    {
        return mCardState;
    }

    public virtual void LeaveState()
    {
        Debug.LogWarning("Not implemented LeaveState()!!!");
    }
}