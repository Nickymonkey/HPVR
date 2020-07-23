using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class Spellbook : MonoBehaviour
    {

        //public List<int> pages;
        public GameObject[] currentPages;
        public GameObject PagePrefab;
        public Transform middle;
        public int leftMost = -1;
        public int rightMost = 2;
        private SpellDictionary sd = null;

        private void Awake()
        {
            if (!isMineOrLocal())
            {
                this.enabled = false;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            sd = new SpellDictionary();
            //Debug.Log(sd.SpellList.Count);
            //pages = Enumerable.Range(0, sd.SpellList.Count-1).ToList();
            currentPages = new GameObject[4];
            InstantiateNewPage(PageSide.Left, PageState.Underneath, sd.GetPageContents(-1));            
            InstantiateNewPage(PageSide.Left, PageState.Ontop, sd.GetPageContents(0));
            InstantiateNewPage(PageSide.Right, PageState.Ontop, sd.GetPageContents(1));
            InstantiateNewPage(PageSide.Right, PageState.Underneath, sd.GetPageContents(2));
            currentPages[0].GetComponent<MaxAngleReached>().pageCollider.enabled = false;
            currentPages[1].GetComponent<MaxAngleReached>().pageCollider.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            //if()
            //outAngle = Mathf.Lerp(outAngle, minAngle, 1f);
        }

        public void resetPages()
        {
            BroadcastMessage("lerpPages");
            //for (int i=0; i<currentPages.Length; i++)
            //{
            //    currentPages[i].Invoke("lerpPages");
            //}
        }

        public void UpdatePages()
        {

        }

        public void updatePageText(GameObject page, Tuple<Tuple<string, string>, Tuple<string, string>> text)
        {
            page.GetComponent<MaxAngleReached>().FrontPageTitleText.SetText(text.Item2.Item1);
            page.GetComponent<MaxAngleReached>().FrontPageDescriptionText.SetText(text.Item2.Item2);
            page.GetComponent<MaxAngleReached>().BackPageTitleText.SetText(text.Item1.Item1);
            page.GetComponent<MaxAngleReached>().BackPageDescriptionText.SetText(text.Item1.Item2);
        }

        public void InstantiateNewPage(PageSide side, PageState state, Tuple<Tuple<string, string>, Tuple<string, string>> text)
        {
            GameObject page;
            //if (PhotonNetwork.InRoom || PhotonNetwork.InLobby)
            //{
            //    page = PhotonNetwork.Instantiate(PagePrefab.name, Vector3.zero, Quaternion.identity);
            //    page.transform.parent = middle;
            //    page.transform.localPosition = Vector3.zero;
            //}
            //else
            //{
            page = Instantiate(PagePrefab, middle, false);
            //}

            updatePageText(page, text);
            page.GetComponent<MaxAngleReached>().currentSide = side;
            page.GetComponent<MaxAngleReached>().currentState = state;

            if (side == PageSide.Left)
            {
                if (state == PageState.Ontop)
                {
                    page.GetComponent<PageDrive>().startAngle = 0f;
                    currentPages[1] = page;
                } else if (state == PageState.Underneath)
                {
                    page.transform.localPosition = new Vector3(0, -.001f, 0);
                    page.GetComponent<PageDrive>().startAngle = 0f;
                    page.GetComponent<MaxAngleReached>().pageCollider.enabled = false;
                    currentPages[0] = page;
                }
            }

            if (side == PageSide.Right)
            {
                if (state == PageState.Ontop)
                {
                    page.GetComponent<PageDrive>().startAngle = 180f;
                    currentPages[2] = page;
                }
                else if (state == PageState.Underneath)
                {
                    page.transform.localPosition = new Vector3(0, -.001f, 0);
                    page.GetComponent<PageDrive>().startAngle = 180f;
                    page.GetComponent<MaxAngleReached>().pageCollider.enabled = false;
                    currentPages[3] = page;
                }
            }
        }

        public void PageFlipped(bool nextPage)
        {
            resetPages();
            //move forward
            if (nextPage)
            {
                leftMost++;
                rightMost++;
                //Delete left underneath page
                Destroy(currentPages[0]);
                //Move left top page to underneath
                currentPages[1].transform.localPosition = new Vector3(0f, -.001f, 0f);
                currentPages[1].GetComponent<MaxAngleReached>().pageCollider.enabled = false;
                currentPages[1].GetComponent<MaxAngleReached>().currentState = PageState.Underneath;
                //Move right undreneath to on top
                currentPages[3].transform.localPosition = new Vector3(0f, 0f, 0f);
                currentPages[3].GetComponent<MaxAngleReached>().currentState = PageState.Ontop;
                //if its the farther spell to the right disable collider
                if (rightMost >= sd.SpellPageList.Count)
                {
                    currentPages[3].GetComponent<MaxAngleReached>().pageCollider.enabled = false;
                }
                else
                {
                    currentPages[3].GetComponent<MaxAngleReached>().pageCollider.enabled = true;
                }
                //Instantiate new right underneath page
                currentPages[0] = currentPages[1];
                currentPages[1] = currentPages[2];
                currentPages[2] = currentPages[3];
                if (rightMost >= sd.SpellPageList.Count)
                {
                    InstantiateNewPage(PageSide.Right, PageState.Underneath, new Tuple<Tuple<string, string>, Tuple<string, string>>(new Tuple<string, string>("", ""), new Tuple<string, string>("", "")));
                }
                else
                {
                    InstantiateNewPage(PageSide.Right, PageState.Underneath, sd.GetPageContents(rightMost));
                }
            }
            else //move backwards
            {
                leftMost--;
                rightMost--;
                //Delete right underneath page
                Destroy(currentPages[3]);
                //Move right top page to underneath
                currentPages[2].transform.localPosition = new Vector3(0f, -.001f, 0f);
                currentPages[2].GetComponent<MaxAngleReached>().pageCollider.enabled = false;
                currentPages[2].GetComponent<MaxAngleReached>().currentState = PageState.Underneath;
                //Move left page underneath to on top
                currentPages[0].transform.localPosition = new Vector3(0f, 0f, 0f);
                currentPages[0].GetComponent<MaxAngleReached>().currentState = PageState.Ontop;
                //if its the last spell to the left disable collider
                if (leftMost == -1)
                {
                    currentPages[0].GetComponent<MaxAngleReached>().pageCollider.enabled = false;
                }
                else
                {
                    currentPages[0].GetComponent<MaxAngleReached>().pageCollider.enabled = true;
                }
                //currentPages[0].GetComponent<MaxAngleReached>().pageCollider.enabled = true;
                //Instantiate new left page underneath
                currentPages[3] = currentPages[2];
                currentPages[2] = currentPages[1];
                currentPages[1] = currentPages[0];
                if (leftMost == -1)
                {
                    InstantiateNewPage(PageSide.Left, PageState.Underneath, new Tuple<Tuple<string, string>, Tuple<string, string>>(new Tuple<string, string>("", ""), new Tuple<string, string>("", "")));
                }
                else
                {
                    InstantiateNewPage(PageSide.Left, PageState.Underneath, sd.GetPageContents(leftMost));
                }
            }
        }

        bool isMineOrLocal()
        {
            bool photonViewIsMine = GetComponent<PhotonView>().IsMine;
            return photonViewIsMine || (PhotonNetwork.InRoom == false && PhotonNetwork.InLobby == false);
        }
    }
}
