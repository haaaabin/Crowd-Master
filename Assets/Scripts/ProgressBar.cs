using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public static ProgressBar instance;
    public Transform player;
    public Transform finishLine;
    public Slider progressBar;

    private float startDistance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        player = PlayerManager.instance.transform;
        finishLine = GameManager.Instance().currentLevel.transform.Find("FinishLine").gameObject.transform;
        startDistance = Mathf.Abs(player.position.z - finishLine.position.z);
    }

    void Update()
    {
        if (PlayerManager.instance.isFinish) return;
        if (GameManager.Instance().gameState == GameManager.GameState.GAME)
        {
            UpdateProgressBar();
        }
    }

    private void UpdateProgressBar()
    {
        float currentDistance = Mathf.Abs(player.position.z - finishLine.position.z);

        // 거리를 0~1 사이 비율로 변환
        float progress = 1 - (currentDistance / startDistance);
        progress = Mathf.Clamp01(progress); // 값이 0~1을 넘지 않도록 제한

        progressBar.value = progress;
    }
}
