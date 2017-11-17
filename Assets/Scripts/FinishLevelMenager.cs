﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishLevelMenager : MonoBehaviour
{
    PlayerMovement playerMove;
    CameraMovement cameraScript;
    finishTrigger finishTr;
    startTrigger startTr;

    public bool finishCounting = true;

    public float levelMoney = 0;
    [Range(0, 50)]
    public float countMoneySpeed = 0;

    public GameObject Player;
    public GameObject FinishLine;
    public GameObject StartLine;
    public GameObject Coin;
    public GameObject[] Coins;
    
    public Text FinishText;
    public Text CoinText;

    void Start()
    {
       // GameManager.Instance.coins += CoinMenager.money;
        levelMoney = 0;
        Coins = new GameObject[Coin.transform.childCount];
        for (int i = 0; i < Coin.transform.childCount; i++)
        {
            Coins[i] = Coin.transform.GetChild(i).gameObject;
        }

        //for (int i = 0; i < CoinMenager.money; i++)
        //{
        //    levelMoney += Time.deltaTime;
        //    CoinText.text = "Coins: " + levelMoney.ToString("f0");
        //}
        playerMove = Player.GetComponent<PlayerMovement>();
        cameraScript = Camera.main.GetComponent<CameraMovement>();
        finishTr = FinishLine.GetComponent<finishTrigger>();
        startTr = StartLine.GetComponent<startTrigger>();

    }

    public void NextLevel(string nextLevel)
    {
        SceneManager.LoadScene(nextLevel);
        CoinMenager.money = 0;          
    }
	

    public void Retry()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        startTr.timer = 0f;
        FinishText.text = "";
        finishTr.finished = false;
        Player.GetComponent<PlayerMovement>().isFinished = false;
        cameraScript.finishLerp = 0;
        cameraScript.FinishMenu.SetActive(false);

        foreach (GameObject coin in Coins)
            coin.SetActive(true);

        Player.transform.position = playerMove.startPosition;
        cameraScript.isSpawning = true;
        cameraScript.CameraAliveMovement();
        CoinMenager.money = 0;
        levelMoney = 0;
    }

    public void BackToMenu(string backTo)
    {
        SceneManager.LoadScene(backTo);
        CoinMenager.money = 0;
    }

    private void CoinCount()
    {
        if (finishCounting == true)
        {
            if (levelMoney <= CoinMenager.money)
            {
                levelMoney += Time.deltaTime * countMoneySpeed;
                CoinText.text = "Coins: " + levelMoney.ToString("f0");
            }
        }
    }
	void Update () 
	{
        FinishText.text = startTr.TimerText.text;
        CoinCount();
        if (levelMoney == CoinMenager.money)
        {
            finishCounting = false;
        }
    }
}
