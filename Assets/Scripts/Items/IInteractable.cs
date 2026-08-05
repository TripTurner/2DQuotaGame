using UnityEngine;

public interface IInteractable
{
    int Priority{get;}
    GameObject GameObject {get;}
    bool getHeld();
    void interact(GameObject other);
    void drop();
    void drop(Vector2 addVel);
    void store();
    void use();
}
