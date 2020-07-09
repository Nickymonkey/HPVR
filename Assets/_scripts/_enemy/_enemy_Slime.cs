using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class _enemy_Slime : MonoBehaviour
    {
        public GameObject player;
        public int power;
        public int health = 3;
        public int splits = 2;
        private Renderer renderer;
        private Material originalMaterial;
        private Material selectedMaterial;
        private bool selected;
        private Texture originalTexture;
        private Color originalColor;
        private string slimeExplosionString = "EnemyExplosion";

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (player == null)
            {
                player = Launcher.LocalPlayerInstance;
                StartCoroutine(jumpTowards());
            }
        }


        private IEnumerator jumpTowards()
        {
            while (true)
            {
                yield
                return new WaitForSeconds(2.0f);
                //Debug.Log("JUMP!");
                transform.LookAt(player.transform);
                Vector3 ray = Vector3.Normalize(player.transform.position - transform.position);
                GetComponent<Rigidbody>().AddForce(ray.x * power / 2, GetComponent<Rigidbody>().mass * power, ray.z * power / 2);
            }
        }

        public void TookDamage(int damageAmount)
        {
            health -= damageAmount;
            StartCoroutine(hitEffect());
            if (health <= 0)
            {
                split();
                Destroy(this.gameObject);
            }
        }

        IEnumerator hitEffect()
        {
            renderer.material.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            renderer.material.color = originalColor;
        }

        public void Awake()
        {
            renderer = GetComponentInChildren<Renderer>();
            originalColor = renderer.material.color;
            originalMaterial = renderer.material;
        }

        public void split()
        {
            Instantiate(Resources.Load(slimeExplosionString), transform.position, transform.rotation);
            if (splits > 0)
            {
                splits--;
                GameObject slimeOne = (GameObject)Instantiate(Resources.Load("_enemy_slime"), transform.position, transform.rotation);
                GameObject slimeTwo = (GameObject)Instantiate(Resources.Load("_enemy_slime"), transform.position, transform.rotation);
                slimeOne.transform.localScale = (gameObject.transform.localScale / 2);
                slimeTwo.transform.localScale = (gameObject.transform.localScale / 2);
                slimeOne.GetComponent<_enemy_Slime>().splits = splits;
                slimeTwo.GetComponent<_enemy_Slime>().splits = splits;
                slimeOne.GetComponent<_enemy_Slime>().health = splits + 1;
                slimeTwo.GetComponent<_enemy_Slime>().health = splits + 1;
            }
        }
    }

}