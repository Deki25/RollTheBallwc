﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    PlayerMovement playerMove;
    Joystick joystick;
    startTrigger startTr;

    public GameObject Player;
    public GameObject PreviewText;
    public GameObject FinishMenu;
    public GameObject Joystick;
    public GameObject SkipPreviewObject;
    public GameObject SkipDeadObject;
    public GameObject TimerText;
    public GameObject startTrigg;
    public GameObject Coin;
    public GameObject[] Coins;

    public Image fadeImg;

    public AnimationCurve fadeCurve;

    private int thisLevel;

    private float lerpTimer;
    private float lerpExecute = 7f;
    public float finishLerp = 0;
    private float finishLerpExecute = 1f;
    private float deathTimer = 0f;
    private float deathExecute = 3f;
    private float startLerpTimer = 0;
    private float startLerpExecute = 1.4f;
    private float timer = 1.5f;

    public bool previewCamera;
    public bool isSpawning;
    public bool isPlaying;
    public bool skipPreview;
    public bool skipDead;

    public string currentLevel;

    private Vector3[] previewEndPosition;
    private Vector3[] deadCameraPosition;
    private Vector3[] finishCameraPosition;

    private Vector3 offset;

    //Level 1 Lerp positions
    private Vector3 startPositionLevelOne;
    private Vector3 endPositionLevelOne;

    //Spawn Lerp
    private Vector3 startOffest;
    private Vector3 fovCamera;

    //Finish Lerp
    private Vector3 finishOffset;
    private Vector3 finishSave;
    
    private void Start()
    {
        Coins = new GameObject[Coin.transform.childCount];

        for (int i = 0; i < Coin.transform.childCount; i++)
        {
            Coins[i] = Coin.transform.GetChild(i).gameObject;
        }

        playerMove = Player.GetComponent<PlayerMovement>();
        joystick = Joystick.GetComponent<Joystick>();
        startTr = startTrigg.GetComponent<startTrigger>();

        thisLevel = int.Parse(currentLevel);
        previewCamera = true; 
    }

    private void previewCameraMovement()
    {
		PreviewText.GetComponent<Text>().text = "Level " + currentLevel;
        PreviewText.SetActive(true);
        SkipPreviewObject.SetActive(true);

        startPositionLevelOne = new Vector3(0, 20f, 0f);
        transform.position = startPositionLevelOne;
        transform.rotation = Quaternion.Euler(90f, 0, 0);

        #region Preview Pozicije Kamere
        previewEndPosition = new Vector3[3];

        previewEndPosition[0] = new Vector3(0, 20f, 38f);
        previewEndPosition[1] = new Vector3(0, 20f, 50f);
        // previewEndPosition[2] = new Vector3(0, 20f, 38f);
        #endregion

        endPositionLevelOne = previewEndPosition[thisLevel - 1];

        lerpTimer += Time.deltaTime / lerpExecute;

        if (lerpTimer < lerpExecute)
            transform.position = Vector3.Lerp(startPositionLevelOne, endPositionLevelOne, lerpTimer);


        if (transform.position == endPositionLevelOne || Input.GetKey(KeyCode.Space) || skipPreview)
        {
            previewCamera = false;
            skipPreview = false;
            isSpawning = true;
            SkipPreviewObject.SetActive(false);
            PreviewText.SetActive(false);
            lerpTimer = 0f;
        }
    }

    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.fixedDeltaTime / 2f;
            float c = fadeCurve.Evaluate(t);
            fadeImg.color = new Color(0f, 0f, 0f, c);
            yield return 0;
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.fixedDeltaTime / 0.6f;
            float c = fadeCurve.Evaluate(t);
            fadeImg.color = new Color(0f, 0f, 0f, c);
            yield return 0;
        }
    }

    private void CameraDeadMovement()
    {
        CoinMenager.money = 0;
        startTr.timerIsOn = false;
        startTr.timer = 0;
        TimerText.SetActive(false);
        
        SkipDeadObject.SetActive(true);

        deathTimer += Time.deltaTime;

        if (deathTimer < deathExecute)
        {
            if (skipDead == true)
                deathTimer = deathExecute;
            isPlaying = false;
            Joystick.SetActive(false);

            #region Dead Pozicije Kamere   
            deadCameraPosition = new Vector3[3];

            deadCameraPosition[0] = new Vector3(-15f, 12f, 2.5f);
            deadCameraPosition[1] = new Vector3(-15f, 12f, 10f);
            //deadCameraPosition[2] = new Vector3(-15f, 12f, 2.5f);
            #endregion

            transform.position = deadCameraPosition[thisLevel - 1];

            timer -= Time.deltaTime;

            if (timer > 0)
				transform.LookAt(Player.transform);
            else
            {
                StartCoroutine(FadeOut());
				transform.LookAt(Player.transform);
            }
        }
        else
        {
            SkipDeadObject.SetActive(false);
            skipDead = false;
			playerMove.isDead = false;

            foreach (GameObject coin in Coins)
                coin.SetActive(true);

            isSpawning = true;
			Player.transform.position = playerMove.startPosition;
            CameraAliveMovement();
            deathTimer = 0f;
            timer = 2f;
        }
    }

    public void CameraAliveMovement()
    {
        transform.position = fovCamera;
        offset = transform.position - Player.transform.position;
    }

    public void Spawn()
    {
        TimerText.GetComponent<Text>().text = "0:0.0";

        startOffest = new Vector3(0, 3.5f, -3f);
        transform.position = startOffest;
        transform.rotation = Quaternion.Euler(15f, 0, 0);
        fovCamera = new Vector3(0, 2.5f, -3f);

        StartCoroutine(FadeIn());

        startLerpTimer += Time.deltaTime / startLerpExecute;

        if (startLerpTimer < startLerpExecute)
            transform.position = Vector3.Lerp(startOffest, fovCamera, startLerpTimer);
        else
        {
            isPlaying = true;
            CameraAliveMovement();
            startLerpTimer = 0f;
            isSpawning = false;
            if (GameManager.Instance.useJoystick == 1)
            {
                Joystick.SetActive(true);
            }
            TimerText.SetActive(true);
        }
    }

    private void CameraFinishMovement()
    {
        isPlaying = false;

		finishSave = new Vector3(8f, 2.5f, 34f);

        #region Finish Pozicije Kamere
        finishCameraPosition = new Vector3[3];

        finishCameraPosition[0] = new Vector3(8f, 3f, 31f);
        finishCameraPosition[1] = new Vector3(6.5f, 3f, 41f);
        //finishCameraPosition[2] = new Vector3(6f, 3f, 31f);
        #endregion

        finishOffset = finishCameraPosition[thisLevel - 1];

        finishLerp += Time.deltaTime / 1.5f;

        if (finishLerp < finishLerpExecute)
        {
            transform.position = Vector3.Lerp(finishSave, finishOffset, finishLerp);
            Joystick.SetActive(false);
        }
        else
        {
            FinishMenu.SetActive(true);
        }
    }

    public void SkipPreview()
    {
        skipPreview = true;
    }

    public void SkipDead()
    {
        skipDead = true;
    }

    private void Update()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (previewCamera == true)
            previewCameraMovement();

		if (playerMove.isFinished == true)
        {
            CameraFinishMovement();
            joystick.OnDead();
        }
		if (playerMove.isDead == true)
        {
            CameraDeadMovement();
            joystick.OnDead();
        }

        if (isSpawning == true)
        {
            Spawn();
            Player.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
            Player.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void LateUpdate()
    {
        if (isPlaying == true)
           transform.position = Player.transform.position + offset;
    }
}
