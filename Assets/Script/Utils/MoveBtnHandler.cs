using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.Utils
{
    public enum BtnType
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public class MoveBtnHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Transform user;
        public BtnType type;
        private float _fMoveSpeed = 8;
        private bool _bMove;
        public static int prevStep;

        private Vector3 vMoveDir;
        private Animator anima;
        private void Start()
        {
            anima = user.gameObject.GetComponent<Animator>();
            switch (type)
            {
                case BtnType.UP:
                    vMoveDir = Vector3.up;
                    break;
                case BtnType.DOWN:
                    vMoveDir = Vector3.down;
                    break;
                case BtnType.LEFT:
                    vMoveDir = Vector3.left;
                    break;
                case BtnType.RIGHT:
                    vMoveDir = Vector3.right;
                    break;
            }
        }

        private void Update()
        {
            if (_bMove)
            {
                user.Translate(vMoveDir * _fMoveSpeed * Time.deltaTime);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _bMove = true;
            switch (type)
            {
                case BtnType.UP:
                    anima.Play("Up");
                    prevStep = 0;
                    break;
                case BtnType.DOWN:
                    anima.Play("Down");
                    prevStep = 1;
                    break;
                case BtnType.LEFT:
                    anima.Play("Left");
                    prevStep = 2;
                    break;
                case BtnType.RIGHT:
                    anima.Play("Right");
                    prevStep = 3;
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _bMove = false;
            switch (prevStep)
            {
                case 0:
                    anima.Play("UpStill");
                    break;
                case 1:
                    anima.Play("DownStill");
                    break;
                case 2:
                    anima.Play("LeftStill");
                    break;
                case 3:
                    anima.Play("RightStill");
                    break;
            }
            
        }
    }
}