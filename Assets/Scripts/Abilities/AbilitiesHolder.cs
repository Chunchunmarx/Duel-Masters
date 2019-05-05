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
        if(_data.GetConditionData().Targets == TARGETS.ALL)
        {
            List<Card> cardList;
            cardList = GameManager.instance.GetConditionalList(_data.GetConditionData());

            for(int i = 0; i < cardList.Count; ++i)
            {
                cardList[i].Defeated();
            }
        }
    }

    public void ReturnToHand(AbilitiesData _data)
    {
        List<Card> cardList;
        if (_data.GetConditionData().Targets == TARGETS.ALL)
        {
            cardList = GameManager.instance.GetConditionalList(_data.GetConditionData());

            for (int i = 0; i < cardList.Count; ++i)
            {
                cardList[i].ToHand();
            }
        }
        else if(_data.GetConditionData().Targets == TARGETS.SELF)
        {
            _data.mCaster.ToHand();
        }
    }

        public void Tap(AbilitiesData _card)
    {
        //to be done
    }
}
