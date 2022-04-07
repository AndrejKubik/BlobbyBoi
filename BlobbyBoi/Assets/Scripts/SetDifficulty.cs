using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetDifficulty : MonoBehaviour
{
    [SerializeField]private PlayerController playerController;
    [SerializeField]private SpawnManager spawnManager;
    public GameObject titleScreen;
    public GameObject menuButton;
    public TextMeshProUGUI speed;
    public bool isTutorial;
    void Start()
    {
        speed.gameObject.SetActive(false);

        isTutorial = false;
    }
    private void Update()
    {
        speed.text = "SPEED: " + Mathf.RoundToInt(spawnManager.moveSpeed - 7f);
    }

    //check which difficulty button was pressed, change difficulty value and starting speed accordingly
    public void StartTutorial()
    {
        spawnManager.startSpeed = 10.0f;
        spawnManager.maxMoveSpeed = 10.0f;
        spawnManager.moveSpeed = spawnManager.startSpeed;
        playerController.gameOver = false;
        titleScreen.SetActive(false);
        menuButton.SetActive(true);
        isTutorial = true;
        Time.timeScale = 1f;
        playerController.playerTrail.Play();
    }

    public void SetDiffEasy()
    {
        spawnManager.startSpeed = 10.0f;
        spawnManager.maxMoveSpeed = 20.0f;
        spawnManager.moveSpeed = spawnManager.startSpeed;
        playerController.gameOver = false;
        titleScreen.SetActive(false);
        menuButton.SetActive(true);
        speed.gameObject.SetActive(true);
        Time.timeScale = 1f;
        playerController.playerTrail.Play();
    }

    public void SetDiffNormal()
    {
        spawnManager.startSpeed = 12.0f;
        spawnManager.maxMoveSpeed = 25.0f;
        spawnManager.moveSpeed = spawnManager.startSpeed;
        playerController.gameOver = false;
        titleScreen.SetActive(false);
        menuButton.SetActive(true);
        speed.gameObject.SetActive(true);
        Time.timeScale = 1f;
        playerController.playerTrail.Play();
    }

    public void SetDiffHard()
    {
        spawnManager.startSpeed = 14.0f;
        spawnManager.maxMoveSpeed = 30.0f;
        spawnManager.moveSpeed = spawnManager.startSpeed;
        playerController.gameOver = false;
        titleScreen.SetActive(false);
        menuButton.SetActive(true);
        speed.gameObject.SetActive(true);
        Time.timeScale = 1f;
        playerController.playerTrail.Play();
    }
}
