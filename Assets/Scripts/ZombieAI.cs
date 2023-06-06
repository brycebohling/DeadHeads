using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] Transform player; 
    [SerializeField] LayerMask whatIsGround, whatIsPlayer; 

    //Patrolling
    Vector3 walkPoint; 
    bool walkPointSet; 
    [SerializeField] float walkPointRange; 

    //Attacking 
    [SerializeField] float timeBetweenAttacks; 
    bool alreadyAttacked; 

    //States 
    [SerializeField] float sightRange, attackRange;
    bool playerInSightRange, playerInAttackRange; 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); 
    }

    private void Update()
    {
        //Check for sight and attack range 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); 
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); 

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling(); 
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer(); 
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void Patrolling() 
    { 
        if (!walkPointSet)
        {
            SearchWalkPoint(); 
        } else
        {
            agent.SetDestination(walkPoint); 
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false; 
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange); 
        float randomX = Random.Range(-walkPointRange, walkPointRange); 

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        { 
            walkPointSet = true;
        }
    }

    private void ChasePlayer() 
    { 
        agent.SetDestination(player.position); 
    }

    private void AttackPlayer()
    {
        //make sure zombie doesnt move 
        agent.SetDestination(transform.position); 

        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
        Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);

        Vector3 lookVector = playerPos - thisPos;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.1f);

        if (!alreadyAttacked)
        { 
            Debug.Log("attack");

            alreadyAttacked = true; 
            Invoke("ResetAttack", timeBetweenAttacks); 
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false; 
    }
}