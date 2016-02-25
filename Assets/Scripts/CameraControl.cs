using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	Ray ray;
	RaycastHit hit;
	
	public GameObject Focus;
	// Use this for initialization
	void Start () {
		//Cursor.visible = false;
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		ray=Camera.main.ScreenPointToRay(Input.mousePosition);
		/*

		if (Input.GetButton("X") || Input.GetKey(KeyCode.Keypad4))
		{
			for (int i = 0; i < 3; i++)
			{
				GameObject obj = Instantiate(prefab1, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity) as GameObject;
			}
		}
		
		if (Input.GetButton("Y") || Input.GetKey(KeyCode.Keypad5))
		{
			for (int i = 0; i < 3; i++)
			{
				GameObject obj = Instantiate(prefab2, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity) as GameObject;
			}
		}

		if (Input.GetButton("B") || Input.GetKey(KeyCode.Keypad6))
		{
			for (int i = 0; i < 3; i++)
			{
				GameObject obj = Instantiate(prefab3, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity) as GameObject;
			}
		}
		
		if (Input.GetButton("A") || Input.GetKey(KeyCode.Keypad8))
		{
			for (int i = 0; i < 3; i++)
			{
				GameObject obj = Instantiate(prefab4, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity) as GameObject;
			}
		}
		
		*/

		if (Input.GetAxis("LeftJoyStickY")>0.1 || Input.GetKey("w"))
		{
			this.transform.position -= this.transform.up;
		}
		
		if(Input.GetAxis("LeftJoyStickY")<-0.1 || Input.GetKey("s"))
		{
			this.transform.position += this.transform.up;
		}
		
		if(Input.GetAxis("LeftJoyStickX")>0.1 || Input.GetKey("d"))
		{
			this.transform.position += this.transform.right;
		}
		
		if(Input.GetAxis("LeftJoyStickX")<-0.1 || Input.GetKey("a"))
		{
			this.transform.position -= this.transform.right;
		}
		
		if(Input.GetAxis("Triggers")>0.1 || Input.GetKey(KeyCode.LeftShift))
		{
			this.transform.position -= this.transform.forward;
		}
		
		if(Input.GetAxis("Triggers")<-0.1 || Input.GetKey(KeyCode.Space))
		{
			this.transform.position += this.transform.forward;
		}
		
		/*
		if(Input.GetKey(KeyCode.Escape))
		{
			EditorApplication.Exit(0);
		}
*/
		transform.LookAt (Focus.transform,transform.up);
		
		
	}}