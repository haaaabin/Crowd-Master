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

    public GameObject panel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feverText;
    private float score = 0f;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tapToPlayBtn.transform.DOScale(1.2f, 0.5f).SetLoops(1000, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        tapToPlayBtn.onClick.AddListener(() =>
        {
            gameStartPanel.SetActive(false);
            PlayerManager.instance.gameState = true;
        });
        handIcon.transform.DOMoveX(250f, 1f).SetLoops(1000, LoopType.Yoyo).SetEase(Ease.InOutSine);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator UpdateScore(int numStickmans, float curscore)
    {
        yield return new WaitForSeconds(2f);

        panel.SetActive(true);

        score = numStickmans;
        score *= curscore;
        score *= 10;
        scoreText.text = score.ToString();
        feverText.text = $"Great!!\n <size=80>X    {numStickmans}\n     X{curscore:F1}</size=80>";

        RewardManager.instance.RewardPileOfCoin((int)score);
    }
}
