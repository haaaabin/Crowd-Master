using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;
    [SerializeField] private GameObject pileOfCoinParent;
    [SerializeField] private TextMeshProUGUI counter;
    private Vector3[] initialPos;
    private Quaternion[] initialRotation;
    private int coinNo = 10;
    private int currentCoinCount;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        currentCoinCount = PlayerPrefs.GetInt("CountCoin", 0);
        counter.text = currentCoinCount.ToString();

        initialPos = new Vector3[coinNo];
        initialRotation = new Quaternion[coinNo];

        for (int i = 0; i < pileOfCoinParent.transform.childCount; i++)
        {
            initialPos[i] = pileOfCoinParent.transform.GetChild(i).position;
            initialRotation[i] = pileOfCoinParent.transform.GetChild(i).rotation;
        }
    }

    private void ResetCoins()
    {
        for (int i = 0; i < pileOfCoinParent.transform.childCount; i++)
        {
            pileOfCoinParent.transform.GetChild(i).position = initialPos[i];
            pileOfCoinParent.transform.GetChild(i).rotation = initialRotation[i];
            pileOfCoinParent.transform.GetChild(i).localScale = Vector3.zero;
        }
    }

    public void RewardPileOfCoin(int noCoin)
    {
        ResetCoins();
        pileOfCoinParent.SetActive(true);

        float delay = 0f;
        bool hasStartedCoroutine = false;

        for (int i = 0; i < pileOfCoinParent.transform.childCount; i++)
        {
            var coin = pileOfCoinParent.transform.GetChild(i);
            if (coin != null)
            {
                coin.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
                coin.GetComponent<RectTransform>().DOAnchorPos(new Vector3(557f, 905f), 1f).SetDelay(delay + 0.5f)
                    .OnStart(() =>
                    {
                        if (!hasStartedCoroutine)
                        {
                            hasStartedCoroutine = true;
                            StartCoroutine(IncrementCoins(noCoin));
                        }
                    });

                coin.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);

                delay += 0.2f;
            }
        }
    }

    private IEnumerator IncrementCoins(int noCoin)
    {
        int targetValue = currentCoinCount + noCoin;

        for (int i = currentCoinCount; i <= targetValue; i++)
        {
            currentCoinCount = i;
            counter.text = currentCoinCount.ToString();
            PlayerPrefs.SetInt("CountCoin", currentCoinCount);
            yield return new WaitForSeconds(0.02f);
        }
        PlayerPrefs.Save();
    }
}
