using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int numberOfStickmans;
    [SerializeField] private TextMeshPro counterTxt;
    [SerializeField] private GameObject stickMan;

    // 스틱맨의 원형 배열 제어
    [Range(0f, 1f)][SerializeField] private float distanceFactor, radius;

    public bool moveByTouch, gameState;
    private Vector3 mouseStartPos, playerStartPos;
    public float playerSpeed, roadSpeed;
    [SerializeField] private Transform road;
    [SerializeField] private Transform enemy;

    private bool isAttack;

    // Start is called before the first frame update
    void Start()
    {
        numberOfStickmans = transform.childCount - 1;
        counterTxt.text = numberOfStickmans.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAttack)
        {
            // y축을 고정하여 2차원 평면에서의 방향을 계산
            var enemyDirection = new Vector3(enemy.position.x, transform.position.y, enemy.position.z) - transform.position;

            // 각 스틱맨을 적을 향해 부드럽게 회전시킨다.
            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation =
                     Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * 3f);
            }
        }
        else
        {
            MoveThePlayer();
        }
    }

    private void MoveThePlayer()
    {
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

        if (gameState)
        {
            road.Translate(road.forward * -1 * Time.deltaTime * roadSpeed);
        }
    }

    // 플레이어 중심으로 스틱맨을 원형으로 배열
    private void FormatStickMan()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            var x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            var z = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            var NewPos = new Vector3(x, 0f, z);

            transform.GetChild(i).DOLocalMove(NewPos, 1f).SetEase(Ease.OutBack);
        }
    }

    private void MakeStickMan(int number)
    {
        for (int i = numberOfStickmans; i < number; i++)
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

            numberOfStickmans = transform.childCount - 1;

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
        }
    }
}
