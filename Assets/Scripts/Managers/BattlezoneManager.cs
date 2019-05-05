using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlezoneManager : GameZoneManager
{
  
    public List<Transform> mInitCardList;
    private PLAYER_ID mPlayerOwner = PLAYER_ID.INVALID;
    private List<Card> mBlockerList;

    void Awake()
    {
        mBlockerList = new List<Card>();
    }
    protected override void NotifyCardWasAdded(Card _card)
    {
        BattleState battleState = new BattleState(_card);
        battleState.SetBattlezoneManager(this);

        _card.SetCardState(battleState);

        if (_card.HasTraits(TRAITS.BLOCKER) == true)
        {
            mBlockerList.Add(_card);
        }

        battleState.WhenSummoned();
    }

   /*public void AddCardToManager(Card _card)
    {
        mCardList.Add(_card);
        _card.transform.position = new Vector3(transform.position.x + mNextCardPoz, transform.position.y + 0.1f, transform.position.z + 0.1f);
        mNextCardPoz += 1.5f;
        _card.transform.localScale = new Vector3(0.1f, 0.1f, 0.15f);
        _card.transform.eulerAngles = new Vector3(0, _card.transform.eulerAngles.y == 0 ? 0 : 180, 0);

        BattleState battleState = new BattleState(_card);
        battleState.SetBattlezoneManager(this);

        _card.SetCardState(battleState);

        if (_card.HasTraits(TRAITS.BLOCKER) == true)
        {
            mBlockerList.Add(_card);
        }

        battleState.WhenSummoned();
    }*/

    public void RemoveCard(Card _card)
    {
        mCardList.Remove(_card);
    }
    
    /*
    private void Init()
    {
        for (int i = 0; i < mInitCardList.Count; ++i)
        {
            Transform card = Instantiate(mInitCardList[i]);
            card.GetComponent<Card>().TestSetBattlezone();
            card.GetComponent<Card>().TestSetOwner();
            AddCard(card.GetComponent<Card>());
        }
    }*/

    public void NewTurn()
    {
        for (int i = 0; i < mCardList.Count; ++i)
        {
            mCardList[i].NewTurn();
        }
    }

    void Start ()
    {
        //Init();
	}
	
	void Update ()
    {

    }

    public void SetOwner(PLAYER_ID _owner)
    {
        mPlayerOwner = _owner;
    }

    public List<Card> GetConditionalList(ConditionData _data)
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < mCardList.Count; ++i)
        {
            _data.GetConditionCallback().Invoke(mCardList[i], _data);
            if (_data.Response == true)
            {
                list.Add(mCardList[i]);
            }
        }

        return list;
    }
}
