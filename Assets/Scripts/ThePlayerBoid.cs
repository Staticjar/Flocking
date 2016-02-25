using UnityEngine;
using System.Collections;

public class ThePlayerBoid : MonoBehaviour {

    public Rigidbody thisBird;
    public float thrust = 0.2f;
    public float accel = 0.1f;
    public float minSpd = 0.2f;
    public float maxSpd = 1.5f;
    public float lift;
    public float pGravity;
    public float mouseSensitivity = 2.0f;
    public Animator anim;
    private float rotationY = 0.0f;
    private float maximumY = 75.0f;
    private float minimumY = -45.0f;
    private bool flap;
    private float oldV;
    // Use this for initialization
    void Start () {
        //thisBird.AddRelativeForce(Vector3.forward * 2500);
        //TODO
        //Set up Predator and Leader meshes
        //Set controls (keyboard / controller)
            //Instantiate Predator
            //Instantiate Leader
            //Flap Wings
            //Full Spread (braking, soaring)
            //Dive
            //Target
            //Quit(already have for keyboard)
        
	
	}

    void FixedUpdate()
    {
        Vector3 newV = thisBird.velocity;
        oldV = thisBird.velocity.magnitude;
        //thisBird.velocity = new Vector3 (0,0,0);
        //thisBird.AddRelativeForce(Vector3.back * thrust);
        // Mouse commands.


        //if (Input.GetAxis("LeftJoyStickY") > 0.1 || Input.GetKey("w"))
        //{
        //    this.transform.position -= this.transform.up;
        //}

        //if (Input.GetAxis("LeftJoyStickY") < -0.1 || Input.GetKey("s"))
        //{
        //    this.transform.position += this.transform.up;
        //}

        //if (Input.GetAxis("LeftJoyStickX") > 0.1 || Input.GetKey("d"))
        //{
        //    this.transform.position += this.transform.right;
        //}

        //if (Input.GetAxis("LeftJoyStickX") < -0.1 || Input.GetKey("a"))
        //{
        //    this.transform.position -= this.transform.right;
        //}
        float pivot = 0.0f;
        float rotationX;
        //if (Input.GetAxis("LeftJoyStickX") > 0 || Input.GetAxis("LeftJoyStickY") > 0)
        //{
        //    rotationX = transform.localEulerAngles.y + Input.GetAxis("LeftJoyStickX");
        //    rotationY += Input.GetAxis("LeftJoyStickY") * mouseSensitivity;
        //}
		
		//if(Input.GetAxis("LeftTrigger") > 0.0f)
		//{
		//	pivot -= Input.GetAxis ("LeftTrigger") * mouseSensitivity;
		//}
		//if(Input.GetAxis("RightTrigger") > 0.0f)
		//{
		//	pivot += Input.GetAxis ("LeftTrigger") * mouseSensitivity;
		//}

        rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        transform.localEulerAngles = new Vector3(rotationY, rotationX, pivot);
        //    public static void RotateY(this Vector3 v, float angle)
        //float sin = Mathf.Sin(rotationX);
        //float cos = Mathf.Cos(rotationX);
        //float rot = Mathf.Sqrt((oldV * oldV) - (newV.y * newV.y));
        //newV.x = rot * cos;
        //newV.z = rot * sin;

        //newV += Vector3.up * pGravity;
        float rot = transform.localEulerAngles.x;
        if(rot > 25 && rot < 76)
        {
            if (rot > 45)
            {
                thrust += accel*3 * Time.deltaTime;
            }
            if (rot > 65)
            {
                thrust += accel*4 * Time.deltaTime;
            }
            thrust += accel*2 * Time.deltaTime;
        }
        if (rot > 314 && rot < 340)
        {
            if(rot > 314 && rot < 325)
            {
                thrust -= accel/4 * Time.deltaTime;
            }
            if(rot > 325 && rot < 335)
            {
                thrust -= accel * Time.deltaTime;
            }
            thrust -= accel/10 *Time.deltaTime; ;
        }
        if (flap)
        {
            //newV += transform.forward * thrust;
            thrust += accel;
            //newV += transform.up * lift;
            flap = false;
        }

        //transform.position += transform.forward * 0.2f;
        //thisBird.velocity = newV;
        //thisBird.AddRelativeForce(newV);

        if (thrust > minSpd)
        {
            thrust -= minSpd/5 * Time.deltaTime;
        }
        if (thrust < minSpd)
        {
            thrust = minSpd;
        }
        if (thrust > maxSpd)
        {
            thrust = maxSpd;
        }

    }
	
	// Update is called once per frame
	void Update () {
        //TODO
        //Listen for buttons
        //Trigger gravity to affect the camera/player if bird is created (low value)
        //If targeting look for targets below player but within a certain threshold
        //If flapping counteract the gravity (vector? hmm.... this could be fun)
        //If diving, fire animation, increase gravity, increase interia
        //If coming out of a dive and braking fire animation
        //If target found reorient camera towards target (how are we gonna do this?)
        if(Input.GetKeyUp(KeyCode.Space) || Input.GetButton("A"))
        {
            flap = true;
            anim.Play("Bird_Flight1");
        }

        transform.position += transform.forward * thrust;

    }

    void thrustDecay()
    {
    }

}
