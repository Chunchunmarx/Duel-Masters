using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> mCardList;
    private List<Card> mDeck;
    private PLAYER_ID mPlayerOwner = PLAYER_ID.INVALID;

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
        Debug.Log(mPlayerOwner);
        for (int i = 0; i < 40; ++i)
        {
            mDeck.Add(Instantiate(GetRandomCard(), transform.position, transform.rotation).GetComponent<Card>());
            mDeck[i].SetOwner(mPlayerOwner);
            mDeck[i].ChangeCardState(CARD_STATE.DECK);
        }
    }

    private Transform GetRandomCard()
    {
        return mCardList[(int)Random.Range(0, mCardList.Count)].transform;
    }

    public void SetOwner(PLAYER_ID _owner)
    {
        mPlayerOwner = _owner;
    }

    void Update ()
    {
		
	}
}
