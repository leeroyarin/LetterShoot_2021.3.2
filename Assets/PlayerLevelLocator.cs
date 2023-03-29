using PathCreation;
using System.Collections;
using UnityEngine;

public class PlayerLevelLocator: MonoBehaviour
{
    [SerializeField] PathCreator pathCreator;
    float moveDistance = 0;
    [SerializeField] float speed;
    bool moveable = false;
    [SerializeField] Vector3 offset;

    Vector3 previousScale;
    private void Start()
    {
        StartCoroutine(MovePick());
    }
    public bool OnMoveToNext(bool right)
    {
        if (moveable) return false;
        moveable = true;
        speed = Mathf.Sqrt(speed*speed);
        speed *= right ? 1 : -1;
        return true;
    }

    IEnumerator MovePick()
    {
        yield return new WaitWhile(() => pathCreator == null);
        while (gameObject.activeInHierarchy)
        {
            if (moveable)
            {
                moveDistance += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(moveDistance) + offset;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Level")
        {
            moveable = false;
            previousScale = collision.transform.localScale;
            collision.transform.localScale = previousScale + (previousScale * 0.30f);
        }
    }

    public void SetPositionOfPlayerLevelLocator(float distance)
    {
        moveDistance = distance;
        transform.position = pathCreator.path.GetPointAtDistance(moveDistance)+offset;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Level"))
        {
            collision.transform.localScale = previousScale;
        }
    }
}