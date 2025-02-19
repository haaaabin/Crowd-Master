using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using System.Collections;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Button playBtn;
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
        playBtn.transform.DOScale(1.2f, 0.5f).SetLoops(1000, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        playBtn.onClick.AddListener(() =>
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
        scoreText.text = (score * 10).ToString();
        feverText.text = $"Great!!\n <size=80>X    {numStickmans}\n     X{curscore:F1}</size=80>";
    }
}
