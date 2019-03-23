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
        GameManager.instance.CanDraw(_data.mMaxNumber);
    }

    public void Destroy(AbilitiesData _data)
    {
        if(_data.Condition_Data.Targets == TARGETS.ALL)
        {
            List<Card> cardList;
            cardList = GameManager.instance.GetConditionalList(_data.Condition, _data.Condition_Data);

            for(int i = 0; i < cardList.Count; ++i)
            {
                cardList[i].Defeated();
            }
        }
    }

    public void Tap(AbilitiesData _card)
    {
        //to be done
    }
}
