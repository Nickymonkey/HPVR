using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static GameObject LocalPlayerInstance;
        public CharacterController controller;
        public float speed = 12f;
        public float mouseSensitivity = 100f;

        private float xRotation = 0f;
        private Camera camera;
        private AudioListener audioListener;
        private static string minorSpellString = "MinorBaseSpell";
        public GameObject wandTip;
        public bool shieldSpawned = false;
        private int localShieldCount = 5;
        private int localHealthCount = 3;
        public bool shieldRecharging = false;
        public int wandRechargeTime = 15;
        public GameObject currentShield;

        public void Awake()
        {
            setCamera();
            setAudioListener();
            if (isConnectedOrLocal())
            {
                LocalPlayerInstance = gameObject;
            }
            DontDestroyOnLoad(gameObject);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            if (!shieldRecharging && getShieldCount() < 1)
            {
                StartCoroutine(rechargeShield());
            }
            movementUpdate();
            mouseInput();
        }

        void setCamera()
        {
            if (GetComponentInChildren<Camera>() != null)
            {
                camera = GetComponentInChildren<Camera>();
                camera.enabled = isConnectedOrLocal();
            }
        }

        void setAudioListener()
        {
            if (GetComponentInChildren<AudioListener>() != null)
            {
                audioListener = GetComponentInChildren<AudioListener>();
                audioListener.enabled = isConnectedOrLocal();
            }
        }

        bool isConnectedOrLocal()
        {
            return photonView.IsMine || (!PhotonNetwork.InRoom && !PhotonNetwork.InLobby) ;
        }

        void movementUpdate()
        {
            if (isConnectedOrLocal())
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                camera.gameObject.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                transform.Rotate(Vector3.up * mouseX);
            }

            if (NetworkedGameManager.Instance != null)
            {
                if (NetworkedGameManager.Instance.matchStarted)
                {
                    if (isConnectedOrLocal())
                    {
                        float x = Input.GetAxis("Horizontal");
                        float z = Input.GetAxis("Vertical");

                        Vector3 move = transform.right * x + transform.forward * z;

                        controller.Move(move * speed * Time.deltaTime - new Vector3(0, 9.81f, 0) * Time.deltaTime);
                    }
                }
            }
            else
            {
                if (isConnectedOrLocal())
                {
                    float x = Input.GetAxis("Horizontal");
                    float z = Input.GetAxis("Vertical");

                    Vector3 move = transform.right * x + transform.forward * z;

                    controller.Move(move * speed * Time.deltaTime - new Vector3(0, 9.81f, 0) * Time.deltaTime);
                }
            }
        }

        public void updateSpell(GameObject currentSpell, int healthDamage, int shieldDamage, float knockbackStrength, float knockbackRadius, bool hasExplosion, Color color, string colorName)
        {
            currentSpell.GetComponent<_spell_baseSpellScript>().healthDamage = healthDamage;
            currentSpell.GetComponent<_spell_baseSpellScript>().shieldDamage = shieldDamage;
            currentSpell.GetComponent<_spell_baseSpellScript>().knockbackStrength = knockbackStrength;
            currentSpell.GetComponent<_spell_baseSpellScript>().knockbackRadius = knockbackRadius;
            currentSpell.GetComponent<_spell_baseSpellScript>().hasExplosion = hasExplosion;
            currentSpell.GetComponent<_spell_baseSpellScript>().setColor(color, colorName);
        }

        void mouseInput()
        {
            if (isConnectedOrLocal())
            {
                bool leftMouseClick = Input.GetMouseButtonDown(0);
                bool rightMouseClick = Input.GetMouseButtonDown(1);
                bool rightMouseUnclick = Input.GetMouseButtonUp(1);
                //GameObject baseSpell;
                if (leftMouseClick)
                {
                    if (shieldSpawned)
                    {
                        shieldTrigger();
                    }
                    GameObject baseSpell;
                    if (!PhotonNetwork.InRoom)
                    {
                        baseSpell = (GameObject)Instantiate(Resources.Load(minorSpellString), wandTip.transform.position, wandTip.transform.rotation);
                    }
                    else
                    {
                        baseSpell = PhotonNetwork.Instantiate(minorSpellString, wandTip.transform.position, wandTip.transform.rotation, 0);
                    }
                    baseSpell.GetComponent<Rigidbody>().AddForce(wandTip.transform.forward * 500);
                    baseSpell.AddComponent<_spell_baseSpellScript>();
                    updateSpell(baseSpell, 1, 1, 2f, 2f, true, Color.white, "White");
                    baseSpell.GetComponent<_spell_baseSpellScript>().activate();
                } else if (rightMouseClick && !shieldSpawned)
                {
                    shieldTrigger();
                } else if(rightMouseUnclick && shieldSpawned)
                {
                    shieldTrigger();
                }

            }
        }

        //trigger shield either on or off
        private void shieldTrigger()
        {
            if (!shieldSpawned)
            {
                if (currentShield == null)
                {
                    Vector3 wandTipPosition = wandTip.gameObject.transform.position;
                    Quaternion wandRotation = wandTip.transform.rotation;
                    if (!PhotonNetwork.InRoom)
                    {
                        currentShield = (GameObject)Instantiate(Resources.Load("Shield"), wandTip.transform.position, wandRotation);
                    }
                    else
                    {
                        currentShield = PhotonNetwork.Instantiate("Shield", wandTip.transform.position, wandRotation, 0);
                    }
                    currentShield.transform.parent = wandTip.transform.parent.parent;
                    currentShield.transform.Rotate(new Vector3(90, 0, 0));
                    currentShield.tag = "shield";
                }
                shieldSpawned = true;
            }
            else
            {
                if (currentShield != null)
                {
                    if (!PhotonNetwork.InRoom)
                    {
                        Destroy(currentShield);
                    }
                    else
                    {
                        PhotonNetwork.Destroy(currentShield);
                    }
                }
                shieldSpawned = false;
            }

        }

        private void shieldUpdate()
        {
            if (getShieldCount() < 1)
            {
                if (shieldSpawned)
                {
                    shieldTrigger();
                }
            }
        }

        private IEnumerator rechargeShield()
        {
            shieldRecharging = true;
            yield return new WaitForSeconds(wandRechargeTime);
            shieldRecharging = false;
            if (NetworkedGameManager.Instance != null)
            {
                NetworkedGameManager.Instance.playerShield = 5;
            }
            else
            {
                localShieldCount = 5;
            }
        }

        private int getShieldCount()
        {
            if (NetworkedGameManager.Instance != null)
            {
                return NetworkedGameManager.Instance.playerShield;
            }

            return localShieldCount;
        }
    }
}
