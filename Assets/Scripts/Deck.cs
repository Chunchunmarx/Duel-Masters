using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<CardFactory> mCardList;
    public Transform mCardPrefab;
    private List<Card> mDeck;
    private PLAYER_ID mPlayerOwner = PLAYER_ID.INVALID;
    private int mDrawNumber = -1;
    private Material mDeckMaterial = null;
    private Color mDeckColor;

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
        mDeckMaterial = GetComponentInParent<Renderer>().material;
        mDeckColor = mDeckMaterial.GetColor("_Color");
    }

    void Start ()
    {
        for (int i = 0; i < 40; ++i)
        {
            mDeck.Add(CreateCard());
            mDeck[i].SetOwner(mPlayerOwner);
            mDeck[i].SetCardState(new DeckState(mDeck[i]));
            //mDeck[i].ChangeCardState(CARD_STATE.DECK);
        }
    }

    private Card CreateCard()
    {
        Transform modelBody = GetRandomCard();
        CardFactory modelInfo = modelBody.GetComponent<CardFactory>();
        Material modelMaterial = modelBody.GetComponent<MeshRenderer>().sharedMaterials[0];

        Card newCard = Instantiate(mCardPrefab, transform.position, transform.rotation).GetComponent<Card>();

        newCard.SetCardCivilization(modelInfo.CardCivilization);
        newCard.SetPower(modelInfo.Power);
        newCard.SetManaRequired(modelInfo.ManaRequired);
        
        newCard.GetComponent<MeshRenderer>().material = modelMaterial;
        newCard.SetTraits(modelInfo.Traits);
        newCard.SetAbilityData(modelInfo.AbilityData);

        return newCard;
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

    public void CanDraw(int _numberOfCards)
    {
        mDrawNumber = _numberOfCards;

        //mDeckColor.
        if (_numberOfCards > 0)
        {
            mDeckMaterial.SetColor("_Color", Color.white);
        }
        else
        {
            mDeckMaterial.SetColor("_Color", mDeckColor);
        }
    }

    void OnMouseDown()
    {
        if(mDrawNumber < 1)
        {
            return;
        }

        GameManager.instance.Draw();
    }
}
