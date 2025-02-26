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
    }

    {
    }

    {
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

        }
    }
}