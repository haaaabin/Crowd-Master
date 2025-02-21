using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int poolSize = 100;
    private Queue<GameObject> playerPool = new Queue<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Player 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab);
            playerObj.SetActive(false);
            playerPool.Enqueue(playerObj);
        }
    }

    // Player 오브젝트 가져오기
    public GameObject GetPlayerObject()
    {
        if (playerPool.Count > 0)
        {
            GameObject obj = playerPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // 풀에 오브젝트가 부족하면 새로운 Player 오브젝트 생성
            GameObject obj = Instantiate(playerPrefab);
            return obj;
        }
    }

    // Player 오브젝트 반환하기
    public void ReturnPlayerObject(GameObject obj)
    {
        obj.SetActive(false);
        playerPool.Enqueue(obj);
    }

    public void ClearPlayerObjects()
    {
        foreach (var player in playerPool)
        {
            Destroy(player);
        }
    }
}
