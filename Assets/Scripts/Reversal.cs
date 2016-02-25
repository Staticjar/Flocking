using UnityEngine;
using System.Collections;

public class Reversal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	
	}

    void OnTriggerExit(Collider thing)
    {
        Rigidbody body = thing.GetComponent<Rigidbody>();
        body.velocity *= -1;
    }
}
