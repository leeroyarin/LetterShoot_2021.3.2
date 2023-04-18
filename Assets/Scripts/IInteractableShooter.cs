using System.Collections;
using UnityEngine;

internal interface IInteractableShooter
{
    void Fire();
    void LookAtPosition(Vector2 targetPosition);

    void EnableHarpoonLights(bool enable);

    void DisableHarpoonForWhile();

    void EnableLightAfterSecondsCoroutine(float time,bool enable);

    void EnableAction(bool enable);
}