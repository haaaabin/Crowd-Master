using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 100;
    private GameObject poolParent;
    private GameObject enemypoolParent;

    private Queue<GameObject> playerPool = new Queue<GameObject>();
    private Queue<GameObject> enemyPool = new Queue<GameObject>();


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        poolParent = new GameObject("PlayerPool");
        enemypoolParent = new GameObject("EnemyPool");

        // Player 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab);
            playerObj.transform.SetParent(poolParent.transform);
            playerObj.SetActive(false);
            playerPool.Enqueue(playerObj);
        }
        
        // Enemy 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemyObj = Instantiate(enemyPrefab);
            enemyObj.transform.SetParent(enemypoolParent.transform);
            enemyObj.SetActive(false);
            enemyPool.Enqueue(enemyObj);
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

    
    // Player 오브젝트 가져오기
    public GameObject GetEnemyObject()
    {
        if (enemyPool.Count > 0)
        {
            GameObject obj = enemyPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // 풀에 오브젝트가 부족하면 새로운 Player 오브젝트 생성
            GameObject obj = Instantiate(enemyPrefab);
            return obj;
        }
    }


    // Player 오브젝트 반환하기
    public void ReturnPlayerObject(GameObject obj)
    {
        obj.transform.SetParent(poolParent.transform);
        obj.SetActive(false);
        playerPool.Enqueue(obj);
    }

    // Player 오브젝트 반환하기
    public void ReturnEnemyObject(GameObject obj)
    {
        obj.transform.SetParent(enemypoolParent.transform);
        obj.SetActive(false);
        enemyPool.Enqueue(obj);
    }

    // 모든 오브젝트 반환하기 (선택적)
    public void ReturnAllToPool()
    {
        foreach (var player in playerPool)
        {
            player.SetActive(false);
        }
        foreach (var enemy in enemyPool)
        {
            enemy.SetActive(false);
        }
    }

    public void ClearPlayerObjects()
    {
        foreach (var player in playerPool)
        {
            Destroy(player);
        }
    }

    public void ClearEnemyObjects()
    {
        foreach (var enemy in enemyPool)
        {
            Destroy(enemy);
        }
    }

    public void ClearAllToPool()
    {
        foreach (var player in playerPool)
        {
            Destroy(player);
        }
        foreach (var enemy in enemyPool)
        {
            Destroy(enemy);
        }
    }


}
