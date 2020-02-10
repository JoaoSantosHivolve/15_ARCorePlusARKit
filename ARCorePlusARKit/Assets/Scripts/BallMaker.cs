using System.Collections.Generic;
using GoogleARCore.Examples.ObjectManipulation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;

public class BallMaker : MonoBehaviour
{
    public GameObject andyPrefab;
    public GameObject manipulatorPrefab;
    public float createHeight;
    public float maxRayDistance = 30.0f;
    public LayerMask collisionLayer = 1 << 10; //ARKitPlane layer

    public Camera mainCamera;

    void Update()
    {
		if (Input.touchCount > 0 )
		{
			var touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
				ARPoint point = new ARPoint {
					x = screenPosition.x,
					y = screenPosition.y
				};

                if (IsPointerOverObject(touch.position))
                    return;

                if (IsPointerOverUiObject(touch.position))
                    return;

						
				List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, 
					ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
				if (hitResults.Count > 0) {
					foreach (var hitResult in hitResults) {
						Vector3 position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
						CreateObject (new Vector3 (position.x, position.y + createHeight, position.z));
						break;
					}
				}

			}
		}
    }

    private bool IsPointerOverObject(Vector2 position)
    {
        Ray ray = mainCamera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out var hit, 100))
        {
            if (hit.transform.CompareTag("Andy"))
                return true;
        }

        return false;
    }

    private static bool IsPointerOverUiObject(Vector2 touchPos)
    {
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = touchPos
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void CreateObject(Vector3 position)
    {

        var andy = Instantiate(andyPrefab, position, Quaternion.identity);

        var manipulator = Instantiate(manipulatorPrefab, position, Quaternion.identity);

        andy.transform.parent = manipulator.transform;

        var anchor = new GameObject();
        anchor.transform.position = position;

        manipulator.transform.parent = anchor.transform;

        manipulator.GetComponent<Manipulator>().Select();
    }
}