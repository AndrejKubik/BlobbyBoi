using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private SpawnManager spawnManager;

    //particle effects
    public ParticleSystem playerTrail;
    public ParticleSystem playerJump;
    public ParticleSystem playerDeath;

    //audio emitter
    public AudioSource playerAudio;

    //player sounds
    public AudioClip jumpSound;
    public AudioClip crashSound;

    //TEXT
    public TextMeshProUGUI gameOverText;

    //buttons
    public Button restartButton;
    public GameObject menuButton;
    public GameObject pausedGame;

    public bool onGround;

    //duck states
    private bool isDucking;
    private bool isGrowing;

    //jump states
    public bool isCharging;
    public bool chargeCanStart;
    public bool isJumping;

    //game states
    public bool gameOver;

    public bool mouseHeld;

    //jump modifiers
    public float jumpHeight = 35f;
    public float jumpBound = 6.0f;
    public float jumpStrength = 40f;

    //duck vectors
    private Vector3 minScaleDown;
    private Vector3 maxScaleDown;
    private Vector3 minScaleDownLerp;
    private Vector3 maxScaleDownLerp;

    private Vector3 startPos;
    private Vector3 jumpForce;

    //grow-back vectors
    private Vector3 minScaleUp;
    private Vector3 startScale;
    private Vector3 minScaleUpLerp;
    private Vector3 startScaleLerp;

    //click pos values
    private float lastClickPosY;

    private Touch touch;

    void Start()
    {
        //set player's starting position
        startPos = transform.position;
        startScale = transform.localScale;
        startScaleLerp = startScale + new Vector3(-0.5f, 0.5f, 0);

        //player downscale bounds
        minScaleDown = new Vector3(5.0f, 1.0f, 2.0f);
        maxScaleDown = new Vector3(2.0f, 4.0f, 2.0f);

        //lerp bounds
        minScaleDownLerp = new Vector3(5.5f, 0.5f, 2.0f);
        maxScaleDownLerp = new Vector3(1.5f, 4.5f, 2.0f);

        minScaleUp = new Vector3(2.5f, 3.0f, 2.0f);
        minScaleUpLerp = new Vector3(3.0f, 2.5f, 2.0f);

        //starting player states
        onGround = true;
        isGrowing = false;
        isDucking = false;
        isCharging = false;
        chargeCanStart = true;
        isJumping = false;

        //make the game inactive at start
        gameOver = true;

        mouseHeld = false;
    }

    void Update()
    {
        //player jump check and Y position correction
        if (transform.position.y < startPos.y)
        {
            transform.position = startPos;
        }
        else if (transform.position.y > jumpBound)
        {
            transform.position = new Vector3(transform.position.x, jumpBound, transform.position.z);
        }

        //mouse input: swerve up for jump and down for duck
        if (Input.GetMouseButtonDown(0))
        {
            lastClickPosY = Input.mousePosition.y;
        }
        else if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.y > lastClickPosY + 50f && !mouseHeld && onGround && !isCharging && chargeCanStart && !isDucking && !isGrowing && !gameOver)
            {
                jumpHeight *= 7f;
                mouseHeld = true;
                isCharging = true;
                chargeCanStart = false;
            }
            else if (Input.mousePosition.y < lastClickPosY - 50f && !mouseHeld && onGround && !isDucking && !isGrowing && !isCharging && !gameOver)
            {
                mouseHeld = true;
                isDucking = true;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            mouseHeld = false;
        }

        //touch input control
        /*if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }

        if(touch.phase == TouchPhase.Began)
        {
            lastClickPosY = touch.position.y;
        }
        else if(touch.phase == TouchPhase.Moved)
        {
            if (touch.position.y < lastClickPosY - 50f && !mouseHeld && onGround && !isDucking && !isGrowing && !isCharging && !gameOver)
            {
                isDucking = true;
                mouseHeld = true;
            }
            else if (touch.position.y > lastClickPosY + 50f && !mouseHeld && onGround && !isCharging && chargeCanStart && !isDucking && !isGrowing && !gameOver)
            {
                isCharging = true;
                chargeCanStart = false;
                mouseHeld = true;
            }
        }
        else if(touch.phase == TouchPhase.Ended)
        {
            mouseHeld = false;
        }*/

        //up arrow input for jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && onGround && !isDucking && !isGrowing && !gameOver)
        {
            jumpHeight *= 7f;
            mouseHeld = true;
            isCharging = true;
            chargeCanStart = false;
        }

        //down arrow input for duck
        if (Input.GetKeyDown(KeyCode.DownArrow) && onGround && !isGrowing && !gameOver)
        {
            isDucking = true;
        }

    }

    private void FixedUpdate()
    {
        if (isDucking && !gameOver)
        {
            StartCoroutine(DownScale());
        }
        if (isGrowing && !gameOver)
        {
            UpScale();
        }

        if (isCharging && onGround && !isDucking && !isGrowing && !chargeCanStart)
        {
            StartCoroutine(JumpCharge());
        }
        else if (isJumping && onGround && !isDucking && !isGrowing)
        {
            StartCoroutine(JumpExec());
        }

        //if player object reaches half allowed hight, push it back down with force
        if (transform.position.y >= jumpBound / 2)
        {
            playerRb.AddForce(Vector3.down * jumpHeight * Time.deltaTime * jumpStrength, ForceMode.Impulse);
        }
    }
    //downscaling player and after a short period changing player states
    IEnumerator DownScale()
    {
        //object scale vector changes gradually to minScaleLerp
        transform.localScale = Vector3.Lerp(transform.localScale, minScaleDownLerp, 7 * Time.deltaTime);
        //limiting object scale change to minScaleDown vector
        if (transform.localScale.x > minScaleDown.x && transform.localScale.y < minScaleDown.y)
        {
            transform.localScale = minScaleDown;
        }

        //after a brief period after reaching minimum scale, end the ducking phase, start growing phase
        yield return new WaitUntil(() => transform.localScale.x >= minScaleDown.x && transform.localScale.y <= minScaleDown.y);
        yield return new WaitForSeconds(0.4f);
        isDucking = false;
        isGrowing = true;

    }
    //upscaling player and ending growing phase right after
    void UpScale()
    {
        //gradually upscale object to maxScaleLerp
        transform.localScale = Vector3.Lerp(transform.localScale, maxScaleDownLerp, 7 * Time.deltaTime);
        //limiting object scale change to maxScaleDown vector and ending the growing phase
        if (transform.localScale.x < maxScaleDown.x && transform.localScale.y > maxScaleDown.y)
        {
            transform.localScale = maxScaleDown;
            isGrowing = false;
        }
    }
    //jump-charge rescaling
    IEnumerator JumpCharge()
    {
        //gradually rescale player to minScaleUp, end charge phase, start jump phase,play the jump effect and stop trail effect
        transform.localScale = Vector3.Lerp(transform.localScale, minScaleUpLerp, 20 * Time.deltaTime);
        if (transform.localScale.x > minScaleUp.x && transform.localScale.y < minScaleUp.y)
        {
            transform.localScale = minScaleUp;
        }
        yield return new WaitUntil(() => transform.localScale == minScaleUp);
        isCharging = false;
        isJumping = true;
        playerJump.Play();
        playerTrail.Stop();
        playerAudio.PlayOneShot(jumpSound, 0.1f);
    }
    //making the player jump using force right after rescaling back to normal and ending the jump phase after
    IEnumerator JumpExec()
    {
        jumpForce = new Vector3(0, jumpHeight, 0);
        transform.localScale = Vector3.Lerp(transform.localScale, startScaleLerp, 10 * Time.deltaTime);
        if (transform.localScale.x < startScale.x && transform.localScale.y > startScale.y)
        {
            transform.localScale = startScale;
        }
        yield return new WaitUntil(() => transform.localScale == startScale);
        playerRb.AddForce(jumpForce * Time.deltaTime * 40f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f);
        isJumping = false;
        chargeCanStart = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //when player touches the ground, 0nGround = true and restart trail effect
        if (collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            onGround = true;
            playerTrail.Play();
            jumpHeight /= 7f;
        }
        //when player touches an obstacle game is over
        else if (collision.gameObject.CompareTag("Obstacle") && !gameOver)
        {
            gameOver = true;
            playerAudio.PlayOneShot(crashSound, 0.5f);
            gameOverText.gameObject.SetActive(true);
            playerDeath.Play();
            restartButton.gameObject.SetActive(true);
            playerTrail.Stop();
            menuButton.SetActive(false);
        }
    }
    //if player jumps, onGround = false
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Pause()
    {
        pausedGame.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pausedGame.SetActive(false);
        Time.timeScale = 1f;
    }
}


