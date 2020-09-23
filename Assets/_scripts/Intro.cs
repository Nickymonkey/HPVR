using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{

    public UnityEngine.UI.Image logo, fading;
    public float rate = 0.01f;
    public string toLoad;
    public GameObject CameraRig;
    public GameObject SteamVR;
    // Use this for initialization
    void Start()
    {
        logo.gameObject.SetActive(true);
        fading = logo;
        Invoke("StartFade", 7f);
    }
    public IEnumerator FadeOut(UnityEngine.UI.Image sprite)
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - rate);
        yield return null;
        if (sprite.color.a > 0)
        {
            StartCoroutine(FadeOut(sprite));
        }
        else
        {
            if (sprite == logo)
            {
                CameraRig.SetActive(false);
                SteamVR.SetActive(false);
                SceneManager.LoadScene(toLoad);
            }
        }
    }
    void StartFade()
    {
        StartCoroutine(FadeOut(fading));
    }
    // Update is called once per frame
    void Update()
    {

    }
}
