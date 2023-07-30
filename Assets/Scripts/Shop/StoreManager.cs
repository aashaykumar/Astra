using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Arrow;

public class StoreManager : MonoBehaviour
{
    private Transform container;
    private Transform itemTemplate;
    [SerializeField]
    private TextMeshProUGUI txtGold;
    [SerializeField]
    private PlayerStats stats;
    [SerializeField]
    private Arrow arrowEffect;
    private List<ArrowEffectType> storeList;
    public List<Transform> transformlist;
    public InventoryManger inventoryManger;

    private void Awake()
    {
        txtGold.text = stats.totalGoldCoin.ToString();
        container = transform.Find("Container");
        itemTemplate = container.Find("ItemTemplate");
        itemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        storeList = arrowEffect.GetUnOwnedArrow();
        int index = 0;
        foreach (ArrowEffectType arrow in storeList)
        {
            Debug.Log(storeList);
            CreateItemButton(arrow, arrowEffect.GetSprite(arrow), arrowEffect.GetName(arrow), arrowEffect.GetCost(arrow), index);
            index++;
        }
    }
    
    private void CreateItemButton(Arrow.ArrowEffectType itemType,Texture itemSprite, string itemName, int ItemCost, int positionIndex)
    {
        Transform itemTemplateTransform = Instantiate(itemTemplate, container);
        RectTransform itemRectTemplateTransform = itemTemplateTransform.GetComponent<RectTransform>();
        float shopItemHeight = -280f;
        float shopItemWidth = 300f;
        itemRectTemplateTransform.anchoredPosition = new Vector2 (-shopItemWidth + shopItemWidth * GetPosition(positionIndex, true), -shopItemHeight + (shopItemHeight - 200) * GetPosition(positionIndex, false));
        itemTemplateTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        itemTemplateTransform.Find("ItemValue").GetComponent<TextMeshProUGUI>().SetText(ItemCost.ToString());
        itemTemplateTransform.Find("ItemImage").GetComponent<RawImage>().texture = itemSprite;
        itemTemplateTransform.GetComponent<Button>().onClick.AddListener(delegate { TryBuyItem(itemType, itemTemplateTransform); }); 
        itemTemplateTransform.gameObject.SetActive(true);
        transformlist.Add((itemTemplateTransform));
    }

    private void TryBuyItem(Arrow.ArrowEffectType itemType, Transform obj)
    {
        container.gameObject.SetActive(false);
        if(TrySpendGoldAmount(arrowEffect.GetCost(itemType)))
        {
            arrowEffect.SetArrowPurchaseStatus(itemType);
            transformlist.Remove(obj);
            Destroy(obj.gameObject);
            refreshList();
            inventoryManger.UpdateInventory(itemType);
        }
        container.gameObject.SetActive(true);
    }
    private void refreshList()
    {
        int positionIndex = 0;
        float shopItemHeight = -280f;
        float shopItemWidth = 300f;
        foreach (Transform item in transformlist)
        {
            RectTransform rt =  item.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(-shopItemWidth + shopItemWidth * GetPosition(positionIndex, true), -shopItemHeight + (shopItemHeight - 200) * GetPosition(positionIndex, false));
            positionIndex++;
        }
    }

    private bool TrySpendGoldAmount(int spendGoldAmount)
    {
        if(stats.totalGoldCoin >= spendGoldAmount)
        {
            stats.totalGoldCoin -= spendGoldAmount;
            txtGold.text = stats.totalGoldCoin.ToString();
            return true;
        }
        else
            return false;
    }

    public int GetPosition(int positionIndex,bool isWidth)
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
}
