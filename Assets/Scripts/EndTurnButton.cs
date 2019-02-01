using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    void OnMouseDown()
    {
        GameManager.instance.EndTurn();
    }
}
