using DG.Tweening;
using UnityEngine;

public class StickManManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;


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
                transform.parent.DOJump(transform.parent.position, 3f, 1, 1f).SetEase(Ease.Flash);
                break;
        }
    }
}
