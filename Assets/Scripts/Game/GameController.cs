using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public RunMode runMode = RunMode.Release;
    public GameStatus gameStatus = GameStatus.GameStopped;
    public GameDifficulty gameDifficulty;
    public GameObject enemy;
    public uint score = 0u;
    public BGMList bgmList;

    private float enemySpawnLimit = 100f;
    private Text scoreText;
    private ControllableBehave controllableBehave;
    private AudioSource audioSource;
    private GameObject gameRunningGUI;
    private GameObject gamePauseGUI;

    public float spawnDeltaTime
    {
        private set
        {
            _spawnDeltaTime = value;
        }
        get
        {
            switch (gameDifficulty)
            {
                case GameDifficulty.Easy:
                    return _spawnDeltaTime;
                case GameDifficulty.Normal:
                    return _spawnDeltaTime * 0.8f;
                case GameDifficulty.Hard:
                    return _spawnDeltaTime * 0.6f;
                case GameDifficulty.Lunatic:
                    return _spawnDeltaTime * 0.4f;
                default:
                    return 0f;
            }
        }

    }
    public float _spawnDeltaTime = 2f;

    public float spawnDistanceLimit
    {
        set
        {
            _spawnDistanceLimit = value;
        }

        get
        {
            switch (gameDifficulty)
            {
                case GameDifficulty.Easy:
                    return _spawnDistanceLimit;
                case GameDifficulty.Normal:
                    return _spawnDistanceLimit * 0.8f;
                case GameDifficulty.Hard:
                    return _spawnDistanceLimit * 0.6f;
                case GameDifficulty.Lunatic:
                    return _spawnDistanceLimit * 0.4f;
                default:
                    return 0f;
            }
        }
    }
    public float _spawnDistanceLimit = 10f;

    public float spawnNunmberLimit
    {
        set
        {
            _spawnNumberLimit = value;
        }

        get
        {
            switch (gameDifficulty)
            {
                case GameDifficulty.Easy:
                    return _spawnNumberLimit;
                case GameDifficulty.Normal:
                    return _spawnNumberLimit * 2f;
                case GameDifficulty.Hard:
                    return _spawnNumberLimit * 3f;
                case GameDifficulty.Lunatic:
                    return _spawnNumberLimit * 4f;
                default:
                    return 0f;
            }
        }
    }
    public float _spawnNumberLimit = 30f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (runMode == RunMode.Release)
        {
            SceneManager.LoadScene(1);
        }

        StartCoroutine(SpawnEnemyTimer());

        audioSource = GetComponent<AudioSource>();
        bgmList = GetComponent<BGMList>();

        Physics.gravity *= 3f;
    }

    private void Start()
    {
        PlayBGM(runMode == RunMode.Debug ? bgmList.gameBGM : bgmList.titleBGM);
    }

    private IEnumerator SpawnEnemyTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDeltaTime);
            if (gameStatus == GameStatus.GameRunning && FindObjectsOfType<EnemyBehave>().Length <= spawnNunmberLimit)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        Vector3 enemyPostion = transform.position + 
            Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up) * Vector3.forward * UnityEngine.Random.Range(0f, enemySpawnLimit);

        if (!controllableBehave)
        {
            controllableBehave = FindObjectOfType<ControllableBehave>();
        }

        if (Vector3.Distance(enemyPostion, controllableBehave.gameObject.transform.position) > spawnDistanceLimit)
        {
            Instantiate(enemy, enemyPostion, enemy.transform.rotation * Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up));
        }
        else
        {
            SpawnEnemy();
        }
    }

    public void GameStop()
    {
        gameStatus = GameStatus.GameStopped;
        Cursor.lockState = CursorLockMode.Confined;

#if UNITY_EDITOR
        Debug.Log("GameStopped");
#endif
    }

    public void GameStart(GameDifficulty difficulty)
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);

        score = 0u;
        Cursor.lockState = CursorLockMode.Locked;
        gameDifficulty = difficulty;
        controllableBehave = FindObjectOfType<ControllableBehave>();
        PlayBGM(bgmList.gameBGM);

        gameStatus = GameStatus.GameRunning;
    }

    public void GamePause(bool ifPause, bool ifShowPuaseGUI)
    {
        if (!controllableBehave)
        {
            controllableBehave = FindObjectOfType<ControllableBehave>();
        }
        controllableBehave.gameObject.GetComponent<Rigidbody>().useGravity = !ifPause;

        if (ifPause)
        {
            audioSource.Pause();
            gameStatus = GameStatus.GamePaused;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            audioSource.Play();
            gameStatus = GameStatus.GameRunning;
            Cursor.lockState = CursorLockMode.Locked;
        }

        ShowPauseGUI(ifShowPuaseGUI);
    }

    private void ShowPauseGUI(bool ifPause)
    {
        if (!gameRunningGUI || !gamePauseGUI)
        {
            gameRunningGUI = FindObjectOfType<GameRunningGUI>().gameObject;
            gamePauseGUI = (Resources.FindObjectsOfTypeAll(typeof(GamePauseGUI))[0] as GamePauseGUI).gameObject;
        }

        gameRunningGUI.SetActive(!ifPause);
        gamePauseGUI.SetActive(ifPause);
    }

    public void ReturnTitle()
    {
        gameStatus = GameStatus.GameStopped;

        SceneManager.LoadScene(1);
        PlayBGM(bgmList.titleBGM);
    }

    public void AddScore(uint offset = 0u)
    {
        if (!scoreText)
        {
            scoreText = FindObjectOfType<Canvas>().GetComponentInChildren<Text>();
        }

        if (score < uint.MaxValue - offset)
        {
            score += offset;
            scoreText.text = $"Score:{score}";
        }
    }

    public void PlayBGM(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}

public enum GameStatus
{
    GameStopped,
    GameRunning,
    GamePaused
}

public enum GameDifficulty { Easy, Normal, Hard, Lunatic}

public enum MoveStatus
{
    Stay = 0,
    WalkForward,
    RunForward,
    Jump,
    Die
}

public enum RunMode { Debug, Release}
