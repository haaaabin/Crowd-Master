using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Button playBtn;
    public GameObject gameStartPanel;
    public Image handIcon;

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
}
