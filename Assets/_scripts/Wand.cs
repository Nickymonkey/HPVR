using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using System.Linq;
using Photon.Pun;
using Valve.VR;
using Mixspace.Lexicon;
using System.Text;
using System;

namespace HPVR
{
    public class Wand : MonoBehaviour
    {
        private Hand hand;
        public GameObject wandTip;
        //public GameObject gameObject;
        public GameObject shield;
        private GameObject pointerSphere;
        public Material pointerMaterial;
        public float pointerSize = 0.01f;
        private string[] m_Keywords;
        public GameObject currentSpell;
        public SteamVR_Action_Boolean shootInput;
        public SteamVR_Action_Boolean blockInput;
        private bool shieldSpawned = false;
        private GameObject currentShield;
        private int localShieldCount = 5;
        private int localHealthCount = 3;
        public int wandRechargeTime = 15;
        public bool shieldRecharging = false;
        public Text shieldCountText;
        public Text healthCountText;
        public GameObject wandCanvas;
        private Vector3 lastPosition;
        public float dwellSpeed = 0.1f;
        public LexiconFocusManager focusManager;
        public LayerMask defaultLayerMask;
        public LayerMask wandReticleIgnoreMask;
        private KeywordRecognizer m_Recognizer;
        private static string majorSpellString = "MajorBaseSpell";
        private static string minorSpellString = "MinorBaseSpell";
        [Tooltip("The time offset used when releasing the object with the RawFromHand option")]
        public float releaseVelocityTimeOffset = -0.011f;
        public ParticleSystem flash;
        public ParticleSystem primedGlow;
        public float scaleReleaseVelocity = 1.1f;

  [Tooltip("The release velocity magnitude representing the end of the scale release velocity curve. (-1 to disable)")]
        public float scaleReleaseVelocityThreshold = -1.0f;
  [Tooltip("Use this curve to ease into the scaled release velocity based on the magnitude of the measured release velocity. This allows greater differentiation between a drop, toss, and throw.")]
        public AnimationCurve scaleReleaseVelocityCurve = AnimationCurve.EaseInOut(0.0f, 0.1f, 1.0f, 1.0f);
        private SpellDictionary sd = null;
        // Start is called before the first frame update
        void Start()
        {
            focusManager = LexiconFocusManager.Instance;
            //wand should only be active for player holding it
            if (!isMineOrLocal())
            {
                this.enabled = false;
                if(GetComponent<Rigidbody>() != null)
                {
                    Destroy(GetComponent<Rigidbody>());
                }
                Destroy(this);
            }

            //enable wand canvas
            wandCanvas.SetActive(true);

            //create wand reticle
            pointerSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointerSphere.GetComponent<Renderer>().material = pointerMaterial;
            pointerSphere.transform.localScale = new Vector3(pointerSize, pointerSize, pointerSize);
            Destroy(pointerSphere.GetComponent<Collider>());
            sd = new SpellDictionary();
            m_Keywords = new string[sd.SpellList.Count];
            for (int i=0; i<sd.SpellList.Count; i++)
            {
                m_Keywords[i] = sd.SpellList[i].Item1.ToLower();
            }
            //intialize speach recognition
            m_Recognizer = new KeywordRecognizer(m_Keywords, ConfidenceLevel.Low);
            m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
            m_Recognizer.Start();
        }

        void OnEnable()
        {
            focusManager = LexiconFocusManager.Instance;
            // Register for the capture focus callback.
            LexiconFocusManager.OnCaptureFocus += CaptureFocus;
        }

        void OnDisable()
        {
            LexiconFocusManager.OnCaptureFocus -= CaptureFocus;
        }

        // Update is called once per frame
        void Update()
        {

            //handle input from wand hand
            wandHandInput();

            shieldUpdate();

            if (!shieldRecharging && getShieldCount() < 1)
            {
                StartCoroutine(rechargeShield());
            }
            shieldCountText.text = getShieldCount() + "";
            healthCountText.text = getHealthCount() + "";
        }

        public void updateSpell(int healthDamage, int shieldDamage, float knockbackStrength, float knockbackRadius, bool hasExplosion, Color color, String colorName)
        {
            currentSpell.GetComponent<_spell_baseSpellScript>().healthDamage = healthDamage;
            currentSpell.GetComponent<_spell_baseSpellScript>().shieldDamage = shieldDamage;
            currentSpell.GetComponent<_spell_baseSpellScript>().knockbackStrength = knockbackStrength;
            currentSpell.GetComponent<_spell_baseSpellScript>().knockbackRadius = knockbackRadius;
            currentSpell.GetComponent<_spell_baseSpellScript>().hasExplosion = hasExplosion;
            currentSpell.GetComponent<_spell_baseSpellScript>().setColor(color, colorName);
        }

        private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            //StringBuilder builder = new StringBuilder();
            //builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
            //builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
            //builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
            //Debug.Log(builder.ToString());

            if (args.text == "light" || args.text == "lumos")
            {
                spawnBaseSpell(wandTip.gameObject.transform.position, wandTip.transform.rotation, "_PS_lumos");
                updateSpell(0, 0, 0f, 0f, false, Color.white, "White");
            }

            if (args.text == "stupify")
            {
                spawnBaseSpell(wandTip.gameObject.transform.position, wandTip.transform.rotation, majorSpellString);
                updateSpell(2, 1, 2f, 1.5f, true, Color.blue, "Blue");
                currentSpell.GetComponent<_spell_baseSpellScript>().SpellExplosionSoundClip = "spellExplosion_4";
            }

            if (args.text == "incendio" || args.text == "fireball")
            {
                spawnBaseSpell(wandTip.gameObject.transform.position, wandTip.transform.rotation, majorSpellString);
                updateSpell(1, 2, 3f, 1.75f, true, new Color(255, 102, 0), "Orange");
                currentSpell.GetComponent<_spell_baseSpellScript>().SpellExplosion = "SmallExplosion";
                NetworkedFireSource fs = currentSpell.AddComponent<NetworkedFireSource>();
                fs.fireParticlePrefab = (GameObject)Resources.Load("FireSmall");
                fs.startActive = true;
                currentSpell.GetComponent<_spell_baseSpellScript>().SpellExplosionSoundClip = "incendioSound_2";
            }

            if (args.text == "nox" || args.text == "extinguish")
            {
                StartCoroutine(Nox());
                destroyCurrentSpell();
            }

            if (args.text == "bombarda" || args.text == "bomb")
            {
                spawnBaseSpell(wandTip.gameObject.transform.position, wandTip.transform.rotation, majorSpellString);
                updateSpell(2, 1, 7f, 7f, true, Color.black, "Black");
                currentSpell.GetComponent<_spell_baseSpellScript>().SpellExplosion = "DustExplosion";
                currentSpell.GetComponent<_spell_baseSpellScript>().SpellExplosionSoundClip = "bombardaSound";
            }

            FocusSelection focusSelection = focusManager.GetFocusData<FocusSelection>(Time.realtimeSinceStartup);
            if (focusSelection != null)
            {
                LexiconSelectable selectable = focusSelection.SelectedObject.GetComponent<LexiconSelectable>();
                if(selectable.transform.parent.gameObject.GetComponent<Rigidbody>() != null)
                {
                    if(selectable.transform.parent.gameObject.GetComponent<Interactable>() != null)
                    {
                        if(selectable.transform.parent.gameObject.GetComponent<Interactable>().attachedToHand == null)
                        {
                            selectable.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                        }
                    }
                }

                if (args.text == "levitate" || args.text == "wingardium leviosa")
                {
                    selectable.transform.parent.gameObject.AddComponent<_spell_WingardiumLeviosaScript>();
                    destroyCurrentSpell();
                }

                if (args.text == "to me" || args.text == "accio")
                {
                    selectable.transform.parent.gameObject.AddComponent<_spell_AccioScript>();
                    selectable.transform.parent.gameObject.GetComponent<_spell_AccioScript>().hand = hand.otherHand.gameObject;
                    destroyCurrentSpell();
                }

                if (args.text == "bigger" || args.text == "engorgio")
                {
                    selectable.transform.parent.gameObject.AddComponent<_spell_EngorgioScript>();
                    destroyCurrentSpell();
                }

                if (args.text == "smaller" || args.text == "diminuendo")
                {
                    selectable.transform.parent.gameObject.AddComponent<_spell_DiminuendoScript>();
                    destroyCurrentSpell();
                }

                if (args.text == "alarte" || args.text == "up")
                {
                    selectable.transform.parent.gameObject.AddComponent<_spell_AlarteScript>();
                    destroyCurrentSpell();
                }

                if (args.text == "geminio" || args.text == "duplicate")
                {
                    selectable.transform.parent.gameObject.AddComponent<_spell_GeminioScript>();
                    destroyCurrentSpell();
                }

                if (args.text == "arresto momentum" || args.text == "freeze")
                {
                    selectable.transform.parent.gameObject.AddComponent<_spell_ArrestoMomentumScript>();
                    destroyCurrentSpell();
                }

                if (args.text == "finite incantatem" || args.text == "nullify")
                {
                    selectable.transform.parent.gameObject.AddComponent<_spell_FiniteIncantatemScript>();
                    destroyCurrentSpell();
                }

                if (args.text == "duro" || args.text == "stone")
                {
                    selectable.transform.parent.gameObject.AddComponent<_spell_DuroScript>();
                    destroyCurrentSpell();
                }

            }

            if (hand != null)
            {
                hand.TriggerHapticPulse(1000);
            }
        }

        private IEnumerator Select(LexiconSelectable selectable)
        {
            selectable.Select();
            yield return new WaitForSeconds(1.0f);
            selectable.Deselect();
        }

        IEnumerator Nox()
        {
            if(gameObject.GetComponent<AudioSource>() != null)
            {
                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.clip = Resources.Load("noxSound") as AudioClip;
                audioSource.Play();
                yield return new WaitWhile(() => audioSource.isPlaying);
            }
            yield return null;
        }

        private GameObject spawnBaseSpell(Vector3 wandTipPosition, Quaternion wandRotation, string spellToSpawn)
        {
            //if we had a shield despawn it
            despawnShield();

            //if we had a spell already override it
            destroyCurrentSpell();

            //insantiate spell local or networked
            GameObject baseSpell;

            if (!PhotonNetwork.InRoom)
            {
                baseSpell = (GameObject)Instantiate(Resources.Load(spellToSpawn), wandTip.transform.position, wandRotation);
            }
            else
            {
                baseSpell = PhotonNetwork.Instantiate(spellToSpawn, wandTip.transform.position, wandRotation, 0);
            }

            baseSpell.transform.parent = gameObject.transform;
            currentSpell = baseSpell;
            currentSpell.AddComponent<_spell_baseSpellScript>();
            primedGlow.gameObject.SetActive(true);
            primedGlow.Play();
            return baseSpell;
        }

        //get input from hand to shoot spell out
        private void wandHandInput()
        {
            bool indexPressedDown = shootInput.GetStateDown(hand.handType);
            bool indexLiftedUp = shootInput.GetStateUp(hand.handType);
            bool gripPressedDown = blockInput.GetStateDown(hand.handType);
            bool gripLiftedUp = blockInput.GetStateUp(hand.handType);

            //fire spell
            if (indexPressedDown && currentSpell != null)
            {
                if (currentSpell.tag.Contains("spell"))
                {                   
                    currentSpell.GetComponent<Rigidbody>().AddForce(wandTip.transform.forward * 1500);
                    fireSpell();
                }
            }
            else if (indexPressedDown && currentSpell == null)
            {
                spawnBaseSpell(wandTip.gameObject.transform.position, wandTip.transform.rotation, minorSpellString);
                updateSpell(1, 1, 2f, 2f, true, Color.white, "White");
                currentSpell.GetComponent<_spell_baseSpellScript>().SpellExplosionSoundClip = "spellExplosion_4";
            }
            else if (indexLiftedUp && currentSpell != null)
            {
                if (currentSpell.name.Contains("MinorBaseSpell"))
                {
                    Vector3 velocity;
                    Vector3 angularVelocity;
                    hand.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
                    float scaleFactor = 1.0f;
                    if (scaleReleaseVelocityThreshold > 0)
                    {
                        scaleFactor = Mathf.Clamp01(scaleReleaseVelocityCurve.Evaluate(velocity.magnitude / scaleReleaseVelocityThreshold));
                    }

                    velocity *= (scaleFactor * scaleReleaseVelocity);
                    //Debug.Log(velocity.magnitude);
                    if (velocity.magnitude >= 3.0f) {
                        currentSpell.GetComponent<Rigidbody>().velocity = velocity;
                        currentSpell.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
                        fireSpell();
                    } else {
                        destroyCurrentSpell();
                        primedGlow.gameObject.SetActive(false);
                    }
                }
            }

            //trigger shield
            if (gripPressedDown)
            {
                if (getShieldCount() > 0)
                {
                    destroyCurrentSpell();
                    spawnShield();
                }
            }
            else if (gripLiftedUp)
            {
                despawnShield();
            }
        }

        private void fireSpell()
        {
            currentSpell.GetComponent<_spell_baseSpellScript>().activate();
            currentSpell.transform.parent = null;
            currentSpell = null;
            flash.Play();
            flash.GetComponent<AudioSource>().Play();
            primedGlow.gameObject.SetActive(false);
        }

        //trigger shield either on or off
        private void spawnShield()
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
                    currentShield.transform.parent = gameObject.transform;
                    currentShield.transform.Rotate(new Vector3(90, 0, 0));
                    currentShield.tag = "shield";
                }
                shieldSpawned = true;
            }
        }

        private void despawnShield()
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

        private void shieldUpdate()
        {
            if (getShieldCount() < 1)
            {
                despawnShield();
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

        //destroy whatever spell is currently on the wand
        private void destroyCurrentSpell()
        {
            primedGlow.gameObject.SetActive(false);
            if (!PhotonNetwork.InRoom)
            {
                if (currentSpell != null)
                {
                    Destroy(currentSpell);
                }
            }
            else
            {
                if (currentSpell != null)
                {
                    PhotonNetwork.Destroy(currentSpell);
                }
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

        private int getHealthCount()
        {
            if (NetworkedGameManager.Instance != null)
            {
                return NetworkedGameManager.Instance.playerHealth;
            }

            return localHealthCount;
        }
        //-------------------------------------------------
        private void OnAttachedToHand(Hand attachedHand)
        {
            hand = attachedHand;
            //Debug.Log("LOCAL BOOK INTERACTABlE: ATTACHED TO HAND");
            defaultLayerMask = hand.hoverLayerMask;
            LayerMask mask = LayerMask.GetMask("Pickup");
            mask &= ~(1 << this.gameObject.layer);
            hand.hoverLayerMask = mask;
            //hand.HoverLock(interactable);
            //attached = true;
            //onPickUp.Invoke();
        }


        //-------------------------------------------------
        private void ShutDown()
        {
            if (hand != null) { }
        }

        //-------------------------------------------------
        private void OnHandFocusLost(Hand hand)
        {
            gameObject.SetActive(false);
        }


        //-------------------------------------------------
        private void OnHandFocusAcquired(Hand hand)
        {
            gameObject.SetActive(true);
            OnAttachedToHand(hand);
        }


        //-------------------------------------------------
        private void OnDetachedFromHand(Hand hand)
        {
            hand.hoverLayerMask = defaultLayerMask;
            //attached = false;
            //onDetachFromHand.Invoke();

            m_Recognizer.Dispose();
            LexiconFocusManager.OnCaptureFocus -= CaptureFocus;
            Destroy(pointerSphere);
            Destroy(gameObject);
        }

        //-------------------------------------------------
        void OnDestroy()
        {
            ShutDown();
        }

        bool isMineOrLocal()
        {
            bool photonViewIsMine = GetComponent<PhotonView>().IsMine;
            return photonViewIsMine || (PhotonNetwork.InRoom == false && PhotonNetwork.InLobby == false);
        }

        public void CaptureFocus()
        {
            // Get a focus position data entry from the pool.
            FocusPosition focusPosition = focusManager.GetPooledData<FocusPosition>();

            Ray handRay = new Ray(wandTip.transform.position, wandTip.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(handRay, out hit, 1000f, ~wandReticleIgnoreMask))
            {
                // This sample uses a LexiconSelectable component to determine object selectability.
                // You could just as easily use layers, tags, or your own scripts.
                LexiconSelectable selectable = hit.collider.gameObject.GetComponentInParent<LexiconSelectable>();
                if (selectable != null)
                {
                    FocusSelection focusSelection = focusManager.GetPooledData<FocusSelection>();
                    focusSelection.SelectedObject = selectable.gameObject;
                    focusManager.AddFocusData(focusSelection);
                }

                // Set the focus position to the hit point if present.
                focusPosition.Position = hit.point;
                focusPosition.Normal = hit.normal;
            }
            else
            {
                // Set the focus position in front of the camera if no hit point.
                focusPosition.Position = wandTip.transform.position + wandTip.transform.forward * 2.0f;
                focusPosition.Normal = -wandTip.transform.forward;
            }

            // Add our focus position data entry.
            focusManager.AddFocusData(focusPosition);

            // Update the pointer position.
            if(pointerSphere != null)
            {
                pointerSphere.transform.position = focusPosition.Position;
                float dist = Vector3.Distance(focusPosition.Position, wandTip.transform.position);
                float scale = pointerSize * dist;
                pointerSphere.transform.localScale = new Vector3(scale, scale, scale);
            }

            // Add a dwell position entry if the user's gaze is lingering on a spot.
            float speed = Vector3.Magnitude(lastPosition - focusPosition.Position) / Time.deltaTime;
            if (speed < dwellSpeed)
            {
                FocusDwellPosition dwellPosition = focusManager.GetPooledData<FocusDwellPosition>();
                dwellPosition.Position = focusPosition.Position;
                dwellPosition.Normal = focusPosition.Normal;
                focusManager.AddFocusData(dwellPosition);
            }

            lastPosition = focusPosition.Position;
        }
    }
}