using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "ArrowEffect", menuName = "ScriptableObjects/Arrow")]
public class Arrow : ScriptableObject
{
    [SerializeField]
    public GameObject fireEffect;
    public GameObject iceEffect;
    public GameObject waterEffect;
    public GameObject windEffect;
    public GameObject voidEffect;

    [SerializeField]
    public Texture NormalSprite;
    public Texture fireSprite;
    public Texture iceSprite;
    public Texture waterSprite;
    public Texture windSprite;
    public Texture voidSprite;

    private bool normalArrowPurchaseStatus = true;
    [SerializeField] private bool fireArrowPurchaseStatus;
    [SerializeField] private bool iceArrowPurchaseStatus;
    [SerializeField] private bool waterArrowPurchaseStatus;
    [SerializeField] private bool windArrowPurchaseStatus;
    [SerializeField] private bool voidArrowPurchaseStatus;

    public ArrowEffectType currentArrowEffect = ArrowEffectType.Normal;

    public enum ArrowEffectType
    {
        Normal,
        Fire,
        Ice,
        Water,
        Wind,
        Void        
    }

    public int GetCost(ArrowEffectType arrowEffectType)
    {
        switch (arrowEffectType)
        {
            default:
                case ArrowEffectType.Normal:    return 0;
                case ArrowEffectType.Fire:      return 1000;
                case ArrowEffectType.Ice:       return 2000;
                case ArrowEffectType.Water:     return 3000;
                case ArrowEffectType.Wind:      return 4000;
                case ArrowEffectType.Void:      return 5000;
        }
    }

    public GameObject GetVFX(ArrowEffectType arrowEffectType)
    {
        switch (arrowEffectType)
        {
            default:
            case ArrowEffectType.Normal: return null;
            case ArrowEffectType.Fire: return fireEffect;
            case ArrowEffectType.Ice: return iceEffect;
            case ArrowEffectType.Water: return waterEffect;
            case ArrowEffectType.Wind: return windEffect;
            case ArrowEffectType.Void: return voidEffect;
        }
    }

    public Texture GetSprite(ArrowEffectType arrowEffectType)
    {
        switch (arrowEffectType)
        {
            default:
            case ArrowEffectType.Normal: return NormalSprite;
            case ArrowEffectType.Fire: return fireSprite;
            case ArrowEffectType.Ice: return iceSprite;
            case ArrowEffectType.Water: return waterSprite;
            case ArrowEffectType.Wind: return windSprite;
            case ArrowEffectType.Void: return voidSprite;
        }
    }

    public String GetName(ArrowEffectType arrowEffectType)
    {
        switch (arrowEffectType)
        {
            default:
            case ArrowEffectType.Normal: return null;
            case ArrowEffectType.Fire: return "Agneyastra";
            case ArrowEffectType.Ice: return "Himastra";
            case ArrowEffectType.Water: return "Varunastra";
            case ArrowEffectType.Wind: return "Vayuvyastra";
            case ArrowEffectType.Void: return "Brahmastra";
        }
    }

    public string GetTypeString(ArrowEffectType arrowEffectType)
    {
        switch (arrowEffectType)
        {
            default:
            case ArrowEffectType.Normal: return "Normal";
            case ArrowEffectType.Fire: return "Fire";
            case ArrowEffectType.Ice: return "Ice";
            case ArrowEffectType.Water: return "Water";
            case ArrowEffectType.Wind: return "Wind";
            case ArrowEffectType.Void: return "Void";
        }
    }

    public bool GetArrowPurchaseStatus(ArrowEffectType arrowEffectType)
    {
        switch (arrowEffectType)
        {
            default:
            case ArrowEffectType.Normal: return normalArrowPurchaseStatus;
            case ArrowEffectType.Fire: return fireArrowPurchaseStatus;
            case ArrowEffectType.Ice: return iceArrowPurchaseStatus;
            case ArrowEffectType.Water: return waterArrowPurchaseStatus;
            case ArrowEffectType.Wind: return windArrowPurchaseStatus;
            case ArrowEffectType.Void: return voidArrowPurchaseStatus;
        }
    }

    public void SetArrowPurchaseStatus(ArrowEffectType arrowEffectType)
    {
        switch (arrowEffectType)
        {
            default:
            case ArrowEffectType.Normal:
                break;
            case ArrowEffectType.Fire:  fireArrowPurchaseStatus = true;
                break;    
            case ArrowEffectType.Ice:   iceArrowPurchaseStatus = true;
                break;
            case ArrowEffectType.Water: waterArrowPurchaseStatus = true;
                break;
            case ArrowEffectType.Wind:  windArrowPurchaseStatus = true;
                break;
            case ArrowEffectType.Void:  voidArrowPurchaseStatus = true;
                break;
        }
    }

    public List<ArrowEffectType> GetOwnedArrow()
    {
        List<ArrowEffectType> list = new List<ArrowEffectType>();
        list.Clear();
        foreach (ArrowEffectType i in Enum.GetValues(typeof(ArrowEffectType)))
        {
            if(GetArrowPurchaseStatus(i))
            {
               list.Add(i);
            }
        }
        return list;
    }

    public List<ArrowEffectType> GetUnOwnedArrow()
    {
        List<ArrowEffectType> list = new List<ArrowEffectType>();
        list.Clear();
        foreach (ArrowEffectType i in Enum.GetValues(typeof(ArrowEffectType)))
        {
            if (!GetArrowPurchaseStatus(i) && i != ArrowEffectType.Normal)
            {
                list.Add(i);
            }
        }
        return list;
    }
}
