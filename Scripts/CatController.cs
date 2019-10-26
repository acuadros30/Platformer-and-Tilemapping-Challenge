﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatController : MonoBehaviour
{

    private Rigidbody2D rd2d;
    private float moveObject;

    public float speed;
    public Text score;
    public Text live;
    public Text win;

    public AudioSource musicSource;
    public AudioClip musicClip;

    private int liveValue = 3;
    private int scoreValue = 0;
    private bool facingRight = true;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        live.text = liveValue.ToString();
        win.text = "";
        anim = GetComponent<Animator>();
        Teleport();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveObject = Input.GetAxis("Horizontal");

        rd2d.velocity = new Vector2(moveObject * speed, rd2d.velocity.y);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetTrigger("Jump");
        }

        if (facingRight == false && moveObject > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveObject < 0)
        {
            Flip();
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            Teleport();
        }

        if (collision.collider.tag == "Enemy")
        {
            liveValue -= 1;
            live.text = liveValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (liveValue == 0)
        {
            win.text = "You lose!";
            Destroy(rd2d);
        }

        if (scoreValue == 8)
        {
            win.text = "You win! Game created by Adam Cuadros";
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }

    void Teleport()
    {
        if (scoreValue == 4)
        {
            transform.position = new Vector2(37.46f, -0.28f);
            liveValue = 3;
            live.text = liveValue.ToString();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }
}
