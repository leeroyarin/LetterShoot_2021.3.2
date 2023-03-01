using UnityEngine;

public abstract class InputType
{
    protected Vector3 previousPosition;
    public abstract void InputAction(InputManager inputManager);

    protected void OnRelease(Vector2 position,InputManager inputManager)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, 1f);
        if (hit?.tag == "Shooter")
        {
            Debug.Log("OK");
            inputManager.ChangeShooter(hit);
            return;
        }
        if (inputManager.HasShooter)
        {
            inputManager.GetShooterToFire();
        }
    }
}
