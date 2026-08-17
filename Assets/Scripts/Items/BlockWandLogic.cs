using UnityEngine;
using UnityEngine.InputSystem;

public class BlockWandLogic : ItemLogic
{
    private bool playerHolding = false;
    [SerializeField] private float wandLength;
    private TileDestroyer world;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        world = GameObject.FindWithTag("World").GetComponent<TileDestroyer>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (held) {
            if (stored) {
                transform.position = new Vector2(0,10);
            } else {
                if (playerHolding) {
                    Vector3 centerPos = holder.transform.position + new Vector3(0,0,-0.5f);
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    Vector2 dir = ((Vector2)mousePos-(Vector2)centerPos).normalized;
                    transform.position = centerPos + (Vector3)(dir*wandLength);
                    float angle = Mathf.Atan2(transform.position.y - mousePos.y, transform.position.x - mousePos.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0,0,angle-90);
                } else {
                    transform.position = holder.transform.position + new Vector3(0,0,-0.5f);
                }
            }
        }
    }

    public override void interact(GameObject other) {
        base.interact(other);
        if (other.CompareTag("Player")) {
            playerHolding = true;
        }
    }

    public override void drop() {
        transform.position = holder.transform.position;
        base.drop();
        playerHolding = false;
    }

    public override void drop(Vector2 addVel) {
        transform.position = holder.transform.position;
        base.drop(addVel);
        playerHolding = false;
    }

    public override void use() {
        Vector3 centerPos = holder.transform.position + new Vector3(0,0,-0.5f);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = ((Vector2)mousePos-(Vector2)centerPos).normalized;
        Vector2 checkPos = (Vector2)transform.position + dir*wandLength;
        if (!world.cellTypeAt(checkPos, "normal")) {
            world.switchTile(checkPos, "normal", false);
        }
    }
}
