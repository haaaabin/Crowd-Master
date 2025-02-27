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

    public delegate void SetMenuDelegate();
    public static SetMenuDelegate setMenuDelegate;

    public delegate void SetGameDelegate();
    public static SetGameDelegate setGameDelegate;

    public delegate void SetLevelCompleteDelegate();
    public static SetLevelCompleteDelegate setLevelCompleteDelegate;

    public delegate void SetGameOverDelegate();
    public static SetGameOverDelegate setGameOverDelegate;

    public delegate void SetSettingsDelegate();
    public static SetSettingsDelegate setSettingsDelegate;

    public GameObject currentLevel;
    public GameObject levelsParent;

    void Awake()
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

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
            ProgressBar.instance.player = PlayerManager.instance.transform;
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
                    setMenuDelegate?.Invoke();
                    break;
                case GameState.GAME:
                    setGameDelegate?.Invoke();
                    break;
                case GameState.LEVELCOMPLETE:
                    setLevelCompleteDelegate?.Invoke();
                    break;
                case GameState.GAMEOVER:
                    setGameOverDelegate?.Invoke();
                    break;
                case GameState.SETTINGS:
                    setSettingsDelegate?.Invoke();
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

        Debug.Log("Current Level: " + currentLevel.name); // 현재 활성화된 레벨 출력
    }
}