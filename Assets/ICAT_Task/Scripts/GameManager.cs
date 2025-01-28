using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class GameManager : MonoBehaviour
{
    [Header("UI Variables")]
    [SerializeField] Button menuButton;
    [SerializeField] GameObject UI_Menu;
    [SerializeField] private TMP_Text spawn_count;

    [Header("Time based variables")]
    [SerializeField] private float scaleDuration = 0.5f;
    private float lastTapTime = 0f;
    private float doubleTapThreshold = 0.3f;

    [Header("AR variables")]
    [SerializeField] private ARSessionOrigin arOrigin;
    [SerializeField] private ARRaycastManager _arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Pose placementPose;
    private ARPlane verticalPlane;
    
    [Header("Bool variables")]
    private bool placementIndicatorEnabled = true;
    private bool placementPoseIsValid = false;
    private bool isSingleTapInvoked = false;
    private bool isScaledUp = false;
    private bool pause = false;

    public int model_index { get; set; }
    public GameObject[] Model_Prefabs;
    public GameObject placementIndicator;
    [SerializeField] private Material planeMaterial;
    [SerializeField] private AudioSource audioSource;
    private Vector2 touchPosition;
    private GameObject spawnObject;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private int animation_Index { get; set; }
    private int spawnModelCount = 0;
   

    
    void Start()
    {
        // Load the saved spawnModelCount from PlayerPrefs
        spawnModelCount = PlayerPrefs.GetInt("SpawnModelCount", 0);
        spawn_count.text = $"Count: {spawnModelCount}";
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ToggleScale); //Adding a listener at the start, so when we click the menuButton ToggleScale fuction is called
        }
        ScaleUp();
    }

    public void ScaleUp()
    {
        if (UI_Menu == null || menuButton == null) return;
        pause = false;
        // Disable the button interaction
        menuButton.interactable = false;

        // Scale up using DOTween
        UI_Menu.transform.DOScale(Vector3.one, scaleDuration).OnComplete(() =>
        {
            isScaledUp = true;
            Debug.Log("Pause value is at scale down  : " + pause + " and model_Index is " + model_index + "and Animotion index : " + animation_Index);
            // Enable the button interaction once scaling is complete
            menuButton.interactable = true;
        });
    }

    public void ScaleDown()
    {
        if (UI_Menu == null || menuButton == null) return;
        pause = false;
        // Disable the button interaction
        menuButton.interactable = false;

        // Scale down using DOTween
        UI_Menu.transform.DOScale(Vector3.zero, scaleDuration).OnComplete(() =>
        {
            isScaledUp = false;
            pause = true;
            Debug.Log("Pause value is at scale down  : " + pause + " and model_Index is " + model_index);
            // Enable the button interaction once scaling is complete
            menuButton.interactable = true;
        });
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        _arRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneEstimated);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            // Get the plane alignment
            ARPlane plane = hits[0].trackable as ARPlane;
            if (plane != null)
            {
                if (plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
                {
                    // Detected a horizontal plane
                    animation_Index = 0;
                    Debug.Log("Horizontal plane detected.");
                    planeMaterial.color = Color.green;
                    placementIndicator.transform.rotation = Quaternion.Euler(90, 0, 0);
                    // Placeholder for horizontal plane actions
                }
                else if (plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.Vertical)
                {
                    // Detected a vertical plane
                    animation_Index = 1;
                    Debug.Log("Vertical plane detected.");
                    planeMaterial.color = Color.blue;
                    placementIndicator.transform.rotation = Quaternion.Euler(0, 0, 0);
                    // Store the vertical plane for later adjustment
                    verticalPlane = plane;
                }
            }
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }

    }
    public void ResetSpawnedObjects()
    {
        // Destroy all spawned objects
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        // Clear the list and reset the spawn count
        spawnedObjects.Clear();
        spawnModelCount = 0;
        PlayerPrefs.SetInt("SpawnModelCount", spawnModelCount);
        PlayerPrefs.Save();
        spawn_count.text = $"Count: {spawnModelCount}";
    }

    // Update is called once per frame
    void Update()
    {
        if (pause == true)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchPosition = touch.position;
                    // Check for double-tap
                    if (Time.time - lastTapTime <= doubleTapThreshold)
                    {
                        CancelInvoke(nameof(PerformSingleTapAction)); // Cancel any pending single-tap action
                        isSingleTapInvoked = false; // Reset flag for single-tap
                        PerformDoubleTapAction(); // Execute double-tap action
                    }
                    else
                    {
                        // Set the last tap time
                        lastTapTime = Time.time;

                        // Schedule single-tap action after the double-tap threshold
                        isSingleTapInvoked = true;
                        Invoke(nameof(PerformSingleTapAction), doubleTapThreshold);
                    }

                }
            }
            if (placementIndicatorEnabled == true && pause == true)
            {
                UpdatePlacementPose();
                UpdatePlacementIndicator();
            }
        }
    }

    void PerformSingleTapAction()
    {
        // Ensure no double-tap occurred in the meantime
        if (isSingleTapInvoked)
        {
            isSingleTapInvoked = false; // Reset flag
            Debug.Log("Single tap detected!");
            // Add your single-tap action here
            if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                //spawnObject = Instantiate(Model_Prefabs[model_index], hitPose.position, hitPose.rotation);
                spawnObject = Instantiate(Model_Prefabs[model_index], placementPose.position, placementPose.rotation);
                spawnObject.transform.Rotate(0, 180, 0);
                // Get the Animator component from the spawned object
                //Animator animator = spawnObject.GetComponent<Animator>();
                Animator animator = spawnObject.GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    // Set the bool parameter based on model_index
                    bool value = animation_Index == 0;
                    animator.SetBool("Walk", value);
                }
                spawnedObjects.Add(spawnObject);
                spawnModelCount++;
                // Update TMP_Text with the new spawn count
                spawn_count.text = $"Count: {spawnModelCount}";

                // Save the updated count to PlayerPrefs
                PlayerPrefs.SetInt("SpawnModelCount", spawnModelCount);
                PlayerPrefs.Save(); // Ensure changes are written to disk
                // Adjust the object if the detected plane is vertical
                if (animation_Index == 1 && verticalPlane != null)
                {
                    AdjustSpawnObjectToVerticalPlane(spawnObject, verticalPlane);
                }
            }

        }
    }

    void PerformDoubleTapAction()
    {
        Debug.Log("Double tap detected!");
        // Add your double-tap action here
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"Hit object: {hit.collider.gameObject.name}");

            // Check if the hit object has a BodyPartDetection component
            BodyPartDetection bodyPartDetection = hit.collider.GetComponent<BodyPartDetection>();
            if (bodyPartDetection != null)
            {
                // Access the assignedBodyPart enum
                BodyPartDetection.BodyPart bodyPart = bodyPartDetection.assignedBodyPart;
               

                // Update the TMP_Text with the name of the body part
                if (bodyPartDetection.bodyPartName != null)
                {
                    bodyPartDetection.bodyPartName.text = bodyPart.ToString();
                }
                if (audioSource != null && bodyPartDetection.audioClip != null)
                {
                    audioSource.clip = bodyPartDetection.audioClip;
                    audioSource.PlayOneShot(audioSource.clip);
                }
            }
        }

    }
    private void AdjustSpawnObjectToVerticalPlane(GameObject spawnObject, ARPlane plane)
    {
        // Check if the plane is valid
        if (plane == null)
        {
            Debug.LogWarning("Plane is null. Cannot adjust spawnObject.");
            return;
        }

        // Get the size of the plane
        Vector2 planeSize = plane.size; // plane.size gives the width and height of the plane in meters.

        // Get the bounds of the spawnObject (including its children)
        Renderer[] renderers = spawnObject.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(spawnObject.transform.position, Vector3.zero);

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        // Calculate the scale factor to fit the object within the plane's size
        float scaleFactor = Mathf.Min(planeSize.x / bounds.size.x, planeSize.y / bounds.size.y);

        // Apply the scale factor to the parent object
        spawnObject.transform.localScale *= scaleFactor;

        Debug.Log($"Adjusted spawnObject to fit within the vertical plane: ScaleFactor={scaleFactor}");
    }

    public void ToggleScale()
    {
        if (isScaledUp)
        {
            ScaleDown(); // If scaled up, scale down
        }
        else
        {
            ScaleUp(); // If scaled down, scale up
        }
    }
}
