using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class _spell_ArrestoMomentumScript : MonoBehaviour
    {
        public string spellName = "_spell_ArrestoMomentumScript";

        void Start()
        {
            if (GetComponent<NetworkedObject>())
            {
                GetComponent<NetworkedObject>().requestThenTransfer();
                GetComponent<NetworkedObject>().currentSpell = spellName;
            }

            StartCoroutine(freeze());
        }

        IEnumerator freeze()
        {
            if (GetComponent<Animator>() != null)
            {
                Destroy(GetComponent<Animator>());
            }
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Rigidbody>().isKinematic = false;
            if (GetComponent<Fan>())
            {
                GetComponent<Fan>().enabled = false;
            }
            yield return new WaitForSeconds(2.0f);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            if (GetComponent<Fan>())
            {
                GetComponent<Fan>().enabled = true;
            }
            Destroy(this);
        }
    }
}