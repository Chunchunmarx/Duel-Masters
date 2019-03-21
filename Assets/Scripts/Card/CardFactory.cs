using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardFactory : MonoBehaviour
{
    public CARD_CIVILIZATION CardCivilization;
    public int Power;
    public int ManaRequired;
    public List<EFFECTS> Effects;
    //public AbilitiesCallback Ability;

    [SerializeField]
    public AbilitiesData AbilityData;
}
