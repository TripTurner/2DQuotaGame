using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public LayerMask interactables;
    private List<ItemLogic> touching = new List<ItemLogic>();
    public LayerMask ladderMask;
    public string dangerTilemapTag = "DangerTilemap";
    public LayerMask enemyMask;
    private PlayerInventory inventory;
    private PlayerMovement pMovement;
    private PlayerHealth pHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = gameObject.GetComponent<PlayerInventory>();
        pMovement = gameObject.GetComponent<PlayerMovement>();
        pHealth = gameObject.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void tryPickUp() {
        // Debug.Log(touching);
        if (touching.Count!=0) {
            int maxPriority = touching[0].priority;
            ItemLogic bestItem = touching[0];
            foreach (ItemLogic item in touching) {
                if (item.priority > maxPriority) {
                    maxPriority = item.priority;
                    bestItem = item;
                }
            }
            if (bestItem.gameObject.CompareTag("Bomb") && inventory.checkHoldingTag()=="BombBag") {
                Destroy(bestItem.gameObject);
                inventory.getHolding().gameObject.GetComponent<BombBagLogic>().grabBomb();
                return;
            }
            bestItem.interact(gameObject);
            inventory.pickUp(bestItem);
        }   
    }
    
    public void OnTriggerEnter2D(Collider2D other) {
        if ((interactables & (1<<other.gameObject.layer)) != 0) {
            ItemLogic otherItem = other.gameObject.GetComponentInParent<ItemLogic>();
            if (!touching.Contains(otherItem)) {
                touching.Add(otherItem);
            }
        }

        if ((enemyMask & (1<<other.gameObject.layer)) != 0) {
            Debug.Log("Hit enemy");
            EnemyData ED = other.gameObject.GetComponent<EnemyData>();
            if (ED == null) return;
            if (ED.isDealingDamage()) {
                Debug.Log(ED.getKnockback());
                pHealth.takeDamage(ED.getDamage(), ED.getKnockback());
            }
        }
        // if ((ladderMask & (1<<other.gameObject.layer)) != 0) {
        //     LadderLogic otherItem = other.gameObject.GetComponentInParent<LadderLogic>();
        //     if (!ladders.Contains(otherItem)) {
        //         ladders.Add(otherItem);
        //     }
        // }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if ((interactables & (1<<other.gameObject.layer)) != 0) {
            ItemLogic otherItem = other.gameObject.GetComponentInParent<ItemLogic>();
            if (touching.Contains(otherItem)) {
                touching.Remove(otherItem);
            }
        }
        // if ((ladderMask & (1<<other.gameObject.layer)) != 0) {
        //     LadderLogic otherItem = other.gameObject.GetComponentInParent<LadderLogic>();
        //     if (!ladders.Contains(otherItem)) {
        //         ladders.Remove(otherItem);
        //     }
        // }
    }

    public void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag(dangerTilemapTag)) {
            float damage = 5;
            if (other.gameObject.GetComponent<HazardData>()!=null) {
                damage = other.gameObject.GetComponent<HazardData>().damage;
            }
            pHealth.takeDamage(damage, (new Vector2(Random.Range(-0.5f,1f),5f)).normalized);
            Debug.Log("Touched hazard");
        }
    }

    public bool getTouchingLadder(Vector2 pos) {
        Collider2D hit = Physics2D.OverlapPoint(pos, ladderMask);
        if (hit!=null) pMovement.setRecentLadder(hit.gameObject);
        return hit!=null;
    }
}
