using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CARD_CIVILIZATION
{
    INVALID,
    NATURE,
    DARKNESS,
    FIRE,
    LIGHT,
    WATER
};

public delegate void SendCardTo();

public class Card : MonoBehaviour
{
    private List<TRAITS> mTraits;
    private CARD_CIVILIZATION mCardCivilization;
    //private CARD_STATE mCardState.GetState()= CARD_STATE.INVALID;
    private PLAYER_ID mPlayerOwner = PLAYER_ID.INVALID;
    public Vector3 mOrigPosition;
    public Quaternion mOrigRotation;
    private float mUntappedEulerAngleY;
    private float mTappedEulerAngleY;

    private BattlezoneManager mBattlezoneManager;
    //private ManazoneManager mManazoneManager;
    private CardState mCardState;

    private int mPower;
    private int mManaRequired;
    private int mID;
    private bool mHasEnteredBattlezone = false;
    private bool mHasEnteredManazone = false;
    private bool mIsInAir = false;

    private AbilitiesData mAbilityData = null;
   

    private LineRenderer mLineRenderer;

    //spamez carti pe bord sa testez atacul
    public void TestSetBattlezone()//functie pusa doar pt test, de sters
    {
        //ChangeCardState(CARD_STATE.BATTLEZONE);
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
        mLineRenderer.enabled = false;

        mID = GameManager.instance.GetID();
        
        //ChangeCardState(CARD_STATE.DECK);
        //SetCardState(new HandState(GetComponent<Card>()));
    }	

    public void NewTurn()
    {
        //mManazoneManager = GameManager.instance.GetActiveManazone();
        mCardState.NewTurn();
    }

    public bool IsTapped()
    {
        return mCardState.IsTapped();
    }

    public void LockTap()
    {
        mCardState.LockTap();
    }

    void Start ()
    {
        //mManazoneManager = GameManager.instance.GetActiveManazone();
    }
	
	void Update ()
    {
        HandleAir();
        TargetingLine();

        if (GameManager.instance.GetIsTargeting() == true || mIsInAir == true)
        {
            GameManager.instance.SetCanHover(false);
        }
        else
        {
            GameManager.instance.SetCanHover(true);
        }

    }

    void HandleAir()
    {
        if(mIsInAir == true)
        {
            //idk what is this and I am lazy so it shall rule this kingdom.
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
        mCardState.OnClick();
    }

    void OnMouseUp()
    {
        if (mIsInAir == false)
        {
            return;
        }

        GameManager.instance.CardOnAir(false);

        if (mHasEnteredBattlezone == true && GameManager.instance.CanSummon(GetComponent<Card>()) == true)
        {
            mBattlezoneManager.AddCardToManager(this);
            return;
        }

        if (mHasEnteredManazone == true && GameManager.instance.CanPlayMana(GetComponent<Card>()) == true)
        {
            SetCardState(new ManaState(GetComponent<Card>()));
            GameManager.instance.GetActiveManazone().AddCardToManager(this);
            return;
        }

        mCardState.OnMouseUp();
    }

    void OnTriggerEnter(Collider _collider)
    {
        if (_collider.gameObject.GetComponent<BattlezoneManager>() != null)
        {
           
                mHasEnteredBattlezone = true;
                mBattlezoneManager = _collider.gameObject.GetComponent<BattlezoneManager>();
            
        }
        if (_collider.gameObject.GetComponent<ManazoneManager>() != null)
        {
            mHasEnteredManazone = true;
            //mManazoneManager = _collider.gameObject.GetComponent<ManazoneManager>();
        }
    }

    public void Defeated()
    {
        if (mAbilityData.mAbilityMoment == ABILITY_MOMENT.DEATHRATTLE)
        {
            mAbilityData.DoAbility(GetComponent<Card>());
        }
        else
        {
            mCardState.ToGraveyard();
        }
    }
	
	public void DestroyCard()
	{
		Destroy(gameObject);
	}

    public void ToHand()
    {
        mCardState.ToHand();
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
    /*
    public void ChangeCardState(CARD_STATE _cardState)
    {
        //Debug.Log(_cardState + " " + mIsTargeting);
        if(_cardState == CARD_STATE.HAND)
        {
            return;
        }

        if (_cardState == mCardState.GetState())
        {
            Debug.LogWarning("Se schimba stateul cartii intr-o stare in care suntem deja!!!");
            //return;
        }

     

        if (mCardState.GetState()== CARD_STATE.AIR && _cardState != CARD_STATE.HAND)
        {
        }

        //mCardState.GetState()= _cardState;


        if (mCardState.GetState()== CARD_STATE.HAND)
        {
            transform.rotation = mOrigRotation;
            transform.position = mOrigPosition;
        }

        if (mCardState.GetState()== CARD_STATE.BATTLEZONE)
        {
            SetCardState(new BattleState(GetComponent<Card>()));
        }
        if (mCardState.GetState()== CARD_STATE.MANAZONE)
        {
            SetCardState(new ManaState(GetComponent<Card>()));
        }
    }*/

  

    void OnMouseEnter()
    {
        if(mCardState.GetState()== CARD_STATE.MANAZONE)
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

    public void SetCardCivilization(CARD_CIVILIZATION _civilization)
    {
        mCardCivilization = _civilization;
    }

    public int GetManaRequired()
    {
        return mManaRequired;
    }

    public void SetManaRequired(int _manaRequired)
    {
        mManaRequired = _manaRequired;
    }

    public void SetOrgigPosition(Vector3 _origPosition)
    {
        mOrigPosition = _origPosition;
    }

    public void SetOrigRotation(Quaternion _origRotation)
    {
        mOrigRotation = _origRotation;
    }

    public void SetUntappedEulerAngleY(float _angle)
    {
        mUntappedEulerAngleY = _angle;
        mTappedEulerAngleY = mUntappedEulerAngleY + 90;
    }

    public float GetUntappedEulerAngleY()
    {
        return mUntappedEulerAngleY;
    }

    public void SetOwner(PLAYER_ID _owner)
    {
        mPlayerOwner = _owner;
    }

    void AtTheEndOfTheTurn()
    {

    }

    void AtTheBeginningOfTheTurn()
    {

    }
   public void TurnLineRenderer(bool _isOn)
    {
        mLineRenderer.enabled = _isOn;

    }
    public void SetCardState(CardState _state)
    {
        if (mCardState != null)
        {
            mCardState.LeaveState();
        }
        mCardState = _state;
    }

    public void SetIsInAir(bool _inAir)
    {
        mIsInAir = _inAir;
    }

    public void SetPower(int _power)
    {
        mPower = _power;
    }

    public int GetPower()
    {
        return mPower;
    }

    public bool HasTraits(TRAITS _trait)
    {
        for(int i = 0; i < mTraits.Count; ++i)
        {
            if(mTraits[i] == _trait)
            {
                return true;
            }
        }

        return false;
    }

    public void SetTraits(List<TRAITS> _list)
    {
        mTraits = _list;
    }

    public void SetAbilityData(AbilitiesData _data)
    {
        mAbilityData = _data;
    }

    public AbilitiesData GetAbilityData()
    {
        return mAbilityData;
    }
}
