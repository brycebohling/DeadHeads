using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public void DmgEnemy(float dmg, Collider enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            // enemy.gameObject.GetComponent<EnemyScript>().DmgThem(dmg);
        } else if (enemy.CompareTag(""))
        {
            // enemy.gameObject.GetComponent<EnemyScript>().DmgThem(dmg);
        }
    }
}
