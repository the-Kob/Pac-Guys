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
        SceneManager.LoadScene(2);
    }
}
