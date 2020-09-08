using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public GameObject difficultyButtons;
    public GameObject titleButtons;
    public Text titleText;

    private Button startButton;

    private void Awake()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(ShowDifficultyButtons);
    }

    public void ShowDifficultyButtons()
    {
        difficultyButtons.SetActive(true);
        titleButtons.SetActive(false);

        titleText.text = "Select Difficulty";
    }
}
