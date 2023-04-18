using UnityEngine;

public class ComputerInput : InputType
{
    public override void InputAction(InputManager p_inputManager)
    {
        ///<summary>
        ///Gets the mousePosition 
        ///Makes the currently activated harpoon shooter to look towards the mouse position
        ///If the mouse gets clicked then necessary action is done
        /// </summary>
        Vector2 l_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (p_inputManager.hasActiveHarpoonShooter) p_inputManager.GetHarpoonShooterToLookAt(l_mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            GetAction(p_position:l_mousePosition, p_inputManager: p_inputManager);
        }
    }
    void GetAction(Vector2 p_position, InputManager p_inputManager)
    {
        ///<summary>
        ///checks if there is any harpoon shooter in currently clicked position
        ///if yes the clicked shooter gets activated instead
        ///makes the harpoon to launch hook if it shooter is active
        ///</summary>
        Collider2D l_hit = Physics2D.OverlapCircle(p_position, 0.3f);
        if (l_hit?.tag == "Shooter")
        {
            p_inputManager.ChangeCurrentlyActiveHarpoonShooter(l_hit);
            return;
        }
        if (p_inputManager.hasActiveHarpoonShooter)
        {
            p_inputManager.GetHarpoonShooterToLaunchHook();
        }
    }
}
