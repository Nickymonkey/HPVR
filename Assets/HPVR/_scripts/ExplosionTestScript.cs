using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTestScript : MonoBehaviour
{
    public float power = 100.0f;
    public float radius = 100.0f;
    public float upforce = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.enabled)
        {
            Invoke("Detonate", 5);
        }
    }

    void Detonate()
    {
        Debug.Log("DETOnATED");
        //Vector3 explositionPosition = this.gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, transform.position, radius, upforce, ForceMode.Impulse);
            }
        }
    }
}
