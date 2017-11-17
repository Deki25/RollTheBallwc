﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	Rigidbody rb;

    public GameObject joystick;

    private float speed;
    public float currentSpeed;
    private float maxSpeed = 8f;
   
    public bool isFinished = false;
    public bool isDead = false;

    private float moveHorizontaly;
    private float moveVerticaly;


    [HideInInspector]
    public Vector3 startPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        speed = 200f;

        startPosition = new Vector3(0, 1f, 0);
    }

    private void FixedUpdate()
    {
        if (Camera.main.GetComponent<CameraMovement>().previewCamera || isFinished || Camera.main.GetComponent<CameraMovement>().isSpawning || isDead)
            return;

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //moveHorizontaly = Input.GetAxis("Horizontal");
            //moveVerticaly = Input.GetAxis("Vertical");

            if (GameManager.Instance.useJoystick == 0)
            {
                moveHorizontaly = Input.GetAxis("Horizontal");
                moveVerticaly = Input.GetAxis("Vertical");
            }
            else
            {
                moveHorizontaly = joystick.GetComponent<Joystick>().Horizontal();
                moveVerticaly = joystick.GetComponent<Joystick>().Vertical();
            }

            Vector3 movement = new Vector3(moveHorizontaly, 0, moveVerticaly);

            rb.AddForce (movement * speed * Time.fixedDeltaTime);

            currentSpeed = rb.velocity.magnitude;

            if (rb.velocity.magnitude > maxSpeed)
                rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        else if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            Vector3 movement;

            if (GameManager.Instance.useJoystick == 0)
            {
                movement = new Vector3(Input.acceleration.x, 0f, Input.acceleration.y);
            }
            else
            {
                movement = new Vector3(moveHorizontaly, 0, moveVerticaly);
            }

            moveHorizontaly = joystick.GetComponent<Joystick>().Horizontal();
            moveVerticaly = joystick.GetComponent<Joystick>().Vertical();


            rb.AddForce(movement * speed * Time.fixedDeltaTime);

            currentSpeed = rb.velocity.magnitude;

            if (rb.velocity.magnitude > maxSpeed)
                rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
