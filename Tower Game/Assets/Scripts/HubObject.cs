using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubObject : MonoBehaviour
{
    public Transform waypoint;
    public GameObject menu;

    private CanvasGroup cGroup;
    private float transition = 0.0f;
    private bool inTransition = false;
    private bool isPoping = true;


    // Start is called before the first frame update
    void Start()
    {
        cGroup = menu.GetComponent<CanvasGroup>();
        cGroup.alpha = 0;
        cGroup.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transition < 0 || transition > 1)
            inTransition = false;
        
        if (!inTransition)
            return;

        transition += (isPoping) ? Time.deltaTime : -Time.deltaTime;
        cGroup.alpha = transition;
    }

    public void FadeMenu(bool show)
    {
        isPoping = show;
        cGroup.interactable = show;
        inTransition = true;
        transition = Mathf.Clamp(transition, 0, 1);
    }
}
