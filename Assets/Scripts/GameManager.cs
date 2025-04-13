using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance()
    {
        if (instance == null)
        {
            return null;
        }
        return instance;
    }

    public enum GameState
    {
        MENU,
        GAME,
        LEVELCOMPLETE,
        GAMEOVER,
        SETTINGS,
    }

    public GameState gameState = GameState.MENU;

    public static event Action setMenuEvent;
    public static event Action setGameEvent;
    public static event Action setLevelCompleteEvent;
    public static event Action setGameOverEvent;
    public static event Action setSettingsEvent;

    public GameObject currentLevel;
    public GameObject levelsParent;

    public SoundManager soundManager = new SoundManager();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        soundManager.Init();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (levelsParent == null)
        {
            levelsParent = GameObject.Find("----LEVEL----");
        }

        ChangeLevel();

        if (ProgressBar.instance.finishLine == null || ProgressBar.instance.player == null)
        {
            ProgressBar.instance.finishLine = currentLevel.transform.Find("FinishLine").gameObject.transform;
            GameObject player = GameObject.FindWithTag("Player");
            ProgressBar.instance.player = player.transform;
        }

        if (PlayerManager.instance.road == null)
        {
            PlayerManager.instance.road = currentLevel.transform;
        }
    }

    public void ChangeState(GameState newState)
    {
        if (gameState != newState)
        {
            gameState = newState;

            switch (gameState)
            {
                case GameState.MENU:
                    setMenuEvent?.Invoke();
                    break;
                case GameState.GAME:
                    setGameEvent?.Invoke();
                    break;
                case GameState.LEVELCOMPLETE:
                    setLevelCompleteEvent?.Invoke();
                    break;
                case GameState.GAMEOVER:
                    setGameOverEvent?.Invoke();
                    soundManager.Play("GameOver", SoundType.EFFECT);
                    break;
                case GameState.SETTINGS:
                    setSettingsEvent?.Invoke();
                    break;
            }
        }
    }

    public void SetNextLevel()
    {
        int currentlevel = PlayerPrefs.GetInt("Level");

        // 다음 레벨로 진행 (마지막 레벨이면 더 이상 증가 X)
        if (currentlevel <= 3)
        {
            currentlevel++;
            PlayerPrefs.SetInt("Level", currentlevel);
        }
        else
        {
            Debug.Log("마지막 레벨입니다.");
        }
    }

    public void ChangeLevel()
    {
        int childIndex = PlayerPrefs.GetInt("Level");

        if (childIndex > 3)
        {
            Debug.Log("end");
            return;
        }

        // 모든 레벨 비활성화
        foreach (Transform child in levelsParent.transform)
        {
            child.gameObject.SetActive(false);
        }

        // 현재 레벨만 활성화
        Transform levelTransform = levelsParent.transform.GetChild(childIndex);
        currentLevel = levelTransform.gameObject;
        currentLevel.SetActive(true);
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}