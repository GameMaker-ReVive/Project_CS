using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitBase;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("# Main State")]
    [SerializeField] // 유닛 아이디
    private int unitID;
    public int UnitID { get { return unitID; } }

    [SerializeField] // 유닛 체력
    private float health;
    public float Health { get { return health; } }

    [SerializeField] // 유닛 이동속도
    private float speed;
    public float Speed { get { return speed; } }

    [SerializeField] // 유닛 공격력
    private float power;
    public float Power { get { return power; } }

    [SerializeField] // 유닛 공격 쿨타임
    private float attackTime;
    public float AttackTime { get { return attackTime; } }


    //[Header("# Level State")]

    //[SerializeField] // 레벨에 따른 체력 증가량
    //private float healths;
    //public float Healths { get { return healths; } }

    //[SerializeField] // 레벨에 따른 공격력 증가량
    //private float powers;
    //public float Powers { get { return powers; } }
}
