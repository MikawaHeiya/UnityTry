using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ControllableBag : MonoBehaviour
{
    private GameObject currentItem;
    private List<GameObject> bag;
    private GameController gameController;
    private ItemList allItems;
    private PlayerBagGUI playerBagGUI;

    private void Start()
    {
        bag = new List<GameObject>();

        gameController = FindObjectOfType<GameController>();
        playerBagGUI = Resources.FindObjectsOfTypeAll<PlayerBagGUI>()[0];
        allItems = gameController.GetComponent<ItemList>();

        AddItem(2);
    }

    private void Update()
    {
        if (gameController.gameStatus == GameStatus.GameRunning && Input.GetKeyDown(KeyCode.Q))
        {
            if (!playerBagGUI)
            {
                playerBagGUI = Resources.FindObjectsOfTypeAll<PlayerBagGUI>()[0];
            }
            gameController.GamePause(true, false);
            playerBagGUI.gameObject.SetActive(true);
        }
        else if (gameController.gameStatus == GameStatus.GamePaused && Input.GetKeyDown(KeyCode.Q))
        {
            if (playerBagGUI.gameObject.activeInHierarchy)
            {
                gameController.GamePause(false, false);
                playerBagGUI.gameObject.SetActive(false);
            }
        }
    }

    public GameObject AddItem(int itemID)
    {
        var _item = Resources.Load($"Prefabs/Items/Item/Item_{itemID}");
        var item = Instantiate(_item, playerBagGUI.itemBag.transform) as GameObject;
        bag.Add(item);
        return item;
    }

    public void SetCurrentItem(GameObject item)
    {
        currentItem = item;
        var itemBasic = item.GetComponent<ItemBasic>();
        playerBagGUI.currentItemName.text = "Name: " + itemBasic.ItemName;
        playerBagGUI.currentItemDescription.text = "Description:\n" + itemBasic.ItemDescribtion.Replace("\\n", "\n");
        playerBagGUI.currentItemCover.sprite = itemBasic.ItemCover;
    }

    public GameObject CurrentItem()
    {
        return currentItem;
    }
}
