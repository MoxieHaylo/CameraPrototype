using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TakePhoto : MonoBehaviour
{
    public ImageManager imageManager;
    [Tooltip("Insert the Photography Camera")]
    public Camera photoCamera;
    public GameObject reticle;
    public float minZoom = 1.0f;
    public float maxZoom = 10.0f;
    public float zoomSpeed = 2.0f;
    public float currentZoom;
    private float startingZoom = 50f;

    public Transform anchor;
    public float lookSensitivity = 200f;
    float xRotation = 0f;
    private string speciesName;

    private bool isChoosing = false;
    private bool hasMadeChoice = false;
    public bool replacingScreenshot = true;
    public GameObject choiceUI;

    private void Start()
    {
        photoCamera = Camera.main;
        currentZoom = startingZoom;
        Cursor.lockState = CursorLockMode.Locked;
        choiceUI.SetActive(false);
    }

    private void Update()
    {
        if (!isChoosing)
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            anchor.Rotate(Vector3.up * mouseX);

            float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
            currentZoom -= scrollWheelInput * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            CameraZoom();

            if (Input.GetKeyDown(KeyCode.Space) && !isChoosing)
            {
                CaptureScreenshotWithObjectName();
            }
        }
    }

    public void CaptureScreenshotWithObjectName()
    {
        string objectName = "Unknown"; // Default value if no object name is found

        // Check if the camera is looking at an object with an ObjectIdentifier script
        RaycastHit hit;
        if (Physics.Raycast(photoCamera.transform.position, photoCamera.transform.forward, out hit))
        {
            ObjectIdentifier objectIdentifier = hit.transform.GetComponent<ObjectIdentifier>();
            if (objectIdentifier != null)
            {
                objectName = objectIdentifier.speciesName;
            }
        }

        StartCoroutine(CaptureScreenshot(objectName));
    }

    private IEnumerator CaptureScreenshot(string objectName)
    {
        Debug.Log("Click");
        reticle.SetActive(false);
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);

        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();
        string filePath = Application.dataPath + "/Images/" + objectName + ".png";
        Directory.CreateDirectory(Application.dataPath + "/Images");

        if (!File.Exists(filePath))
        {
            Debug.Log($"{filePath} exists");
            Debug.Log("I'm new");
            File.WriteAllBytes(filePath, byteArray);
            Debug.Log("Screenshot saved as: " + filePath);
            reticle.SetActive(true);
            yield break;
        }

        StartCoroutine(ShowDecisionUI());
        isChoosing = true;
        while (isChoosing)
        {
            yield return null;

            if (hasMadeChoice)
            {
                if (replacingScreenshot)
                {
                    File.WriteAllBytes(filePath, byteArray);
                    Debug.Log("Replaced");
                    replacingScreenshot = false;
                    break;
                }
                else
                {
                    Debug.Log("Screenshot remained");
                    break;
                }
            }
        }
    }

    private void CameraZoom()
    {
        if (photoCamera.orthographic)
        {
            photoCamera.orthographicSize = currentZoom;
        }
        else
        {
            photoCamera.fieldOfView = currentZoom;
        }
    }

    private IEnumerator ShowDecisionUI()
    {
        yield return new WaitForEndOfFrame();
        reticle.SetActive(false);
        choiceUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private IEnumerator HideDecisionUI()
    {
        yield return new WaitForEndOfFrame();
        reticle.SetActive(true);
        choiceUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        choiceUI.SetActive(false);
        replacingScreenshot = false;
        isChoosing = false;
        hasMadeChoice = false;
    }
    void OnYesButtonClick()
    {
        replacingScreenshot = true;
        Debug.Log("Clicked YES");
        hasMadeChoice = true;
        StartCoroutine(HideDecisionUI());
    }

    public void OnNoButtonClick()
    {
        replacingScreenshot = false;
        Debug.Log("Clicked NAH");
        hasMadeChoice = true;
        StartCoroutine(HideDecisionUI());
    }
}
