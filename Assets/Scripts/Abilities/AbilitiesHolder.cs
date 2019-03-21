using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void AbilityDelegate(AbilitiesData _data);

[System.Serializable]
public class AbilitiesCallback: UnityEvent<AbilitiesData>
{

};

public class AbilitiesHolder : MonoBehaviour
{
    private AbilityDelegate mAbilityDelegate;

    public void Draw(AbilitiesData _data)
    {
        //Debug.Log("Draw X");
        GameManager.instance.CanDraw(_data.mMaxNumber);
    }

    public void Tap(AbilitiesData _card)
    {
        //to be done
    }
}
