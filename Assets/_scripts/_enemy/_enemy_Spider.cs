using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
 
namespace HPVR
{
    public class _enemy_Spider : MonoBehaviour
    {
        public Animator anim;
        public GameObject player;
        public Transform target;
        public float movementSpeed;
        public float rotationSpeed = 1.0f;
        public int health = 3;
        public float followingDistance;
        public bool following = false;
        public bool evading = false;
        public bool attacking = false;
        public bool recovering = false;
        public bool dead = false;
        private Renderer renderer;
        private Color originalColor;

        // Start is called before the first frame update
        void Start()
        {
            //player = Launcher.LocalPlayerInstance;
        }

        // Update is called once per frame
        void Update()
        {
            if (!dead)
            {
                if (player == null)
                {
                    player = Launcher.LocalPlayerInstance;
                    target = player.transform;
                }

                if (Vector3.Distance(transform.position, target.transform.position) > followingDistance && !attacking)
                {
                    following = true;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), movementSpeed);
                }
                else if (!recovering)
                {
                    following = false;
                    StartCoroutine(attack(1.0f));
                }

                Vector3 targetDirection = target.position - transform.position;

                // The step size is equal to speed times frame time.
                float singleStep = rotationSpeed * Time.deltaTime;

                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, new Vector3(targetDirection.x, 0f, targetDirection.z), singleStep, 0.0f);

                // Draw a ray pointing at our target in
                Debug.DrawRay(transform.position, newDirection, Color.red);

                // Calculate a rotation a step closer to the target and applies rotation to this object
                transform.rotation = Quaternion.LookRotation(newDirection);
                anim.SetBool("following", following);
                //anim.SetBool("attacking", attacking);
                anim.SetBool("evading", evading);
            }
        }

        public void TookDamage(int damageAmount)
        {
            if (!dead)
            {
                health -= damageAmount;
                StartCoroutine(hitEffect());
                if (health <= 0)
                {
                    StartCoroutine(death());
                }
            }
        }

        public void Awake()
        {
            renderer = GetComponentInChildren<Renderer>();
            originalColor = renderer.material.color;
        }


        private IEnumerator hitEffect()
        {
            renderer.material.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            renderer.material.color = originalColor;
        }

        private IEnumerator death()
        {
            dead = true;
            //renderer.material.color = Color.red;
            anim.SetBool("dead", dead);
            yield return new WaitForSeconds(3.0f);
            Destroy(this.gameObject);
        }

        private IEnumerator attack(float waitTime)
        {
            attacking = true;
            anim.SetBool("attacking", attacking);
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(recover(2.5f));
        }

        private IEnumerator recover(float waitTime)
        {
            recovering = true;
            attacking = false;
            anim.SetBool("attacking", attacking);
            yield return new WaitForSeconds(waitTime);
            recovering = false;
        }
    }
}
