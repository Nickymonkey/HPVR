using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class NetworkedFindMatchUI : UIElement
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnButtonClick()
        {
            base.OnButtonClick();

            Destroy(Launcher.LocalPlayerInstance);
            NetworkManager.Instance.Connect();
        }
    }
}
