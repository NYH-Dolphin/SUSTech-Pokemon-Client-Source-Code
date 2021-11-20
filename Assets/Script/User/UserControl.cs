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
            anima.SetBool(Left, false);
            anima.SetBool(Right, true);
            anima.SetBool(Up, false);
            anima.SetBool(Down, false);
            anima.SetFloat(Speed, speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction = 1;
            anima.SetBool(Left, true);
            anima.SetBool(Right, false);
            anima.SetBool(Up, false);
            anima.SetBool(Down, false);
            anima.SetFloat(Speed, speed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            direction = 2;
            anima.SetBool(Left, false);
            anima.SetBool(Right, false);
            anima.SetBool(Up, true);
            anima.SetBool(Down, false);
            anima.SetFloat(Speed, speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction = 3;
            anima.SetBool(Left, false);
            anima.SetBool(Right, false);
            anima.SetBool(Up, false);
            anima.SetBool(Down, true);
            anima.SetFloat(Speed, speed);
        }

        if (!Input.anyKey)
        {
            switch (direction)
            {
                case 0:
                {
                    anima.SetBool(Left, false);
                    anima.SetBool(Right, true);
                    anima.SetBool(Up, false);
                    anima.SetBool(Down, false);
                    break;
                }
                case 1:
                {
                    anima.SetBool(Left, true);
                    anima.SetBool(Right, false);
                    anima.SetBool(Up, false);
                    anima.SetBool(Down, false);
                    break;
                }
                case 2:
                {
                    anima.SetBool(Left, false);
                    anima.SetBool(Right, false);
                    anima.SetBool(Up, true);
                    anima.SetBool(Down, false);
                    break;
                }
                case 3:
                {
                    anima.SetBool(Left, false);
                    anima.SetBool(Right, false);
                    anima.SetBool(Up, false);
                    anima.SetBool(Down, true);
                    break;
                }
            }

            anima.SetFloat(Speed, 0.0f);
        }
    }
    
}