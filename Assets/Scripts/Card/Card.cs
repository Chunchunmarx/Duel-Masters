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
    public CARD_CIVILIZATION mCardCivilization;
    private CARD_STATE mCardState = CARD_STATE.INVALID;
    private PLAYER_ID mPlayerOwner;
    private Vector3 mOrigPosition;
    private Quaternion mOrigRotation;

    private BattlezoneManager mBattlezoneManager;
    private HandManager mHandManager;
    private ManazoneManager mManazoneManager;
    private CardState mCardStateR;

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
        ChangeCardState(CARD_STATE.BATTLEZONE);
    }

    public void TestSetOwner()
    {
        mPlayerOwner = PLAYER_ID.TWO;
    }
    
    public PLAYER_ID GetPlayerOwner()
    {
        return mPlayerOwner;
    }
    
    void Awake ()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        mID = GameManager.instance.GetID();
        
        ChangeCardState(CARD_STATE.DECK);
        mPlayerOwner = PLAYER_ID.ONE;
        mCardStateR = new HandState(GetComponent<Card>());
    }	

    void Start ()
    {
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
            /*
            Debug.Log(Input.mousePosition + " " + Screen.width + " " + Screen.height);
            float middleWidth = Screen.width / 2;
            float middleHeight = Screen.height / 2;

            float translatedWidth = Input.mousePosition.x - middleWidth;
            float translatedHeight = Input.mousePosition.y - middleHeight;

            float newPosition_X = 8 * translatedWidth / 188;
            //Debug.Log(translatedHeight)
            float newPosition_Z = 5 * translatedHeight / 106;

            transform.position = new Vector3(newPosition_X, 0.1f, newPosition_Z);
            */
            Vector3 mousePoz = Input.mousePosition;
            mousePoz.z = 8;
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePoz);
            transform.position = new Vector3(newPosition.x, newPosition.y > .1f ? newPosition.y : .1f, newPosition.z);
        }
    }

    void OnMouseDown()
    {
        mCardStateR.OnClick();
    }

    void OnMouseDownBattlezone()
    {
        if (GameManager.instance.IsTargeting() == false)
        {
            if(GameManager.instance.GetActivePlayer() != mPlayerOwner)
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
        if(GameManager.instance.GetActivePlayer() != mPlayerOwner)
        {
            return;
        }

        ManazoneManager activeManazone = GameManager.instance.GetActiveManazone();
        if (mIsTapped == false)
        {
            activeManazone.ManaTap(mCardCivilization);
                
            mIsTapped = true;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z);
        }
        else
        {
            activeManazone.ManaUntap(mCardCivilization);
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
    }

    void OnMouseDownHand()
    {
        if (GameManager.instance.GetActivePlayer() != mPlayerOwner)
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
            bool canSummon = GameManager.instance.GetActiveManazone().CanSummon(this);

           
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

    public void ChangeCardState(CARD_STATE _cardState)
    {
        if (_cardState == mCardState)
        {
            Debug.LogWarning("Se schimba stateul cartii intr-o stare in care suntem deja!!!");
            return;
        }

        if (mCardState == CARD_STATE.AIR && _cardState != CARD_STATE.HAND)
        {
            mHandManager.RemoveCardFromHand(this);
        }

        mCardState = _cardState;

        if (mCardState == CARD_STATE.TARGETING || mCardState == CARD_STATE.AIR)
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

        if (mCardState == CARD_STATE.HAND)
        {
            transform.rotation = mOrigRotation;
            transform.position = mOrigPosition;
        }

        if (mCardState == CARD_STATE.BATTLEZONE)
        {
            mCardStateR = new BattleState(GetComponent<Card>());
        }
        if (mCardState == CARD_STATE.MANAZONE)
        {
            mCardStateR = new ManaState(GetComponent<Card>());
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

    public void SetOrgigPosition(Vector3 _origPosition)
    {
        mOrigPosition = _origPosition;
    }

    public void SetOrigRotation(Quaternion _origRotation)
    {
        mOrigRotation = _origRotation;
    }

    public void SetInHand()
    {
        ChangeCardState(CARD_STATE.HAND);
    }
    public void SetIsTapped(bool _isTapped)
    {
        mIsTapped = _isTapped;
    }
    public bool GetIsTapped()
    {
        return mIsTapped;
    }
}
