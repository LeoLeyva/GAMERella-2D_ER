using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
  // Platform 
  [SerializeField] private GameObject thePlatform;

  [SerializeField] private GameObject[] thePlatforms;

  // New Platforms 
  [SerializeField] private Transform generationPoint;
  private int platformSelector;

  // Distance 
  [SerializeField] private float distanceBetween;
  [SerializeField] private float distanceBetweenMin;
  [SerializeField] private float distanceBetweenMax;

  // Platform Widths 
  private float platformWidth;
  private float[] platformWidths;

  // Object Pool 
  [SerializeField] private ObjectPooler theObjectPool;


  // Start is called before the first frame update
  private void Start()
  {
    // platformWidth = thePlatform.GetComponent<BoxCollider2D>().size.x;
    platformWidths = new float[thePlatforms.Length];
    for (int i = 0; i < thePlatforms.Length; i++)
    {
      platformWidths[i] = thePlatforms[i].GetComponent<BoxCollider2D>().size.x;
    }
  }

  // Update is called once per frame
  private void Update()
  {
    if (transform.position.x < generationPoint.position.x)
    {
      distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax);
      platformSelector = Random.Range(0, thePlatforms.Length);

      transform.position = new Vector3(transform.position.x + platformWidths[platformSelector] + distanceBetween, transform.position.y, transform.position.z);

      Instantiate(thePlatforms[platformSelector], transform.position, transform.rotation);

      //   GameObject newPlatform = theObjectPool.GetPooledObject();
      //   newPlatform.transform.position = transform.position;
      //   newPlatform.transform.rotation = transform.rotation;
      //   newPlatform.SetActive(true);

    }
  }
}
