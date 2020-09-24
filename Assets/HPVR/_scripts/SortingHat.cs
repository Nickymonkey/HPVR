using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SortingHat : MonoBehaviour
{
    public AudioClip slytherin;
    public AudioClip hufflepuff;
    public AudioClip gryffindor;
    public AudioClip ravenclaw;

    private AudioSource source;
    private int house;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        house = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollissionEnter(Collider other)
    //{
    //    if (other.gameObject.name.Contains("Head"))
    //    {
    //        if (!source.isPlaying)
    //        {
    //            switch (house)
    //            {
    //                case 1:
    //                    source.PlayOneShot(slytherin);
    //                    break;
    //                case 2:
    //                    source.PlayOneShot(hufflepuff);
    //                    break;
    //                case 3:
    //                    source.PlayOneShot(gryffindor);
    //                    break;
    //                case 4:
    //                    source.PlayOneShot(ravenclaw);
    //                    break;
    //                default:
    //                    Debug.Log("Default case");
    //                    break;
    //            }
    //        }
    //    }
    //}

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Head"))
        {
            if (!source.isPlaying)
            {
                switch (house)
                {
                    case 1:
                        source.PlayOneShot(slytherin);
                        break;
                    case 2:
                        source.PlayOneShot(hufflepuff);
                        break;
                    case 3:
                        source.PlayOneShot(gryffindor);
                        break;
                    case 4:
                        source.PlayOneShot(ravenclaw);
                        break;
                    default:
                        Debug.Log("Default case");
                        break;
                }
            }
        }
    }
}
