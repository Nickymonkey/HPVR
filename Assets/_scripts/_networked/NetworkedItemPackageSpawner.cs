using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{

    public class NetworkedItemPackageSpawner : ItemPackageSpawner
    {
        public bool networkedRoom = true;
        //-------------------------------------------------
        public override void SpawnAndAttachObject(Hand hand, GrabTypes grabType)
        {
            networkedRoom = PhotonNetwork.InRoom;
            if (hand.otherHand != null)
            {
                //If the other hand has this item package, take it back from the other hand
                ItemPackage otherHandItemPackage = GetAttachedItemPackage(hand.otherHand);
                if (otherHandItemPackage == itemPackage)
                {
                    TakeBackItem(hand.otherHand);
                }
            }

            if (showTriggerHint)
            {
                hand.HideGrabHint();
            }

            if (itemPackage.otherHandItemPrefab != null)
            {
                if (hand.otherHand.hoverLocked)
                {
                    Debug.Log("<b>[SteamVR Interaction]</b> Not attaching objects because other hand is hoverlocked and we can't deliver both items.");
                    return;
                }
            }

            // if we're trying to spawn a one-handed item, remove one and two-handed items from this hand and two-handed items from both hands
            if (itemPackage.packageType == ItemPackage.ItemPackageType.OneHanded)
            {
                RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.OneHanded, hand);
                RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand);
                RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand.otherHand);
            }

            // if we're trying to spawn a two-handed item, remove one and two-handed items from both hands
            if (itemPackage.packageType == ItemPackage.ItemPackageType.TwoHanded)
            {
                RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.OneHanded, hand);
                RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.OneHanded, hand.otherHand);
                RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand);
                RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand.otherHand);
            }

            if(spawnedItem != null)
            {
                if (networkedRoom == true)
                {
                    PhotonNetwork.Destroy(spawnedItem);
                }
                else
                {
                    Destroy(spawnedItem);
                }
            }

            if (networkedRoom == true)
            {
                spawnedItem = PhotonNetwork.Instantiate(itemPackage.itemPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
            }
            else
            {
                spawnedItem = GameObject.Instantiate(itemPackage.itemPrefab);
            }
            spawnedItem.SetActive(true);
            hand.AttachObject(spawnedItem, GrabTypes.Grip, attachmentFlags);

            if ((itemPackage.otherHandItemPrefab != null) && (hand.otherHand.isActive))
            {
                GameObject otherHandObjectToAttach;
                if (networkedRoom == true)
                {
                    otherHandObjectToAttach = PhotonNetwork.Instantiate(itemPackage.otherHandItemPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    otherHandObjectToAttach = GameObject.Instantiate(itemPackage.otherHandItemPrefab);
                }
                //= GameObject.Instantiate(itemPackage.otherHandItemPrefab);
                otherHandObjectToAttach.SetActive(true);
                hand.otherHand.AttachObject(otherHandObjectToAttach, grabType, attachmentFlags);
            }

            itemIsSpawned = true;

            justPickedUpItem = true;

            if (takeBackItem)
            {
                useFadedPreview = true;
                pickupEvent.Invoke();
                CreatePreviewObject();
            }
        }

        //-------------------------------------------------
        public override void RemoveMatchingItemsFromHandStack(ItemPackage package, Hand hand)
        {
            if (hand == null)
                return;

            for (int i = 0; i < hand.AttachedObjects.Count; i++)
            {
                ItemPackageReference packageReference = hand.AttachedObjects[i].attachedObject.GetComponent<ItemPackageReference>();
                if (packageReference != null)
                {
                    ItemPackage attachedObjectItemPackage = packageReference.itemPackage;
                    if ((attachedObjectItemPackage != null) && (attachedObjectItemPackage == package))
                    {
                        GameObject detachedItem = hand.AttachedObjects[i].attachedObject;
                        hand.DetachObject(detachedItem);
                        if (networkedRoom == true)
                        {
                            for (int j = 0; j < detachedItem.transform.childCount; j++)
                            {
                                GameObject child = detachedItem.transform.GetChild(j).gameObject;

                                for(int k = 0; k< child.transform.childCount; k++)
                                {
                                    GameObject grandChild = child.transform.GetChild(k).gameObject;
                                    if (grandChild != null)
                                    {
                                        grandChild.SetActive(false);
                                        if (grandChild.GetComponent<PhotonView>() != null)
                                        {
                                            if (PhotonNetwork.InRoom || PhotonNetwork.InLobby)
                                            {
                                                PhotonNetwork.Destroy(grandChild);
                                            }
                                        }
                                    }
                                }

                                if (child != null)
                                {
                                    child.SetActive(false);
                                    if (child.GetComponent<PhotonView>() != null)
                                    {
                                        if (PhotonNetwork.InRoom || PhotonNetwork.InLobby)
                                        {
                                            PhotonNetwork.Destroy(child);
                                        }
                                    }
                                }
                            }

                            PhotonNetwork.Destroy(detachedItem);
                        }
                        else
                        {
                            Destroy(detachedItem);
                        }
                    }
                }
            }
        }

        //-------------------------------------------------
        public override void RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType packageType, Hand hand)
        {
            for (int i = 0; i < hand.AttachedObjects.Count; i++)
            {
                ItemPackageReference packageReference = hand.AttachedObjects[i].attachedObject.GetComponent<ItemPackageReference>();
                if (packageReference != null)
                {
                    if (packageReference.itemPackage.packageType == packageType)
                    {
                        GameObject detachedItem = hand.AttachedObjects[i].attachedObject;

                        hand.DetachObject(detachedItem);

                        if (networkedRoom == true)
                        {
                            PhotonNetwork.Destroy(detachedItem);
                        }
                        else
                        {
                            Destroy(detachedItem);
                        }
                    }
                }
            }
        }
    }
}