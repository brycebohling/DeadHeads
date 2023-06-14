using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public void DmgEnemy(float dmg, Collider enemy, Vector3 hitPoint)
    {
        if (enemy.CompareTag("Zombie"))
        {
            enemy.gameObject.GetComponent<ZombieAI>().DamageZombie(dmg, hitPoint);
        } else if (enemy.CompareTag(""))
        {
            // enemy.gameObject.GetComponent<EnemyScript>().DmgThem(dmg);
        }
    }
}
