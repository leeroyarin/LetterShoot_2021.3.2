using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterEndPoint : MonoBehaviour
{
    [SerializeField] LetterManager letterManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("TrainHead")|| collision.tag == "TrainHead")
        {
            collision.GetComponent<TrainHead>().stop = true;
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("Letter"))
        {
            letterManager.AddLetterObjectToTheList(collision.gameObject);
        }
    }
}
