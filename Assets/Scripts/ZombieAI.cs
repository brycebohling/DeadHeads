using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] LayerMask whatIsGround, whatIsPlayer; 

    [Header("Patrolling")]
    Vector3 walkPoint; 
    bool walkPointSet; 
    [SerializeField] float walkPointRange; 
    [SerializeField] float sightRange, targetRange;
    bool playerInSightRange, playerInTargetRange; 

    [Header("Attack")]
    [SerializeField] float zombieDamage;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float timeBetweenAttacks; 
    bool attacking; 

    [Header("Heath")]
    [SerializeField] float maxHealth;
    float currentHealth;
    [SerializeField] GameObject bloodSplatter;
    bool isDead;

    // Animations
    string _currentState;
    Animator zombieAnim;
    string attackAnim = "Armature|Attack";
    string idleAnim = "Armature|Idle";
    string walkAnim = "Armature|Walk";


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); 
        zombieAnim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead)
        {
            Destroy(gameObject);
            return;
        }

        //Check for sight and attack range 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); 
        playerInTargetRange = Physics.CheckSphere(transform.position, targetRange, whatIsPlayer); 

        if (!attacking)
        {
            if (!playerInSightRange && !playerInTargetRange)
            {
                Patrolling(); 
                ChangeAnimationState(walkAnim);
            }

            if (playerInSightRange && !playerInTargetRange)
            {
                ChasePlayer(); 
                ChangeAnimationState(walkAnim);
            }

            if (playerInSightRange && playerInTargetRange)
            {
                AttackPlayer();
                ChangeAnimationState(attackAnim);
            }
        } else
        {
            playerInAttack();
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
        agent.SetDestination(GameManager.gm.playerPos); 
    }

    private void AttackPlayer()
    {
        // make sure zombie doesnt move 
        agent.SetDestination(transform.position); 

        Vector3 playerPos = new Vector3(GameManager.gm.playerPos.x, 0, GameManager.gm.playerPos.z);
        Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);

        Vector3 lookVector = playerPos - thisPos;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.1f);

        if (!attacking)
        { 
            attacking = true; 
            StartCoroutine(ResetAttack());
        }
    }
    private IEnumerator ResetAttack()
    {
        // returning null waits for next frame
        yield return null;
        yield return new WaitUntil(() => !IsAnimationPlaying(zombieAnim, attackAnim));
        ChangeAnimationState(idleAnim);
        yield return new WaitForSeconds(timeBetweenAttacks);
        attacking = false; 
    }

    public void DamageZombie(float dmg, Vector3 hitPoint)
    {
        currentHealth -= dmg;
        Instantiate(bloodSplatter, hitPoint, Quaternion.identity);

        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }

    private void DamageTarget(float dmg)
    {
        GameManager.gm.DamagePlayer(dmg);
    }

    private void playerInAttack()
    {
        bool hitPlayer = Physics.CheckSphere(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer)
        {
            DamageTarget(zombieDamage);
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (newState == _currentState)
        {
            return;
        }

        zombieAnim.Play(newState);
        _currentState = newState;
    }

    private bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);    
    }
}