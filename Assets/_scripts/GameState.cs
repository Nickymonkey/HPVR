﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class GameState : Singleton<GameState>
    {
        //public static GameState instance;
        public bool isPlayerVR;
        public string locomotion = "Teleport";
        public GameObject VRPrefab;
        public GameObject NormalPrefab;
        // Start is called before the first frame update

        public GameState(bool isVR)
        {
            isPlayerVR = isVR;
        }

        void Awake()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject PrefabToInstantiate()
        {
            if (isPlayerVR)
            {
                return VRPrefab;
            }
            return NormalPrefab;
        }
        
        public void ToggleLocomotionType()
        {
            if(Launcher.Instance != null)
            {
                if (GameState.Instance.locomotion == "Teleport")
                {
                    GameState.Instance.locomotion = "Smooth";
                    Launcher.Instance.nt.enabled = false;
                }
                else if (GameState.Instance.locomotion == "Smooth")
                {
                    GameState.Instance.locomotion = "Teleport";
                    Launcher.Instance.nt.enabled = true;
                }
            } else if (GameManager.Instance != null)
            {
                if (GameState.Instance.locomotion == "Teleport")
                {
                    GameState.Instance.locomotion = "Smooth";
                    GameManager.Instance.nt.enabled = false;
                }
                else if (GameState.Instance.locomotion == "Smooth")
                {
                    GameState.Instance.locomotion = "Teleport";
                    GameManager.Instance.nt.enabled = true;
                }
            }

        }
    }
}

