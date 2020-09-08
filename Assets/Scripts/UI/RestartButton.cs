using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private GameController gameController;
    private Button restartButton;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        restartButton = GetComponent<Button>();

        restartButton.onClick.AddListener(Restart);
    }

    public void Restart()
    {
        gameController.GameStart(gameController.gameDifficulty);
    }
}
