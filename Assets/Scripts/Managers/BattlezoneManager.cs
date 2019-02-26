using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlezoneManager : MonoBehaviour
{
    public List<Card> mCardList;
    public List<Transform> mInitCardList;
    private float mNextCardPoz = -3;

    public void AddCard(Card _card)
    {
        mCardList.Add(_card);
        _card.transform.position = new Vector3(transform.position.x + mNextCardPoz, transform.position.y + 0.1f, transform.position.z);
        mNextCardPoz += 2;
        _card.transform.localScale = new Vector3(0.15f, 0.15f, 0.2f);
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
