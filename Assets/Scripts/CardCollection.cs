using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    public List<Card> mCardList;
    public Material mCardBack;
    public static CardCollection instance = null;

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

    }

    public Transform GetRandomCard()
    {
        return mCardList[(int)Random.Range(0, mCardList.Count)].transform;
    }
}
