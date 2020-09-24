using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class MaxAngleReached : MonoBehaviour
    {
        public Transform page;
        public TMPro.TextMeshProUGUI FrontPageTitleText;
        public TMPro.TextMeshProUGUI FrontPageDescriptionText;
        public TMPro.TextMeshProUGUI BackPageTitleText;
        public TMPro.TextMeshProUGUI BackPageDescriptionText;

        public PageSide currentSide;
        public PageState currentState;
        public Collider pageCollider;
        public Interactable interactable;
        private float speed = 0.0075f;
        public PageDrive pageDrive;
        private bool handDetached = false;

        void Update()
        {
            if (!pageDrive.driving && (pageDrive.outAngle != pageDrive.maxAngle && pageDrive.outAngle != pageDrive.minAngle))
            {
                if (Mathf.Abs(pageDrive.minAngle - pageDrive.outAngle) < Mathf.Abs(pageDrive.maxAngle - pageDrive.outAngle))
                {
                    pageDrive.outAngle = Mathf.LerpAngle(pageDrive.outAngle, pageDrive.minAngle, Time.time * speed);
                    pageDrive.outAngle = Mathf.Clamp(pageDrive.outAngle, pageDrive.minAngle, pageDrive.maxAngle);
                    if(Mathf.Abs(pageDrive.minAngle - pageDrive.outAngle) <= .5f)
                    {
                        pageDrive.outAngle = pageDrive.minAngle;
                        minAngleReached();
                    }
                } else{
                    pageDrive.outAngle = Mathf.LerpAngle(pageDrive.outAngle, pageDrive.maxAngle, Time.time * speed);
                    pageDrive.outAngle = Mathf.Clamp(pageDrive.outAngle, pageDrive.minAngle, pageDrive.maxAngle);
                    if (Mathf.Abs(pageDrive.maxAngle - pageDrive.outAngle) <= .5f)
                    {
                        pageDrive.outAngle = pageDrive.maxAngle;
                        maxAngleReached();
                    }
                }
                pageDrive.UpdateGameObject();
            }
        }

        public void maxAngleReached()
        {
            if (currentSide == PageSide.Left)
            {
                currentSide = PageSide.Right;
                pageDrive.startAngle = 180f;
                SendMessageUpwards("PageFlipped", false);
            }
        }

        public void minAngleReached()
        {
            if (currentSide == PageSide.Right)
            {
                currentSide = PageSide.Left;
                pageDrive.startAngle = 0f;
                SendMessageUpwards("PageFlipped", true);
            }
        }

        public void lerpPages()
        {
             //cd.outAngle = cd.startAngle;
             transform.localRotation = Quaternion.Euler(pageDrive.startAngle, 0f, 0f);
             pageDrive.outAngle = pageDrive.startAngle;
            //cd.outAngle = cd.startAngle;
        }
    }
}
