using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using System.Collections;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Button tapToPlayBtn;
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject levelCompletePanel;
    public GameObject gameOverPanel;
    public GameObject settingsPanel;

    public Button nextBtn;
    public Button replyBtn;
    public Button settingBtn;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public Text levelText;

    private float score = 0f;

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

    void Start()
    {
        InitalizeUI();
        RegisterDelegate();
    }

    private void InitalizeUI()
    {
        if (tapToPlayBtn != null)
        {
            tapToPlayBtn.transform.DOScale(1.15f, 0.5f).SetLoops(1000, LoopType.Yoyo).SetEase(Ease.InOutQuad);
            tapToPlayBtn.onClick.AddListener(() =>
            {
                GameManager.Instance().ChangeState(GameManager.GameState.GAME);
            });
        }

        coinText.text = PlayerPrefs.GetInt("CountCoin").ToString();
    }

    private void RegisterDelegate()
    {
        GameManager.setMenuDelegate += OnMenuUI;
        GameManager.setGameDelegate += OnGameUI;
        GameManager.setGameOverDelegate += OnGameOverUI;
        GameManager.setLevelCompleteDelegate += OnLevelCompleteUI;
    }

    private void OnDestroy()
    {
        GameManager.setMenuDelegate -= OnMenuUI;
        GameManager.setGameDelegate -= OnGameUI;
        GameManager.setGameOverDelegate -= OnGameOverUI;
        GameManager.setLevelCompleteDelegate -= OnLevelCompleteUI;
    }

    public IEnumerator UpdateScore(int numStickmans, float curscore)
    {
        yield return new WaitForSeconds(2.5f);

        levelCompletePanel.SetActive(true);

        score = numStickmans * curscore;
        int roundedScore = Mathf.RoundToInt(score);

        scoreText.text = $"+ {roundedScore}";
        RewardManager.instance.RewardPileOfCoin(roundedScore);
    }

    private void OnMenuUI()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        settingsPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void OnGameUI()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        levelText.text = "Level " + (PlayerPrefs.GetInt("Level") + 1).ToString();
    }

    public void OnLevelCompleteUI()
    {
        gamePanel.SetActive(false);

        nextBtn.onClick.RemoveAllListeners();
        nextBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance().ChangeState(GameManager.GameState.MENU);
            GameManager.Instance().SetNextLevel();
        });
    }

    private void OnGameOverUI()
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        replyBtn.onClick.RemoveAllListeners();
        replyBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance().ChangeState(GameManager.GameState.MENU);
        });
    }

}
