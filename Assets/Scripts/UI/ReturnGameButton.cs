using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class ReturnGameButton : MonoBehaviour
{
    private GameController gameController;
    private Button returnGameButton;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        returnGameButton = GetComponent<Button>();

        returnGameButton.onClick.AddListener(ReturnGame);
    }

    public void ReturnGame()
    {
        gameController.GamePause(false, false);
    }
}
