using UnityEngine;
using UnityEngine.SceneManagement;
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
        InitProgressBar();
    }

    public void InitProgressBar()
    {
        if (GameManager.Instance().currentLevel != null)
        {
            player = GameObject.FindWithTag("Player").transform;
            finishLine = GameManager.Instance().currentLevel.transform.Find("FinishLine").gameObject.transform;
            startDistance = Mathf.Abs(player.position.z - finishLine.position.z);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitProgressBar();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

}
