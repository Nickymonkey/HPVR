using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class Launcher : MonoBehaviour
    {
        public static GameObject LocalPlayerInstance;
        public static Launcher Instance;
        public Transform initialSpawnPoint;
        public Transform returnSpawnPoint;
        //public static bool isPlayerVR = false;
        //public NetworkManager NetworkManager;
        public GameObject Player;
        public GameObject Player_VR;
        public bool isPlayerVR;
        private GameObject prefabToInstantiate;
        private GameObject objToSpawn;
        public NetworkedTeleport nt;
        public string level = "";
        public string lobby = "";
        public void Awake()
        {
            Instance = this;

            //set gamestate
            GameState.Instance.isPlayerVR = isPlayerVR;
            GameState.Instance.VRPrefab = Player_VR;
            GameState.Instance.NormalPrefab = Player;
            GameState.Instance.locomotion = "Teleport";

            prefabToInstantiate = GameState.Instance.PrefabToInstantiate();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (LocalPlayerInstance == null)
            {
                LocalPlayerInstance = GameObject.Instantiate(prefabToInstantiate, initialSpawnPoint.position, Quaternion.identity);
            }

            if (GameState.Instance.locomotion == "Teleport" && GameState.Instance.isPlayerVR)
            {
                nt.gameObject.SetActive(true);
                nt.enabled = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                Destroy(Launcher.LocalPlayerInstance);
                GameState.Instance.lobbyToLoad = lobby;
                GameState.Instance.levelToLoad = level;
                NetworkManager.Instance.Connect();
            }

            //if (Input.GetKeyDown("tab"))
            //{
            //    Debug.Log(GameState.Instance.locomotion);
            //    if(GameState.Instance.locomotion == "Teleport")
            //    {
            //        GameState.Instance.locomotion = "Smooth";
            //        nt.enabled = false;
            //    }
            //    else if(GameState.Instance.locomotion == "Smooth")
            //    {
            //        GameState.Instance.locomotion = "Teleport";
            //        nt.enabled = true;
            //    }
            //}
        }
    }
}
