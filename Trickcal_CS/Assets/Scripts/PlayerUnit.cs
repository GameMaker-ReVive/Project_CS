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
    Vector2 moveDir; //  ����
    Vector2 disVec; // �Ÿ�
    Vector2 nextVec; // ������ ������ ��ġ�� ��

    public float lerpTime = 1.0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        scanner = GetComponentInChildren<Scanner>();

        unitState = UnitState.Move;
        moveDir = Vector3.right;
    }

    void OnEnable()
    {
        Transform original = gameObject.transform;
        StartCoroutine(
            lerpCoroutine(original.position, GameManager.instance.point, lerpTime));
    }

    void Update()
    {
        AttackRay();
    }

    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // ��ġ ���� = Ÿ�� ��ġ - ���� ��ġ
            disVec = (Vector2)scanner.nearestTarget.position - rigid.position;

            // �̵�
            nextVec = disVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero; // ���� �ӵ��� MovePosition �̵��� ������ ���� �ʵ��� �ӵ� ����
            unitState = UnitState.Move;

            // ���� ���⿡ ���� Sprite ���� ����
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

            // AttackRay �� �νĵǴ� ������Ʈ�� ���� ���, �ٽ� ��ĵ ����
            Scanner();
        }

    }

    IEnumerator lerpCoroutine(Vector3 current, Vector3 target, float time)
    {
        float elapsedTime = 0.0f;

        this.transform.position = current;
        while (elapsedTime < time)
        {
            elapsedTime += (Time.deltaTime);

            this.transform.position
                = Vector3.Lerp(current, target, elapsedTime / time);

            yield return null;
        }

        transform.position = target;

        yield return null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.3f, 0.5f));
    }
}
