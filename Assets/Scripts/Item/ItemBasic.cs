using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBasic : MonoBehaviour
{
    public int ItemID = 0;
    public string ItemName = "EmptyItem";
    public string ItemDescribtion = "Empty Item";
    public uint level = 1;
    public float manaCost = 0f;
    public ItemUseBasic itemUse;

    public Sprite ItemCover { private set; get; }
    private ControllableBag controllableBag;

    private void Start()
    {
        ItemCover = GetComponent<Image>().sprite;
        controllableBag = FindObjectOfType<ControllableBag>();
    }

    public void ItemSelect()
    {
        controllableBag.SetCurrentItem(gameObject);

#if UNITY_EDITOR
        Debug.Log($"Item Select");
#endif
    }
}
