using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_PlayerController : MonoBehaviour
{
    
    Rigidbody2D rbPlayer;

    #region Movement System variables
    //Movement related declared variables
    float moveSpeed;
    [SerializeField] float defaultMoveSpeed = 2;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float minMoveSpeed;

    //Boost related declared variables
    bool isBoosting = false;
    [SerializeField] float defaultBoostMultiplier;
    int boostTime;
    [SerializeField] int defaultBoostTime;

    //Turning related declared variables
    float turnRate;
    [SerializeField] float defaultTurnRate = 20;
    [SerializeField] float maxTurnRate;

    //Warp related declared variables
    float warpDistance;
    [SerializeField] float defaultWarpDistance = 2;
    [SerializeField] float warpTime = 0.2f;
    [SerializeField] float minWarpDistance = 0.25f;

    //Decelaration
    [SerializeField] float defaultDecelartionRate;
    //Acceleration
    [SerializeField] float defaultAccelerationRate;
    #endregion

    #region Shooting system
    [SerializeField] float defaultFiringRate; //in seconds

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

        //Warp
        if (Input.GetKeyDown(KeyCode.X))
        {
            //warp
            WarpPlayer();
        }
    }

    void MovePlayer(float deltaHorMove, float deltaVerMove, float boostMultiplier)
    {
        float speed = rbPlayer.velocity.magnitude;

        if (rbPlayer.velocity.magnitude < maxMoveSpeed)
        {
            if(rbPlayer.velocity.magnitude < minMoveSpeed)
            {
                Vector2 deltaMove = new Vector2(deltaHorMove, deltaVerMove).normalized;
                //rbPlayer.velocity += deltaMove * boostMultiplier * (moveSpeed * Time.deltaTime);
                rbPlayer.AddForce(deltaMove * boostMultiplier * (5 * (1 - (speed / maxMoveSpeed))) * (moveSpeed * 50 * Time.deltaTime));
                Vector2 moveDir = new Vector2(transform.position.x + (rbPlayer.velocity.x * speed), transform.position.y + (rbPlayer.velocity.y * speed));
                //Debug.DrawLine(transform.position, moveDir, Color.red);
            }
            else
            {
                Vector2 deltaMove = new Vector2(deltaHorMove, deltaVerMove).normalized;
                //rbPlayer.velocity += deltaMove * boostMultiplier * (moveSpeed * Time.deltaTime);
                rbPlayer.AddForce(deltaMove * boostMultiplier * (moveSpeed * 50 * Time.deltaTime));
                Vector2 moveDir = new Vector2(transform.position.x + (rbPlayer.velocity.x * speed), transform.position.y + (rbPlayer.velocity.y * speed));
                //Debug.DrawLine(transform.position, moveDir, Color.red);
            }

            //float acceleration = defaultAccelerationRate;
            //if(speed < minMoveSpeed && speed > 0)
            //{
                //acceleration = (1 - (speed / maxMoveSpeed) * defaultAccelerationRate / 100);
            //}
            
            
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
            rbPlayer.drag = 0.5f;
        }
    }

    void WarpPlayer()
    {
        Vector2 WarpDir = transform.right * 2;
        transform.position = new Vector2(transform.position.x + WarpDir.x, transform.position.y + WarpDir.y);
        rbPlayer.velocity = rbPlayer.velocity.magnitude * transform.right;
    }



}
