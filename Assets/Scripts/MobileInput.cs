using UnityEngine;

public class MobileInput : InputType
{
    public override void InputAction(InputManager inputManager)
    {
        /*
         * if the screen touch is moved then it helps to look at something
         * if the screen touch is released then onrelease function is callled that is  responsible for checking if the touch position has shooter to change with or is it empty space to shoot at
         */
        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        if(touch.phase == TouchPhase.Moved)
        {
            if(inputManager.HasShooter) inputManager.GetShooterToLookAt(touchPosition);
        }
        if(touch.phase == TouchPhase.Ended)
        {
            OnRelease(touchPosition, inputManager);
        }
    }
}
