using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class NetworkedCoOpGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static NetworkedCoOpGameManager Instance;
    public List<NumberDail> dailList;
    public List<Portcullis> portcullisList;
    public List<Indicator> indicatorList;
    public List<CircularDrive> leverList;

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
        transforms.Clear();
        transforms = GameObject.Find("[Indicator]").transform.Cast<Transform>().ToList();
        foreach (Transform t in transforms)
        {
            indicatorList.Add(t.gameObject.GetComponent<Indicator>());
        }
        transforms.Clear();
        transforms = GameObject.Find("[Lever]").transform.Cast<Transform>().ToList();
        foreach (Transform t in transforms)
        {
            leverList.Add(t.gameObject.GetComponent<CircularDrive>());
        }
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

            foreach (Indicator indicator in indicatorList)
            {
                stream.SendNext(indicator.isOn);
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

            foreach (Indicator indicator in indicatorList)
            {
                indicator.isOn = (bool)stream.ReceiveNext();
                indicator.checkSwitch();
            }
            // Network player, receive data
            //playerHealth = (int)stream.ReceiveNext();
        }
    }
}
