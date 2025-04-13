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
    public Button closeSettingBtn;
    public Button soundBtn;
    public Button vibrationBtn;

    public Sprite soundOnImage;
    public Sprite soundOffImage;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public Text levelText;

    private float score = 0f;
    private bool isSoundOn = true;
    private bool isVibrationOn;

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
        RegisterEvent();
    }

    private void InitalizeUI()
    {
        if (tapToPlayBtn != null)
        {
            tapToPlayBtn.transform.DOScale(1.15f, 0.5f).SetLoops(1000, LoopType.Yoyo).SetEase(Ease.InOutQuad);
            tapToPlayBtn.onClick.AddListener(() => GameManager.Instance().ChangeState(GameManager.GameState.GAME));
        }
        if (settingBtn != null)
        {
            settingBtn.onClick.AddListener(() =>
            {
                GameManager.Instance().soundManager.Play("Click");
                GameManager.Instance().ChangeState(GameManager.GameState.SETTINGS);
            });
        }
        coinText.text = PlayerPrefs.GetInt("CountCoin").ToString();
        soundBtn.image.sprite = soundOnImage;
    }

    private void RegisterEvent()
    {
        GameManager.setMenuEvent += OnMenuUI;
        GameManager.setGameEvent += OnGameUI;
        GameManager.setGameOverEvent += OnGameOverUI;
        GameManager.setLevelCompleteEvent += OnLevelCompleteUI;
        GameManager.setSettingsEvent += OnSettingsUI;
    }

    private void OnDestroy()
    {
        GameManager.setMenuEvent -= OnMenuUI;
        GameManager.setGameEvent -= OnGameUI;
        GameManager.setGameOverEvent -= OnGameOverUI;
        GameManager.setLevelCompleteEvent -= OnLevelCompleteUI;
        GameManager.setSettingsEvent -= OnSettingsUI;
    }

    public IEnumerator UpdateScore(int numStickmans, float curscore)
    {
        yield return new WaitForSeconds(2.5f);

        levelCompletePanel.SetActive(true);

        score = numStickmans * curscore;
        int roundedScore = Mathf.RoundToInt(score);

        scoreText.text = $"+ {roundedScore}";
        RewardManager.instance.RewardPileOfCoin(roundedScore);
        GameManager.Instance().soundManager.Play("Coin");
    }

    private void OnMenuUI()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        settingsPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        coinText.text = PlayerPrefs.GetInt("CountCoin").ToString();
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

        if (GameManager.Instance().currentLevel.name == "Level3")
        {
            nextBtn.onClick.RemoveAllListeners();
            nextBtn.GetComponentInChildren<Text>().text = "Clear";
        }
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

    private void OnSettingsUI()
    {
        settingsPanel.SetActive(true);

        if (closeSettingBtn != null)
            closeSettingBtn.onClick.AddListener(() => GameManager.Instance().ChangeState(GameManager.GameState.MENU));

        soundBtn.onClick.AddListener(() =>
        {
            GameManager.Instance().soundManager.Play("Click");

            if (isSoundOn)
            {
                // 효과음 Off, change sprite
                isSoundOn = false;
                AudioListener.volume = 0f;

                soundBtn.image.sprite = soundOffImage;
                soundBtn.image.color = Color.gray;
                Debug.Log("volume 0");
            }
            else
            {
                //효과음 On
                isSoundOn = true;
                AudioListener.volume = 1f;

                soundBtn.image.sprite = soundOnImage;
                soundBtn.image.color = new Color(70f / 255f, 166f / 255f, 56f / 255f);

                Debug.Log("volume 1");

            }
        });
    }
}
