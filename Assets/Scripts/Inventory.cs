using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _itemTemplate;
    [SerializeField] private Transform _itemParent;
    [SerializeField] private List<ItemData> _itemDatas = new List<ItemData>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpItem(ItemObject itemObj)
    {
        if (itemObj == null) return;

        ItemData item = itemObj.itemData;
        Debug.Log($"Picked up {item.ItemName}, duration: {item.Duration} sec");

        GameObject newItemUI = Instantiate(_itemTemplate, _itemParent);
        newItemUI.SetActive(true);

        Image img = newItemUI.GetComponent<Image>();
        if (img != null && item.Icon != null)
            img.sprite = item.Icon;

        TextMeshProUGUI txt = newItemUI.GetComponentInChildren<TextMeshProUGUI>();
        if (txt != null)
            txt.text = item.ItemName;

        _itemDatas.Add(item);
        Destroy(itemObj.gameObject);
    }

    public void UseItem(List<ItemData> itemDatas, int index)
    {
        if (index < 0 || index >= _itemParent.childCount)
        {
            Debug.LogWarning("Invalid item index.");
            return;
        }

        ItemData item = _itemDatas[index];
        Debug.Log($"Used item: {item.ItemName}, duration: {item.Duration} sec");

        Destroy(_itemParent.GetChild(index + 1).gameObject);
        _itemDatas.RemoveAt(index);
    }
}
