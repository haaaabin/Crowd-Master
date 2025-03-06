using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tower : MonoBehaviour
{
    private int playerAmount;

    [Range(5f, 10f)][SerializeField] private int maxPlayerPerRow;
    [Range(0f, 2f)][SerializeField] private float xGap;
    [Range(0f, 2f)][SerializeField] private float yGap;
    [Range(0f, 10f)][SerializeField] private float yOffset;

    [SerializeField] private List<int> towerCountList = new List<int>();
    [SerializeField] private List<GameObject> towerList = new List<GameObject>();
    public static Tower instance;

    void Start()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateTower(int stickManNo)
    {
        playerAmount = stickManNo;
        FillTowerList();
        StartCoroutine(BuildTowerCoroutine());
    }

    private void FillTowerList()
    {
        towerCountList.Clear();
        List<int> tempList = new List<int>();

        int remainingPlayers = playerAmount;
        int rowSize = 1; // 처음은 1명부터 시작

        // 1, 2, 3, 4, ..., maxPlayerPerRow까지 증가하며 층을 채움
        while (remainingPlayers >= rowSize && rowSize <= maxPlayerPerRow)
        {
            tempList.Add(rowSize);
            remainingPlayers -= rowSize;
            rowSize++;
        }

        // 이후 maxPlayerPerRow 명씩 추가
        while (remainingPlayers > 0)
        {
            int rowCount = Mathf.Min(remainingPlayers, maxPlayerPerRow);
            tempList.Add(rowCount);
            remainingPlayers -= rowCount;
        }

        // 마지막 층이 maxPlayerPerRow보다 작은 경우, 윗층에서 빼서 5명 이상으로 맞춤
        if (tempList.Count > 1 && tempList[tempList.Count - 1] < maxPlayerPerRow)
        {
            int lastRow = tempList[tempList.Count - 1];
            tempList.RemoveAt(tempList.Count - 1);
            tempList[tempList.Count - 1] += lastRow;  // 바로 윗층에 남은 인원을 합침
        }

        towerCountList = tempList;
    }

    IEnumerator BuildTowerCoroutine()
    {
        var towerId = 0;
        transform.DOMoveX(0f, 0.5f).SetEase(Ease.Flash);
        yield return new WaitForSecondsRealtime(0.55f);

        foreach (int towerStickManCnt in towerCountList)
        {
            foreach (GameObject child in towerList)
            {
                child.transform.DOLocalMove(child.transform.localPosition + new Vector3(0, yGap, 0), 0.2f).SetEase(Ease.OutQuad);
            }

            var tower = new GameObject("Tower" + towerId);
            if (tower != null)
            {
                tower.transform.parent = transform;
                tower.transform.localPosition = new Vector3(0, 0, 0);

                towerList.Add(tower);

                var towerNewPos = Vector3.zero;
                float tempTowerHumanCount = 0;

                for (int i = 1; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    child.transform.parent = tower.transform;
                    child.transform.localPosition = new Vector3(tempTowerHumanCount * xGap, 0, 0);
                    towerNewPos += child.transform.position;
                    tempTowerHumanCount++;
                    i--;

                    if (tempTowerHumanCount >= towerStickManCnt)
                    {
                        break;
                    }
                }

                tower.transform.position = new Vector3(-towerNewPos.x / towerStickManCnt, tower.transform.position.y - yOffset, tower.transform.position.z);

                towerId++;
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }

        yield return new WaitForSecondsRealtime(1f);
        ObjectPool.instance.ClearPlayerObjects();
    }
}