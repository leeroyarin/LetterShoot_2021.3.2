using System.Collections;
using UnityEngine;

internal interface IInteractableShooter
{
    void Fire();
    void LookAtPosition(Vector2 targetPosition);

    void EnableCannonLights(bool enable);

    void DisableHarpoonForWhile();
}