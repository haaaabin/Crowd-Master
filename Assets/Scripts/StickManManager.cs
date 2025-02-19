using System.Collections;
using DG.Tweening;
using UnityEngine;

public class StickManManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    private Rigidbody rigid;
    private CapsuleCollider coll;
    private Animator anim;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();

        anim.SetBool("run", true);
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "red":
                Instantiate(blood, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                break;

            case "ramp":
                transform.DOJump(transform.position, 3f, 1, 1).SetEase(Ease.Flash).OnComplete(() => StartCoroutine(DelayedFormatStickMan(0.8f)));
                break;

            case "stair":
                transform.parent.parent = null;
                transform.parent = null;
                coll.isTrigger = false;
                rigid.isKinematic = false;

                anim.SetBool("run", false);

                if (!PlayerManager.instance.moveTheCamera)
                    PlayerManager.instance.moveTheCamera = true;

                if (PlayerManager.instance.transform.childCount == 2)
                {
                    StartCoroutine(ChangeStairRender(other));
                }

                break;
        }
    }

    IEnumerator ChangeStairRender(Collider other)
    {
        yield return new WaitForSeconds(0.5f);

        Renderer stairRender = other.GetComponent<Renderer>();
        if (stairRender != null)
        {
            stairRender.material.DOColor(new Color(0.4f, 0.98f, 0.65f), 0.5f).SetLoops(1000, LoopType.Yoyo)
                .SetEase(Ease.Flash);
        }
    }

    IEnumerator DelayedFormatStickMan(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerManager.instance.FormatStickMan();
    }
}