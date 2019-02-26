using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandState : CardState
{
    public HandState(Card _card) : base(_card) { }

    public override void OnClick()
    {
        if (GameManager.instance.GetActivePlayer() != mCardReference.GetPlayerOwner())
        {
            return;
        }
        mCardReference.ChangeCardState(CARD_STATE.AIR);
        GameManager.instance.CardOnAir(true);
        mCardReference.transform.rotation = Quaternion.identity;
        mCardReference.transform.eulerAngles = new Vector3(0, 180, 0);
    }
}
