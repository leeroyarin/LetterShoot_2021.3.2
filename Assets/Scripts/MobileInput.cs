using UnityEngine;

public class MobileInput : InputType
{
    public override void InputAction(InputManager p_inputManager)
    {
        ///<summary>
        ///First if touch count is more than one the functoin actually works
        ///Then if touch phase is began then checks if there is any possible harpoon shooter it gets activated by deactivating current one
        ///Incase the touch phase is on moving it makes the shooter to loook at the touchedPosition without releasing the harpoon
        ///and if the touch phase is on ended state it first checks if it hit/touched any shooter 
        ///if the shooter hit the shooter the function just stops
        ///if no then then it is checked if there is any shooter activated or not
        ///if it has shooter it makes the harpoon shooter to look at the touched position and launches the harpoon hook aka fires the hook
        /// </summary>
        if (Input.touchCount > 0)
        {
            //gets detail of the touch input
            Touch l_touch = Input.GetTouch(0);

            //gets the current touch position
            Vector2 l_touchPosition = Camera.main.ScreenToWorldPoint(l_touch.position);

            //checks if the touch phase is in began state or moving state or ended state
            switch (l_touch.phase)
            {
                case TouchPhase.Began:

                    //check if there is any possible harpoon shooter at touchPosition
                    Collider2D l_hit = Physics2D.OverlapCircle(l_touchPosition, 0.3f);
                    if (l_hit?.tag == "Shooter")
                    {
                        p_inputManager.ChangeCurrentlyActiveHarpoonShooter(l_hit);
                        return;
                    }
                    break;
                case TouchPhase.Moved:
                    //makes the harpoon shooter to look at touch position
                    if (p_inputManager.hasActiveHarpoonShooter) p_inputManager.GetHarpoonShooterToLookAt(l_touchPosition);
                    break;
                case TouchPhase.Ended:

                    //makes the harpoon unshootable towards the another shooter that is currently at touch position
                    Collider2D l_endHit = Physics2D.OverlapCircle(l_touchPosition, 0.3f);
                    if (l_endHit?.tag == "Shooter")
                    {
                        return;
                    }
                    //mkaes the shooter to look at the currently touched position and shoot at the direction of touched position
                    if (p_inputManager.hasActiveHarpoonShooter)
                    {
                        p_inputManager.GetHarpoonShooterToLookAt(l_touchPosition);
                        p_inputManager.GetHarpoonShooterToLaunchHook();
                    }
                    break ;
                default:
                    break;
            }
        }
           
    }
}
