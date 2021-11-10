using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Allows easy access to screen rect in game units.
    /// 
    /// rect center is just camera position
    /// width and height are window width and height in game units.
    /// 
    /// To work, this must be attached to ONE gameObject
    /// 
    /// rect accessed through ScreenHelper.GameBounds
    /// </summary>
    public class ScreenHelper : MonoBehaviour
    {
        public static Rect GameBounds;

        private void Awake()
        {
            UpdateAll();
        }

        private void Update()
        {
            UpdateAll();
        }
        void UpdateAll()
        {
            GameBounds.height = Camera.main.orthographicSize * 2.0f;
            GameBounds.width = GameBounds.height * Screen.width / Screen.height;
            GameBounds.center = Camera.main.transform.position;
        }

        public static bool IsRendererOnScreen(GameObject obj)
        {
            return IsRendererOnScreen(obj.GetComponent<SpriteRenderer>());
        }

        public static bool IsRendererOnScreen(SpriteRenderer renderer)
        {
            return renderer.bounds.max.y > GameBounds.yMin &&
                renderer.bounds.min.y < GameBounds.yMax &&
                renderer.bounds.max.x > GameBounds.xMin &&
                renderer.bounds.min.x < GameBounds.xMax;
        }
    }
}