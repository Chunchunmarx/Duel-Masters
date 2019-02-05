using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CARD_STATE
{
    HAND,
    BATTLEZONE,
    AIR,
    MANAZONE,
    TARGETING
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
    private CARD_CIVILIZATION mCardCivilization;
    private CARD_STATE mCardState;
    private PLAYER_ID mOwner;
    private Vector3 mOrigPosition;
    private Quaternion mOrigRotation;

    private BattlezoneManager mBattlezoneManager;
    private HandManager mHandManager;
    private ManazoneManager mManazoneManager;

    public int mPower;
    public int mManaRequired;
    private int mID;
    bool mHasEnteredBattlezone = false; 
    bool mHasEnteredManazone = false;
    bool mIsTapped = false;

    private LineRenderer mLineRenderer;

    //spamez carti pe bord sa testez atacul
    public void TestSetBattlezone()//functie pusa doar pt test, de sters
    {
        mCardState = CARD_STATE.BATTLEZONE;
    }

    public void TestSetOwner()
    {
        mOwner = PLAYER_ID.TWO;
    }
    
    public PLAYER_ID GetOwner()
    {
        return mOwner;
    }
    
    void Awake ()
    {
        mID = GameManager.instance.GetID();
        mLineRenderer = GetComponent<LineRenderer>();
        ChangeCardState(CARD_STATE.HAND);
        mOrigPosition = transform.position;
        mOrigRotation = transform.rotation;
        mOwner = PLAYER_ID.ONE;
    }	
	
	void Update ()
    {
        HandleAir();
        TargetingLine();
	}

    void HandleAir()
    {
        if(mCardState == CARD_STATE.AIR)
        {
            Vector3 mousePoz = Input.mousePosition;
            mousePoz.z = 8;
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePoz);
            transform.position = new Vector3(newPosition.x, newPosition.y > .1f? newPosition.y : .1f, newPosition.z);
        }
    }

    void OnMouseDown()
    {
        switch (mCardState)
        {
            case CARD_STATE.HAND:
                OnMouseDownHand();
                break;
            case CARD_STATE.MANAZONE:
                OnMouseDownManazone();
                break;
            case CARD_STATE.BATTLEZONE:
                OnMouseDownBattlezone();
                break;
            default:
                break;
        }
    }

    void OnMouseDownBattlezone()
    {
        if (GameManager.instance.IsTargeting() == false)
        {
            if(GameManager.instance.GetActivePlayer() != mOwner)
            {
                return;
            }

            GameManager.instance.SetTargeting(transform);
            ChangeCardState(CARD_STATE.TARGETING);
        }
        else
        {
            GameManager.instance.SetTargeted(transform);
        }
    }

    void OnMouseDownManazone()
    {
        if(GameManager.instance.GetActivePlayer() != mOwner)
        {
            return;
        }

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
            ChangeCardState(CARD_STATE.BATTLEZONE);
            mBattlezoneManager.AddCard(this);
            return;
        }

        if (mHasEnteredManazone == true)
        {
            ChangeCardState(CARD_STATE.MANAZONE);
            mManazoneManager.AddCard(this);
            return;
        }
        
        ChangeCardState(CARD_STATE.HAND);
        transform.rotation = mOrigRotation;
        transform.position = mOrigPosition;
    }

    void OnMouseDownHand()
    {
        if (GameManager.instance.GetActivePlayer() != mOwner)
        {
            return;

        }
        ChangeCardState(CARD_STATE.AIR);
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

    private void TargetingLine()
    {
        if(mLineRenderer.enabled == true)
        {
            Vector3 mousePoz = Input.mousePosition;
            mousePoz.z = 8;
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePoz);

            mLineRenderer.SetPosition(0, transform.position);
            mLineRenderer.SetPosition(1, new Vector3(newPosition.x, newPosition.y > .1f ? newPosition.y : .1f, newPosition.z));
        }
    }

    private void ChangeCardState(CARD_STATE _cardState)
    {
        mCardState = _cardState;

        if(mCardState == CARD_STATE.TARGETING || mCardState == CARD_STATE.AIR)
        {
            GameManager.instance.SetCanHover(false);
        }
        else
        {
            GameManager.instance.SetCanHover(true);
        }

        if (mCardState == CARD_STATE.TARGETING)
        {
            mLineRenderer.enabled = true;
        }
        else
        {
            mLineRenderer.enabled = false;
        }
    }

    public void StopTargeting()
    {
        ChangeCardState(CARD_STATE.BATTLEZONE);
    }

    void OnMouseEnter()
    {
        if(mCardState == CARD_STATE.MANAZONE)
        {
            return;
        }

        GameManager.instance.HoverEnter(transform);
    }

    void OnMouseExit()
    {
        GameManager.instance.HoverExit();
    }

    public int GetID()
    {
        return mID;
    }

    public CARD_CIVILIZATION GetCardCivilization()
    {
        return mCardCivilization;
    }

    public int GetManaRequired()
    {
        return mManaRequired;
    }
}
