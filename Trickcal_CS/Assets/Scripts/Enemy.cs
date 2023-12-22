using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed; // �ӵ�
    public float health; // ü��
    public float maxHealth; // �ִ� ü��

    bool isLive; // ��������

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    Scanner scanner;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponentInChildren<Scanner>();
    }

    void OnEnable()
    {
        // ������Ʈ Ȱ��ȭ ��, �ʱ� ������ ����
        isLive = true;
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (!isLive)
            return;

        // ��ġ ���� = Ÿ�� ��ġ - ���� ��ġ
        Vector2 dirVec = (Vector2)scanner.nearestTarget.position - rigid.position; // ����
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // ������ ������ ��ġ�� ��
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // ���� �ӵ��� MovePosition �̵��� ������ ���� �ʵ��� �ӵ� ����
    }

    void LateUpdate()
    {
        if (!isLive)
            return;

        spriter.flipX = scanner.nearestTarget.position.x < rigid.position.x;
    }

}
