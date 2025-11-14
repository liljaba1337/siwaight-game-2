using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float movementspeed = 5f;
    [SerializeField] float jumpheight = 6;
    [SerializeField] Transform groundcheck;
    [SerializeField] LayerMask ground;

    [SerializeField] AudioSource jump1;
    [SerializeField] AudioSource jump2;
    [SerializeField] AudioSource jump3;
    [SerializeField] AudioSource jump4;
    [SerializeField] AudioSource jump5;
    [SerializeField] AudioSource jump6;
    [SerializeField] AudioSource defaultmusic;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //defaultmusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //float horizontalinp = Input.GetAxis("Horizontal");
        //float verticalinp = Input.GetAxis("Vertical");

        //rb.velocity = new Vector3(horizontalinp * movementspeed, rb.velocity.y, verticalinp * movementspeed);

        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * Time.deltaTime * movementspeed;
        }
        if (Input.GetKey("s"))
        {
            transform.position += (-transform.forward) * Time.deltaTime * movementspeed;
        }
        if (Input.GetKey("a"))
        {
            transform.position += (-transform.right) * Time.deltaTime * movementspeed;
        }
        if (Input.GetKey("d"))
        {
            transform.position += (transform.right) * Time.deltaTime * movementspeed;
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Screen.fullScreen = !Screen.fullScreen;
            if(!Screen.fullScreen) Screen.SetResolution(800, 600, false);
        }


        if (Input.GetKeyDown("space") && isgrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpheight, rb.velocity.z);
            System.Random rnd = new System.Random();
            int num = rnd.Next(5);
            AudioSource[] sounds = {jump1,jump2,jump3,jump4,jump5,jump6};
            sounds[num].Play();
        }
    }

    bool isgrounded()
    {
        return Physics.CheckSphere(groundcheck.position, 0.5f, ground);
    }
}
