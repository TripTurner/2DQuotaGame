using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInteraction pInteraction;
    private PlayerInventory pInventory;

    public GameObject rope;
    public float ropeHeight;

    private float stunTimer = 0;
    private bool stunned = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private int jumpBuffer = 0;
    [SerializeField] private int bufferFrames;
    private int onGround = 0;
    [SerializeField] private int coyoteFrames;
    private int varJump;
    [SerializeField] private int varJumpMax;
    [SerializeField] private float jumpEndMult;
    [SerializeField] private float terminalVelocity;
    [SerializeField] private Vector2 jumpBoxOffset;
    [SerializeField] private Vector2 jumpBoxScale;
    public LayerMask groundLayer;
    [SerializeField] private float climbSpeed;
    private bool onLadder = false;

    public float dir = 1;
    public float throwStrength;
    private GameObject recentLadder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pInteraction = GetComponent<PlayerInteraction>();
        pInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpBuffer--;
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            jumpBuffer = bufferFrames;
        }

        if (stunTimer>0) {
            stunTimer-=Time.deltaTime;
        } else {
            if (stunned) {
                stunned = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                transform.rotation = Quaternion.Euler(0,0,0);
            }
            if (Mouse.current.leftButton.wasPressedThisFrame) {
                ItemLogic currentItem = pInventory.getHolding();
                if (currentItem!= null) currentItem.use();
            }
        }
    }

    void FixedUpdate() {

        if (!onLadder && rb.linearVelocityY<-terminalVelocity) {
            rb.linearVelocityY=-terminalVelocity;
        }

        onGround--;
        RaycastHit2D hit = Physics2D.BoxCast((Vector2) transform.position + jumpBoxOffset, jumpBoxScale, 0, Vector2.zero, Mathf.Infinity, groundLayer);
        if (hit.collider != null) {
            onGround = coyoteFrames;
        }

        if (!stunned) {
            if (Keyboard.current.aKey.isPressed) {
                rb.linearVelocityX = -moveSpeed;
                dir = -1;
            } else if (Keyboard.current.dKey.isPressed) {
                rb.linearVelocityX = moveSpeed;
                dir = 1;
            } else {
                rb.linearVelocityX = 0;
            }

            if (jumpBuffer>0 && (onGround>0||onLadder)) {
                if (onLadder && Keyboard.current.sKey.isPressed) {
                    rb.linearVelocityY = -climbSpeed;
                } else {
                    rb.linearVelocityY = jumpForce;
                }
                varJump = varJumpMax;
                jumpBuffer=0;
                onGround=0;
                onLadder = false;
                rb.gravityScale = 1;
            }

            if (varJump>0) {
                if (!Keyboard.current.spaceKey.isPressed) {
                    varJump=0;
                    rb.linearVelocityY*=jumpEndMult;
                } else {
                    varJump--;
                }
            }
            if (Keyboard.current.wKey.isPressed) {
                if ((onLadder||rb.linearVelocityY<=4) && pInteraction.getTouchingLadder((Vector2) transform.position + new Vector2(0,climbSpeed*Time.fixedDeltaTime))) {
                    rb.linearVelocityY = climbSpeed;
                    if (!onLadder) {
                        rb.gravityScale = 0;
                        onLadder = true;
                        transform.position = new Vector3(recentLadder.transform.position.x, transform.position.y, transform.position.z);
                    }
                } else if (onLadder) {
                    rb.linearVelocityY=0;
                }
            } else if (Keyboard.current.sKey.isPressed) {
                if (onLadder && pInteraction.getTouchingLadder((Vector2) transform.position + new Vector2(0,climbSpeed*Time.fixedDeltaTime))) {
                    rb.linearVelocityY = -climbSpeed;
                    if (!onLadder) {
                        rb.gravityScale = 0;
                        onLadder = true;
                        transform.position = new Vector3(recentLadder.transform.position.x, transform.position.y, transform.position.z);
                    }
                } else if (onLadder) {
                    rb.gravityScale = 1;
                    onLadder = false;
                }
            } else if (onLadder) {
                rb.linearVelocityY=0;
            }
            if (onLadder) rb.linearVelocityX = 0;
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube((Vector2)transform.position+jumpBoxOffset, jumpBoxScale);
    }

    public void throwRope() {
        Instantiate(rope,new Vector3(Mathf.Floor(transform.position.x)+0.5f+dir,Mathf.Floor(transform.position.y),0),Quaternion.identity);
    }

    public void damagePlayer(float stunTime = .2f) {
        rb.constraints = RigidbodyConstraints2D.None;
        stunned = true;
        stunTimer = stunTime;
        onLadder = false;
        rb.gravityScale = 1;
        if (stunTime>1f) {
            rb.AddTorque(Random.Range(-5f,5f), ForceMode2D.Impulse);
        }
    }

    public void damagePlayer(Vector2 knockback, float stunTime = .2f) {
        rb.constraints = RigidbodyConstraints2D.None;
        stunned = true;
        stunTimer = stunTime;
        onLadder = false;
        rb.gravityScale = 1;
        rb.linearVelocity = knockback;
        if (stunTime>1f) {
            rb.AddTorque(Random.Range(-5f,5f), ForceMode2D.Impulse);
        }
    }

    public void setRecentLadder(GameObject ladder) {
        recentLadder = ladder;
    }
}
