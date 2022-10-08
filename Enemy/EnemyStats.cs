using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [SerializeField, Range(20, 250)] private int maxHP = 100;
    [SerializeField, Range(0, 50)] private int xp = 20;

    [Header("Damage Attribute")]
    [SerializeField] private float attackAnim;
    [SerializeField, Range(0, 5f)] private float attackCooldown = 2f;
    [SerializeField, Range(1, 50)] private int dmg = 40;

    [Header("Showed for debug purpose")]
    [SerializeField] private int hp = 100;
    [SerializeField] private Slider slider;
    public int MaxHP { get { return maxHP; } }
    public int Dmg { get { return dmg; } }
    public int HP { get { return hp; } }
    public bool Alive { get { return hp > 0; } }
    public int XP { get { return xp; } }
    public float AttackAnimation { get => attackAnim; }
    public float AttackCooldown { get => attackCooldown; }

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        ResetStat();
    }

    public EnemyStats ReceiveDamage(int damage)
    {
        hp = Math.Max(0, Math.Min(hp - damage, maxHP));
        RefreshSlider();
        return this;
    }

    public EnemyStats RefreshSlider()
    {
        slider.maxValue = maxHP;
        slider.minValue = 0;
        slider.value = hp;
        return this;
    }

    public EnemyStats ResetStat()
    {
        hp = maxHP;
        return this;
    }
}
