using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.SceneManagement;
using Collision = UnityEngine.Collision;

public class UserControl : MonoBehaviour
{
    private float speed = 8.0f;
    public Animator anima; // Animator 组件
    private int direction = 0; // 0-右边，1-左边，2-上边 3-下边
    private static readonly int Left = Animator.StringToHash("left");
    private static readonly int Right = Animator.StringToHash("right");
    private static readonly int Up = Animator.StringToHash("up");
    private static readonly int Down = Animator.StringToHash("down");
    private static readonly int Speed = Animator.StringToHash("speed");

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        // 人物移动
        float moveY = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(moveX, moveY, 0) * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.D))
        {
            direction = 0;
            anima.Play("Right");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = 1;
            anima.Play("Left");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = 2;
            anima.Play("Up");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = 3;
            anima.Play("Down");
        }

        if (!Input.anyKey)
        {
            switch (direction)
            {
                case 0:
                {
                    anima.Play("RightStill");
                    break;
                }
                case 1:
                {
                    anima.Play("LeftStill");
                    break;
                }
                case 2:
                {
                    anima.Play("UpStill");
                    break;
                }
                case 3:
                {
                    anima.Play("DownStill");
                    break;
                }
            }
        }
    }
    
}