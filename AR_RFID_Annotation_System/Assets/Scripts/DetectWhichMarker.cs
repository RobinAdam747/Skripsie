using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DetectWhichMarker : MonoBehaviour
{
    //Variables:
    ARTrackedImageManager imageManager;
    public ARClient arClientScript;

    // Start is called before the first frame update
    void Start()
    {
        arClientScript = GameObject.FindGameObjectWithTag("scriptObject").GetComponent<ARClient>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        imageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            arClientScript.scannedMarker = trackedImage;
        }
    }
}
