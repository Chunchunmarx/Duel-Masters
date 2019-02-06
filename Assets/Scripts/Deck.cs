using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> mCardList;
    private List<Card> mDeck;

    public Card Draw()
    {
        if(mDeck.Count == 0)
        {
            Debug.LogWarning("Se trage carte, dar in deck sunt 0 carti!!!");
            return null;
        }

        Card card = mDeck[mDeck.Count - 1];

        mDeck.Remove(card);

        return card;
    }

    void Awake()
    {
        mDeck = new List<Card>();
    }

    void Start ()
    { 
        for (int i = 1; i < 40; ++i)
        {
            mDeck.Add(Instantiate(GetRandomCard(), transform.position, transform.rotation).GetComponent<Card>());
        }
    }

    private Transform GetRandomCard()
    {
        return mCardList[(int)Random.Range(0, mCardList.Count)].transform;
    }
	
	void Update ()
    {
		
	}
}
