using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float hitPoints;
    public float turnSpeed;
    public float chaseRange;
    public float attackDamage;

    [Header("Target Setup")]
    public Transform target;

    // Private Variables
    private float distanceToTarget;
    private float distanceToDefault;
    private NavMeshAgent agent;
    private Animator anim;
    Vector3 defaultPosition;

    private void FaceTarget(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public void Attack()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Attack", true);
    }

    public void ChaseTarget()
    {
        agent.SetDestination(target.position);
        anim.SetBool("Run", true);
        anim.SetBool("Attack", false);
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        anim.SetTrigger("GetHit");
        anim.SetFloat("HitPoints", hitPoints);
        if (hitPoints <= 0) {
            Destroy(gameObject, 3.4f);
        }
    }

    public void HitConnect()
    {
        if (distanceToTarget <= agent.stoppingDistance) {
            target.GetComponent<PlayerLogic>().PlayerGetHit(attackDamage);
        }
    }

    private void Start()
    {
        target = FindAnyObjectByType<PlayerLogic>().transform;
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponentInChildren<Animator>();
        anim.SetFloat("HitPoints", hitPoints);
        defaultPosition = this.transform.position;
    }

    // Update method needs correct capitalization
    private void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        distanceToDefault = Vector3.Distance(defaultPosition, transform.position);

        if (distanceToTarget <= chaseRange && hitPoints != 0) {
            FaceTarget(target.position);
            
            if (distanceToTarget > agent.stoppingDistance) {
                ChaseTarget();
            } else if (distanceToTarget <= agent.stoppingDistance) {
                Attack();
            }
        } else if (distanceToTarget >= chaseRange) {
            agent.SetDestination(defaultPosition);
            FaceTarget(defaultPosition);

            distanceToDefault -= 1;

            if (distanceToDefault <= agent.stoppingDistance) {
                anim.SetBool("Run", false);
                anim.SetBool("Attack", false);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}