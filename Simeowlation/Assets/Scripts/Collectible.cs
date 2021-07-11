using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
// private ScoreManager theScoreManager; 

{
  [SerializeField] private int scoreToGive;

  // Start is called before the first frame update
  void Start()
  {
    // theScoreManager = FindObjectOfType<ScoreManager>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      //   theScoreManager.scoreCount += scoreToGive;
      //   this.gameObject.SetActive(false);
    }
  }
}
