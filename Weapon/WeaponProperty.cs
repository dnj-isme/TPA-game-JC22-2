using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProperty : MonoBehaviour
{
    [SerializeField] private int index = 0;
    [SerializeField, Range(10, 100)] private float range = 15;
    [SerializeField, Range(5, 100)] private int damage = 30;
    [SerializeField, Range(0, 10)] private float attackCooldown = 1f;

    public int Index { get => index; }
    public bool Fist { get => index == 0; }
    public float Range { get => range; }
    public int Damage { get => damage; }
    public int AttackTimes { get => 2; }
    public float AttackCooldown { get => attackCooldown; }
}
