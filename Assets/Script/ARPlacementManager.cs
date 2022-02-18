using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{
    public Camera ARCamera;

    public GameObject battleArenaGameObject;

    ARRaycastManager m_ARRaycastManager;
    static List<ARRaycastHit> raycast_Hits = new List<ARRaycastHit> ();


    private void Awake()
    {
        m_ARRaycastManager = GetComponent<ARRaycastManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = ARCamera.ScreenPointToRay(centerOfScreen);

        if (m_ARRaycastManager.Raycast(ray,raycast_Hits, TrackableType.PlaneWithinPolygon))
        {
            //Intersection!
            Pose hitPose = raycast_Hits[0].pose;

            Vector3 positionToBePlaced = hitPose.position;

            battleArenaGameObject.transform.position = positionToBePlaced;
        }
    }
}
