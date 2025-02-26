using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public enum SceneName
    {
        Start, Play, End
    }

    public SceneName nextSceneName;

    public void NextScene()
    {
        SceneManager.LoadSceneAsync((int)nextSceneName, LoadSceneMode.Single);
    }
}
