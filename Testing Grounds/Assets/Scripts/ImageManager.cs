using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImageManager : MonoBehaviour
{
    public TakePhoto takePhoto;
    //public TMP_Text confirmationText;
    public bool isReplaceConfirmed = false;

    public void ConfirmReplace()
    {
        isReplaceConfirmed = true;
        //confirmationText.text = "Replace confirmed";

    }

    public void CancelReplace()
    {
        isReplaceConfirmed = false;
        //confirmationText.text = "Replace canceled";
    }
}
