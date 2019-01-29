using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CARD_STATE
{
    HAND,
    BATTLEZONE,
    AIR,
    MANAZONE
        

};

public enum CARD_CIVILIZATION
{
    NATURE,
    DARKNESS,
    FIRE,
    LIGHT,
    WATER
    
};

public class Card : MonoBehaviour
{
    private CARD_CIVILIZATION mCardCivilization = CARD_CIVILIZATION.NATURE;
    private CARD_STATE mCardState;
    private Vector3 mOrigPosition;
    private Quaternion mOrigRotation;
    private BattlezoneManager mBattlezoneManager;
    private HandManager mHandManager;
    private ManazoneManager mManazoneManager;
    private int mManaRequired = 1;
    bool mHasEnteredBattlezone = false; 
    bool mHasEnteredManazone = false;
    bool mIsTapped = false;
    
    
    void Start ()
    {
        mCardState = CARD_STATE.HAND;
        mOrigPosition = transform.position;
        mOrigRotation = transform.rotation;
    }
	
	
	void Update ()
    {
        HandleAir();
	}

    void HandleAir()
    {
        if(mCardState == CARD_STATE.AIR)
        {
            Vector3 mousePoz = Input.mousePosition;
            mousePoz.z = 8;
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePoz);
            transform.position = new Vector3(newPosition.x, newPosition.y > 1? newPosition.y : 1, newPosition.z);
        }
    }

    void OnMouseDown()
    {
        Debug.Log(mCardState);
        switch(mCardState)
        {
            case CARD_STATE.HAND:
                OnMouseDownHand();
                break;
            case CARD_STATE.MANAZONE:
                OnMouseDownTap();
                break;
            
            default:

                break;
        }
    }
    void OnMouseDownTap()
    {
        if (mIsTapped == false)
        {
            GameManager.instance.ManaTap(mCardCivilization);
                
            mIsTapped = true;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z);
        }
        else
        {
            GameManager.instance.ManaUntap(mCardCivilization);
            mIsTapped = false;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
        }
    }
  
    void OnMouseUp()
    {
        switch (mCardState)
        {
            case CARD_STATE.AIR:
                OnMouseUpAir();
                break;
            default:

                break;
        }
    }

    void OnMouseUpAir()
    {
        GameManager.instance.CardOnAir(false);

        if (mHasEnteredBattlezone == true)
        {
            mCardState = CARD_STATE.BATTLEZONE;
            mBattlezoneManager.AddCard(this);
            return;
        }

        if (mHasEnteredManazone == true)
        {
           
            mCardState = CARD_STATE.MANAZONE;
            mManazoneManager.AddCard(this);
            return;
        }

        mCardState = CARD_STATE.HAND;
        transform.rotation = mOrigRotation;
        transform.position = mOrigPosition;
    }

    void OnMouseDownHand()
    {
        mCardState = CARD_STATE.AIR;
        GameManager.instance.CardOnAir(true);
        transform.rotation = Quaternion.identity;
        transform.eulerAngles = new Vector3(0, 180, 0);
    }
    void OnTriggerEnter(Collider _collider)
    {
        if (_collider.gameObject.GetComponent<BattlezoneManager>() != null)
        {
            bool canSummon = GameManager.instance.CanSummon(mCardCivilization, mManaRequired);

           
            if (canSummon == true)
            {
                mHasEnteredBattlezone = true;
                mBattlezoneManager = _collider.gameObject.GetComponent<BattlezoneManager>();
            }
        }
        if (_collider.gameObject.GetComponent<ManazoneManager>() != null)
        {
            mHasEnteredManazone = true;
            mManazoneManager = _collider.gameObject.GetComponent<ManazoneManager>();
        }
    }

     void OnTriggerExit(Collider _collider)
    {
        if (_collider.gameObject.GetComponent<BattlezoneManager>() != null)
        {
            mHasEnteredBattlezone = false;
        }
        if (_collider.gameObject.GetComponent<ManazoneManager>() != null)
        {
            mHasEnteredManazone = false;
        }
    }
    

    public void SetHandManager(HandManager _handManager)
    {
        mHandManager = _handManager;
    }
}
