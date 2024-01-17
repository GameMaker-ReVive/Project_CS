using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    Scanner scanner;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer renderer;

    Vector3 moveDir;
    public LayerMask attackLayer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        scanner = GetComponentInChildren<Scanner>();

        unitState = UnitState.Idle;

        switch (unitID)
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
        if (unitState != UnitState.Fight)
        {
            Scanner();
        }
        AttackRay();


        if (renderer.flipX)
        {
            moveDir = Vector3.right;
        }
        else
        {
            moveDir = Vector3.left;
        }
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

            // 가는 방향에 따라 moveDir 변경
            if (dirVec.x > 0)
            {
                renderer.flipX = true;
            }
            else if (dirVec.x < 0)
            {
                renderer.flipX = false;
            }

            anim.SetInteger("AnimState", 2);

            unitState = UnitState.Move;
        }
        else
        {
            switch (unitID)
            {
                case 0:
                    moveDir = Vector2.zero;
                    unitState = UnitState.Idle;
                    anim.SetInteger("AnimState", 0);
                    break;
                case 1:
                    transform.position += moveDir * speed * Time.deltaTime;
                    anim.SetInteger("AnimState", 2);
                    unitState = UnitState.Move;
                    moveDir = Vector3.left;
                    renderer.flipX = false;
                    break;
            }
        }
    }

    void AttackRay()
    {
        Collider2D attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(moveDir.x * 0.6f, 0.6f, 0), new Vector2(0.5f, 1.2f), 0, attackLayer);

        if (attackTarget != null)
        {
            Unit targetLogic = attackTarget.gameObject.GetComponent<Unit>();

            unitState = UnitState.Fight;
            anim.SetInteger("AnimState", 0);

            switch (unitID)
            {
                case 0:
                    gameObject.layer = 8;
                    break;
                case 1:
                    gameObject.layer = 9;
                    break;
            }
        }
        else
        {
            unitState = UnitState.Move;

            switch (unitID)
            {
                case 0:
                    gameObject.layer = 6;
                    break;
                case 1:
                    gameObject.layer = 7;
                    break;
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(moveDir.x * 0.6f, 0.6f, 0), new Vector2(0.5f, 1.2f));
    }

}
