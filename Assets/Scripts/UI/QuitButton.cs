using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    private GameController gameController;
    private Button quitButton;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        quitButton = GetComponent<Button>();

        quitButton.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        gameController.GameStop();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
