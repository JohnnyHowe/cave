using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Singleton<InputController>
{
    public SwipeDirection GetSwipeDirection()
    {
        if (TouchInput.swipedUp)
        {
            return SwipeDirection.up;
        }
        else if (TouchInput.swipedDown)
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