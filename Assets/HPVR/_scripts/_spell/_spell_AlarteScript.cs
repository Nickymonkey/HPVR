using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class _spell_AlarteScript : MonoBehaviour
    {

        public string spellName = "_spell_AlarteScript";
        public float forceMultiplier = 450f;
  public float torqueMultiplier = 0.5f;

  void Start()
        {

            if (gameObject.GetComponent<NetworkedObject>())
            {
                gameObject.GetComponent<NetworkedObject>().requestThenTransfer();
                gameObject.GetComponent<NetworkedObject>().currentSpell = spellName;
            }

            if (this.transform.parent != null)
            {
                Destroy(this);
            }

            StartCoroutine(shootUp());
        }

        IEnumerator shootUp()
        {
            GetComponent<Rigidbody>().AddForce(0, GetComponent<Rigidbody>().mass * forceMultiplier, 0);
            GetComponent<Rigidbody>().AddTorque(transform.right * torqueMultiplier);
            GetComponent<Rigidbody>().AddTorque(transform.forward * torqueMultiplier);
            GetComponent<Rigidbody>().AddTorque(transform.up * torqueMultiplier);
            yield
            return new WaitForSeconds(1.0f);
            Destroy(this);
        }
    }

}