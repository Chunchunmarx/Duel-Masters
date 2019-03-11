using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManazoneManager : MonoBehaviour
{
    
    public List<Card> mCardList;
    private float mNextCardPoz = -2;
    //private PLAYER_ID mOwner;
    private int mNatureManaTapped = 0;
    private int mLightManaTapped = 0;
    private int mWaterManaTapped = 0;
    private int mDarknessManaTapped = 0;
    private int mFireManaTapped = 0;
    
    public bool CanPlayMana(Card _card)
    {
        if (GameManager.instance.GetGamePhase() == GAME_PHASE.MANA_PHASE)
        {
            GameManager.instance.SetGamePhase(GAME_PHASE.SUMMONING_PHASE);
            
            return true;
        }
        return false;
    }
    public void AddCard(Card _card)
    {
        mCardList.Add(_card);
        _card.transform.position = new Vector3(transform.position.x + mNextCardPoz, transform.position.y + 0.21f, transform.position.z);
        mNextCardPoz += 1.5f;
        _card.transform.eulerAngles = new Vector3(_card.transform.eulerAngles.x, _card.transform.eulerAngles.y + 180, _card.transform.eulerAngles.z);
        _card.transform.localScale = new Vector3(0.1f, 0.1f, 0.15f);
    }

    public bool CanSummon(Card _card)
    {
        switch (_card.GetCardCivilization())
        {
            case CARD_CIVILIZATION.NATURE:
                if (mNatureManaTapped == 0)
                    return false;
                break;
            case CARD_CIVILIZATION.FIRE:
                if (mFireManaTapped == 0)
                    return false;
                break;
            case CARD_CIVILIZATION.WATER:
                if (mWaterManaTapped == 0)
                    return false;
                break;
            case CARD_CIVILIZATION.DARKNESS:
                if (mDarknessManaTapped == 0)
                    return false;
                break;
            case CARD_CIVILIZATION.LIGHT:
                if (mLightManaTapped == 0)
                    return false;
                break;
            default:
                Debug.LogWarning("GameManager::CanSummon() parametru invalid");
                return false;
                break;
        }
        if (_card.GetManaRequired() == mLightManaTapped + mFireManaTapped + mNatureManaTapped + mDarknessManaTapped + mWaterManaTapped)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ManaTap(CARD_CIVILIZATION _cardCivilization)
    {
        switch (_cardCivilization)
        {
            case CARD_CIVILIZATION.NATURE:
                mNatureManaTapped = mNatureManaTapped + 1;
                break;
            case CARD_CIVILIZATION.FIRE:
                mFireManaTapped++;
                break;
            case CARD_CIVILIZATION.WATER:
                mWaterManaTapped++;
                break;
            case CARD_CIVILIZATION.DARKNESS:
                mDarknessManaTapped++;
                break;
            case CARD_CIVILIZATION.LIGHT:
                mLightManaTapped++;
                break;
            default:
                Debug.LogWarning("GameManager::ManaTap() parametru invalid");
                break;
        }
    }

    public void ManaUntap(CARD_CIVILIZATION _cardCivilization)
    {
        switch (_cardCivilization)
        {
            case CARD_CIVILIZATION.NATURE:
                mNatureManaTapped = mNatureManaTapped - 1;
                break;
            case CARD_CIVILIZATION.FIRE:
                mFireManaTapped--;
                break;
            case CARD_CIVILIZATION.WATER:
                mWaterManaTapped--;
                break;
            case CARD_CIVILIZATION.DARKNESS:
                mDarknessManaTapped--;
                break;
            case CARD_CIVILIZATION.LIGHT:
                mLightManaTapped--;
                break;
            default:
                Debug.LogWarning("GameManager::ManaTap() parametru invalid");
                break;
        }
    }
  

    void Start ()
    {

	}
	
	
	void Update () {
		
	}
}
