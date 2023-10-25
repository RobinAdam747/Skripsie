using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    // Variables:
    [SerializeField]
    private GameObject[] placeablePrefabs;      //array of prefabs (annotations) to be placed

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();   //dictionary to keep track of spawned prefabs
    private ARTrackedImageManager trackedImageManager;      //image manager object to access the tracked images set

    private void Awake()
    {
        //Assign the tracked image manager in the scene (component from AR Session Origin) to the variable
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        //Spawn one of each annotation in the scene but invisible (scale of Vector3.zero)
        foreach (GameObject prefab in  placeablePrefabs) 
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    //Subscribing and unsubscribing detected images
    private void ImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in args.removed)
        {
            //Search for item under same name in dictionary and disable it
            spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        //Select the prefab required to be spawned from the dictionary
        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

        //Disable the other prefabs when we look at a new image (might not be applicable, may want them all visible)
        foreach (GameObject go in spawnedPrefabs.Values)
        {
            if (go.name != name) 
            {
                go.SetActive(false);
            }
        }
    }
}
