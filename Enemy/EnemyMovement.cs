using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField, Range(5, 20)] private float randomPositionTime = 10f;
    [SerializeField, Range(5, 15)] private float randomRadius = 8;

    private PlayerDetection detection;
    private Animator animator;
    private NavMeshAgent agent;
    private Vector3 spawnPoint;
    private Vector3 idlePoint;

    private float lastResetPosition = 0f;
    private bool attack = false;
    public bool Attack { get => attack; set => attack = value; }


    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = idlePoint = GetComponentInParent<Transform>().position;
        lastResetPosition = Time.time;
        detection = GetComponentInParent<PlayerDetection>();
    }

    private void Update()
    {
        if (GetComponent<Animator>().GetBool("Death")) return;
        if (attack)
        {
            animator.SetTrigger("Attack1");
            attack = false;
        }
        else if (detection.Target != null)
        {
            agent.SetDestination(detection.Target.transform.position);
        }
        else
        {
            if (Time.time - lastResetPosition >= randomPositionTime)
            {
                Vector3 direction = Random.value * randomRadius * new Vector3(Random.value, 0, Random.value).normalized;
                idlePoint = spawnPoint + direction;
                lastResetPosition = Time.time;
            }
            agent.SetDestination(idlePoint);
        }
        SetAnimation();
    }

    private void SetAnimation()
    {
        animator.SetBool("WalkForward", agent.velocity.magnitude >= 0.1f);
    }
}
