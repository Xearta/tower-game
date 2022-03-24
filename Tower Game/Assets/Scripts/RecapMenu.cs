using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecapMenu : MonoBehaviour
{
    public void ToMenu()
    {
        SceneManager.LoadScene("Hub");
    }
}
