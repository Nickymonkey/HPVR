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
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            yield
            return new WaitForSeconds(2.0f);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Destroy(this);
        }
    }
}