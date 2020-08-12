using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace HPVR
{
    public class SpawnManager : MonoBehaviour
    {
        public GameObject networkedGameManager;

        [SerializeField]
        private List<GameObject> Objects;

        [SerializeField]
        Transform SpawnPointsRoot;

        private List<Transform> SpawnPoints;

        // Start is called before the first frame update
        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnObjects();
                SpawnNetworkedGameManager();
            }

            gameObject.SetActive(false);
            //SpawnObjects();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SpawnObjects()
        {
            List<Transform> SpawnPoints = SpawnPointsRoot.Cast<Transform>().ToList();

            for(int i=0; i<SpawnPoints.Count; i++)
            {
                PhotonNetwork.Instantiate(Objects[i].name, SpawnPoints[i].position, Quaternion.identity, 0);
            }
        }

        public void SpawnNetworkedGameManager()
        {
            if(networkedGameManager != null)
            {
                GameObject networkedGameManagerObject = PhotonNetwork.Instantiate(this.networkedGameManager.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
                //NetworkedGameManager networkedGameManagerScript = networkedGameManagerObject.GetComponent<NetworkedGameManager>();
            }
        }
    }
}
