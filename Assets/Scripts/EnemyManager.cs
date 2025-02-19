using UnityEngine;
using TMPro;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public TextMeshPro counterTxt;
    public Transform enemy;
    [SerializeField] private GameObject stickMan;
    [Range(0f, 1f)][SerializeField] private float distanceFactor, radius;
    public bool isAttack;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < Random.Range(20, 120); i++)
        {
            Instantiate(stickMan, transform.position, new Quaternion(0f, 180f, 0f, 1f), transform);
        }

        counterTxt.text = transform.childCount.ToString();
        FormatStickMan();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isAttack && transform.childCount > 1)
        {
            HandleAttack();
        }
    }

    private void HandleAttack()
    {
        var enemyDirection = enemy.position - transform.position;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation, quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * 3f);

            if (enemy.childCount > 0)
            {
                var distance = enemy.GetChild(1).position - transform.GetChild(i).position;

                if (distance.magnitude < 4f)
                {
                    transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position, enemy.GetChild(1).position, Time.deltaTime);
                }
            }
        }
    }

    private void FormatStickMan()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            var z = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            var NewPos = new Vector3(x, 0f, z);

            transform.transform.GetChild(i).localPosition = NewPos;
        }
    }

    public void Attacking(Transform enemyForce)
    {
        enemy = enemyForce;
        isAttack = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("run", true);
        }
    }

    public void StopAttacking()
    {
        isAttack = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("run", false);
        }
    }
}
