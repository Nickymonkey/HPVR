using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class NetworkedSnapTurn : SnapTurn
    {
        private bool canRotate = true;

        private void Update()
        {
            Player player = Player.instance;

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


                bool leftHandTurnLeft = snapLeftAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
                bool rightHandTurnLeft = snapLeftAction.GetStateDown(SteamVR_Input_Sources.RightHand);

                bool leftHandTurnRight = snapRightAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
                bool rightHandTurnRight = snapRightAction.GetStateDown(SteamVR_Input_Sources.RightHand);

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
    }
}