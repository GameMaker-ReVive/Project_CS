using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elf_Warrior;

public class PlayerUnit : UnitBase
{
    Scanner scanner;
    Rigidbody2D rigid;

    public LayerMask attackLayer;
    Vector2 moveDir; //  방향
    Vector2 disVec; // 거리
    Vector2 nextVec; // 다음에 가야할 위치의 양
    bool startMoveFinish = false;

    [Header("# Spine")]
    //스파인 애니메이션을 위한 것
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    //현재 애니메이션 처리가 무엇인지에 대한 변수
    private AnimState _AnimState;

    //현재 어떤 애니메이션이 재생되고 있는지에 대한 변수
    private string CurrentAnimation;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        scanner = GetComponentInChildren<Scanner>();

        unitState = UnitState.Move;
        moveDir = Vector3.right;
    }

    void OnEnable()
    {
        Transform original = gameObject.transform;

        StartCoroutine(
            lerpCoroutine(GameManager.instance.unitSpawnPoint[0].position, GameManager.instance.point, speed));
    }

    void Update()
    {
        AttackRay();
        Animation();
    }

    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // 위치 차이 = 타겟 위치 - 나의 위치
            disVec = (Vector2)scanner.nearestTarget.position - rigid.position;
            Enemy enemyLogic = scanner.nearestTarget.GetComponent<Enemy>();

            // 이동
            nextVec = disVec.normalized * speed * Time.deltaTime;
            transform.position += (Vector3)nextVec;
            unitState = UnitState.Move;

            // 가는 방향에 따라 Sprite 방향 변경
            if (disVec.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                moveDir = Vector2.right;
            }
            else if (disVec.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                moveDir = Vector2.left;
            }

        }
        else
        {
            if(startMoveFinish)
            {
                moveDir = Vector2.zero;
                transform.localScale = new Vector3(1f, 1f, 1f);
                unitState = UnitState.Idle;
            }
        }
    }

    void AttackRay()
    {
        Collider2D attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.5f, 0.5f), 0, attackLayer);

        if (attackTarget != null)
        {
            Enemy targetLogic = attackTarget.gameObject.GetComponent<Enemy>();

            unitState = UnitState.Fight;
            startMoveFinish = true;

            gameObject.layer = 8;
        }
        else
        {
            gameObject.layer = 6;

            // AttackRay 에 인식되는 오브젝트가 없는 경우, 다시 스캔 시작
            Scanner();
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.5f, 0.5f));
    }

    void Animation()
    {
        int animIndex = 0;

        switch(unitState)
        {
            case UnitState.Idle:
                _AnimState = AnimState.Idle;
                animIndex = 0;
                break;
            case UnitState.Move:
                _AnimState = AnimState.Run;
                animIndex = 1;
                break;
            case UnitState.Fight:
                _AnimState = AnimState.Idle;
                animIndex = 0;
                break;
            case UnitState.Attack:
                break;
            case UnitState.Damaged:
                break;
            case UnitState.Die:
                break;

        }

        //애니메이션 적용
        _AsyncAnimation(AnimClip[animIndex], true, 1f);
    }

    IEnumerator lerpCoroutine(Vector3 current, Vector3 target, float speed)
    {
        float distance = Vector3.Distance(current, target); // 거리(길이) 구하기
        float time = distance / speed; // 거리(길이) 에 따라 이동하는 시간 설정

        float elapsedTime = 0.0f;

        this.transform.position = current;
        while (elapsedTime < time && !scanner.nearestTarget)
        {
            elapsedTime += Time.deltaTime;

            this.transform.position = Vector3.Lerp(current, target, elapsedTime / time);

            yield return null;

            unitState = UnitState.Move;

        }

        startMoveFinish = true;

        yield return null;
    }

    private void _AsyncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        //동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 X
        if (animClip.name.Equals(CurrentAnimation))
        {
            return;
        }

        //해당 애니메이션으로 변경한다.
        skeletonAnimation.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;
        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        //현재 재생되고 있는 애니메이션 값을 변경
        CurrentAnimation = animClip.name;
    }

    private void SetCurrentAnimation(AnimState _state)
    {
        switch (_state)
        {
            case AnimState.Idle:
                _AsyncAnimation(AnimClip[(int)AnimState.Idle], true, 1f);
                break;
            case AnimState.Run:
                _AsyncAnimation(AnimClip[(int)AnimState.Run], true, 1f);
                break;
        }

        //짧게 작성한다 요렇게..
        //_AsyncAnimation(AnimClip[(int)AnimState], true, 1f);
    }
}
