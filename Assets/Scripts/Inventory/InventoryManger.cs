using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Arrow;

public class InventoryManger : MonoBehaviour
{
    private Transform container;
    private Transform itemTemplate;
    [SerializeField]
    private TextMeshProUGUI txtGold;
    [SerializeField]
    private PlayerStats stats;
    [SerializeField]
    private Arrow arrowEffect;
    public int index = 0;
    public List<ArrowEffectType> list;

    private void Awake()
    {
        
        container = transform.Find("Container");
        itemTemplate = container.Find("ItemTemplate");
        itemTemplate.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        txtGold.text = stats.totalGoldCoin.ToString();
    }

    private void Start()
    {
        list = arrowEffect.GetOwnedArrow();
        foreach (ArrowEffectType arrow in list)
        {
            CreateItemButton(arrow, arrowEffect.GetSprite(arrow), arrowEffect.GetName(arrow), arrowEffect.GetCost(arrow), index);
            index++;
        }
    }

    public void CreateItemButton(ArrowEffectType itemType, Texture itemSprite, string itemName, int ItemCost, int positionIndex)
    {
        Transform itemTemplateTransform = Instantiate(itemTemplate, container);
        RectTransform itemRectTemplateTransform = itemTemplateTransform.GetComponent<RectTransform>();
        float shopItemHeight = -230f;
        float shopItemWidth = 300f;
        itemRectTemplateTransform.anchoredPosition = new Vector2(-shopItemWidth + shopItemWidth * GetPosition(positionIndex, true), -shopItemHeight + (shopItemHeight - 200) * GetPosition(positionIndex, false));
        itemTemplateTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        itemTemplateTransform.Find("ItemImage").GetComponent<RawImage>().texture = itemSprite;
        itemTemplateTransform.GetComponent<Button>().onClick.AddListener(delegate { TrySelectItem(itemType); });
        itemTemplateTransform.gameObject.SetActive(true);
    }

    private void TrySelectItem(Arrow.ArrowEffectType itemType)
    {
        if (arrowEffect.currentArrowEffect != itemType)
        {
            arrowEffect.currentArrowEffect = itemType;
        }
    }

    public int GetPosition(int positionIndex, bool isWidth)
    {
        int a;
        if (isWidth)
        {
            a = (positionIndex % 3) != 0 ? (positionIndex % 3) : 0;
            //Debug.Log("positionIndex" + positionIndex + "width" + a);
            return a;
        }
        else
            a = (positionIndex % 3) == 0 ? positionIndex / 3 : (int)(positionIndex / 3);
        // Debug.Log("positionIndex" + positionIndex  + "height" +a);
        return a;
    }

    public void UpdateInventory(ArrowEffectType arrow)
    {
        CreateItemButton(arrow, arrowEffect.GetSprite(arrow), arrowEffect.GetName(arrow), arrowEffect.GetCost(arrow), index);
        list.Add(arrow);
        index++;
    }
}
