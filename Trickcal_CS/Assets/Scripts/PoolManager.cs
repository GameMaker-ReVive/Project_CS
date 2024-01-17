using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹
    public GameObject[] prefabs;

    // 풀 담당을 하는 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        int ran;
        GameObject select = null;

        // 선택한 풀이 놀고 (비활성화 된) 있는 게임 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 모든 오브젝트 사용 중일 시 (못 찾았을 때) 오브젝트를 새로 생성해서 select 변수에 할당
        if (select == null)
        {
            select = Instantiate(prefabs[index], transform);
            // transform -> 오브젝트 생성위치 (= 부모 오브젝트 -> 자기자신(PoolManager))

            // 생성된 오브젝트는 해당 오브젝트 풀 리스트에 추가
            pools[index].Add(select);
        }

        // 스폰 포인트
        switch(index)
        {
            case 0:
                select.transform.position = GameManager.instance.unitSpawnPoint[0].position;
                break;
            case 1:
                ran = Random.Range(1, 4);
                select.transform.position = GameManager.instance.unitSpawnPoint[ran].position;
                break;
        }

        return select;
    }
}
