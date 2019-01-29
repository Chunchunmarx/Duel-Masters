using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManazoneManager : MonoBehaviour
{
    public List<Card> mCardList;
    private float mNextCardPoz = -3;

    public void AddCard(Card _card)
    {
        mCardList.Add(_card);
        _card.transform.position = new Vector3(transform.position.x + mNextCardPoz, transform.position.y + 0.1f, transform.position.z);
        mNextCardPoz += 2;
       _card.transform.eulerAngles = new Vector3(_card.transform.eulerAngles.x, _card.transform.eulerAngles.y + 180, _card.transform.eulerAngles.z);
    }
   
    void Start () {
		
	}
	
	
	void Update () {
		
	}
}
