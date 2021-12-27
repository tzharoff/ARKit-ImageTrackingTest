using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageAnchor : MonoBehaviour
{
    [SerializeField] private GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    private void Awake() {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in placeablePrefabs){
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
    }

    private void OnEnable() {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach(ARTrackedImage trackedImage in obj.added){
            UpdateImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in obj.updated){
            UpdateImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in obj.removed){
            spawnedPrefabs[trackedImage.name].SetActive(false);
        }

    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        if(!prefab.activeInHierarchy){
            prefab.SetActive(true);
        }

        foreach(GameObject goTemp in spawnedPrefabs.Values){
            if(goTemp.name != name && !goTemp.activeInHierarchy){
                goTemp.SetActive(false);
            }
        }
    }

    private void OnDisable(){
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
