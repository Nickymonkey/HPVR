using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _spell_LumosScript : MonoBehaviour
{

    public string spellName = "Lumos";
    //public PlayerManager PlayerManagerScript;
    private GameObject wandTip;
    private Component[] children;

    // Use this for initialization
    void Start()
    {
        //PlayerManagerScript = GameObject.Find("[Player_Manager]").GetComponent<PlayerManager>();
        //Destroy(PlayerManagerScript.currSpell);
        children = GetComponentsInChildren<ParticleSystem>();
        StartCoroutine(Lumos());
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Wand(Clone)")
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Lumos()
    {
        //first thing
        wandTip = GameObject.Find("[Wand](Clone)").transform.Find("Tip").gameObject;
        transform.parent = wandTip.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = Resources.Load("lumosSound") as AudioClip;
        audioSource.Play();
        yield
        return new WaitWhile(() => audioSource.isPlaying);
        Debug.Log("Sound played");
        ParticleSystem.EmissionModule childEmission = gameObject.transform.Find("center").GetComponent<ParticleSystem>().emission;
        childEmission.enabled = true;
        ParticleSystem.EmissionModule thisEmission = gameObject.GetComponent<ParticleSystem>().emission;
        thisEmission.enabled = true;
        gameObject.GetComponentInChildren<Light>().enabled = true;
        Debug.Log("Sound played");
        //PlayerManagerScript.currSpell = gameObject;
    }

}