﻿using System.Collections;
using UnityEngine.SceneManagement;

public static class SceneUtil
{
    public static void LoadScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public static void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static IEnumerator LoadSceneAsync(string _name)
    {
        var async = SceneManager.LoadSceneAsync(_name);

        while(!async.isDone)
        {
            yield return null;
        }
    }
}