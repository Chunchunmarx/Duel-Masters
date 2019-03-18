using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaState : CardState
{
    bool mIsTapped = false;
    bool mIsTappedLocked = false;


    public ManaState(Card _card) : base(_card)
    {
        mCardState = CARD_STATE.MANAZONE;
    }
	
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

    private void Tap()
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

    private void Untap()
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
        //very nasty stuff
        if(mIsTappedLocked == true)
        {
            mIsTappedLocked = false;

            Vector3 origAngles = mCardReference.transform.eulerAngles;
            mCardReference.transform.eulerAngles = new Vector3(origAngles.x, origAngles.y, origAngles.z - 180);
        }
        Untap();

    }

    public override void LockTap()
    {
        if(mIsTappedLocked == true)
        {
            return;
        }

        mIsTappedLocked = true;

        Vector3 origAngles = mCardReference.transform.eulerAngles;
        mCardReference.transform.eulerAngles = new Vector3(origAngles.x, origAngles.y, origAngles.z + 180);
    }

    public override bool IsTapped()
    {
        return mIsTapped;
    }
}
