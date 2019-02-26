using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaState : CardState
{
    public ManaState(Card _card) : base(_card) { }
	
    public override void OnClick()
    {
        if (GameManager.instance.GetActivePlayer() != mCardReference.GetPlayerOwner())
        {
            return;
        }

        ManazoneManager activeManazone = GameManager.instance.GetActiveManazone();
        if (mCardReference.GetIsTapped() == false)
        {
            activeManazone.ManaTap(mCardReference.GetCardCivilization());

            mCardReference.SetIsTapped(true);
            mCardReference.transform.eulerAngles = new Vector3(mCardReference.transform.eulerAngles.x, mCardReference.transform.eulerAngles.y - 90, mCardReference.transform.eulerAngles.z);
        }
        else
        {
            activeManazone.ManaUntap(mCardReference.GetCardCivilization());
            mCardReference.SetIsTapped(false);
            mCardReference.transform.eulerAngles = new Vector3(mCardReference.transform.eulerAngles.x, mCardReference.transform.eulerAngles.y + 90, mCardReference.transform.eulerAngles.z);
        }
    }

    
}
