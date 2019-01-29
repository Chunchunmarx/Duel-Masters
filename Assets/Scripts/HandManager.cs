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
        Transform prefab;
        mOrigPosition = transform.localPosition;

        prefab = Instantiate(mCardPrefab,transform.position, transform.rotation);
        prefab.localScale = transform.lossyScale;
        prefab.GetComponent<Card>().SetHandManager(this);


        transform.localPosition = new Vector3(0, 0, mOrigPosition.z - 10);
        prefab = Instantiate(mCardPrefab, transform.position, transform.rotation);
        prefab.localScale = transform.lossyScale;
        prefab.GetComponent<Card>().SetHandManager(this);


        transform.localPosition = new Vector3(0, 0, mOrigPosition.z + 10);
        prefab = Instantiate(mCardPrefab, transform.position, transform.rotation);
        prefab.localScale = transform.lossyScale;
        prefab.GetComponent<Card>().SetHandManager(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
