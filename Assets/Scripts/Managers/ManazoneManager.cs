using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManazoneManager : GameZoneManager
{
    //private PLAYER_ID mOwner;
    private int mNatureManaTapped = 0;
    private int mLightManaTapped = 0;
    private int mWaterManaTapped = 0;
    private int mDarknessManaTapped = 0;
    private int mFireManaTapped = 0;
    
    public bool CanPlayMana(Card _card)
    {
        if (GameManager.instance.GetGamePhase() != GAME_PHASE.MANA_PHASE)
        {
            return false;
        }

        GameManager.instance.SetGamePhase(GAME_PHASE.SUMMONING_PHASE);
        return true;
    }
    protected override void NotifyCardWasAdded(Card _card)
    {
        _card.SetUntappedEulerAngleY(_card.transform.eulerAngles.y);
    }
    /* public void AddCardToManager(Card _card)
      {     
          mCardList.Add(_card);
          _card.transform.position = new Vector3(transform.position.x + mNextCardPoz, transform.position.y + 0.21f, transform.position.z);
          mNextCardPoz += 1.5f;
          _card.transform.eulerAngles = new Vector3(transform.eulerAngles.x, _card.transform.eulerAngles.y == 0 ? 180 : 0, transform.eulerAngles.z);//_card.transform.eulerAngles.y == 0 ? 180 : 0
          _card.transform.localScale = new Vector3(0.1f, 0.1f, 0.15f);
          _card.SetUntappedEulerAngleY(_card.transform.eulerAngles.y);
      } */

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

    public void Summoned()
    {
        for (int i = 0; i < mCardList.Count; ++i)
        {
            if(mCardList[i].IsTapped() == true)
            {
                mCardList[i].LockTap();
            }

            mDarknessManaTapped = 0;
            mLightManaTapped = 0;
            mNatureManaTapped = 0;
            mWaterManaTapped = 0;
            mFireManaTapped = 0;
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

        mDarknessManaTapped = mDarknessManaTapped < 0 ? 0 : mDarknessManaTapped;
        mLightManaTapped = mLightManaTapped < 0 ? 0 : mLightManaTapped;
        mNatureManaTapped = mNatureManaTapped < 0 ? 0 : mNatureManaTapped;
        mWaterManaTapped = mWaterManaTapped < 0 ? 0 : mWaterManaTapped;
        mFireManaTapped = mFireManaTapped < 0 ? 0 : mFireManaTapped;
    }

    public void NewTurn()
    {
        for (int i = 0; i < mCardList.Count; ++i)
        {
            mCardList[i].NewTurn();
        }
    }
  

    void Start ()
    {

	}
	
	
	void Update () {
		
	}
}
