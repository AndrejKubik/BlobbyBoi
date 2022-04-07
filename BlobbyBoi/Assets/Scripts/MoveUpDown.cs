using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    //movement bounds along the Y axis
    public static float topBound = 6.0f;
    public static float botBound = 0.5f;

    public float moveSpeed;

    private PlayerController playerController;
    private Rigidbody obstacleRb;

    //move direction states
    public bool isMovingDown;
    public bool isMovingUp;

    //random start direction values
    private int down = 0;
    private int up = 1;
    private int startDirection;
    void Start()
    {
        //using PlayerController script and obstacle Rigidbody
        playerController = FindObjectOfType<PlayerController>();
        obstacleRb = GetComponent<Rigidbody>();

        //getting random direction (+1 because bounds are in int format) and activating a moving state acordingly
        startDirection = Random.Range(down, up + 1);

        if (startDirection == down) isMovingDown = true;
        else if (startDirection == up) isMovingUp = true;
    }
    void Update()
    {
        //allow obstacle movement only while player is alive and freeze their positions when player dies
        if (!playerController.gameOver)
        {
            if (isMovingUp)
            {
                StartCoroutine(MoveUp());
            }
            else if (isMovingDown)
            {
                StartCoroutine(MoveDown());
            }
        }
    }

    IEnumerator MoveDown()
    {
        //move obstacle downwards while it's above the bottom bound
        while(true)
        {
            transform.position += Vector3.down * moveSpeed / 150 * Time.deltaTime;
            if(transform.position.y <= botBound)
            {
                isMovingDown = false;
                isMovingUp = true;
                yield break;
            }
            else if(playerController.gameOver)
            {
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator MoveUp()
    {
        //move obstacle upwards while it's below the top bound
        while(true)
        {
            transform.position += Vector3.up * moveSpeed / 150 * Time.deltaTime;
            if(transform.position.y >= topBound)
            {
                isMovingUp = false;
                isMovingDown = true;
                yield break;
            }
            else if (playerController.gameOver)
            {
                yield break;
            }
            yield return null;
        }
    }
}
