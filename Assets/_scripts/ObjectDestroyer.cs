using HPVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            Destroy(Launcher.LocalPlayerInstance);
            SceneManager.LoadScene("SteamVR-Launcher", LoadSceneMode.Single);
        } else
        {
            Destroy(other.gameObject);
        }
    }
}
