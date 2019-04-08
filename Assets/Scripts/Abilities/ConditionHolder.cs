using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
[System.Serializable]
public class AbilitiesCallback : UnityEvent<Card>
{

};
*/

/*
public interface ICondition
{
    bool CheckCondition();
}
*/

[System.Serializable]
public class ConditionCallback : UnityEvent<Card, ConditionData>
{

};

public class ConditionHolder : MonoBehaviour
{
    public void NoCondition(Card _card, ConditionData _data)
    {

    }

    public void CheckTrait(Card _card, ConditionData _data)
    {
        if(_card.HasTraits(TRAITS.BLOCKER) == true)
        {
            _data.Response = true;
        }
        else
        {
            _data.Response = false;
        }
        // return true;
    }
    public void CheckPower(Card _card, ConditionData _data)
    {
        if (_card.GetPower() <= _data.Number)
        {
            _data.Response = true;
        }
        else
        {
            _data.Response = false;
        }
        // return true;
    }
}
