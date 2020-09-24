using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class ButtonSpawn : MonoBehaviour
    {
        public HoverButton hoverButton;

        public GameObject prefab;

        private void Start()
        {
            hoverButton.onButtonDown.AddListener(OnButtonDown);
        }

        private void OnButtonDown(Hand hand)
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            GameObject cube = GameObject.Instantiate<GameObject>(prefab);
            cube.transform.position = this.transform.position;
            yield return null;
        }
    }
}

