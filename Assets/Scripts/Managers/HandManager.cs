using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    private List<Card> mCardList;
    private Transform mCardPrefab;
    private Deck mDeck;
    private PLAYER_ID mPlayerOwner = PLAYER_ID.INVALID;
    // Use this for initialization

    void Awake()
    {
        mCardList = new List<Card>();
    }

	void Start ()
    {
        InstantiateHand(5);
    }

    private void InstantiateHand(int _handSize)
    {
        for (int i = _handSize - 1; i >= 0; --i)
        {
            Draw();
        }
    }

    public void Draw()
    {
        AddCardToHand(mDeck.Draw());
    }

    private void RepositionCards()
    {
        int handSize = mCardList.Count;
        for (int i = handSize - 1; i >= 0; --i)
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(transform.eulerAngles.x + 1, transform.eulerAngles.y, transform.eulerAngles.z);
            position = transform.TransformPoint(new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 10 + 27f / (handSize) * i));
            position = new Vector3(transform.position.x, position.y, position.z);
            mCardList[i].transform.rotation = rotation;
            mCardList[i].transform.position = position;
            mCardList[i].transform.localScale = transform.lossyScale;

            mCardList[i].SetOrgigPosition(position);
            mCardList[i].SetOrigRotation(rotation);
        }
    }

    public void RemoveCardFromHand(Card _card)
    {
        mCardList.Remove(_card);
        RepositionCards();
    }

    public void AddCardToHand(Card _card)
    {
        mCardList.Add(_card);
        //mCardList[mCardList.Count - 1].SetHandManager(this);

        HandState handState = new HandState(mCardList[mCardList.Count - 1]);
        handState.SetHandManager(this);
        mCardList[mCardList.Count - 1].SetCardState(handState);
        mCardList[mCardList.Count - 1].SetUntappedEulerAngleY(transform.eulerAngles.y);
        
        RepositionCards();
    }
	
    public void SetDeck(Deck _deck)
    {
        mDeck = _deck;
    }

    public void SetOwner(PLAYER_ID _owner)
    {
        mPlayerOwner = _owner;
    }

    // Update is called once per frame
    void Update ()
    {
        RepositionCards();
    }
}
