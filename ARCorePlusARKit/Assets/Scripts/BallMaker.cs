using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.ObjectManipulation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;

public class BallMaker : Manipulator
{
	public GameObject andyPrefab;
    public GameObject manipulatorPrefab;
	public float createHeight;
	public float maxRayDistance = 30.0f;
	public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

    public Camera mainCamera;

	protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if (gesture.TargetObject == null)
        {
            return true;
        }

        return false;
    }
    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.WasCancelled)
        {
            return;
        }

        // If gesture is targeting an existing object we are done.
        if (gesture.TargetObject != null)
        {
            return;
        }

        if (IsPointerOverUiObject(gesture))
            return;

        var screenPosition = mainCamera.ScreenToViewportPoint(gesture.StartPosition);
        ARPoint point = new ARPoint
        {
            x = screenPosition.x,
            y = screenPosition.y
        };

        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point,
            ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
        if (hitResults.Count > 0)
        {
            foreach (var hitResult in hitResults)
            {
                Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);

                CreateObject(position);

                break;
            }
        }
    }

    private static bool IsPointerOverUiObject(TapGesture gesture)
    {
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(gesture.StartPosition.x, gesture.StartPosition.y)
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

        manipulator.GetComponent<Manipulator>().Select();
	}

}
