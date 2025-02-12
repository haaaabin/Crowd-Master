using DG.Tweening;
using UnityEngine;

public class StickManManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    private Transform counterLabel;

    void Start()
    {
        counterLabel = transform.parent.Find("CounterLabel");
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "red":
                if (other.transform.parent.childCount > 0)
                {
                    Destroy(other.gameObject);
                    Destroy(gameObject);

                    Instantiate(blood, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
                }

                break;

            case "ramp":
                Vector3 labelStartPos = counterLabel.localPosition;

                transform.DOJump(transform.position, 3f, 1, 1f).SetEase(Ease.Flash);
                counterLabel.DOLocalMoveY(labelStartPos.y + 0.5f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                        counterLabel.DOLocalMoveY(labelStartPos.y, 0.5f).SetEase(Ease.InQuad));
                break;

            case "stair":
                Debug.Log("stair");
                
                break;
        }
    }
}
