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
        public CircularDrive cd;
        private bool handDetached = false;

        //public Spellbook sb;
        // Start is called before the first frame update
        void Start()
        {

        }


        void Update()
        {
            if (!cd.driving && (cd.outAngle != cd.maxAngle && cd.outAngle != cd.minAngle))
            {
                if (Mathf.Abs(cd.minAngle - cd.outAngle) < Mathf.Abs(cd.maxAngle - cd.outAngle))
                {
                    cd.outAngle = Mathf.LerpAngle(cd.outAngle, cd.minAngle, Time.time * speed);
                    cd.outAngle = Mathf.Clamp(cd.outAngle, cd.minAngle, cd.maxAngle);
                    if(Mathf.Abs(cd.minAngle - cd.outAngle) <= .5f)
                    {
                        cd.outAngle = cd.minAngle;
                        minAngleReached();
                    }
                } else{
                    cd.outAngle = Mathf.LerpAngle(cd.outAngle, cd.maxAngle, Time.time * speed);
                    cd.outAngle = Mathf.Clamp(cd.outAngle, cd.minAngle, cd.maxAngle);
                    if (Mathf.Abs(cd.maxAngle - cd.outAngle) <= .5f)
                    {
                        cd.outAngle = cd.maxAngle;
                        maxAngleReached();
                    }
                }
                cd.UpdateGameObject();
            }
        }

        public void maxAngleReached()
        {
            if (currentSide == PageSide.Left)
            {
                currentSide = PageSide.Right;
                cd.startAngle = 180f;
                SendMessageUpwards("PageFlipped", false);
            }
        }

        public void minAngleReached()
        {
            if (currentSide == PageSide.Right)
            {
                currentSide = PageSide.Left;
                cd.startAngle = 0f;
                SendMessageUpwards("PageFlipped", true);
            }
        }

        public void lerpPages()
        {
             //cd.outAngle = cd.startAngle;
             transform.localRotation = Quaternion.Euler(cd.startAngle, 0f, 0f);
             cd.outAngle = cd.startAngle;
            //cd.outAngle = cd.startAngle;
        }
    }
}
