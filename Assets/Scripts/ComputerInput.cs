using UnityEngine;

public class ComputerInput : InputType
{
    public override void InputAction(InputManager inputManager)
    {
        /*
         * if the screen mouse position is moved then it helps to look at something
         * checks if the 
         * if the mouse button is clicked then onrelease function is callled that is  responsible for checking if the touch position has shooter to change with or is it empty space to shoot at
         */
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (inputManager.HasShooter) inputManager.GetShooterToLookAt(mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            OnRelease(mousePosition, inputManager);
        }
    }
}
