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
            // ��ġ ���� = Ÿ�� ��ġ - ���� ��ġ
            Vector2 dirVec = (Vector2)scanner.nearestTarget.position - rigid.position; // ����
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // ������ ������ ��ġ�� ��
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero; // ���� �ӵ��� MovePosition �̵��� ������ ���� �ʵ��� �ӵ� ����

            anim.SetInteger("AnimState", 2);
        }
        else
        {
            transform.position += moveDir * speed * Time.deltaTime;
            anim.SetInteger("AnimState", 2);
        }
    }
}
