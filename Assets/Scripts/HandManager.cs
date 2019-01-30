using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public List<Card> mCardList;
    public Transform mCardPrefab;
    public Vector3 mOrigPosition;
	// Use this for initialization

	void Start ()
    {
        InstantiateHand(5);
    }

    private void InstantiateHand(int _handSize)
    {
        Transform prefab;
        mOrigPosition = transform.localPosition;
        for (int i = _handSize - 1; i >= 0; --i)
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(transform.eulerAngles.x + 1, transform.eulerAngles.y, transform.eulerAngles.z);
            //transform.localPosition = new Vector3(0, i/100, mOrigPosition.z + 10 - 20/(_handSize - 1)*i);
            position = transform.TransformPoint(new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 10 + 27f/(_handSize) * i));
            prefab = Instantiate(CardCollection.instance.GetRandomCard(), position, rotation);
            prefab.localScale = transform.lossyScale;
            prefab.GetComponent<Card>().SetHandManager(this);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
