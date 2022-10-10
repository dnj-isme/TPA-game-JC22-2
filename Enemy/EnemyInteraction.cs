using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class EnemyInteraction : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private EnemyStats stat;
    [SerializeField] private EnemyMovement movement;

    [Header("Enemy SFX")]
    [SerializeField] private AudioClip enemyAttack;
    [SerializeField] private AudioClip enemyDeath;

    public void PlayDeathAudio() => manager.PlayClip(enemyDeath, .5f, 4);

    private SFXManager manager;
    private float startAnim = 0;
    private float startCooldown = 0;
    private bool attack = false;

    public void SetTarget(GameObject target) => this.target = target;
    public void RemoveTarget() => target = null;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        stat = GetComponent<EnemyStats>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SFXManager>();
    }
    void Update()
    {
        if (target == null || !stat.Alive || !target.GetComponent<PlayerStats>().Alive) return;

        // condition when not in cooldown
        if(Time.time - startCooldown >= stat.AttackCooldown)
        {
            // if it hasn't attack yet,  
            if(!attack)
            {
                manager.PlayClip(enemyAttack, .5f, 4);
                Attack();
                startAnim = Time.time;
                movement.Attack = true;
                attack = true;
            }

            // if the first attack animation finishes
            if(Time.time - startAnim >= stat.AttackAnimation)
            {
                attack = false;
                startCooldown = Time.time;
            }
        }
    }
    private void Attack()
    {
        PlayerStats targetStat = target.GetComponent<PlayerStats>();
        targetStat.AddHP(-stat.Dmg);
        if (!targetStat.Alive)
        {
            manager.PlayDeath();
            target.GetComponent<Animator>().SetTrigger("Dead");
            target = null;
        } 
        else
        {
            manager.PlayReceiveHit();
        }
    }
}
