using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class _spell_DuroScript : MonoBehaviour
    {
        public string spellName = "_spell_DuroScript";
        private Renderer _renderer;
        //private Material originalMaterial;
        private Material rockMaterial;

        void Start()
        {
            if (GetComponent<NetworkedObject>())
            {
                GetComponent<NetworkedObject>().requestThenTransfer();
                GetComponent<NetworkedObject>().currentSpell = spellName;
            }

            _renderer = GetComponentInChildren<Renderer>();

            //originalMaterial = _renderer.material;
            //rockMaterial = Resources.Load("Mat_Stone", typeof(Material)) as Material;            
            _renderer.material = Resources.Load("Mat_Stone", typeof(Material)) as Material;
            GetComponent<NetworkedObject>().currentMaterial = _renderer.sharedMaterial;
            //Debug.Log(_renderer.sharedMaterial.name);
            GetComponent<Rigidbody>().mass = 5;

            if (GetComponentInChildren<FireSource>() != null)
            {
                GetComponentInChildren<FireSource>().isBurning = false;
                //GetComponentInChildren<FireSource>().isDisabled = true;
                foreach (Transform child in GetComponentInChildren<FireSource>().gameObject.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            if(GetComponent<Breakable>() != null)
            {
                GetComponent<Breakable>().BreakOnCollision = false;
            }
            Destroy(this);
        }
    }
}