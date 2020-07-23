using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class NetworkedTeleport : Teleport
    {
        protected Player player = null;
        public TeleportArc ta;

        void OnEnable()
        {
            ta.enabled = true;

            if (Launcher.LocalPlayerInstance != null) {
                player = Launcher.LocalPlayerInstance.GetComponent<Player_VR>();
            } else if(GameManager.LocalPlayerInstance != null){
                player = GameManager.LocalPlayerInstance.GetComponent<Player_VR>();
            }

            if (player == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> Teleport: No Player instance found in map.", this);
                return;
            }
        }

        private void OnDisable()
        {
            ta.enabled = false;
        }

    }
}

