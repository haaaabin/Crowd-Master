using System.Collections;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private int numberOfStickmans;
    private int numberOfEnemyStickmans;
    [SerializeField] private TextMeshPro counterTxt;
    [SerializeField] private GameObject stickMan;
    [SerializeField] private Transform road;
    [SerializeField] private Transform enemy;
    [Range(0f, 1f)][SerializeField] private float distanceFactor;   // 원형 배열 간격
    [Range(0f, 1f)][SerializeField] private float radius;           // 원형 배열 각도


    public bool moveByTouch;
    public bool gameState;
    private bool isAttack;

    private Vector3 mouseStartPos;
    private Vector3 playerStartPos;
    public float playerSpeed;
    public float roadSpeed;

    public GameObject gameOverPanel;
    public GameObject secondCam;
    [HideInInspector] public bool isFinish;
    [HideInInspector] public bool moveTheCamera;
    [SerializeField] private GameObject counterLabel;


    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateCounterText();
    }

    private void UpdateCounterText()
    {
        numberOfStickmans = transform.childCount - 1;
        counterTxt.text = numberOfStickmans.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameState) return;

        counterLabel.SetActive(true);
        road.Translate(road.forward * -1 * Time.deltaTime * roadSpeed);

        if (isAttack)
        {
            HandleAttack();
        }
        else
            MoveThePlayer();

        HandleCameraMovement();

        if (transform.childCount == 1 && isFinish)
            gameState = false;
    }

    private void HandleCameraMovement()
    {
        if (!moveTheCamera || transform.childCount <= 1) return;

        GameObject tower0 = GameObject.Find("Tower0");

        var cinemachineTransposer = secondCam.GetComponent<CinemachineVirtualCamera>()
              .GetCinemachineComponent<CinemachineTransposer>();

        var cinemachineComposer = secondCam.GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineComposer>();

        cinemachineTransposer.m_FollowOffset = new Vector3(13f, Mathf.Lerp(cinemachineTransposer.m_FollowOffset.y,
            tower0.transform.position.y + 20f, Time.deltaTime * 3f), -13f);

        cinemachineComposer.m_TrackedObjectOffset = new Vector3(0f, Mathf.Lerp(cinemachineComposer.m_TrackedObjectOffset.y,
            tower0.transform.position.y + 5f, Time.deltaTime * 3f), 0f);

    }

    private void MoveThePlayer()
    {
        if (isFinish) return;

        if (Input.GetMouseButtonDown(0) && gameState)
        {
            moveByTouch = true;
            var plane = new Plane(Vector3.up, 0f);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 광선이 평면과 충돌하는지 확인하고, 충돌했다면 충돌 지점까지의 거리를 distance 변수에 저장
            if (plane.Raycast(ray, out var distance))
            {
                // 초기 마우스 및 플레이어 위치 3d 공간 좌표로 저장
                mouseStartPos = ray.GetPoint(distance + 1f);
                playerStartPos = transform.position;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            moveByTouch = false;
        }

        if (moveByTouch)
        {
            var plane = new Plane(Vector3.up, 0f);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                var mousePos = ray.GetPoint(distance + 1f);
                var move = mousePos - mouseStartPos;
                var control = playerStartPos + move;

                if (numberOfStickmans > 50)
                    control.x = Mathf.Clamp(control.x, -1.93f, 1.93f);
                else
                    control.x = Mathf.Clamp(control.x, -3f, 3f);

                transform.position = new Vector3(Mathf.Lerp(transform.position.x, control.x, Time.deltaTime * playerSpeed),
                                 transform.position.y, transform.position.z);
            }
        }
    }

    private void HandleAttack()
    {
        // y축을 고정하여 2차원 평면에서의 방향을 계산
        var enemyDirection = new Vector3(enemy.position.x, transform.position.y, enemy.position.z) - transform.position;

        // 각 스틱맨을 적을 향해 부드럽게 회전시킨다.
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).rotation =
                    Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * 1.5f);
        }

        if (enemy.GetChild(1).childCount > 0)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                var stickman = transform.GetChild(i);
                var distance = enemy.GetChild(1).GetChild(0).position - stickman.position;

                // 스틱맨이 적에게 가까워지면 Lerp로 이동
                if (distance.magnitude < 4f)
                {
                    stickman.position = Vector3.Lerp(stickman.position,
                        new Vector3(enemy.GetChild(1).GetChild(0).position.x, stickman.position.y,
                        enemy.GetChild(1).GetChild(0).position.z), Time.deltaTime);
                }
            }
        }
        else
        {
            EndAttack();
        }

        // 모든 스틱맨이 제거되었을 때
        if (transform.childCount <= 0)
        {
            enemy.transform.GetChild(1).GetComponent<EnemyManager>().StopAttacking();
            gameObject.SetActive(false);
            Invoke("GameOverPanel", 0.5f);
        }
    }

    private void EndAttack()
    {
        enemy.gameObject.SetActive(false);
        isAttack = false;
        roadSpeed = 6f;

        FormatStickMan();

        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).rotation = Quaternion.identity;
        }
    }

    private void GameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    // 플레이어 중심으로 스틱맨을 원형으로 배열
    public void FormatStickMan()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            var x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            var z = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            var NewPos = new Vector3(x, 0f, z);

            transform.GetChild(i).DOLocalMove(NewPos, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void MakeStickMan(int number)
    {
        int newStickManCount = number - numberOfStickmans; //추가로 생성해야 하는 스틱맨 수

        for (int i = 0; i < newStickManCount; i++)
        {
            Instantiate(stickMan, transform.position, quaternion.identity, transform);
        }

        numberOfStickmans = transform.childCount - 1;
        counterTxt.text = numberOfStickmans.ToString();

        FormatStickMan();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gate"))
        {
            other.transform.parent.GetChild(0).GetComponent<Collider>().enabled = false;
            other.transform.parent.GetChild(1).GetComponent<Collider>().enabled = false;

            var gateManager = other.GetComponent<GateManager>();

            if (gateManager.multiply)
            {
                MakeStickMan(numberOfStickmans * gateManager.randomNumber);
            }
            else
            {
                MakeStickMan(numberOfStickmans + gateManager.randomNumber);
            }
        }

        if (other.CompareTag("enemy"))
        {
            enemy = other.transform;
            isAttack = true;

            roadSpeed = 2f;
            other.transform.GetChild(1).GetComponent<EnemyManager>().Attacking(transform);
            StartCoroutine(UpdateStickManNumbers());
        }

        if (other.CompareTag("Finish"))
        {
            isFinish = true;
            moveByTouch = false;

            transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log(transform.GetChild(0).gameObject);
            secondCam.SetActive(true);
            Tower.instance.CreateTower(numberOfStickmans);
        }
    }

    public IEnumerator UpdateStickManNumbers()
    {
        if (enemy == null || enemy.GetChild(1) == null)
        {
            yield break;
        }

        numberOfEnemyStickmans = enemy.transform.GetChild(1).childCount;
        numberOfStickmans = transform.childCount - 1;

        while (numberOfEnemyStickmans > 0 && numberOfStickmans > 0)
        {
            numberOfEnemyStickmans--;
            numberOfStickmans--;

            enemy.transform.GetChild(1).GetComponent<EnemyManager>().counterTxt.text = numberOfEnemyStickmans.ToString();
            counterTxt.text = numberOfStickmans.ToString();

            yield return new WaitForSeconds(0.05f);
        }

        if (numberOfEnemyStickmans == 0)
        {
            for (int i = 1; i < transform.childCount; i++)
                transform.GetChild(i).rotation = Quaternion.identity;
        }
    }
}