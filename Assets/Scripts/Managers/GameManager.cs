using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_ID
{
    INVALID = 0,
    ONE = 1,
    TWO = 2
};

public enum GAME_PHASE
{
    INVALID = 0,
    MANA_PHASE = 1,
    SUMMONING_PHASE = 2,
    ATTACKING_PHASE = 3

};

public class GameManager : MonoBehaviour
{
    public GAME_PHASE mGamePhase = GAME_PHASE.INVALID;
    private List<GameObject> mZoneList = null;
    public List<GameObject> mZoneList_P1 = null;
    public List<GameObject> mZoneList_P2 = null;
    public static GameManager instance = null;
    private bool mCanHover = true;
    private bool mIsTargeting = false;
    private Transform mHoveredCard = null;
    public Transform mUITransform;
    private Card mTargetingCard = null;
    private Card mTargetedCard = null;
    private int mNextID = 0;

    public Deck mDeck_One;
    public Deck mDeck_Two;

    public HandManager mHand_One;
    public HandManager mHand_Two;
    public ShieldzoneManager mShieldzone_One = null;
    public BattlezoneManager mBattlezone_One = null;
    public BattlezoneManager mBattlezone_Two = null;
    public ManazoneManager mManazone_One = null;
    public ManazoneManager mManazone_Two = null;
    private ManazoneManager mActveManazone = null;

    private int mTurnCount = 0;
  
    private PLAYER_ID mActivePlayer = PLAYER_ID.ONE;

    

    public void SetTargeting(Card _targetingCard)
    {
        mTargetingCard = _targetingCard;
    }

    public void SetTargeted(Card _targetedCard)
    {
        mTargetedCard = _targetedCard;
    }

    public bool IsTargeting()
    {
        return mIsTargeting;
    }

    public bool CanSummon(Card _card)
    {
        bool canSummon = mActveManazone.CanSummon(_card);

        if(canSummon == false)
        {
            return false;
        }

        mActveManazone.Summoned();
        return true;
    }

    public bool CanPlayMana(Card _card)
    {
        return mActveManazone.CanPlayMana(_card);
    }

    public void EndTurn()
    {
        mUITransform.position = new Vector3(mUITransform.position.x, mUITransform.position.y, mUITransform.position.z * -1);
        mUITransform.eulerAngles = new Vector3(mUITransform.eulerAngles.x, mUITransform.eulerAngles.y + 180, mUITransform.eulerAngles.z);

        mActivePlayer = mActivePlayer == PLAYER_ID.ONE ? PLAYER_ID.TWO : PLAYER_ID.ONE;
        if(mActivePlayer == PLAYER_ID.ONE)
        {
            mZoneList = mZoneList_P1;
            mActveManazone = mManazone_One;
            mHand_One.Draw();
        }
        else
        {
            mZoneList = mZoneList_P2;
            mActveManazone = mManazone_Two;
            mHand_Two.Draw();
        }

        mActveManazone.NewTurn();
        mGamePhase = GAME_PHASE.MANA_PHASE;
    }

    public void SetCanHover( bool _canHover)
    {
        mCanHover = _canHover;
    }

    public bool CanHover()
    {
        return mCanHover;
    }

    private bool PreconditionCheck()
    {
        if (mTargetedCard.GetPlayerOwner() == mTargetingCard.GetPlayerOwner())
        {
            return false;
        }

        return true;
    }

    private void BlockerPhase()
    {

    }

    private void BattleHandler()
    {
        if(PreconditionCheck() == false)
        {
            return;
        }

        mGamePhase = GAME_PHASE.ATTACKING_PHASE;
        BlockerPhase();

        if(mTargetingCard.mPower == mTargetedCard.mPower)
        {
            Destroy(mTargetingCard.gameObject);
            Destroy(mTargetedCard.gameObject);
        }
        else if(mTargetingCard.mPower > mTargetedCard.mPower)
        {
            Destroy(mTargetedCard.gameObject);
        }
        else
        {
            Destroy(mTargetingCard.gameObject);
        }
    }

    

    public void CardOnAir(bool _onAir)
    {
        if (_onAir == false)
        {
            for (int i = 0; i < mZoneList.Count; ++i)
            {
                mZoneList[i].GetComponent<Collider>().enabled = false;
               
            }
        }
        else 
        {
            for (int i = 0; i < mZoneList.Count; ++i)
            {
                mZoneList[i].GetComponent<Collider>().enabled = true;

            }
        }
    }

    void Awake()
    {

        ++mTurnCount;

        mActivePlayer = PLAYER_ID.ONE;
        mGamePhase = GAME_PHASE.MANA_PHASE;
        mActveManazone = mManazone_One;

        mHand_One.SetDeck(mDeck_One);
        mHand_Two.SetDeck(mDeck_Two);
        mDeck_One.SetOwner(PLAYER_ID.ONE);
        mDeck_Two.SetOwner(PLAYER_ID.TWO);
        mShieldzone_One.SetDeck(mDeck_One);

        mBattlezone_One.SetOwner(PLAYER_ID.ONE);
        mBattlezone_Two.SetOwner(PLAYER_ID.TWO);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start ()
    {
        mZoneList = mZoneList_P2;
        CardOnAir(false);
        mZoneList = mZoneList_P1;
        CardOnAir(false);
    }
	
	void Update ()
    {
        ClickHandle();
    }

    private void ClickHandle()
    {
        if(Input.GetMouseButtonDown(0) == false)
        {
            return;
        }

        if(mIsTargeting == true)
        {
            if(mTargetedCard != null)
            {
                BattleHandler();
                
                mTargetedCard = null;
            }

            mTargetingCard.StopTargeting();
            mTargetingCard = null;
            mIsTargeting = false;

            return;
        }

        if(mTargetingCard != null)
        {
            mIsTargeting = true;
        }
    }

    public void HoverEnter(Transform _hoveredCard)
    {
        if (mCanHover == false)
            return;

        if(mHoveredCard != null)
        {
            Destroy(mHoveredCard.gameObject);
        }

        Vector3 cameraScale = Camera.main.gameObject.transform.lossyScale;
        Vector3 cameraAngles = Camera.main.gameObject.transform.eulerAngles;
        Vector3 cameraPosition = Camera.main.gameObject.transform.localPosition;
        Vector3 position = new Vector3(- 2.15f, 0.35f, 3.5f);//valori harcodate, naspa!
        position = Camera.main.gameObject.transform.TransformPoint(position);
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(cameraAngles.x + 90f, cameraAngles.y, cameraAngles.z - 180);
        mHoveredCard = Instantiate(_hoveredCard, position, rotation);
        mHoveredCard.transform.localScale = new Vector3(cameraScale.x * 0.22f, cameraScale.y * 0.22f, cameraScale.z * 0.3f);

        Destroy(mHoveredCard.GetComponent<Card>());
    }

    public void HoverExit()
    {
        if (mHoveredCard != null)
        {
            Destroy(mHoveredCard.gameObject);
        }
    }

    public int GetID()
    {
        ++mNextID;
        return mNextID;
    }

    public PLAYER_ID GetActivePlayer()
    {
        return mActivePlayer;
    }

    public ManazoneManager GetActiveManazone()
    {
        return mActveManazone;
    }

    public GAME_PHASE GetGamePhase()
    {
        return mGamePhase;
    }

    public void SetGamePhase(GAME_PHASE _gamePhase)
    {
        if(_gamePhase <= mGamePhase)
        {
            return;
        }

        mGamePhase = _gamePhase;
    }
}
