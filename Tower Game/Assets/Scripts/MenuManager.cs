using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Camera
    private Transform camTransform;
    private Vector3 offset;
    private float transition = 0.0f;
    private float cameraSpeed = 20.5f;


    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        offset = CalculateCameraOffset();
    }

    private void Update()
    {
        MoveCamera();
    }

    private Vector3 CalculateCameraOffset()
    {
        Vector3 r = Vector3.zero;

        r.y = TheTower.Instance.GetTowerHeight() / 2;
        r.z = r.y + TheTower.Instance.GetTowerWidth() + 3.0f;

        return r;
    }

    private void MoveCamera()
    {
        float y = Mathf.Sin(Time.time) * 0.25f;
        transition += Time.deltaTime * cameraSpeed;
        Vector3 desiredPos = offset + (Vector3.up * y);
        Quaternion orientation = Quaternion.Euler(0, transition, 0);
        camTransform.position = orientation * desiredPos;
        camTransform.LookAt(Vector3.up * camTransform.position.y);
    }
}
