using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PoolManager pool;

    public Transform[] unitSpawnPoint;

    void Awake()
    {
        instance = this;
    }

    
    void Update()
    {
        // 플레이어 유닛 소환
        if(Input.GetKeyDown(KeyCode.Q))
        {
            pool.Get(0);
        }

        // 적 유닛 소환
        if (Input.GetKeyDown(KeyCode.W))
        {
            pool.Get(1);
        }
    }
}
