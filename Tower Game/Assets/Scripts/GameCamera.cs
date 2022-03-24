using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    private const float SCREEN_SIDE_BUTTONS = 50.0f;

    private Vector3 offset = new Vector3(0, 10, 20);
    private float currentX = 0.0f;
    private float sensitivity = 180.0f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x < SCREEN_SIDE_BUTTONS)
                currentX -= Time.deltaTime * sensitivity;
            else if (mousePos.x > Screen.width - SCREEN_SIDE_BUTTONS)
                currentX += Time.deltaTime * sensitivity;
        }

        Vector3 dir = offset;
        Quaternion rotation = Quaternion.Euler(0, currentX, 0);
        transform.position = rotation * dir;
        transform.LookAt(Vector3.up * (TheTower.Instance.GetTowerHeight() / 2));
    }
}
