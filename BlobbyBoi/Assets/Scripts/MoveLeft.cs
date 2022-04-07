using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody obstacleRb;

    private PlayerController playerController;
    private SpawnManager spawnManager;
    private SetDifficulty setDifficulty;

    void Start()
    {
        obstacleRb = GetComponent<Rigidbody>();
        playerController = FindObjectOfType<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>();
        setDifficulty = FindObjectOfType<SetDifficulty>();
    }
    void Update()
    {
        //while the player is alive, keep moving objects to the left along the X axis
        if (!playerController.gameOver)
        {
            transform.Translate(Vector3.left * spawnManager.moveSpeed * Time.deltaTime);
        }
        //when the player dies freeze objects from moving left
        else if (playerController.gameOver)
        {
            obstacleRb.constraints = RigidbodyConstraints.FreezePosition;
        }

        if(setDifficulty.isTutorial && transform.position.x < -20f && gameObject.CompareTag("Obstacle"))
        {
            spawnManager.duckText.gameObject.SetActive(false);
            spawnManager.jumpText.gameObject.SetActive(false);
            spawnManager.choiceText.gameObject.SetActive(false);
        }
    }
}
