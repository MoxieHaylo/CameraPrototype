using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImageManager : MonoBehaviour
{
    public TakePhoto takePhoto;
    //public TMP_Text confirmationText;
    public bool replacingImage = true;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            ConfirmReplace();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            CancelReplace();
        }
    }

    public void ConfirmReplace()
    {

    }

    public void CancelReplace()
    {

    }
}
