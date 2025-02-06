using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro counterTxt;
    [SerializeField] private GameObject stickMan;

    [Range(0f, 1f)][SerializeField] private float distanceFactor, radius;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Random.Range(10, 50); i++)
        {
            Instantiate(stickMan, transform.position, new Quaternion(0f, 180f, 0f, 1f), transform);
        }

        counterTxt.text = (transform.childCount - 1).ToString();

        FormatStickMan();
    }

    // Update is called once per frame
    void Update()
    {

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
}
