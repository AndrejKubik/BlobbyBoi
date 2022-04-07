using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorToRed : MonoBehaviour
{
    //start and end color values
    private Color startColor;
    public Color endColor;
    private Color playerColor;

    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SetDifficulty setDifficulty;

    //change time calculation variables
    public float changeTime;
    private float stepTime;
    private float runTime;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        setDifficulty = FindObjectOfType<SetDifficulty>();

        //using the "MoveLeft" script
        spawnManager = FindObjectOfType<SpawnManager>();

        //getting the starting player color values
        startColor = gameObject.GetComponentInChildren<Renderer>().material.color;

        //giving the increment variable the value of 0 at start 
        runTime = Time.deltaTime;
    }

    void Update()
    {
        if(!playerController.gameOver && !setDifficulty.isTutorial)
        {
            //calculating recolor duration
            changeTime = (spawnManager.maxMoveSpeed - spawnManager.startSpeed) / spawnManager.diffChange;

            //turning recolor duration into 0-1 value for lerp function
            stepTime = 1 / changeTime;

            //changing player color value over time
            playerColor = Color.Lerp(startColor, endColor, runTime);

            //increasing the increment value over time
            runTime += stepTime * Time.deltaTime;
        }

        //applying the color value to change the player color
        gameObject.GetComponentInChildren<Renderer>().material.color = playerColor;
    }
}
