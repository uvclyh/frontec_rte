using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    Transform _root;

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@GameManager" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }



}

