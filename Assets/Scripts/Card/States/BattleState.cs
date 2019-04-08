using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleState : CardState
{
    private bool mIsTapped = false;
    private bool mHasSummoningSickness;
    private bool mIsTargeting = false;
    private BattlezoneManager mBattlezoneManager = null;

    public BattleState(Card _card) : base(_card)
    {
        mHasSummoningSickness = true;
        mCardState = CARD_STATE.BATTLEZONE;
    }

    public PLAYER_ID GetPlayerOwner()
    {
        return mCardReference.GetPlayerOwner();
    }

    public bool CanAttack()
    {
        if (mIsTapped == true)
        {
            return false;
        }
        if(mHasSummoningSickness == true)
        {
            return false;
        }

        return true;
    }

    public bool CanBeAttacked()
    {/*
        if (mIsTapped == true)
        {
            return false;
        }
        */
        return true;
    }

    public override void OnClick()
    {
        if (GameManager.instance.IsTargeting() == false)
        {
            if (GameManager.instance.GetActivePlayer() != mCardReference.GetPlayerOwner())
            {
                return;
            }

            if (mHasSummoningSickness == true)
            {
                return;
            }

            GameManager.instance.SetTargeting(this);
            
        }
        else
        {
            GameManager.instance.SetTargeted(this);
        }
    }

    public override void NewTurn()
    {
        mIsTapped = false;
        mHasSummoningSickness = false;
    }

    public override void LeaveState()
    {
        mBattlezoneManager.RemoveCard(mCardReference);
    }

    public int GetPower()
    {
        return mCardReference.GetPower();
    }

    public GameObject GetGameObject()
    {
        return mCardReference.gameObject;
    }

   
    public void SetIsTargeting(bool _targeting)
    {
        mIsTargeting = _targeting;
        mCardReference.TurnLineRenderer(mIsTargeting);
    }

    public void SetBattlezoneManager(BattlezoneManager _manager)
    {
        mBattlezoneManager = _manager;
    }

    public void WhenSummoned()
    {
        AbilitiesData abilityData = mCardReference.GetAbilityData();
        //AbilitiesCallback ability = abilityData.mAbilityCallback;

        if(abilityData.mAbilityMoment != ABILITY_MOMENT.BATTLECRY)
        {
            //Debug.LogWarning("Se cere battlecry cand abiltiatea nu e battlecry!!!");
            return;
        }

        //de sters
        //ConditionData condition = new ConditionData();
        //abilityData.Condition.Invoke(mCardReference, condition);
        //ability.Invoke(abilityData);
        abilityData.DoAbility(mCardReference);
    }
}
