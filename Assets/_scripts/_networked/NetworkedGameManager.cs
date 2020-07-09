using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HPVR
{
    public class NetworkedGameManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static NetworkedGameManager Instance;
        public int playerHealth = 3;
        public int opponentHealth = 3;
        public int playerShield = 5;
        public int opponentShield = 5;
        public Text playerHealthText;
        public Text opponentHealthText;
        public Text playerShieldText;
        public Text opponentShieldText;
        public Text timerTextOne;
        public Text timerTextTwo;
        public int timer = 10;
        public bool matchFinished = false;
        public bool matchStarted = false;
        public bool endingMatch = false;

        // Start is called before the first frame update

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {

            localMatchSetup();

            if (GetComponent<PhotonView>().IsMine)
            {
                StartCoroutine(Countdown());
            }

        }

        // Update is called once per frame
        void Update()
        {

            if (timer <= 0 && !matchStarted)
            {
                matchStarted = true;
            }

            if (((opponentHealth == 0 || playerHealth == 0) && matchStarted) && !matchFinished)
            {
                //timer = 10;
                matchFinished = true;
            }

            updateText();

            if (matchFinished)
            {
                if (!endingMatch)
                {
                    endingMatch = true;
                    finishMatch();
                }
            }
        }

        public void projectileHitPlayer(int damage)
        {
            if (PhotonNetwork.LocalPlayer.UserId != null)
            {
                if (GetComponent<PhotonView>().Owner.UserId != PhotonNetwork.LocalPlayer.UserId)
                {
                    GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                }
            }
            opponentHealth -= damage;
        }

        public void projectileHitShield(int damage)
        {
            if (PhotonNetwork.LocalPlayer.UserId != null)
            {
                if (GetComponent<PhotonView>().Owner.UserId != PhotonNetwork.LocalPlayer.UserId)
                {
                    GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                }
            }
            opponentShield -= damage;
        }

        IEnumerator Despawn()
        {
            yield return new WaitForSeconds(10);
            PhotonNetwork.Destroy(GameManager.LocalPlayerInstance);
            NetworkManager.Instance.LeaveRoom();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(opponentHealth);
                stream.SendNext(playerHealth);

                stream.SendNext(opponentShield);
                stream.SendNext(playerShield);

                stream.SendNext(timer);

                stream.SendNext(matchFinished);

                stream.SendNext(matchStarted);
                
            }
            else
            {
                // Network player, receive data
                playerHealth = (int)stream.ReceiveNext();
                opponentHealth = (int)stream.ReceiveNext();

                playerShield = (int)stream.ReceiveNext();
                opponentShield = (int)stream.ReceiveNext();

                timer = (int)stream.ReceiveNext();

                matchFinished = (bool)stream.ReceiveNext();

                matchStarted = (bool)stream.ReceiveNext();
            }
        }

        IEnumerator Countdown()
        {
            timer = 10;
            while (timer > 0)
            {
                yield return new WaitForSeconds(1);
                timer--;
            }
        }

        void updateText()
        {
            if (playerHealthText != null)
            {
                playerHealthText.text = playerHealth + "";
            }

            if (opponentHealthText != null)
            {
                opponentHealthText.text = opponentHealth + "";
            }

            if (playerShieldText != null)
            {
                playerShieldText.text = playerShield + "";
            }

            if (opponentShieldText != null)
            {
                opponentShieldText.text = opponentShield + "";
            }

            if (timerTextOne != null)
            {
                timerTextOne.text = timer + "";
            }

            if (timerTextTwo != null)
            {
                timerTextTwo.text = timer + "";
            }
        }

        public void finishMatch()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.positionPlayer();
            }

            if (GetComponent<PhotonView>().IsMine)
            {
                StartCoroutine(Countdown());
            }

            Quaternion resultsRotation;
            if (PhotonNetwork.IsMasterClient)
            {
                resultsRotation = Quaternion.Euler(0f, 90f, 0);
            }
            else
            {
                resultsRotation = Quaternion.Euler(0f, -90f, 0);
            }

            GameObject results = (GameObject)Instantiate(Resources.Load("[Results]"), new Vector3(0f, 1.75f, 0.25f), resultsRotation);

            if (playerHealth == 0)
            {
                results.GetComponentInChildren<Text>().text = "You lose";
            }
            else
            {
                results.GetComponentInChildren<Text>().text = "You win";
            }
            StartCoroutine(Despawn());
        }

        public void localMatchSetup()
        {
            if (GameObject.Find("PlayerCanvas").transform.Find("HealthText").GetComponent<Text>() != null)
            {
                playerHealthText = GameObject.Find("PlayerCanvas").transform.Find("HealthText").GetComponent<Text>();
                playerShieldText = GameObject.Find("PlayerCanvas").transform.Find("ShieldText").GetComponent<Text>();
            }

            if (GameObject.Find("OpponentCanvas").transform.Find("HealthText").GetComponent<Text>() != null)
            {
                opponentHealthText = GameObject.Find("OpponentCanvas").transform.Find("HealthText").GetComponent<Text>();
                opponentShieldText = GameObject.Find("OpponentCanvas").transform.Find("ShieldText").GetComponent<Text>();
            }

            if (GameObject.Find("TimerCanvas_01").transform.Find("TimerText_01").GetComponent<Text>() != null)
            {
                timerTextOne = GameObject.Find("TimerCanvas_01").transform.Find("TimerText_01").GetComponent<Text>();
            }

            if (GameObject.Find("TimerCanvas_02").transform.Find("TimerText_02").GetComponent<Text>() != null)
            {
                timerTextTwo = GameObject.Find("TimerCanvas_02").transform.Find("TimerText_02").GetComponent<Text>();
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.positionPlayer();
            }
        }
    }
}