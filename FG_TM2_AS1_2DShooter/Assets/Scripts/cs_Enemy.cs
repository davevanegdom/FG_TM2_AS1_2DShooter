using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cs_Enemy : MonoBehaviour
{
    public static event Action<int> onEnemyKilled;

    [HideInInspector]
    public int waveIndex;


    private Rigidbody2D rbEnemy;
    private bool enteredArena = false;
    private bool moveToPlayer = false;
    private Vector2 targetPos;

    [SerializeField] Transform tPlayer;
    [SerializeReference] float moveSpeed;
    [SerializeField] [Range(0, 1)] float targetPercentage;
    [SerializeField] LayerMask movementLayer;
    [SerializeField] float randomRadius;

    public bool canShoot;
    public List<GameObject> shootPrefabs;
    [SerializeField] float shootingRange;
    [SerializeField] float shootingAccuracy;
    [SerializeField] float shootSpeed;
    [SerializeField] int shootCooldown;
    private Vector2 shootPos;


    private void Start()
    {
        tPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rbEnemy = GetComponent<Rigidbody2D>();
        MoveToArena();
    }

    private void Update()
    {
        MoveEnemy();
    }

    #region Movement
    void MoveEnemy()
        {
            targetPos = Vector2.one;

            Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;
            rbEnemy.velocity = moveDir * moveSpeed * Time.deltaTime;

            if(Vector2.Distance(transform.position,  targetPos) < 0.1f)
            {
                if (enteredArena)
                {
                    CheckPath();
                }
                else
                {
                    enteredArena = true;
                }
            }
        }

    void MoveToArena()
        {
            targetPos = (Vector2.zero - (Vector2)transform.position).normalized;
            moveToPlayer = false;
        }

    void CheckPath()
        {
            if (enteredArena)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, tPlayer.position, Mathf.Infinity, movementLayer);

                while(hit.collider == null)
                {
                    targetPos = (tPlayer.position - transform.position).normalized * targetPercentage;
                    moveToPlayer = true;
                }
                targetPos = RandomizeMovePosition();
                moveToPlayer = false;
            }
            
        }
    Vector2 RandomizeMovePosition()
        {
            Vector2 acquiredPos = UnityEngine.Random.insideUnitCircle * randomRadius;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, acquiredPos, Vector2.Distance(transform.position, acquiredPos));

            while(hit.collider != null)
            {
                acquiredPos = UnityEngine.Random.insideUnitCircle * randomRadius;
                hit = Physics2D.Raycast(transform.position, acquiredPos, Vector2.Distance(transform.position, acquiredPos));
            }

            return acquiredPos;
        }
    #endregion

    #region Shooting
    void ShootAtPlayer()
     {
        //Check if in range
        if(Vector2.Distance(transform.position, tPlayer.position) < shootingRange)
        {

        }
     }

    IEnumerator CoolDown()
        {
            float time = shootCooldown;
            
            while(time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }

            canShoot = true;
        }
    #endregion

    private void EnemyKilled()
    {
        onEnemyKilled?.Invoke(waveIndex);
    }

    private void SetWaveIndex(int newWaveIndex)
    {
        waveIndex = newWaveIndex;
    }

    private void OnEnable()
    {
        cs_WaveManager.setWaveIndex += SetWaveIndex;
    }

    private void OnDisable()
    {
        cs_WaveManager.setWaveIndex -= SetWaveIndex;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "")
        {

        }
    }
}
