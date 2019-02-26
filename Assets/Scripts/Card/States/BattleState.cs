using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleState : CardState
{
    public BattleState(Card _card) : base(_card) { }

    public override void OnClick()
    {
        if (GameManager.instance.IsTargeting() == false)
        {
            if (GameManager.instance.GetActivePlayer() != mCardReference.GetPlayerOwner())
            {
                return;
            }

            GameManager.instance.SetTargeting(mCardReference.transform);
            mCardReference.ChangeCardState(CARD_STATE.TARGETING);
        }
        else
        {
            GameManager.instance.SetTargeted(mCardReference.transform);
        }
    }


}
