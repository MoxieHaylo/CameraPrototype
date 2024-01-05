using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject interactableWithE;
    [SerializeField] private PlayerInput playerInput;

    private void Update()
    {
        if(playerInput.GetObjectInteraction() !=null)
            {
            ShowInteractable();
            }
        else
        {
            HideInteractable();
        }
    }
    private void ShowInteractable()
    {
        interactableWithE.SetActive(true);
    }
    private void HideInteractable()
    {
        interactableWithE.SetActive(false);
    }
}
