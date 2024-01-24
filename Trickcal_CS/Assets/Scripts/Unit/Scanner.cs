using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; // 스캔 범위
    public LayerMask targetLayer; // 레이어
    public RaycastHit2D[] targets; // 스캔 결과 배열
    public Transform nearestTarget; // 가장 가까운 목표

    void FixedUpdate()
    {
        // CircleCastAll(시작 위치, 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어) : 원형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 10; // 처음 계산을 위한 최소 거리

        // 인식된 오브젝트마다 플에이어와의 거리 계산
        foreach (RaycastHit2D target in targets)
        {
            UnitBase targetLogic = target.transform.gameObject.GetComponent<UnitBase>();

            if (targetLogic.unitState == UnitBase.UnitState.Fight)
                continue;

            Vector3 myPos = transform.position; // 플레이어 위치
            Vector3 targetPos = target.transform.position; // 인식된 오브젝트의 위치
            float curDiff = Vector3.Distance(myPos, targetPos); // Distance(A,B) : 벡터 A 와 B 의 거리를 계산해주는 함수

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        // 모든 적이 싸우고 있으면 싸우고 있는 적 중에서 가장 가까운 적으로 다시 탐색
        if(result == null)
        {
            foreach (RaycastHit2D target in targets)
            {
                UnitBase targetLogic = target.transform.gameObject.GetComponent<UnitBase>();

                Vector3 myPos = transform.position; // 플레이어 위치
                Vector3 targetPos = target.transform.position; // 인식된 오브젝트의 위치
                float curDiff = Vector3.Distance(myPos, targetPos); // Distance(A,B) : 벡터 A 와 B 의 거리를 계산해주는 함수

                if (curDiff < diff)
                {
                    diff = curDiff;
                    result = target.transform;
                }
            }
        }

        return result;
    }
}
