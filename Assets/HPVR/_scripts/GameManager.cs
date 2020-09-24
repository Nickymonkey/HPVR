using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameObject LocalPlayerInstance;
        public static GameManager Instance;
        [SerializeField]
        Transform SpawnPointsRoot;

        private List<Transform> SpawnPoints;
        private GameObject prefabToInstantiate;
        public NetworkedTeleport nt;
        public TeleportArc ta;

        public void Awake()
        {
            Instance = this;
            prefabToInstantiate = GameState.Instance.PrefabToInstantiate();
            if (PlayerManager.LocalPlayerInstance == null && Player_VR.LocalPlayerInstance == null)
            {
                LocalPlayerInstance = PhotonNetwork.Instantiate(this.prefabToInstantiate.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                if(PlayerManager.LocalPlayerInstance != null)
                {
                    LocalPlayerInstance = PlayerManager.LocalPlayerInstance;
                }
                else
                {
                    LocalPlayerInstance = Player_VR.LocalPlayerInstance;
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            positionPlayer();

            if (GameState.Instance.locomotion == "Teleport" && GameState.Instance.isPlayerVR)
            {
                nt.gameObject.SetActive(true);
                nt.enabled = true;
                ta.enabled = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    PhotonNetwork.Destroy(LocalPlayerInstance);
            //    NetworkManager.Instance.LeaveRoom();
            //}
        }

        public void positionPlayer()
        {
            if (SpawnPointsRoot != null)
            {
                //Debug.Log("positionPlayer()");
                List<Transform> SpawnPoints = SpawnPointsRoot.Cast<Transform>().ToList();
                if (SpawnPoints.Count == 2)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        LocalPlayerInstance.transform.SetPositionAndRotation(SpawnPoints[0].transform.position, SpawnPoints[0].transform.rotation);
                    }
                    else
                    {
                        LocalPlayerInstance.transform.SetPositionAndRotation(SpawnPoints[1].transform.position, SpawnPoints[1].transform.rotation);
                    }
                }
            }
        }
        
        public void updatePlayerPositionOnly()
        {
            if (SpawnPointsRoot != null)
            {
                //Debug.Log("updatePlayerPositionOnly()");
                List<Transform> SpawnPoints = SpawnPointsRoot.Cast<Transform>().ToList();
                if (SpawnPoints.Count == 2)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        LocalPlayerInstance.transform.position = SpawnPoints[0].transform.position;
                    }
                    else
                    {
                        LocalPlayerInstance.transform.position = SpawnPoints[1].transform.position;
                    }
                }
            }
        }

        public void requestAccess()
        {
            if (GameObject.Find("[NetworkedCo-OpGameManager](Clone)"))
            {
                GameObject.Find("[NetworkedCo-OpGameManager](Clone)").GetComponent<PhotonView>().RequestOwnership();
            }
        }
    }
}
