using HPVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string lobby = "SteamVR-Launcher";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toHub()
    {
        Destroy(Launcher.LocalPlayerInstance);
        SceneManager.LoadScene(lobby, LoadSceneMode.Single);
    }

    public void reset()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Destroy(Launcher.LocalPlayerInstance);
        SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
    }
}
