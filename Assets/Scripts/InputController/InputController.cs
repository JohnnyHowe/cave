using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Singleton<InputController>
{
    public SwipeDirection GetSwipeDirection()
    {
        if (Input.GetKeyDown("w"))
        {
            return SwipeDirection.up;
        }
        else if (Input.GetKeyDown("s"))
        {
            return SwipeDirection.down;
        }
        else
        {
            return SwipeDirection.none;
        }
    }
}

public enum SwipeDirection
{
    up,
    down,
    none,
}