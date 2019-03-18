using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandState : CardState
{
    private HandManager mHandManager = null;
    public HandState(Card _card) : base(_card)
    {
        mCardState = CARD_STATE.HAND;
        mCardReference.transform.rotation = mCardReference.mOrigRotation;
        mCardReference.transform.position = mCardReference.mOrigPosition;
    }

    public override void OnClick()
    {
        if (GameManager.instance.GetActivePlayer() != mCardReference.GetPlayerOwner())
        {
            return;
        }
        mCardReference.SetIsInAir(true);
        GameManager.instance.CardOnAir(true);
        mCardReference.transform.rotation = Quaternion.identity;
        mCardReference.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public override void OnMouseUp()
    {
        mCardReference.SetIsInAir(false);
        mCardReference.transform.position = mCardReference.mOrigPosition;
        mCardReference.transform.rotation = mCardReference.mOrigRotation;
    }

    public override void LeaveState()
    {
        mHandManager.RemoveCardFromHand(mCardReference);
    }

    public void SetHandManager(HandManager _manager)
    {
        mHandManager = _manager;
    }
}
