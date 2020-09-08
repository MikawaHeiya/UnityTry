using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnTitleButton : MonoBehaviour
{
    private GameController gameController;
    private Button returnTitleButton;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        returnTitleButton = GetComponent<Button>();

        returnTitleButton.onClick.AddListener(ReturnTitle);
    }

    public void ReturnTitle()
    {
        gameController.ReturnTitle();
    }
}
