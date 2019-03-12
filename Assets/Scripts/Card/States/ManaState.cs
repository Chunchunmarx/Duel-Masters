using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaState : CardState
{
    bool mIsTapped = false;
    bool mIsTappedLocked = false;

    public ManaState(Card _card) : base(_card) { }
	
    public override void OnClick()
    {
        if (GameManager.instance.GetActivePlayer() != mCardReference.GetPlayerOwner())
        {
            return;
        }

        if (mIsTapped == false)
        {
            Tap();
        }
        else
        {
            Untap();
        }
    }

    public void Tap()
    {
        if (mIsTapped == true)
        {
            //Handle this??
            return;
        }

        mIsTapped = true;
        ManazoneManager activeManazone = GameManager.instance.GetActiveManazone();
        activeManazone.ManaTap(mCardReference.GetCardCivilization());

        Vector3 oldRotation = mCardReference.transform.eulerAngles;
        mCardReference.transform.eulerAngles = new Vector3(oldRotation.x, mCardReference.GetUntappedEulerAngleY() + 90, oldRotation.z);
    }

    public void Untap()
    {
        if (mIsTapped == false || mIsTappedLocked == true)
        {
            //Handle this??
            return;
        }

        mIsTapped = false;
        ManazoneManager activeManazone = GameManager.instance.GetActiveManazone();
        activeManazone.ManaUntap(mCardReference.GetCardCivilization());

        Vector3 oldRotation = mCardReference.transform.eulerAngles;
        mCardReference.transform.eulerAngles = new Vector3(oldRotation.x, mCardReference.GetUntappedEulerAngleY(), oldRotation.z);
    }

    public override void NewTurn()
    {
        Untap();
    }
}
