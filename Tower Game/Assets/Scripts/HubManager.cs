using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubManager : MonoBehaviour
{
    public static HubManager Instance{ set; get; }

    // HubObject
    public Button backButton;
    private HubObject currentHubObject;

    // Camera
    public Transform defaultCameraWaypoint;
    private Transform camTransform;
    private Vector3 desiredPosition;
    private Quaternion desiredRotation;


    // Start is called before the first frame update
    void Start()
    {
        backButton.interactable = false;
        camTransform = Camera.main.transform;
        SetDesiredWaypoint(defaultCameraWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 75.0f, LayerMask.GetMask("HubObject")))
            {
                currentHubObject = hit.transform.GetComponent<HubObject>();
                currentHubObject.FadeMenu(true);
                SetDesiredWaypoint(currentHubObject.waypoint);
                backButton.interactable = true;
            }
        }
    }

    private void MoveCamera()
    {
        camTransform.position = Vector3.Lerp(camTransform.position, desiredPosition, Time.deltaTime);
        camTransform.rotation = Quaternion.Lerp(camTransform.rotation, desiredRotation, Time.deltaTime);
    }

    public void SetDesiredWaypoint(Transform waypoint)
    {
        desiredPosition = waypoint.position;
        desiredRotation = waypoint.rotation;
    }

    public void DropMenu()
    {
        if (currentHubObject == null)
            return;

        currentHubObject.FadeMenu(false);
        currentHubObject = null;
        backButton.interactable = false;
        SetDesiredWaypoint(defaultCameraWaypoint);
    }
}
