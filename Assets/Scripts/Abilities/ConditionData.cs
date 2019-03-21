using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TARGETS
{
    INVALID = 0,
    ALL,
    ENEMY,
    PLAYER,
    CHOICE
}
public class ConditionData
{
    public bool Response = false;
    public int Number = 0;
    public TRAITS Trait = TRAITS.INVALID;
    public TARGETS Targets = TARGETS.INVALID;
}
