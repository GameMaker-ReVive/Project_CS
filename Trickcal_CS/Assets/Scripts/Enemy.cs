using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitBase
{
    Scanner scanner;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer renderer;

    public LayerMask attackLayer;
    Vector2 moveDir; //  방향
    Vector2 disVec; // 거리
    Vector2 nextVec; // 다음에 가야할 위치의 양

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        scanner = GetComponentInChildren<Scanner>();

        unitState = UnitState.Move;
        moveDir = Vector3.left;
    }

    void Update()
    {
        AttackRay();
    }

    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // 위치 차이 = 타겟 위치 - 나의 위치
            disVec = (Vector2)scanner.nearestTarget.position - rigid.position;

            // 가는 방향에 따라 Sprite 방향 변경
            if (disVec.x > 0)
            {
                renderer.flipX = true;
                moveDir = Vector2.right;
            }
            else if (disVec.x < 0)
            {
                renderer.flipX = false;
                moveDir = Vector2.left;
            }
        }
        else
        {
            disVec = Vector2.left;
            renderer.flipX = false;
        }

        // 이동
        nextVec = disVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // 물리 속도가 MovePosition 이동에 영향을 주지 않도록 속도 제거
        unitState = UnitState.Move;

        anim.SetInteger("AnimState", 2);
    }

    void AttackRay()
    {
        Collider2D attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.6f, 0.5f), 0, attackLayer);

        if (attackTarget != null)
        {
            PlayerUnit targetLogic = attackTarget.gameObject.GetComponent<PlayerUnit>();

            unitState = UnitState.Fight;
            anim.SetInteger("AnimState", 0);

            gameObject.layer = 9;
        }
        else
        {
            gameObject.layer = 7;

            // AttackRay 에 인식되는 오브젝트가 없는 경우, 다시 스캔 시작
            Scanner();
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.6f, 0.5f));
    }

}
