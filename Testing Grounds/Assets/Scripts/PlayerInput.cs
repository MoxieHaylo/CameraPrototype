using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerInput : MonoBehaviour
{
    public float moveSpeed = 5f;
    public CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        // If the variable "controller" is empty...
        if (controller == null)
        {
            // ...then this searches the components on the gameobject and gets a reference to the CharacterController class
            controller = GetComponent<CharacterController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // This takes the Left/Right and Forward/Back values to build a vector
        Vector3 move = transform.right * x + transform.forward * z;

        // Finally, it applies that vector it just made to the character
        controller.Move(move * moveSpeed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 1f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
                if (collider.TryGetComponent(out ObjectInteraction objectInteraction))
                {
                    objectInteraction.Interact();
                }
        }
    }

    public ObjectInteraction GetObjectInteraction()
    {
        float interactRange = 1f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out ObjectInteraction objectInteraction))
            {
                return objectInteraction;
            }
        }
        return null;
    }
}
