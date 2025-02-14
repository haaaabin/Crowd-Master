using System;
using DG.Tweening;
using UnityEngine;

public class StickManManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    private Transform counterLabel;
    private Rigidbody rigid;
    private CapsuleCollider coll;
    private Animator anim;

    void Start()
    {
        counterLabel = transform.parent.Find("CounterLabel");
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
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
}