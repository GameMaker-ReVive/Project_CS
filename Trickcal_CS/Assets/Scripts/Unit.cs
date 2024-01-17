using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitState { Idle, Move, Fight, Attack, Die }
    public UnitState unitState;

    public int unitID;
    public int health;
    public int speed;
    public int power;
}
