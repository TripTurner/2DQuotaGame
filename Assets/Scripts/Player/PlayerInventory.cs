using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    private ItemLogic heldItem;
    private ItemLogic[] inventory = new ItemLogic[5];
    private int currentSlot = -1;
    private int prevSlot = -1;
    private PlayerInteraction pInteraction;
    private PlayerMovement pMovement;
    [SerializeField] private float dropStr = 3;
    private int ropeAmount = 20;

    public GameObject ropeProjectile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pInteraction = gameObject.GetComponent<PlayerInteraction>();
        pMovement = gameObject.GetComponent<PlayerMovement>();
        for (int i=0; i<inventory.Length; i++) {
            inventory[i] = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) {
            if (currentSlot==0) {
                currentSlot = -1;
            } else {
                currentSlot=0;
            }
            updateHolding();
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame) {
            if (currentSlot==1) {
                currentSlot = -1;
            } else {
                currentSlot=1;
            }
            updateHolding();
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame) {
            if (currentSlot==2) {
                currentSlot = -1;
            } else {
                currentSlot=2;
            }
            updateHolding();
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame) {
            if (currentSlot==3) {
                currentSlot = -1;
            } else {
                currentSlot=3;
            }
            updateHolding();
        }
        if (Keyboard.current.digit5Key.wasPressedThisFrame) {
            if (currentSlot==4) {
                currentSlot = -1;
            } else {
                currentSlot=4;
            }
            updateHolding();
        }
        if (Keyboard.current.eKey.wasPressedThisFrame) {
            bool spotOpen = false;
            // Debug.Log(inventory);
            for (int i=0; i<inventory.Length; i++) {
                if (inventory[i]==null) {
                    spotOpen=true;
                    break;
                }
            }
            if (spotOpen) {
                pInteraction.tryPickUp();
            }
        }
        if (Keyboard.current.gKey.wasPressedThisFrame) {
            dropCurrent();
        }
        if (Keyboard.current.qKey.wasPressedThisFrame) {
            if (ropeAmount>0) {
                if (Keyboard.current.sKey.isPressed) {
                    pMovement.throwRope();
                } else {
                    GameObject rp = Instantiate(ropeProjectile,transform.position,Quaternion.identity);
                    rp.GetComponent<RopeProjectileLogic>().Initialize(transform.position.y+6);
                }
                ropeAmount--;
            }
        }
    }

    public void pickUp(ItemLogic item) {
        //if (item.getHeld()) return;
        if (currentSlot==-1||inventory[currentSlot]!=null) {
            for (int i=0; i<inventory.Length; i++) {
                if (inventory[i]==null) {
                    inventory[i] = item;
                    currentSlot = i;
                    updateHolding();
                    break;
                }
            }
        } else {
            inventory[currentSlot] = item;
        }
        // Debug.Log(inventory);
    }

    public void dropCurrent() {
        if (currentSlot != -1 && inventory[currentSlot]!=null) {
            inventory[currentSlot].drop(dropStr * (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())-transform.position).normalized);
            inventory[currentSlot] = null;
        }
    }

    public void updateHolding() {
        /*Debug.Log("PrevSlot: " + prevSlot +". CurrentSlot: " + currentSlot);
        string logMessage = "";
        for (int i=0; i<inventory.Length; i++) {
            logMessage += " "+inventory[i];
        }
        Debug.Log(logMessage);*/
        if (prevSlot!=-1 && inventory[prevSlot] != null) inventory[prevSlot].store();
        if (currentSlot!=-1 && inventory[currentSlot]!=null) inventory[currentSlot].interact(gameObject);
        prevSlot = currentSlot;
    }

    public string checkHoldingTag() {
        if (currentSlot==-1 || inventory[currentSlot]==null) return "none";
        return inventory[currentSlot].gameObject.tag;
    }

    public ItemLogic getHolding() {
        if (currentSlot==-1) return null;
        return inventory[currentSlot];
    }

    public void removeItem(ItemLogic item) {
        for (int i=0; i<inventory.Length; i++) {
            if (inventory[i]==item) {
                inventory[i]=null;
                break;
            }
        }
    }
}
