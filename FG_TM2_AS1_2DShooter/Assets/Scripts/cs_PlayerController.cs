using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_PlayerController : MonoBehaviour
{
    public cs_GameManager gameManager;
    Rigidbody2D rbPlayer;
    [SerializeField] Camera mCamera = null;
    [SerializeField] private Transform bulletSpawnPoint;

    #region Movement System variables

    [Header("Movement")]
    //Movement related declared variables
    [SerializeField] float defaultMoveSpeed = 2;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float minMoveSpeed;
    float moveSpeed;


    [Header("Boost")]
    //Boost related declared variables
    [SerializeField] float defaultBoostMultiplier;
    bool isBoosting = false;
    int boostTime;
    [SerializeField] int defaultBoostTime;

    [Header("Turning")]
    //Turning related declared variables
    [SerializeField] float defaultTurnRate = 20;
    float turnRate;


    [Header("Dashing")]
    //Dash related declared variables
    [SerializeField] float dashMultiplier = 1f;
    [SerializeField] [Range(0, 1)] float retainMomentumPercentage;
    [SerializeField] [Range(0, 5)] float dashCooldown;
    private bool canDash = true;
    public enum dashSystem {DashInMovementInputDirection, DashInLookDirection}
    public dashSystem selectedSystem = dashSystem.DashInMovementInputDirection;

    [Header("Warping")]
    //Warp related declared variables
    [SerializeField] float defaultWarpDistance = 2;
    [SerializeField] float warpTime = 0.2f;
    [SerializeField] float minWarpDistance = 0.25f;
    [SerializeField] float warpCooldown;
    private bool canWarp = true;
    float warpDistance;

    [Header("Other")]
    //Decelaration
    [SerializeField] [Range(0, 2)] float defaultDecelartionRate = 0.5f;
    //Acceleration
    #endregion


    #region Shooting system
    [SerializeField] float defaultFiringRate; //in seconds
    public int puckCount;
    [SerializeField] GameObject prefabBullet;
    [SerializeField] float shootForce;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        moveSpeed = defaultMoveSpeed;
        boostTime = defaultBoostTime;
        turnRate = defaultTurnRate;
        warpDistance = defaultWarpDistance;
    }

    // Update is called once per frame
    void Update()
    {

        //MovePlayer Variables
        {
            float deltaVerMove = 0f; 
            float deltaHorMove = 0f;
            float boostMultiplier = defaultBoostMultiplier;

            #region MovementInput
            //Input from the vertical input axis (W & S, Up and Down arrrow, etc)
            if (Input.GetAxis("Vertical") != 0)
            {
                deltaVerMove = Input.GetAxis("Vertical");
            }
            

            //Input from the horizontal input axis (A & D, Left and Right arrrow, etc)
            if (Input.GetAxis("Horizontal") != 0)
            {
                deltaHorMove = Input.GetAxis("Horizontal");
            }
            
            

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isBoosting = true;
                boostMultiplier = defaultBoostMultiplier * 2;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isBoosting = false;
                boostMultiplier = defaultBoostMultiplier;
            }
            #endregion

            MovePlayer(deltaHorMove, deltaVerMove, boostMultiplier);
        }

        //Look at mouse position
        LookAtMouse(Input.mousePosition);

        //Decelaration 
        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            DecelaratePlayer();
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash|| Input.GetKeyDown(KeyCode.Joystick1Button10) && canDash)
        {
            DashPlayer(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        //Warp
        if (Input.GetKeyDown(KeyCode.X) && canWarp|| Input.GetKeyDown(KeyCode.Joystick1Button1) && canWarp)
        {
            //warp
            WarpPlayer();
        }

        //Shoot
        if(Input.GetMouseButtonDown(0) && puckCount > 0|| Input.GetKeyDown(KeyCode.Joystick1Button7) && puckCount > 0)
        {
            ShootPlayer();
        }

    }

    void MovePlayer(float deltaHorMove, float deltaVerMove, float boostMultiplier)
    {
        float speed = rbPlayer.velocity.magnitude;

        if (rbPlayer.velocity.magnitude < maxMoveSpeed)
        {
            Vector2 deltaMove = new Vector2(deltaHorMove, deltaVerMove).normalized;
            Debug.DrawLine(transform.position, new Vector2(transform.position.x + deltaMove.x, transform.position.y + deltaMove.y), Color.blue);

            if (rbPlayer.velocity.magnitude < minMoveSpeed)
            {
                rbPlayer.AddForce(deltaMove * boostMultiplier * (5 * (1 - (speed / maxMoveSpeed))) * (moveSpeed * 50 * Time.deltaTime));
                Vector2 moveDir = new Vector2(transform.position.x + (rbPlayer.velocity.x * speed), transform.position.y + (rbPlayer.velocity.y * speed));
                Debug.DrawLine(transform.position, moveDir, Color.red);
            }
            else
            {
                rbPlayer.AddForce(deltaMove * boostMultiplier * (moveSpeed * 50 * Time.deltaTime));
                Vector2 moveDir = new Vector2(transform.position.x + (rbPlayer.velocity.x * speed), transform.position.y + (rbPlayer.velocity.y * speed));
                Debug.DrawLine(transform.position, moveDir, Color.red);
            } 
        }
    }

    void LookAtMouse(Vector2 mousePos)
    {
        Vector2 lookAtPos = Camera.main.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
        Vector3 desiredDirection = new Vector3(lookAtPos.x - transform.position.x, lookAtPos.y - transform.position.y, 0);
        transform.right = Vector3.Lerp(transform.right, desiredDirection, turnRate * Time.deltaTime);
    }

    void DecelaratePlayer()
    {
        if(rbPlayer.velocity.x < 0.05 && rbPlayer.velocity.y < 0.05 && rbPlayer.velocity.x > -0.05 && rbPlayer.velocity.y > -0.05)
        {
            rbPlayer.velocity = new Vector2(0, 0);
            rbPlayer.drag = 0;
        }
        else
        {
            rbPlayer.drag = defaultDecelartionRate;
        }
    }

    void DashPlayer(float deltaHorMove, float deltaVerMove)
    {
        switch (selectedSystem)
        {
            case dashSystem.DashInMovementInputDirection:
                rbPlayer.velocity -= rbPlayer.velocity * (1 - retainMomentumPercentage);
                Vector2 dashDir = new Vector2(deltaHorMove, deltaVerMove).normalized;
                rbPlayer.AddForce(dashDir * (dashMultiplier * (1 - (rbPlayer.velocity.magnitude / maxMoveSpeed))));
                break;

            case dashSystem.DashInLookDirection:
                //rbPlayer.velocity = transform.right * (rbPlayer.velocity.magnitude * dashMultiplier);
                rbPlayer.velocity -= rbPlayer.velocity * (1 - retainMomentumPercentage);
                rbPlayer.AddForce(transform.right * (dashMultiplier * (1 - (rbPlayer.velocity.magnitude / maxMoveSpeed))));
                break;
        }
        
        canDash = false;
        StartCoroutine(cooldownTimer(0, dashCooldown));
    }

    void WarpPlayer()
    {
        Vector2 WarpDir = transform.right * defaultWarpDistance;
        transform.position = new Vector2(transform.position.x + WarpDir.x, transform.position.y + WarpDir.y);
        rbPlayer.velocity = rbPlayer.velocity.magnitude * transform.right;
        canWarp = false;
        StartCoroutine(cooldownTimer(1, warpCooldown));
    }

    void ShootPlayer()
    {
        GameObject bullet = Instantiate(prefabBullet, bulletSpawnPoint.position, Quaternion.identity);
        Vector2 shootDir = new Vector2(transform.position.x + transform.right.x * shootForce, transform.position.y + transform.right.y * shootForce);
        bullet.GetComponent<Rigidbody2D>().AddForce(shootDir);
        Debug.DrawLine(transform.position, transform.position + (transform.right * 1), Color.green);
        gameManager.playerPucks--;
        gameManager.uiManager.uiPuckCount.text = gameManager.playerPucks.ToString();
    }

    void PickUpPuckPlayer(GameObject collidedPuck)
    {
        gameManager.playerPucks++;
        gameManager.uiManager.uiPuckCount.text = gameManager.playerPucks.ToString();
        Destroy(collidedPuck);
    }

    public IEnumerator cooldownTimer(int action, float cooldown)
    {
        float time = 0f;

        while (time < cooldown)
        {
            time += Time.deltaTime;
            yield return null;
        }

        switch (action)
        {
            case 0:
                canDash = true;
                break;

            case 1:
                canWarp = true;
                break;
        }
    }

    #region The two seperate timers
    public IEnumerator dashCooldownTimer()
    {
        float time = 0f;

        while (time < dashCooldown)
        {
            time += Time.deltaTime;
            yield return null;
        }
        canDash = true;
    }

    public IEnumerator warpCooldownTimer()
    {
        float time = 0f;

        while (time < warpCooldown)
        {
            time += Time.deltaTime;
            yield return null;
        }
        canWarp = true;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Puck")
        {
            PickUpPuckPlayer(collision.gameObject);
        }
    }
}
