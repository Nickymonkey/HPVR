using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class NetworkedSnapTurn : SnapTurn
    {
        private void Start()
        {
            AllOff();
        }

        private void Update()
        {
            Player_VR player = Player_VR.LocalPlayerInstance.GetComponent<Player_VR>();

            if (canRotate && snapLeftAction != null && snapRightAction != null && snapLeftAction.activeBinding && snapRightAction.activeBinding)
            {
                //only allow snap turning after a quarter second after the last teleport
                if (Time.time < (teleportLastActiveTime + canTurnEverySeconds))
                    return;

                // only allow snap turning when not holding something

                //bool rightHandValid = player.rightHand.currentAttachedObject == null ||
                //    (player.rightHand.currentAttachedObject != null
                //    && player.rightHand.currentAttachedTeleportManager != null
                //    && player.rightHand.currentAttachedTeleportManager.teleportAllowed);

                //bool leftHandValid = player.leftHand.currentAttachedObject == null ||
                //    (player.leftHand.currentAttachedObject != null
                //    && player.leftHand.currentAttachedTeleportManager != null
                //    && player.leftHand.currentAttachedTeleportManager.teleportAllowed);
                bool rightHandValid = true;
                bool leftHandValid = true;

                bool leftHandTurnLeft = snapLeftAction.GetStateDown(SteamVR_Input_Sources.LeftHand) && leftHandValid;
                bool rightHandTurnLeft = snapLeftAction.GetStateDown(SteamVR_Input_Sources.RightHand) && rightHandValid;

                bool leftHandTurnRight = snapRightAction.GetStateDown(SteamVR_Input_Sources.LeftHand) && leftHandValid;
                bool rightHandTurnRight = snapRightAction.GetStateDown(SteamVR_Input_Sources.RightHand) && rightHandValid;

                if (leftHandTurnLeft || rightHandTurnLeft)
                {
                    RotatePlayer(-snapAngle);
                }
                else if (leftHandTurnRight || rightHandTurnRight)
                {
                    RotatePlayer(snapAngle);
                }
            }
        }

        protected new IEnumerator DoRotatePlayer(float angle)
        {
            Player_VR player = Player_VR.LocalPlayerInstance.GetComponent<Player_VR>();

            canRotate = false;

            snapTurnSource.panStereo = angle / 90;
            snapTurnSource.PlayOneShot(rotateSound);

            if (fadeScreen)
            {
                SteamVR_Fade.Start(Color.clear, 0);

                Color tColor = screenFadeColor;
                tColor = tColor.linear * 0.6f;
                SteamVR_Fade.Start(tColor, fadeTime);
            }

            yield
            return new WaitForSeconds(fadeTime);

            Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
            player.trackingOriginTransform.position -= playerFeetOffset;
            player.transform.Rotate(Vector3.up, angle);
            playerFeetOffset = Quaternion.Euler(0.0f, angle, 0.0f) * playerFeetOffset;
            player.trackingOriginTransform.position += playerFeetOffset;

            GameObject fx = angle > 0 ? rotateRightFX : rotateLeftFX;

            if (showTurnAnimation)
                ShowRotateFX(fx);

            if (fadeScreen)
            {
                SteamVR_Fade.Start(Color.clear, fadeTime);
            }

            float startTime = Time.time;
            float endTime = startTime + canTurnEverySeconds;

            while (Time.time <= endTime)
            {
                yield
                return null;
                UpdateOrientation(fx);
            };

            fx.SetActive(false);
            canRotate = true;
        }

        public new void RotatePlayer(float angle)
        {
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
                AllOff();
            }

            rotateCoroutine = StartCoroutine(DoRotatePlayer(angle));
        }

        protected new void ShowRotateFX(GameObject fx)
        {
            if (fx == null)
                return;

            fx.SetActive(false);

            UpdateOrientation(fx);

            fx.SetActive(true);

            UpdateOrientation(fx);
        }

        protected new void UpdateOrientation(GameObject fx)
        {
            Player_VR player = Player_VR.LocalPlayerInstance.GetComponent<Player_VR>();

            //position fx in front of face
            this.transform.position = player.hmdTransform.position + (player.hmdTransform.forward * distanceFromFace);
            this.transform.rotation = Quaternion.LookRotation(player.hmdTransform.position - this.transform.position, Vector3.up);
            this.transform.Translate(additionalOffset, Space.Self);
            this.transform.rotation = Quaternion.LookRotation(player.hmdTransform.position - this.transform.position, Vector3.up);
        }

    }
}