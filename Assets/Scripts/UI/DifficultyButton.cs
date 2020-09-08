using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public GameDifficulty gameDifficulty;
    private GameController gameController;
    private Button difficultyButton;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        difficultyButton = GetComponent<Button>();

        difficultyButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        gameController.GameStart(gameDifficulty);
    }
}
