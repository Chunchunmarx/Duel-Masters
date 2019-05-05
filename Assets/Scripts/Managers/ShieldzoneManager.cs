using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldzoneManager : MonoBehaviour {
    private List<Card> mCardList;
    private Transform mCardPrefab;
    private Deck mDeck;
    //private float mNextCardPoz = -3;
   
    void Awake()
    {
        mCardList = new List<Card>();
    }
     
   void  Start()
    {
        InstantiateShield(5);   

    }
   

    private void InstantiateShield(int _shieldSize)
    {
        for (int i = _shieldSize - 1; i >= 0; --i)
        {

            Card cardToBeAdded;

            cardToBeAdded = mDeck.Draw();
            cardToBeAdded.transform.position = new Vector3(transform.position.x + i*2 - 4, transform.position.y + 0.1f, transform.position.z );
            cardToBeAdded.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            cardToBeAdded.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z + 180);

        }
    }



    public void SetDeck(Deck _deck)
    {
        mDeck = _deck;
    }
    public void AddCard(Card _card)
    {
        
        mCardList.Add(_card);
        //_card.transform.position = new Vector3(transform.position.x + mNextCardPoz, transform.position.y + 0.1f, transform.position.z);
        //mNextCardPoz += 2;
        
    }
  
	
	void Update () {
		
	}
}
