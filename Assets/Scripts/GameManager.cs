using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int mNatureManaTapped = 0;
    private int mLightManaTapped = 0;
    private int mWaterManaTapped = 0;
    private int mDarknessManaTapped = 0;
    private int mFireManaTapped = 0;
    public List<GameObject> mZoneList = null;
    public static GameManager instance = null;
    private bool mCanHover = true;
    private bool mIsTargeting = false;
    private Transform mHoveredCard = null;

    public void SetIsTargeting(bool _isTargeting)
    {
        mIsTargeting = _isTargeting;
    }

    public bool IsTargeting()
    {
        return mIsTargeting;
    }

    public void SetCanHover(bool _canHover)
    {
        mCanHover = _canHover;
    }

    public bool CanHover()
    {
        return mCanHover;
    }

    public bool CanSummon(CARD_CIVILIZATION _cardCivilization, int _manaRequired)
    {
        switch(_cardCivilization)
        {
            case CARD_CIVILIZATION.NATURE:
                if (mNatureManaTapped == 0)
                    return false;
                break;
            case CARD_CIVILIZATION.FIRE:
                if (mFireManaTapped == 0)
                    return false;
                break;
            case CARD_CIVILIZATION.WATER:
                if (mWaterManaTapped == 0)
                    return false;
                break;
            case CARD_CIVILIZATION.DARKNESS:
                if (mDarknessManaTapped == 0)
                    return false;
                break;
            case CARD_CIVILIZATION.LIGHT:
                if (mLightManaTapped == 0)
                    return false;
                break;
            default:
                Debug.LogWarning("GameManager::CanSummon() parametru invalid");
                return false;
                break;
        }
        if (_manaRequired == mLightManaTapped + mFireManaTapped + mNatureManaTapped + mDarknessManaTapped + mWaterManaTapped)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void CardOnAir(bool _onAir)
    {
        if (_onAir == false)
        {
            for (int i = 0; i < mZoneList.Count; ++i)
            {
                mZoneList[i].GetComponent<Collider>().enabled = false;
               
            }
        }
        else 
        {
            for (int i = 0; i < mZoneList.Count; ++i)
            {
                mZoneList[i].GetComponent<Collider>().enabled = true;

            }
        }
    }
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

    public void ManaTap(CARD_CIVILIZATION _cardCivilization)
    {
        switch (_cardCivilization)
        {
            case CARD_CIVILIZATION.NATURE:
                mNatureManaTapped = mNatureManaTapped + 1;
                break;
            case CARD_CIVILIZATION.FIRE:
                mFireManaTapped++;
                break;
            case CARD_CIVILIZATION.WATER:
                mWaterManaTapped++;
                break;
            case CARD_CIVILIZATION.DARKNESS:
                mDarknessManaTapped++;
                break;
            case CARD_CIVILIZATION.LIGHT:
                mLightManaTapped++;
                break;
            default:
                Debug.LogWarning("GameManager::ManaTap() parametru invalid");
                break;
        }
    }
    public void ManaUntap(CARD_CIVILIZATION _cardCivilization)
    {
        switch (_cardCivilization)
        {
            case CARD_CIVILIZATION.NATURE:
                mNatureManaTapped = mNatureManaTapped - 1;
                break;
            case CARD_CIVILIZATION.FIRE:
                mFireManaTapped--;
                break;
            case CARD_CIVILIZATION.WATER:
                mWaterManaTapped--;
                break;
            case CARD_CIVILIZATION.DARKNESS:
                mDarknessManaTapped--;
                break;
            case CARD_CIVILIZATION.LIGHT:
                mLightManaTapped--;
                break;
            default:
                Debug.LogWarning("GameManager::ManaTap() parametru invalid");
                break;
        }
    }


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        ClickHandle();
    }

    private void ClickHandle()
    {
        if(Input.GetMouseButtonDown(0) == true && mIsTargeting == true)
        {

        }
    }

    public void HoverEnter(Transform _hoveredCard)
    {
        if(mHoveredCard != null)
        {
            Destroy(mHoveredCard.gameObject);
        }

        Vector3 cameraScale = Camera.main.gameObject.transform.lossyScale;
        Vector3 cameraAngles = Camera.main.gameObject.transform.eulerAngles;
        Vector3 cameraPosition = Camera.main.gameObject.transform.localPosition;
        Vector3 position = new Vector3(- 2.15f, 0.35f, 3.5f);//valori harcodate, naspa!
        position = Camera.main.gameObject.transform.TransformPoint(position);
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(cameraAngles.x + 90f, cameraAngles.y, cameraAngles.z - 180);
        mHoveredCard = Instantiate(_hoveredCard, position, rotation);
        mHoveredCard.transform.localScale = new Vector3(cameraScale.x * 0.22f, cameraScale.y * 0.22f, cameraScale.z * 0.3f);

        Destroy(mHoveredCard.GetComponent<Card>());
    }

    public void HoverExit()
    {
        if (mHoveredCard != null)
        {
            Destroy(mHoveredCard.gameObject);
        }
    }
}
