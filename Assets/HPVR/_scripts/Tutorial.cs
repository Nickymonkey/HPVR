﻿using HPVR;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Tutorial : MonoBehaviour
{
    public int IsFirst;
    public TextMeshProUGUI textDisplay;
    public GameObject TutorialWall;
    private List<string> instructions;
    public SteamVR_Action_Boolean shootInput;
    public SteamVR_Action_Boolean blockInput;
    public SteamVR_Action_Boolean snapLeftAction;
    public SteamVR_Action_Boolean snapRightAction;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean ButtonInput;
    public SteamVR_Action_Vector2 ThumbstickInput;
    private Player_VR player;
    private Hand LeftHand;
    private Hand RightHand;
    private int instructionStep = 0;
    private GameObject wand;
    public GameObject cube;
    public AudioSource source;

    void Start()
    {
        instructions = new List<string>();
        instructions.Add("Welcome to the Wizarding World of Harry Potter! It looks like this is your first time, to get started lets try turning. Use your right thumbstick (Oculus) or right dpad (Vive) to turn by directing 90 degrees to the left or right.");
        instructions.Add("Amazing! Lets try moving now. You can use either your left or right thumbstick (Oculus) or left or right dpad (Vive) and tilt forward, once you see the teleportation beam you can release and you will be teleported to where your reticle is pointing.");
        instructions.Add("Great, now as you can see, currently teleportation is enabled, if you'd prefer to use smooth locomotion, press any button, A/B/X/Y (Oculus) or press any dpad (Vive) to toggle smooth locomotion on.");
        instructions.Add("Now tilt the left thumbstick (Oculus) or left dpad (Vive) to move in any direction.");
        instructions.Add("You can toggle back to teleportation by pressing the same button as before, press any button, A/B/X/Y (Oculus) or press any dpad (Vive) to toggle back to teleportation.");
        instructions.Add("Awesome! You can always switch between the two, some puzzles might even be easier with one or the other. Now lets pull out our trusty wand, look down and to your right and grab your wand, middle finger grip button (Oculus) or side grip button (Vive) to pull it out.");
        instructions.Add("Great! You don't need to hold the grip button once your wand is pulled out. Now lets test out our first spell, try saying the word 'lumos' with your wand drawn!");
        instructions.Add("Amazing! Now try pressing the index finger grip button (Oculus/Vive) on your wand hand to shoot your lumos outwards.");
        instructions.Add("Awesome! As you can see your wand has a light blue reticle, this shows you where your wand is pointing, this is important for casting spells on objects. But first let's pull out our spellbook, look down and to your left and grab your spellbook middle finger grip button (Oculus) or side grip button (Vive) to pull it out.");
        instructions.Add("Great! This is a magic spellbook that can be pulled from your pocket at anytime, you can also release it and it will float wherever you place it. To turn the pages grab any page middle finger grip button (Oculus) or side grip button (Vive) and turn the page.");
        instructions.Add("Amazing! This spellbook has a lot of different spells but lets start with a basic one. Try pointing your wand at the object in front of you and say the word 'engorgio'.");
        instructions.Add("Great! Now let's try one last spell, this is your weakest most basic form of magic. It requires no spoken word, simply hold your index finger grip down (Oculus/Vive) with whichever hand you are holding your wand in.");
        instructions.Add("You have now primed a basic spell onto your wand, to shoot it flick your wrist and release the index finger grip (Oculus/Vive) on your wand hand, if you performed it properly you should see the spell shot out.");
        instructions.Add("Go ahead and put your wand away by placing your wand hand in the same spot where you pulled it from.");
        instructions.Add("Likewise, do the same with your spellbook.");
        instructions.Add("You are now ready to explore! When you are ready explore the room, portkeys will send you to different locations. I would reccomend starting with Singleplayer first to get your bearings!");

        IsFirst = PlayerPrefs.GetInt("IsFirst");

        player = Launcher.LocalPlayerInstance.GetComponent<Player_VR>();

        for (int i = 0; i < player.handCount; i++)
        {
            if (player.hands[i].handType == SteamVR_Input_Sources.RightHand)
            {
                RightHand = player.hands[i];
            }
            else
            {
                LeftHand = player.hands[i];
            }
        }

        Debug.Log(player.handCount);
        Debug.Log(instructionStep);


        if (IsFirst == 0)
        {
            //while(Launcher.LocalPlayerInstance == null)
            //{
            //    Launcher.LocalPlayerInstance.GetComponent<Player_VR>();
            //}
            //Do stuff on the first time
            Debug.Log("first run");
            PlayerPrefs.SetInt("IsFirst", 1);
        }
        else
        {
            //Do stuff other times
            Debug.Log("welcome again!");
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (instructionStep)
        {

            case 0:
                bool rightHandTurnLeft = snapLeftAction.GetStateDown(SteamVR_Input_Sources.RightHand);
                bool rightHandTurnRight = snapRightAction.GetStateDown(SteamVR_Input_Sources.RightHand);
                if(rightHandTurnLeft || rightHandTurnRight)
                {
                    updateInstructions();
                }
                break;
            case 1:
                //bool rightHandTeleport = teleportAction.GetLastStateUp(RightHand.handType);
                bool rightHandTeleport = teleportAction.GetStateUp(SteamVR_Input_Sources.RightHand);
                bool leftHandTeleport = teleportAction.GetStateUp(SteamVR_Input_Sources.LeftHand);

                if((rightHandTeleport || leftHandTeleport) && GameState.Instance.locomotion.Contains("Teleport"))
                {
                    updateInstructions();
                }
                break;
            case 2:
                bool rightHandButtonPress = ButtonInput.GetStateUp(SteamVR_Input_Sources.RightHand);
                bool leftHandButtonPress = ButtonInput.GetStateUp(SteamVR_Input_Sources.LeftHand);

                if ((rightHandButtonPress || leftHandButtonPress) && GameState.Instance.locomotion.Contains("Smooth"))
                {
                    updateInstructions();
                }
                break;
            case 3:
                if((ThumbstickInput.axis.x != 0 || ThumbstickInput.axis.y != 0) && GameState.Instance.locomotion.Contains("Smooth"))
                {
                    updateInstructions();
                }
                break;
            case 4:
                bool rightHandButtonToggle = ButtonInput.GetStateUp(SteamVR_Input_Sources.RightHand);
                bool leftHandButtonToggle = ButtonInput.GetStateUp(SteamVR_Input_Sources.LeftHand);

                if ((rightHandButtonToggle || leftHandButtonToggle) && GameState.Instance.locomotion.Contains("Teleport"))
                {
                    updateInstructions();
                }
                break;
            case 5:
                if (GameObject.Find("Wand_2(Clone)") != null)
                {
                    updateInstructions();
                }
                break;
            case 6:
                if (GameObject.Find("_PS_lumos(Clone)") != null)
                {
                    updateInstructions();
                }
                break;
            case 7:
                if (GameObject.Find("_PS_lumos(Clone)") == null)
                {
                    updateInstructions();
                }
                break;
            case 8:
                if (GameObject.Find("Spellbook(Clone)") != null)
                {
                    updateInstructions();
                }
                break;
            case 9:
                if (GameObject.Find("Spellbook(Clone)") != null)
                {
                    if (GameObject.Find("Spellbook(Clone)").gameObject.GetComponent<Spellbook>().pageTurned)
                    {
                        updateInstructions();
                    }
                }
                break;
            case 10:
                if (cube.transform.localScale == new Vector3(2.0f, 2.0f, 2.0f))
                {
                    updateInstructions();
                }
                break;
            case 11:
                if (GameObject.Find("MinorBaseSpell(Clone)") != null)
                {
                    updateInstructions();
                }
                break;
            case 12:
                if (GameObject.Find("MinorBaseSpell(Clone)") != null)
                {
                    if(GameObject.Find("MinorBaseSpell(Clone)").transform.parent == null)
                    {
                        updateInstructions();
                    }
                }
                break;
            case 13:
                if (GameObject.Find("Wand_2(Clone)") == null)
                {
                    updateInstructions();
                }
                break;
            case 14:
                if (GameObject.Find("Spellbook(Clone)") == null)
                {
                    updateInstructions();
                }
                break;
            default:
                //Console.WriteLine("Default case");
                break;
        }
    }

    private void updateInstructions()
    {
        instructionStep++;
        Debug.Log(instructionStep);
        textDisplay.text = instructions[instructionStep];
        source.Play();
    }
}