using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm { get; private set; }
    [SerializeField] PlayerC playerScript;
    public Vector3 playerPos;
    public bool isPlayerDead;


    private void Awake() 
    {
        if (gm != null && gm != this)
        {
            Destroy(this);
        } else
        {
            gm = this;
        }
    }

    private void Update() 
    {
        playerPos = playerScript.transform.position;
        isPlayerDead = playerScript.isDead;
    }

    public void DamagePlayer(float dmg)
    {
        playerScript.DamagePlayer(dmg);
    }
}
