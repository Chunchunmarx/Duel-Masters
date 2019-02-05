﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_ID
{
    ONE = 1,
    TWO = 2
};

public enum GAME_STATE
{
    DRAW_PHASE = 0,
    MANA_PHASE = 1,
    SUMMONING_PHASE = 2,
    ATTACK_PHASE = 3
};

public class GameManager : MonoBehaviour
{
  
    public List<GameObject> mZoneList = null;
    public static GameManager instance = null;
    private bool mCanHover = true;
    private bool mIsTargeting = false;
    private Transform mHoveredCard = null;
    public Transform mUITransform;
    private Card mTargetingCard = null;
    private Card mTargetedCard = null;
    private int mNextID = 0;
    public ManazoneManager mManazone_One;
    public ManazoneManager mManazone_Two;
    private int mTurnCount = 0;
    private GAME_STATE mGameState;
    private PLAYER_ID mActivePlayer = PLAYER_ID.ONE;

    public void SetTargeting(Transform _targetingCard)
    {
        Card targetingCard = _targetingCard.GetComponent<Card>();
        if (targetingCard == null)
        {
            Debug.LogWarning("GameManager::SetTargeting() parametrul nu are atasat Card!!!");
            return;
        }

        mTargetingCard = targetingCard;
    }

    public void SetTargeted(Transform _targetedCard)
    {
        Card targetedCard = _targetedCard.GetComponent<Card>();
        if(targetedCard == null)
        {
            Debug.LogWarning("GameManager::SetTargeted() parametrul nu are atasat Card!!!");
            return;
        }

        mTargetedCard = targetedCard;
    }

    public bool IsTargeting()
    {
        return mIsTargeting;
    }

    public void EndTurn()
    {
        mActivePlayer = mActivePlayer == PLAYER_ID.ONE ? PLAYER_ID.TWO : PLAYER_ID.ONE;
        mUITransform.position = new Vector3(mUITransform.position.x, mUITransform.position.y, mUITransform.position.z * -1);
        mUITransform.eulerAngles = new Vector3(mUITransform.eulerAngles.x, mUITransform.eulerAngles.y + 180, mUITransform.eulerAngles.z);
    }

    public void SetCanHover( bool _canHover)
    {
        mCanHover = _canHover;
    }

    public bool CanHover()
    {
        return mCanHover;
    }

    private void BattleHandler()
    {
        if(mTargetedCard.GetOwner() == mTargetingCard.GetOwner())
        {
            return;
        }

        if(mTargetingCard.mPower == mTargetedCard.mPower)
        {
            Destroy(mTargetingCard.gameObject);
            Destroy(mTargetedCard.gameObject);
        }
        else if(mTargetingCard.mPower > mTargetedCard.mPower)
        {
            Destroy(mTargetedCard.gameObject);
        }
        else
        {
            Destroy(mTargetingCard.gameObject);
        }
    }

    

    public void CardOnAir(bool _onAir)
    {
        if (_onAir == false)
        {
            for (int i = 0; i < mZoneList.Count; ++i)
            {
                mZoneList[i].GetComponent<Collider>().enabled = false;
               
            }
        }
        else 
        {
            for (int i = 0; i < mZoneList.Count; ++i)
            {
                mZoneList[i].GetComponent<Collider>().enabled = true;

            }
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        CardOnAir(false);
    }

   


    void Start ()
    {
        ++mTurnCount;
        mGameState = GAME_STATE.MANA_PHASE;
	}
	
	void Update ()
    {
        ClickHandle();
    }

    private void ClickHandle()
    {
        if(Input.GetMouseButtonDown(0) == false)
        {
            return;
        }

        if(mIsTargeting == true)
        {
            if(mTargetedCard != null)
            {
                BattleHandler();
                
                mTargetedCard = null;
            }

            mTargetingCard.StopTargeting();
            mTargetingCard = null;
            mIsTargeting = false;

            return;
        }

        if(mTargetingCard != null)
        {
            mIsTargeting = true;
        }
    }

    public void HoverEnter(Transform _hoveredCard)
    {
        if (mCanHover == false)
            return;

        if(mHoveredCard != null)
        {
            Destroy(mHoveredCard.gameObject);
        }

        Vector3 cameraScale = Camera.main.gameObject.transform.lossyScale;
        Vector3 cameraAngles = Camera.main.gameObject.transform.eulerAngles;
        Vector3 cameraPosition = Camera.main.gameObject.transform.localPosition;
        Vector3 position = new Vector3(- 2.15f, 0.35f, 3.5f);//valori harcodate, naspa!
        position = Camera.main.gameObject.transform.TransformPoint(position);
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(cameraAngles.x + 90f, cameraAngles.y, cameraAngles.z - 180);
        mHoveredCard = Instantiate(_hoveredCard, position, rotation);
        mHoveredCard.transform.localScale = new Vector3(cameraScale.x * 0.22f, cameraScale.y * 0.22f, cameraScale.z * 0.3f);

        Destroy(mHoveredCard.GetComponent<Card>());
    }

    public void HoverExit()
    {
        if (mHoveredCard != null)
        {
            Destroy(mHoveredCard.gameObject);
        }
    }

    public int GetID()
    {
        ++mNextID;
        return mNextID;
    }

    public PLAYER_ID GetActivePlayer()
    {
        return mActivePlayer;
    }
}
