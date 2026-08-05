using UnityEngine;
using UnityEngine.InputSystem;

public class BombBagLogic : ItemLogic
{
    public int bombsStored = 10;
    public GameObject bomb;

    public override void use() {
        if (bombsStored==0) return;
        if (holder.CompareTag("Player")) {
            bombsStored--;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 dir = mousePos-holder.transform.position;
            // float angle = Mathf.Atan2(dir.x,dir.y);
            GameObject iBomb = Instantiate(bomb, holder.transform.position, Quaternion.identity);
            // iBomb.GetComponent<Rigidbody2D>().linearVelocity = dir.normalized * holder.GetComponent<PlayerMovement>().throwStrength + holder.GetComponent<Rigidbody2D>().linearVelocity;
            iBomb.GetComponent<BombLogic>().interact(holder);
            iBomb.GetComponent<BombLogic>().use();
        }
    }

    public void grabBomb() {
        bombsStored++;
    }
}
