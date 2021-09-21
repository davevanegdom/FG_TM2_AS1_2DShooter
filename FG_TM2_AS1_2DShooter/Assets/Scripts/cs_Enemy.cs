using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cs_Enemy : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rbPlayer;
    Rigidbody2D rbEnemy;

    public float moveSpeed;
    public float hitRange;

    private bool enteredArena = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rbEnemy = GetComponent<Rigidbody2D>();
        rbPlayer = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveEnemy();
        AttackPlayer();

    }

    void MoveEnemy()
    {
        if (enteredArena)
        {
            Vector2 playerPos = new Vector2(rbPlayer.velocity.x + player.transform.position.x, rbPlayer.velocity.y + player.transform.position.y);
            Vector2 randomPos = UnityEngine.Random.insideUnitCircle * rbPlayer.velocity.magnitude;
            Vector2 followPos = new Vector2(playerPos.x + randomPos.x, playerPos.y + randomPos.y);

            Vector2 moveDir = new Vector2(followPos.x - transform.position.x, followPos.y - transform.position.y).normalized;
            rbEnemy.velocity = moveDir * moveSpeed * 10 * Time.deltaTime;
            //Debug.DrawLine(transform.position, new  Vector2(rbEnemy.velocity.x + transform.position.x, rbEnemy.velocity.y + transform.position.y), Color.green);

            transform.right = player.transform.position - transform.position;
        }
        else
        {
            Vector2 moveDir = new Vector2(0 - transform.position.x, 0 - transform.position.y).normalized;
            rbEnemy.velocity = moveDir * moveSpeed * 10 * Time.deltaTime; 
        }
        
    }

    void AttackPlayer()
    {
        if(Vector2.Distance(player.transform.position, transform.position) <  hitRange)
        {
            StartCoroutine(HitPlayer());
        }
    }

    IEnumerator HitPlayer()
    {
        yield return new WaitForSeconds(.5f);

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, hitRange, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
        {
            EnemyKilled();
        }
    }

    private void EnemyKilled()
    {
        Debug.Log("Enemy Killed");
    }

    private void SetWaveIndex(int newWaveIndex)
    {

    }

    private void OnEnable()
    {
        cs_WaveManager.setWaveIndex += SetWaveIndex;
    }

    private void OnDisable()
    {
        cs_WaveManager.setWaveIndex -= SetWaveIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enteredArena = true;
    }
}
