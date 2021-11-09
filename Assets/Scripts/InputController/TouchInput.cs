using UnityEngine;

/// <summary>
/// Swipe Input script for Unity by @fonserbc, free to use wherever (originally SwipeInput)
/// Modified by Johnny Howe
/// 
/// Attack to one gameObject (perhaps game controller?), check the static booleans to check if a swipe
/// has been detected this frame
/// Eg: if (TouchInput.swipedRight) ...
/// 
/// if computerInput is true:
///	* Arrow keys make swipe movement
///	* Left mouse and space make touch
/// 
/// Contains static booleans:
/// * swipedRight,
/// * swipedLeft,
/// * swipedUp,
/// * swipedDown,
/// * isTouch: is the screen currently touched?,
/// * isTouchUp: was the screen touched last frame, but not now?,
/// * isTouchDown: is the screen touched now, but not last frame?,
/// </summary>
public class TouchInput : MonoBehaviour {

	// If the touch is longer than MAX_SWIPE_TIME, we dont consider it a swipe
	public const float MAX_SWIPE_TIME = 0.5f; 
	
	// Factor of the screen width that we consider a swipe
	// 0.17 works well for portrait mode 16:9 phone
	public const float MIN_SWIPE_DISTANCE = 0.17f;

	public static bool swipedRight = false;
	public static bool swipedLeft = false;
	public static bool swipedUp = false;
	public static bool swipedDown = false;

	public static bool isTouch = false;
	public static bool isTouchDown = false;
	public static bool isTouchUp = false;

	public bool computerInput = true;	// Use mouse click for touch and arrows for swipe

	Vector2 startPos;
	float startTime;

	public void Update()
	{
		swipedRight = false;
		swipedLeft = false;
		swipedUp = false;
		swipedDown = false;

		isTouch = false;
		isTouchUp = false;
		isTouchDown = false;

		if(Input.touchCount > 0)
		{
            isTouch = false;
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began)
			{
				startPos = new Vector2(t.position.x/(float)Screen.width, t.position.y/(float)Screen.width);
				startTime = Time.time;
				isTouchDown = true;
			}
			if(t.phase == TouchPhase.Ended)
			{
				isTouchUp = true;
				if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
					return;

				Vector2 endPos = new Vector2(t.position.x/(float)Screen.width, t.position.y/(float)Screen.width);

				Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

				if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
					return;

				if (Mathf.Abs (swipe.x) > Mathf.Abs (swipe.y)) { // Horizontal swipe
					if (swipe.x > 0) {
						swipedRight = true;
					}
					else {
						swipedLeft = true;
					}
				}
				else { // Vertical swipe
					if (swipe.y > 0) {
						swipedUp = true;
					}
					else {
						swipedDown = true;
					}
				}
			}
		} 

		if (computerInput) {
			swipedDown = swipedDown || Input.GetKeyDown (KeyCode.DownArrow);
			swipedUp = swipedUp|| Input.GetKeyDown (KeyCode.UpArrow);
			swipedRight = swipedRight || Input.GetKeyDown (KeyCode.RightArrow);
			swipedLeft = swipedLeft || Input.GetKeyDown (KeyCode.LeftArrow);
			isTouch = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) || isTouch;
			isTouchUp = Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space) || isTouchUp;
			isTouchDown = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || isTouchDown;
		}
	}
}