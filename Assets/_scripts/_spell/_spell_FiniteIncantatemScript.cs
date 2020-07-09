using Mixspace.Lexicon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class _spell_FiniteIncantatemScript : MonoBehaviour
    {
        public string spellName = "_spell_FiniteIncantatemScript";
        private Renderer _renderer;

        void Start()
        {
            _renderer = GetComponentInChildren<Renderer>();

            if (GetComponent<NetworkedObject>() != null)
            {
                GetComponent<NetworkedObject>().requestThenTransfer();
                GetComponent<NetworkedObject>().currentSpell = spellName;
                _renderer.material = GetComponent<NetworkedObject>().defaultMaterial;
                GetComponent<NetworkedObject>().currentMaterial = _renderer.sharedMaterial.name;
            }

            transform.localScale = new Vector3(1, 1, 1);
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().mass = 1;

            if (GetComponentInChildren<FireSource>() != null)
            {
                GetComponentInChildren<FireSource>().isBurning = false;
                GetComponentInChildren<FireSource>().isDisabled = false;
            }

            if (GetComponent<AudioSource>() != null)
            {
                Destroy(GetComponent<AudioSource>());
            }

            if (GetComponent<Animator>() != null)
            {
                Destroy(GetComponent<Animator>());
            }

            Destroy(this);
        }


    }
}