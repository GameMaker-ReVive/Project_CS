using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
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
        moveDir = Vector3.right;
    }

    void Update()
    {
        StartMove();
        AttackRay();
    }

    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // 위치 차이 = 타겟 위치 - 나의 위치
            disVec = (Vector2)scanner.nearestTarget.position - rigid.position;

            // 이동
            nextVec = disVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero; // 물리 속도가 MovePosition 이동에 영향을 주지 않도록 속도 제거
            unitState = UnitState.Move;

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

            anim.SetInteger("AnimState", 2);
        }
        else
        {
            moveDir = Vector2.zero;
            renderer.flipX = true;
            unitState = UnitState.Idle;
            anim.SetInteger("AnimState", 0);
        }
    }

    void AttackRay()
    {
        Collider2D attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.3f, 0.5f), 0, attackLayer);

        if (attackTarget != null)
        {
            Enemy targetLogic = attackTarget.gameObject.GetComponent<Enemy>();

            unitState = UnitState.Fight;
            anim.SetInteger("AnimState", 0);

            gameObject.layer = 8;
        }
        else
        {
            gameObject.layer = 6;

            // AttackRay 에 인식되는 오브젝트가 없는 경우, 다시 스캔 시작
            Scanner();
        }

    }

    void StartMove()
    {
        disVec = GameManager.instance.point - transform.position;

        // 이동
        nextVec = disVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // 물리 속도가 MovePosition 이동에 영향을 주지 않도록 속도 제거
        unitState = UnitState.Move;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.3f, 0.5f));
    }
}
