using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlezoneManager : MonoBehaviour
{
    public List<Card> mCardList;
    public List<Transform> mInitCardList;
    private float mNextCardPoz = -2;
    private PLAYER_ID mPlayerOwner = PLAYER_ID.INVALID;
    private List<Card> mBlockerList;

    void Awake()
    {
        mBlockerList = new List<Card>();
    }

    public void AddCard(Card _card)
    {
        mCardList.Add(_card);
        _card.transform.position = new Vector3(transform.position.x + mNextCardPoz, transform.position.y + 0.1f, transform.position.z + 0.1f);
        mNextCardPoz += 1.5f;
        _card.transform.localScale = new Vector3(0.1f, 0.1f, 0.15f);

        BattleState battleState = new BattleState(_card);
        battleState.SetBattlezoneManager(this);

        _card.SetCardState(battleState);

        if (_card.HasTraits(TRAITS.BLOCKER) == true)
        {
            mBlockerList.Add(_card);
        }

        battleState.WhenSummoned();
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

    public List<Card> GetConditionalList(ConditionCallback _conditionCallback, ConditionData _data)
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < mCardList.Count; ++i)
        {
            _conditionCallback.Invoke(mCardList[i], _data);
            if (_data.Response == true)
            {
                list.Add(mCardList[i]);
            }
        }

        return list;
    }
}
