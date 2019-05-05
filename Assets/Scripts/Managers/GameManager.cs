using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    private BattleState mTargetingCard = null;
    private BattleState mTargetedCard = null;
    private int mNextID = 0;
    public Text mPlayerUIText;
    public Deck mDeck_One;
    public Deck mDeck_Two;
    private Deck mActiveDeck = null;

    public HandManager mHand_One;
    public HandManager mHand_Two;
    private HandManager mActiveHand = null;
    public ShieldzoneManager mShieldzone_One = null;
    public BattlezoneManager mBattlezone_One = null;
    public BattlezoneManager mBattlezone_Two = null;
    private BattlezoneManager mActiveBattlezone = null;
    public ManazoneManager mManazone_One = null;
    public ManazoneManager mManazone_Two = null;
    private ManazoneManager mActveManazone = null;

    private int mDrawNumber = -1;
    private int mTurnCount = 0;
  
    private PLAYER_ID mActivePlayer = PLAYER_ID.ONE;

    

    public void SetTargeting(BattleState _targetingCard)
    {
        mTargetingCard = _targetingCard;
    }

    public void SetTargeted(BattleState _targetedCard)
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
        mActivePlayer = mActivePlayer == PLAYER_ID.ONE ? PLAYER_ID.TWO : PLAYER_ID.ONE;
        ChangeUI();
        if (mActivePlayer == PLAYER_ID.ONE)
        {
            mZoneList = mZoneList_P1;
            mActveManazone = mManazone_One;
            mActiveDeck = mDeck_One;
            mActiveBattlezone = mBattlezone_One;
            mActiveHand = mHand_One;
        }
        else
        {
            mZoneList = mZoneList_P2;
            mActveManazone = mManazone_Two;
            mActiveDeck = mDeck_Two;
            mActiveBattlezone = mBattlezone_Two;
            mActiveHand = mHand_Two;
        }

        mActiveHand.Draw();
        mActveManazone.NewTurn();
        mActiveBattlezone.NewTurn();
        mGamePhase = GAME_PHASE.MANA_PHASE;
    }
    private void ChangeUI()
    {
        mUITransform.position = new Vector3(mUITransform.position.x, mUITransform.position.y, mUITransform.position.z * -1);
        mUITransform.eulerAngles = new Vector3(mUITransform.eulerAngles.x, mUITransform.eulerAngles.y + 180, mUITransform.eulerAngles.z);
        if (mActivePlayer == PLAYER_ID.ONE)
        {
            mPlayerUIText.text = "Player 1";
        }
        else
        {
            mPlayerUIText.text = "Player 2";
        }
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
        if(mTargetingCard.CanAttack() == false || mTargetedCard.CanBeAttacked() == false)
        {
            return false;
        }

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

        int attackerPower = mTargetingCard.GetPower();
        int attackedPower = mTargetedCard.GetPower();

        if (attackerPower == attackedPower)
        {
            //Destroy(mTargetingCard.GetGameObject());
            //Destroy(mTargetedCard.GetGameObject());

            mTargetingCard.GetGameObject().GetComponent<Card>().Defeated();
            mTargetedCard.GetGameObject().GetComponent<Card>().Defeated();
        }
        else if(attackerPower > attackedPower)
        {
            //Destroy(mTargetedCard.GetGameObject());
            mTargetedCard.GetGameObject().GetComponent<Card>().Defeated();
        }
        else
        {
            //Destroy(mTargetingCard.GetGameObject());
            mTargetingCard.GetGameObject().GetComponent<Card>().Defeated();
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
        mActiveDeck = mDeck_One;
        mActiveHand = mHand_One;

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

            mTargetingCard.SetIsTargeting(false);
          
            mIsTargeting = false;
            mTargetingCard = null;

            return;
        }

        if(mTargetingCard != null)
        {
            mIsTargeting = true;
            mTargetingCard.SetIsTargeting(true);
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
    public bool GetIsTargeting()
    {
        return mIsTargeting;
    }

    public void Draw()
    {
        mActiveHand.Draw();
        mDrawNumber--;
        CanDraw(mDrawNumber);
    }

    public void CanDraw(int _numberOfCards)
    {
        mDrawNumber = _numberOfCards;
        mActiveDeck.CanDraw(mDrawNumber);
    }

    public List<Card> GetConditionalList(ConditionData _data)
    {
        if(_data.Targets != TARGETS.ALL)
        {
            Debug.LogWarning("NEIMPLEMENTAT!");
            return null;
        }
        if(_data.Targets == TARGETS.ALL)
        {
            List<Card> returnList;
            List<Card> list_1 = new List<Card>();
            List<Card> list_2 = new List<Card>();
            list_1 = mBattlezone_One.GetConditionalList(_data);
            list_2 = mBattlezone_Two.GetConditionalList(_data);
            list_1.AddRange(list_2);
            return list_1;
        }
        return null;
    }

    public HandManager GetMyHandManager(Card _card)
    {
        if(_card.GetPlayerOwner() == PLAYER_ID.ONE)
        {
            return mHand_One;
        }
        else
        {
            return mHand_Two;
        }
    }
}
