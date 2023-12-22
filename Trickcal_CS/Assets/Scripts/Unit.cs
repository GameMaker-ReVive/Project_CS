using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Unit : MonoBehaviour
{
    Scanner scanner;
    Rigidbody2D rigid;
    Animator anim;

    public int unitID;
    public int health;
    public int speed;
    public int power;

    Vector3 moveDir;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        scanner = GetComponentInChildren<Scanner>();

        switch(unitID)
        {
            case 0:
                moveDir = Vector3.right;
                break;
            case 1:
                moveDir = Vector3.left;
                break;
        }
    }

    void Update()
    {
        Scanner();
    }

    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // 위치 차이 = 타겟 위치 - 나의 위치
            Vector2 dirVec = (Vector2)scanner.nearestTarget.position - rigid.position; // 방향
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // 다음에 가야할 위치의 양
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero; // 물리 속도가 MovePosition 이동에 영향을 주지 않도록 속도 제거

            anim.SetInteger("AnimState", 2);
        }
        else
        {
            transform.position += moveDir * speed * Time.deltaTime;
            anim.SetInteger("AnimState", 2);
        }
    }
}
