using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ABILITY_MOMENT
{
    INVALID = 0,
    BATTLECRY,
    END_OF_TURN,
    BEGINNING_OF_TURN,
    DEATHRATTLE
};


[System.Serializable]
public class AbilitiesData
{
    [SerializeField]
    public AbilitiesCallback mAbilityCallback = null;
    [SerializeField]
    public ABILITY_MOMENT mAbilityMoment = ABILITY_MOMENT.INVALID;
    [SerializeField]
    public int mMaxNumber = -1;

}
