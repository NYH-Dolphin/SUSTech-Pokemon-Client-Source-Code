using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PokemonMove : MonoBehaviour
{
    private float speed = 3.0f;

    private Vector2 originalPos; // 宝可梦的初始位置
    public Animator anima;


    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame

    private float interval = 0.0f;
    private string state = "Move";

    void Update()
    {
        if (interval >= 1f)
        {
            interval = 0.0f;
            if (Random.Range(0f, 1f) < 0.2)
            {
                state = "Stop";
            }
            else
            {
                state = "Move";
            }
        }
        else
        {
            interval += Time.deltaTime;
        }

        switch (state)
        {
            case "Move":
                MoveHorizontally(14f);
                break;
            case "Stop":
                break;
        }
    }

    /// <summary>
    /// 水平移动的宝可梦
    /// </summary>
    /// <param name="distance"></param> 水平移动的范围距离初始位置的距离的绝对值
    private string stateMove = "Left";

    private static readonly int IsLeft = Animator.StringToHash("isLeft");

    void MoveHorizontally(float distance)
    {
        switch (stateMove)
        {
            case "Right":
                if (transform.position.x - originalPos.x >= 0 && transform.position.x - originalPos.x <= distance)
                {
                    anima.SetBool(IsLeft, false);
                    transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed);
                }
                else if (transform.position.x - originalPos.x > distance)
                {
                    transform.position = new Vector3(originalPos.x + distance, originalPos.y);
                    stateMove = "Left";
                }

                break;
            case "Left":
                if (transform.position.x - originalPos.x >= 0 && transform.position.x - originalPos.x <= distance)
                {
                    anima.SetBool(IsLeft, true);
                    transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * speed);
                }
                else if (transform.position.x - originalPos.x <= 0)
                {
                    transform.position = originalPos;
                    stateMove = "Right";
                }

                break;
        }
    }
    
}