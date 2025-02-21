using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public enum GameState
    {
        WaitingToStart,
        Playing,
        GameOver,
        GameClear
    }

    public GameState gameState = GameState.WaitingToStart;
    public delegate void GameStateChanged(GameState newState);
    public event GameStateChanged OnGameStateChanged;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public static GameManager Instance()
    {
        if (instance == null)
        {
            return null;
        }
        return instance;
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        if(gameState != newState)
        {
            gameState = newState;
            OnGameStateChanged?.Invoke(newState);
        }
    }
}