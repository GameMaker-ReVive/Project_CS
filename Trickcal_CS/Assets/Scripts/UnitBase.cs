using System.Collections;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public enum UnitState { Idle, Move, Fight, Attack, Damaged, Die}

    [Header("# UnitState")]
    public UnitState unitState;
    public int unitID;
    public int health;
    public int speed;
    public int power;
}
