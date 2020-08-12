using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkedCoOpGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static NetworkedCoOpGameManager Instance;
    public List<NumberDail> dailList;
    public List<Portcullis> portcullisList;

    // Start is called before the first frame update
    void Start()
    {
        List<Transform> transforms = GameObject.Find("[Portcullis]").transform.Cast<Transform>().ToList();
        foreach(Transform t in transforms)
        {
            portcullisList.Add(t.gameObject.GetComponent<Portcullis>());
        }
        transforms.Clear();
        transforms = GameObject.Find("[Dail]").transform.Cast<Transform>().ToList();
        foreach (Transform t in transforms)
        {
            dailList.Add(t.gameObject.GetComponentInChildren<NumberDail>());
        }
        //List<Transform> SpawnPoints = SpawnPointsRoot.Cast<Transform>().ToList();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            foreach (Portcullis portcullis in portcullisList)
            {
                stream.SendNext(portcullis.currentNumTriggers);
            }
            //stream.SendNext(opponentHealth);

        }
        else
        {
            foreach (Portcullis portcullis in portcullisList)
            {
                portcullis.currentNumTriggers = (int)stream.ReceiveNext();
                portcullis.CheckTriggers();
            }
            // Network player, receive data
            //playerHealth = (int)stream.ReceiveNext();
        }
    }
}
