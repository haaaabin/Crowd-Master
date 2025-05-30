using System.Collections;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class StickManManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    [SerializeField] private ParticleSystem blood2;

    private Rigidbody rigid;
    private CapsuleCollider coll;
    private Animator anim;

    private float lastSoundTime = 0f;
    private float soundCooldown = 1f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();

        GameManager.setMenuEvent += OnMenuAnimState;
        GameManager.setGameEvent += OnGameAnimState;
    }

    void OnDestroy()
    {
        GameManager.setMenuEvent -= OnMenuAnimState;
        GameManager.setGameEvent -= OnGameAnimState;
    }

    private void OnMenuAnimState() => anim.SetBool("run", false);
    private void OnGameAnimState() => anim.SetBool("run", true);

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "red":
                Instantiate(blood, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);

                if (Time.time - lastSoundTime >= soundCooldown)
                {
                    GameManager.Instance().soundManager.Play("Runner Died", SoundType.EFFECT, 1f, 0.008f);
                    lastSoundTime = Time.time;
                }
                break;

            case "ramp":
                if (gameObject.activeSelf)
                {
                    transform.DOJump(transform.position, 3f, 1, 1)
                            .SetEase(Ease.Flash)
                            .OnComplete(() => PlayerManager.instance.StartCoroutine(DelayedFormatStickMan(1f)));
                }
                break;

            case "stair":
                HandleStairTrigger(other);
                break;

            case "Prop":
                HandlePropTrigger();
                break;
        }
    }

    private void HandleStairTrigger(Collider other)
    {
        transform.parent.parent = null;
        transform.parent = null;
        coll.isTrigger = false;
        rigid.isKinematic = false;
        anim.SetBool("run", false);

        if (other.TryGetComponent(out Stair stair))
        {
            StairSoundManager.instance.PlayNote(stair.GetStairIndex(), other.GetComponentInParent<AudioSource>());
        }

        if (!PlayerManager.instance.moveTheCamera)
            PlayerManager.instance.moveTheCamera = true;

        if (PlayerManager.instance.transform.childCount == 1)
        {
            StartCoroutine(ChangeStairRender(other));
            StartCoroutine(UIManager.instance.UpdateScore(PlayerManager.instance.numberOfStickmans, GetStairScore(other)));
        }
    }

    private void HandlePropTrigger()
    {
        if (PlayerManager.instance.numberOfStickmans > 0)
        {
            ObjectPool.instance.ReturnPlayerObject(transform.gameObject); // 객체 풀로 반환
            Instantiate(blood2, transform.position, Quaternion.identity);

            PlayerManager.instance.numberOfStickmans--;
            PlayerManager.instance.counterTxt.text = PlayerManager.instance.numberOfStickmans.ToString();

            GameManager.Instance().soundManager.Play("Runner Died", SoundType.EFFECT, 0.5f, 0.5f);
        }
        PlayerManager.instance.StartCoroutine(DelayedFormatStickMan(1f));

    }

    IEnumerator ChangeStairRender(Collider other)
    {
        yield return new WaitForSeconds(0.5f);

        if (other.TryGetComponent(out Renderer stairRender))
        {
            stairRender.material.DOColor(new Color(0.4f, 0.98f, 0.65f), 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Flash);
        }

        yield return new WaitForSeconds(0.01f);
        GameManager.Instance().ChangeState(GameManager.GameState.LEVELCOMPLETE);

    }

    IEnumerator DelayedFormatStickMan(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerManager.instance.FormatStickMan();
    }

    private float GetStairScore(Collider other)
    {
        if (other.transform.GetChild(0).TryGetComponent(out TextMeshPro textMesh))
        {
            string scoreText = Regex.Match(textMesh.text, @"\d+(\.\d+)?").Value;
            return float.TryParse(scoreText, out float number) ? number : 0;
        }

        return 0f;
    }
}