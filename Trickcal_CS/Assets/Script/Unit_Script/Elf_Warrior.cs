using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elf_Warrior : MonoBehaviour
{
    //스파인 애니메이션을 위한 것
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    //애니메이션에 대한 Enum
    public enum AnimState
    {
        Idle,Run
    }

    //현재 애니메이션 처리가 무엇인지에 대한 변수
    private AnimState _AnimState;

    //현재 어떤 애니메이션이 재생되고 있는지에 대한 변수
    private string CurrentAnimation;

    //무브처리
    private Rigidbody2D rig;
    private float xx;

    private void Awake() => rig = GetComponent<Rigidbody2D>();


    private void Update()
    {
        xx = Input.GetAxisRaw("Horizontal");

        if (xx == 0f)
            _AnimState = AnimState.Idle;

        else
        {
            _AnimState = AnimState.Run;

            transform.localScale = new Vector2(xx, 1);
        }

        //애니메이션 적용
        SetCurrentAnimation(_AnimState);
      
    }

    private void FixedUpdate() =>
        rig.velocity = new Vector2(xx * 300 * Time.deltaTime, rig.velocity.y);

    private void _AsyncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {   
        //동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 X
        if(animClip.name.Equals(CurrentAnimation))
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
