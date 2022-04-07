using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    //obstacle array and index
    [SerializeField] private GameObject[] obstacles;
    private int obstacleIndex;

    //obstacle spawning coordinates
    private Vector3 spawnPointHigh;
    private Vector3 spawnPointMid;
    private Vector3 spawnPointLow;

    //modifier for random spawning of moving obstacles along the Y axis
    private float randomY;

    //bounds for random spawn timer
    [SerializeField]private float spawnMin;
    [SerializeField]private float spawnMax;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private SetDifficulty setDifficulty;

    //current move speed
    public float moveSpeed;

    //move speed bounds
    public float startSpeed;
    public float maxMoveSpeed;

    //move speed increment modifier
    public float diffChange = 0.25f;
    public float difficulty;

    //obstacle spawn rate
    private float spawnRate;

    private bool startSpawning;
    private bool playNani;

    public float timer;

    public GameObject endTimerText;
    public GameObject duckText;
    public GameObject jumpText;
    public GameObject choiceText;
    public GameObject goalText;
    public GameObject tutorialEndText;
    public GameObject tutorialExitButton;

    public TextMeshProUGUI endTimer;
    public TextMeshProUGUI victory;

    public AudioClip nani;


    void Start()
    {
        //getting random spawn hight for moving obstacles
        randomY = Random.Range(MoveUpDown.botBound, MoveUpDown.topBound);

        //getting obstacle spawn coordinates
        spawnPointHigh = new Vector3(40, 5, 0);
        spawnPointMid = new Vector3(40, randomY, 0);
        spawnPointLow = new Vector3(40, 1, 0);

        //spawn timer bound values
        spawnMin = 3.0f;
        spawnMax = 5.0f;

        startSpawning = true;
        playNani = false;
        
        timer = 30;

        endTimerText.SetActive(false);
        endTimer.gameObject.SetActive(false);
    }

    void Update()
    {
        //initiating obstacle spawn sequence
        if (startSpawning && !setDifficulty.isTutorial)
        {
            StartCoroutine(SpawnObstacle());
        }
        else if(startSpawning && setDifficulty.isTutorial)
        {
            StartCoroutine(SpawnObstacleTutorial());
        }
        if (!playerController.gameOver && !setDifficulty.isTutorial)
        {
            //while move speed is below the maximum bound, keep increasing it over time
            if (moveSpeed < maxMoveSpeed)
            {
                moveSpeed += Time.deltaTime * diffChange;
            }

            //in case move speed tries to jump over the allowed, lock it's value
            if (moveSpeed >= maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;

                timer -= Time.deltaTime;

                endTimerText.SetActive(true);
                endTimer.gameObject.SetActive(true);
            }
        }
        if(timer < 0 && !setDifficulty.isTutorial)
        {
            timer = 0;
            playerController.gameOver = true;
            victory.gameObject.SetActive(true);
            playerController.restartButton.gameObject.SetActive(true);
            endTimerText.SetActive(false);
            endTimer.gameObject.SetActive(false);
            setDifficulty.speed.gameObject.SetActive(false);
            playNani = true;
        }

        if(playNani)
        {
            playerController.playerAudio.PlayOneShot(nani, 0.2f);
            playNani = false;
        }
        endTimer.text = "" + Mathf.RoundToInt(timer);
    }

    IEnumerator SpawnObstacle()
    {
        //Spawning obstacles only while player is alive
        if(!playerController.gameOver)
        {
            //geting random spawn timer value
            spawnRate = Random.Range(spawnMin, spawnMax);

            //getting random obstacle from the array and spawning the selected one at selected position
            obstacleIndex = Random.Range(0, obstacles.Length);
            switch (obstacleIndex)
            {
                case 0:
                    Instantiate(obstacles[obstacleIndex], spawnPointHigh, transform.rotation);
                    break;
                case 1:
                    Instantiate(obstacles[obstacleIndex], spawnPointLow, transform.rotation);
                    break;
                case 2:
                    Instantiate(obstacles[obstacleIndex], spawnPointMid, transform.rotation);
                    break;
            }

            //spawning rate increasing every time an obstacle is spawned
            spawnMin += 0.05f;
            spawnMax += 0.05f;

            //waiting for <spawnRate>.f seconds
            startSpawning = false;
            yield return new WaitForSeconds(spawnRate);
            startSpawning = true;
        }
        
    }

    IEnumerator SpawnObstacleTutorial()
    {
        startSpawning = false;
        yield return new WaitForSeconds(5);

        duckText.gameObject.SetActive(true);

        Instantiate(obstacles[0], spawnPointHigh, transform.rotation);
        yield return new WaitForSeconds(10);

        yield return new WaitForSeconds(2);
        jumpText.gameObject.SetActive(true);

        Instantiate(obstacles[1], spawnPointLow, transform.rotation);
        yield return new WaitForSeconds(10);

        yield return new WaitForSeconds(2);
        choiceText.gameObject.SetActive(true);

        Instantiate(obstacles[2], spawnPointMid, transform.rotation);
        yield return new WaitForSeconds(8);
        goalText.gameObject.SetActive(true);
        yield return new WaitForSeconds(10);
        if(!playerController.gameOver)
        {
            goalText.gameObject.SetActive(false);
        }
        playerController.gameOver = true;
        tutorialEndText.gameObject.SetActive(true);
        tutorialExitButton.gameObject.SetActive(true);

    }
}
