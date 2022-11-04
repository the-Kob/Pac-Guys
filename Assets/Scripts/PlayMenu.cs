using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    public void PlayGrass()
    {
        SceneManager.LoadScene(1);
    }

    public void PlaySpooky()
    {
        SceneManager.LoadScene(3);
    }

    public void PlayGDT()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayWinter()
    {
        SceneManager.LoadScene(4);
    }

}
