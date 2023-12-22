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

    // 소환 데이터를 담당하는 클래스 선언
    [System.Serializable] // 직렬화 (Serialization) : 개체를 저장 혹은 전송하기 위해 변환 -> 인스펙터창에서 보임
    public class SpawnData
    {
        public float spawnTime; // 소환 시간
        public int spriteType; // 몬스터 타입
        public int health; // 체력
        public float speed; // 속도
    }
}
