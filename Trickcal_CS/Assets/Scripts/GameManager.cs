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
        // �÷��̾� ���� ��ȯ
        if(Input.GetKeyDown(KeyCode.Q))
        {
            pool.Get(0);
        }

        // �� ���� ��ȯ
        if (Input.GetKeyDown(KeyCode.W))
        {
            pool.Get(1);
        }
    }

    // ��ȯ �����͸� ����ϴ� Ŭ���� ����
    [System.Serializable] // ����ȭ (Serialization) : ��ü�� ���� Ȥ�� �����ϱ� ���� ��ȯ -> �ν�����â���� ����
    public class SpawnData
    {
        public float spawnTime; // ��ȯ �ð�
        public int spriteType; // ���� Ÿ��
        public int health; // ü��
        public float speed; // �ӵ�
    }
}
