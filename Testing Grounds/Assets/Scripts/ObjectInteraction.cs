using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public void Interact()
    {
        if(gameObject.CompareTag("SnapInt"))
            {
            Debug.Log("I am an interactable camera");
            }
        if (gameObject.CompareTag("CompInt"))
        {
            Debug.Log("I am a laptop");
        }

    }
}
