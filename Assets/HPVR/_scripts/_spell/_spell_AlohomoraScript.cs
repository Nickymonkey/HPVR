using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class _spell_AlohomoraScript : MonoBehaviour
    {
        public string spellName = "_spell_AlohomoraScript";

        void Start()
        {
            if (GetComponent<NetworkedObject>())
            {
                GetComponent<NetworkedObject>().requestThenTransfer();
                GetComponent<NetworkedObject>().currentSpell = spellName;
            }

            StartCoroutine(unlock());
        }

        IEnumerator unlock()
        {
            if(GetComponent<CircularDrive>() != null)
            {
                GetComponent<CircularDrive>().rotateGameObject = true;

                if (GetComponent<AudioSource>() != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(Resources.Load("alohomoraSound") as AudioClip);
                }
            }
            yield return new WaitForSeconds(2.0f);
            Destroy(this);
        }
    }
}
