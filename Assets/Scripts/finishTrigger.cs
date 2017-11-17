﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class finishTrigger : MonoBehaviour 
{
    startTrigger startTr;
    public GameObject Player;
    public GameObject startTrigg;
    public GameObject TimerText;
    public bool finished;

    private void OnTriggerEnter(Collider coli)
    {
        if (coli.tag == "Player")
        {
            startTr.timerIsOn = false;
            TimerText.SetActive(false);
            finished = true;

            GameManager.Instance.coins += CoinMenager.money;
            GameManager.Instance.Save();
        }
    }

    private void Start()
    {
        startTr = startTrigg.GetComponent<startTrigger>();
    }

    private void Update()
    {
        if (finished == true)
        {
            Player.GetComponent<Rigidbody>().isKinematic = true;
            Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Player.GetComponent<PlayerMovement>().isFinished = true;
        }
    }
}
