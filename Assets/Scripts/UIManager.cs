using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using System.Collections;
using System.Xml.Serialization;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Button tapToPlayBtn;
    public GameObject gameStartPanel;
    public Image handIcon;
    public Button nextBtn;

    public GameObject panel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feverText;
    private float score = 0f;

    private static bool isRestarted = false;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        gameStartPanel.SetActive(!isRestarted);
    }

    // Start is called before the first frame update
    void Start()
    {
        tapToPlayBtn.transform.DOScale(1.2f, 0.5f).SetLoops(1000, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        tapToPlayBtn.onClick.AddListener(() =>
        {
            gameStartPanel.SetActive(false);
            GameManager.Instance().StartGame();
        });

        handIcon.transform.DOMoveX(250f, 1f).SetLoops(1000, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public IEnumerator UpdateScore(int numStickmans, float curscore)
    {
        yield return new WaitForSeconds(1f);

        panel.SetActive(true);
        score = numStickmans * curscore;
        int roundedScore = Mathf.RoundToInt(score);

        // scoreText.text = roundedScore.ToString();
        ShowResultPanel("성공 !" , roundedScore , "next", true);
        RewardManager.instance.RewardPileOfCoin(roundedScore);
    }

    public void ShowResultPanel(string resultText, int resultScore, string resultBtnText, bool isSuccess)
    {
        panel.SetActive(true);

        feverText.text = resultText;
        scoreText.text = $"<size=50>획득</size=50>\n + {resultScore}";
        nextBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = resultBtnText;

        nextBtn.onClick.RemoveAllListeners();

        if (!isSuccess)
        {
            nextBtn.onClick.AddListener(() =>
            {
                isRestarted = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                GameManager.Instance().ChangeState(GameManager.GameState.Playing);
            });
        }
        else
        {
            Debug.Log("next level");
        }
    }
}
