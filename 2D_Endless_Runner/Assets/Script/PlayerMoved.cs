using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoved : MonoBehaviour
{
    [Header("Movement")]
    public float moveAccelstart;
    public float maxSpeedStart;
    public float maxSpeed;
    public float moveAccel;

    [Header("Jump")]
    public float jumpAccel;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;

    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraFollow gameCamera;

    private Rigidbody2D rig;
    private Animator anim;
    private CharSoundControll sound;

    private bool isJumping;
    private bool isOnGround;

    private float lastPositionX;

   //panel start
   [Header("Start Panel")]
    public bool start;
    public GameObject startPanel;

    //panel pause
    [Header("pause")]
    public GameObject btnpause;
    public GameObject panelpause;
    private bool pause;

    private void Start()
    {
        maxSpeed = maxSpeedStart;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharSoundControll>();

        lastPositionX = transform.position.x;

        startPanel.SetActive(true);
    }

    private void Update()
    {
        // read input
        if (Input.GetMouseButtonDown(0)&&pause==false)
        {
            if (isOnGround)
            {
                isJumping = true;

                sound.PlayJump();
            }
        }

        // change animation
        anim.SetBool("isOnGround", isOnGround);

        // calculate score
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if (scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }

        // game over
        if (transform.position.y < fallPositionY)
        {
            GameOver();
            // disable pause button
            Destroy(btnpause);
        }

        //start
        if (start == true)
        {
            GameStart();
        }
        //untuk memulai permainan
        else if (start == false)
        {
            moveAccel = moveAccelstart;
            startPanel.SetActive(false);
            btnpause.SetActive(true);

        }
    }

    private void FixedUpdate()
    {
        // raycast ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if (pause==false)
        {
            if (hit)
            {
                if (!isOnGround && rig.velocity.y <= 0 && start == false)
                {
                    isOnGround = true;
                }
            }
            else
            {
                isOnGround = false;
            }
        }
        // calculate velocity vector
        Vector2 velocityVector = rig.velocity;

        if (isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);
        rig.velocity = velocityVector;
    }
    // start controll
    private void GameStart() 
    {
        if (start==true)
        {
            //agar tidak bergerak
            moveAccel = 0f;
            isOnGround=false;
            btnpause.SetActive(false);
        }
    }
    // pause controll
    public void gamePause() 
    {
        //agar tidak bergerak
        moveAccel = 0f;
        maxSpeed = 0;

        isOnGround = false;
        pause = true;

        panelpause.SetActive(true);
    }
    //resume controll
    public void gameResume() 
    {
        //agar tidak bergerak
        moveAccel = moveAccelstart;
        maxSpeed = maxSpeedStart;
        btnpause.SetActive(true);
        pause = false;
        //menghilangkan panel pause
        panelpause.SetActive(false);
    }

    private void GameOver()
    {
        // set high score
        score.FinishScoring();

        // stop camera movement
        gameCamera.enabled = false;

        // show gameover
        gameOverScreen.SetActive(true);

        // disable this too
        this.enabled = false;
       
    }

    private void OnDrawGizmos()
    {
       Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
