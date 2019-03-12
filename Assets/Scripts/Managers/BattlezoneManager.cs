using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlezoneManager : MonoBehaviour
{
    public List<Card> mCardList;
    public List<Transform> mInitCardList;
    private float mNextCardPoz = -2;

    public void AddCard(Card _card)
    {
        mCardList.Add(_card);
        _card.transform.position = new Vector3(transform.position.x + mNextCardPoz, transform.position.y + 0.1f, transform.position.z + 0.1f);
        mNextCardPoz += 1.5f;
        _card.transform.localScale = new Vector3(0.1f, 0.1f, 0.15f);
    }

    public bool CanSummon(Card _card)
    {
   

        if (GameManager.instance.GetGamePhase() > GAME_PHASE.SUMMONING_PHASE)
        {
            

            return false;
        }
       
         bool CanSummon =  GameManager.instance.GetActiveManazone().CanSummon(_card);
        if(CanSummon == false)
        {
            return false;
        
        }
        GameManager.instance.SetGamePhase(GAME_PHASE.SUMMONING_PHASE);
        return true;
        
    }

    private void Init()
    {
        for (int i = 0; i < mInitCardList.Count; ++i)
        {
            Transform card = Instantiate(mInitCardList[i]);
            card.GetComponent<Card>().TestSetBattlezone();
            card.GetComponent<Card>().TestSetOwner();
            AddCard(card.GetComponent<Card>());
        }
    }

    void Start ()
    {
        Init();
	}
	
	void Update ()
    {
		
	}
}
