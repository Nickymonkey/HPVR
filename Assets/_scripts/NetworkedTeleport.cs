using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class NetworkedTeleport : Teleport
    {
        public TeleportArc ta;

        //-------------------------------------------------
        void OnEnable()
        {
            teleportMarkers = GameObject.FindObjectsOfType<TeleportMarkerBase>();

            HidePointer();
            ta.enabled = true;
            if (Launcher.LocalPlayerInstance != null) {
                player = Launcher.LocalPlayerInstance.GetComponent<Player_VR>();
            }

            if(GameManager.LocalPlayerInstance != null)
            {
                player = GameManager.LocalPlayerInstance.GetComponent<Player_VR>();
            }
            //ta.enabled = true;
            if (player == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> Teleport: No Player instance found in map.", this);
                //Destroy( this.gameObject );
                return;
            }

            //ta.enabled = true;
            CheckForSpawnPoint();

            Invoke("ShowTeleportHint", 5.0f);

            chaperoneInfoInitializedAction.enabled = true;
            OnChaperoneInfoInitialized(); // In case it's already initialized
        }

        private void OnDisable()
        {
            ta.enabled = false;
        }

    }
}

