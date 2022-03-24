using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Fade Animation
    public GameObject uiRoot;
    private CanvasGroup uiRootGroup;
    private float fadeTransition;
    private bool menuAvailable = false;
    private bool fadeInTransition = false;
    private bool menuPoping = true;
    private float idleTimeToFade = 5.0f;
    private float lastTouchTime;

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

        uiRootGroup = uiRoot.GetComponent<CanvasGroup>();
        uiRootGroup.alpha = 0;
        uiRootGroup.interactable = false;
        fadeTransition = 0.0f;
    }

    private void Update()
    {
        MoveCamera();
        UpdateFade();

        if (Input.GetMouseButtonDown(0))
        {
            OnTouch();
        }
    }

    private void OnTouch()
    {
        lastTouchTime = Time.time;
        uiRootGroup.interactable = true;
        fadeInTransition = true;
        menuAvailable = true;
        menuPoping = true;
        fadeTransition = Mathf.Clamp(fadeTransition, 0, 1);
    }

    private void UpdateFade()
    {
        if (Time.time - lastTouchTime > idleTimeToFade)
        {
            fadeInTransition = true;
            menuPoping = false;
            menuAvailable = false;
            uiRootGroup.interactable = false;
        }

        if (!fadeInTransition)
            return;

        fadeTransition += (menuPoping) ? Time.deltaTime : -Time.deltaTime * 0.25f;

        uiRootGroup.alpha = fadeTransition;
        
        if (fadeTransition > 1 || fadeTransition < 0)
            fadeInTransition = false;
    }

    public void ToGame()
    {
        SceneManager.LoadScene("Hub");
    }

    public void AchievementButton()
    {
        Debug.Log("Achievement");
    }
    
    public void LeaderboardButton()
    {
        Debug.Log("Leaderboards");
    }

    public void SwapSaveButton()
    {
        Debug.Log("Swap Save");
    }

    public void ResetSaveButton()
    {
        Debug.Log("Reset Save");
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
