using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_PlayerController : MonoBehaviour
{
    public cs_GameManager gameManager;
    Rigidbody2D rbPlayer;
    [SerializeField] Camera mCamera = null;
    [SerializeField] private Transform puckSpawnPoint;
    private Transform puckHolder;

    #region Movement System variables

    [Header("Movement")]
    //Movement related declared variables
    [SerializeField] float defaultMoveSpeed = 2;
    public float maxMoveSpeed;
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

    [Header("Other")]
    //Decelaration
    [SerializeField] float defaultDecelartionRate;
    //Acceleration
    #endregion

    #region Shooting system
    [SerializeField] float defaultFiringRate; //in seconds
    public int puckCount;
    [SerializeField] GameObject prefabDynamicPuck;
    [SerializeField] GameObject prefabStaticPuck;
    [SerializeField] float shootForce;
    [SerializeField] float chargeTime;
    [SerializeField] float chargeMultiplier = 0.75f;
    Coroutine co;

    private List<GameObject> displayedStaticPucks;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        moveSpeed = defaultMoveSpeed;
        boostTime = defaultBoostTime;
        turnRate = defaultTurnRate;

        displayedStaticPucks = new List<GameObject>();
        //displayPuck(1);
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

        DecelaratePlayer();

        //Dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash|| Input.GetKeyDown(KeyCode.Joystick1Button10) && canDash)
        {
            DashPlayer(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        //Shoot
        if(Input.GetMouseButtonDown(0) && gameManager.playerPucks > 0|| Input.GetKeyDown(KeyCode.Joystick1Button7) && gameManager.playerPucks > 0)
        {
            ShootPlayer(displayedStaticPucks.Count, shootForce);
        }


        if(Input.GetMouseButtonDown(1) && gameManager.playerPucks > 0)
        {
            co = StartCoroutine(chargeShot());
        }

        if(Input.GetMouseButtonUp(1) && gameManager.playerPucks > 0)
        {
            chargeMultiplier = 0.75f;
            StopCoroutine(co);
            displayPuck(1);
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
                rbPlayer.AddForce(deltaMove * boostMultiplier * ((1 - (speed / maxMoveSpeed))) * (moveSpeed * 50 * Time.deltaTime));
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
        float decelarationValue = Vector2.Dot(rbPlayer.velocity, new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized);
        rbPlayer.drag = Mathf.Abs(decelarationValue) * defaultDecelartionRate;

        if((Input.GetAxis("Horizontal") > 0.05f || Input.GetAxis("Horizontal") < -0.05f) && (Input.GetAxis("Vertical") > 0.05f || Input.GetAxis("Vertical") < -0.05f))
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
    void ShootPlayer(int pucks, float shootForce)
    {
        List<Vector2> shootDirections = new List<Vector2>();

        foreach (GameObject puck in displayedStaticPucks)
        {
            Vector2 shootDir = (puck.transform.position - transform.position).normalized;
            shootDirections.Add(shootDir);
            Destroy(puck);
        }
        Debug.Log(displayedStaticPucks.Count);

        foreach (Vector2 direction in shootDirections)
        {
            Vector2 shootDir = new Vector2(transform.position.x + direction.x * (shootForce * chargeMultiplier), transform.position.y + direction.y * (shootForce * chargeMultiplier));
            GameObject puck = Instantiate(prefabDynamicPuck, puckSpawnPoint.position, Quaternion.identity);
            puck.GetComponent<Rigidbody2D>().AddForce(shootDir);
            puck.GetComponent<cs_Puck>().player = transform;
        }

        chargeMultiplier = 0.75f;

        displayPuck(1); //if>0

        gameManager.playerPucks -= shootDirections.Count;
        gameManager.uiManager.uiPuckCount.text = gameManager.playerPucks.ToString();
    }
    void displayPuck(int pucks)
    {

        if(puckSpawnPoint.childCount > 0)
        {
            foreach (GameObject puck in displayedStaticPucks)
            {
                Destroy(puck);
            }
        }
        
        displayedStaticPucks = new List<GameObject>();
        float puckInterval = 0.2f;
        float length = 0;
        float startPos = 0;

        if (pucks > 1)
        {
            length = puckInterval * pucks; //* pucks;
            startPos = (-length / 2) + (puckInterval / 2);

            for (int i = 0; i < pucks; i++)
            {
                GameObject staticPuck = Instantiate(prefabStaticPuck, puckSpawnPoint);
                staticPuck.transform.localPosition = new Vector2(.05f, startPos + puckInterval * i);
                displayedStaticPucks.Add(staticPuck);
            }
        }
        else
        {
            GameObject staticPuck = Instantiate(prefabStaticPuck, puckSpawnPoint);
            staticPuck.transform.localPosition = new Vector2(.05f, 0);
            displayedStaticPucks.Add(staticPuck);
        }

    }
    public IEnumerator chargeShot()
    {
        float time = 0;

        while (time < chargeTime)
        {
            time += Time.deltaTime;
            chargeMultiplier = Mathf.Lerp(0.75f, 1.25f, time / chargeTime);
            yield return null;
        }

        if(gameManager.playerPucks > 2)
        {
            //Super Shot
            displayPuck(gameManager.playerPucks);
        }
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
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        cs_CinemachineCamera.Instance.ShakeCamera(rbPlayer.velocity.magnitude/2, 0.2f);
    }
}
