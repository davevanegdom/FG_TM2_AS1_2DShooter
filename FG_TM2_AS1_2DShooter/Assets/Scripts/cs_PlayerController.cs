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
    [SerializeField] float defaultBoostMultiplier = 1;
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

        Debug.Log("Hello World");
    }

    // Update is called once per frame
    void Update()
    {

        //MovePlayer Variables
        {
            float deltaVerMove = 0f;
            float deltaHorMove = 0f;
            float boostMultiplier = 1f;

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
            
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                rbPlayer.velocity = rbPlayer.velocity * defaultDecelartionRate * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isBoosting = true;
                boostMultiplier = defaultBoostMultiplier;
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

        //Warp
        if (Input.GetKeyDown(KeyCode.X))
        {
            //warp
        }
    }

    void MovePlayer(float deltaHorMove, float deltaVerMove, float boostMultiplier)
    {
        Vector2 deltaMove = new Vector2(deltaHorMove, deltaVerMove);
        rbPlayer.velocity +=  deltaMove * boostMultiplier * (moveSpeed * Time.deltaTime);
    }

    void LookAtMouse(Vector2 mousePos)
    {
        Vector2 lookAtPos = Camera.main.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
        Vector3 desiredDirection = new Vector3(lookAtPos.x - transform.position.x, lookAtPos.y - transform.position.y, 0);
        transform.right = Vector3.Lerp(transform.right, desiredDirection, turnRate * Time.deltaTime);
    }

    void DecelaratePlayer()
    {
        rbPlayer.velocity = rbPlayer.velocity * defaultDecelartionRate * Time.deltaTime;
    }

    void WarpPlayer(Vector2 mousePos)
    {
        
    }



    
}
