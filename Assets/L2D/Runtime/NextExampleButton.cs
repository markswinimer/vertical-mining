using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextExampleButton : MonoBehaviour
{
    public void NextExample()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        index++;
        if (index == SceneManager.sceneCountInBuildSettings)
            index = 0;

        SceneManager.LoadScene(index);

    }
}
