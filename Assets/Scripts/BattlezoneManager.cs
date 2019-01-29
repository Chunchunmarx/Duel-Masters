using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlezoneManager : MonoBehaviour
{
    public List<Card> mCardList;
    private float mNextCardPoz = -3;

    public void AddCard(Card _card)
    {
        mCardList.Add(_card);
        _card.transform.position = new Vector3(transform.position.x + mNextCardPoz, transform.position.y + 0.1f, transform.position.z);
        mNextCardPoz += 2;
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
