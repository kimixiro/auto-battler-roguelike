using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDefeat : MonoBehaviour
{
    public GameObject window;
    public void ShowDefeat()
    {
        window.SetActive(true);
    }


    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    
}
