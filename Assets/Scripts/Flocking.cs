using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Utility;

public class Flocking : MonoBehaviour
{

    public GameObject boid, boidA, playerObj, spawnPoint1, spawnPoint2;
    public GameObject cage;
    public List<GameObject> flock, flockA;
    public float repelMult, repelDistance, centerMult, centerDistance, matchMult, matchDistance, boundaryMult, bound, fearMult, scareRange;
    public GameObject[] bounds, predator;
    public Collider[] walls;
    public List<Collider> enemy;
    private Animator anim;
    private float clip;
    private Random ran;
    private bool playerBird;
    
    private float temp;
    // Use this for initialization
    void Start()
    {
        GameObject[] bounds = GameObject.FindGameObjectsWithTag("boundary");
        GameObject[] predator = GameObject.FindGameObjectsWithTag("predator");
        //Debug.Log("SIZE OF BOUNDS = " + bounds.Length);
        walls = new Collider[bounds.Length];

        for (int i = 0; i < walls.Length; i++)
        {
            walls[i] = bounds[i].GetComponent<Collider>();
        }

        for(int i = 0; i < predator.Length; i++)
        {
            enemy.Add(predator[i].GetComponent<Collider>());
        }

        Vector3 rPos;

        for(int i = 0; i < 100; i++)
        {
            rPos = spawnPoint1.transform.position + Random.insideUnitSphere * 60;
            flock.Add(Instantiate(boid, rPos, boid.transform.rotation) as GameObject);
            
            Rigidbody phys = flock[i].GetComponent<Rigidbody>();
            phys.velocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            phys.velocity = Vector3.ClampMagnitude(phys.velocity, 4);
            phys.transform.LookAt(phys.transform.position + phys.velocity);

            rPos = spawnPoint2.transform.position + Random.insideUnitSphere * 60;
            flockA.Add(Instantiate(boidA, rPos, boid.transform.rotation) as GameObject);

            phys = flockA[i].GetComponent<Rigidbody>();
            phys.velocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            phys.velocity = Vector3.ClampMagnitude(phys.velocity, 4);
            phys.transform.LookAt(phys.transform.position + phys.velocity);
        }

        Instantiate(cage, new Vector3(0, 0, 0), cage.transform.rotation);
    }

    void FixedUpdate()
    {

        
    }	
	// Update is called once per frame
	void Update ()
    {

        //instantiate player bird
        if (Input.GetKeyUp(KeyCode.B) || Input.GetButtonUp("Start"))
        {
            if (!playerBird)
            {
                Vector3 spawnHere = GameObject.FindGameObjectWithTag("playerSpawn").transform.position;
                Quaternion spawnRotation = this.transform.rotation;
                GameObject player = (GameObject) Instantiate(playerObj, spawnHere, spawnRotation);
                playerBird = true;
                enemy.Add(player.GetComponentInChildren<Collider>());
               //this.transform.parent = player.transform;
                gameObject.GetComponent<CamFly>().enabled = false;
                gameObject.GetComponent<SmoothFollow>().enabled = true;
                gameObject.GetComponent<SmoothFollow>().setTarget(player);
            }
            else
            {
                //this.transform.parent = null;
                enemy.RemoveAt((enemy.Count)-1);
                Destroy(GameObject.Find("Player(Clone)"));
                playerBird = false;
                gameObject.GetComponent<SmoothFollow>().enabled = false;
                gameObject.GetComponent<CamFly>().enabled = true;
            }
        }

        if (Input.GetKey("escape") || Input.GetButton("Back"))
        {
            Application.Quit();
        }

        if(Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.C))
        {
            Vector3 rPos = Random.insideUnitSphere * 30;
            flock.Add(Instantiate(boid, rPos, boid.transform.rotation) as GameObject);
            Rigidbody phys = flock[(flock.Count)-1].GetComponent<Rigidbody>();
            phys.velocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            phys.velocity = Vector3.ClampMagnitude(phys.velocity, 4);
            phys.transform.LookAt(phys.transform.position + phys.velocity);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            Vector3 rPos = Random.insideUnitSphere * 30;
            flockA.Add(Instantiate(boidA, rPos, boid.transform.rotation) as GameObject);
            Rigidbody phys = flockA[(flockA.Count) - 1].GetComponent<Rigidbody>();
            phys.velocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            phys.velocity = Vector3.ClampMagnitude(phys.velocity, 4);
            phys.transform.LookAt(phys.transform.position + phys.velocity);
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyUp(KeyCode.G))
        {
            if(centerMult > 0)
            {
                temp = centerMult;
                centerMult *= -1;
                matchMult *= -1;
                Debug.Log("Turning OFF Cohesion!");
            }
            else if(centerMult < 0)
            {
                centerMult *= -1;
                matchMult *= -1;
                Debug.Log("Cohesion is ON!");
            }
            
        }
        if(Input.GetKeyUp(KeyCode.V))
        {
            //Debug.Log("TRAIL");
            for (int i = 0; i < flock.Count; i++)
            {
                flock[i].GetComponent<TrailRenderer>().enabled = !flock[i].GetComponent<TrailRenderer>().enabled;
            }
            for (int i = 0; i < flockA.Count; i++)
            {
                flockA[i].GetComponent<TrailRenderer>().enabled = !flockA[i].GetComponent<TrailRenderer>().enabled;
            }
        }

        for(int i = 0; i < flock.Count; i++)
        {
            anim = flock[i].GetComponent<Animator>();
            clip = Random.Range(0, 10);
            //Debug.Log(clip);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bird_Idle"))
            {
                if (clip > 5 && clip <= 8)
                {
                    anim.Play("Bird_Flight1");
                }
                else if (clip >= 8)
                {
                    anim.Play("Bird_Flight2");
                }
                else
                {
                    anim.Play("Bird_Idle1");
                }
            }
            Vector3 move = new Vector3();

            if (repel(flock[i]).magnitude != 0)
            {
                move += repel(flock[i]);
            }

            if (boundary(flock[i]).magnitude != 0 || fear(flock[i]).magnitude != 0)
            {
                move += boundary(flock[i]);
                move += fear(flock[i]);
            }

            else
            {
                if (cohesion(flock[i], 0).magnitude != 0)
                {
                    move += cohesion(flock[i], 0);
                }

                if (match(flock[i], 0).magnitude != 0)
                {
                    move += match(flock[i], 0);
                }

                move = Vector3.ClampMagnitude(move, 6);

            }

            Rigidbody phys = flock[i].GetComponent<Rigidbody>();

            if (move.magnitude != 0)
            {
                phys.AddForce(move);
            }

            phys.velocity = Vector3.ClampMagnitude(phys.velocity, 6);
            phys.transform.LookAt(phys.transform.position + phys.velocity);
        }

        for (int i = 0; i < flockA.Count; i++)
        {
            anim = flockA[i].GetComponent<Animator>();
            clip = Random.Range(1, 10);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bird_Idle"))
            {
                if (clip > 6 && clip < 9)
                {
                    anim.Play("Bird_Flight1");
                }
                else if (clip > 8)
                {
                    anim.Play("Bird_Flight2");
                }
                else
                {
                    anim.Play("Bird_Idle1");
                }
            }
            Vector3 move = new Vector3();

            if (repel(flockA[i]).magnitude != 0)
            {
                move += repel(flockA[i]);
            }

            if (boundary(flockA[i]).magnitude != 0 || fear(flockA[i]).magnitude != 0)
            {
                move += boundary(flockA[i]);
                move += fear(flockA[i]);
            }

            else
            {
                if (cohesion(flockA[i], 1).magnitude != 0)
                {
                    move += cohesion(flockA[i], 1);
                }

                if (match(flockA[i], 1).magnitude != 0)
                {
                    move += match(flockA[i], 1);
                }

                move = Vector3.ClampMagnitude(move, 6);

            }

            Rigidbody phys = flockA[i].GetComponent<Rigidbody>();

            if (move.magnitude != 0)
            {
                phys.AddForce(move);
            }

            phys.velocity = Vector3.ClampMagnitude(phys.velocity, 6);
            phys.transform.LookAt(phys.transform.position + phys.velocity);
        }
    }

    Vector3 cohesion(GameObject bird, int f)
    {
        Vector3 center = new Vector3();
        int divC = 0;

        if (f == 0)
        {
            for (int i = 0; i < flock.Count; i++)
            {
                if (flock[i] != bird && Vector3.Distance(flock[i].transform.position, bird.transform.position) < centerDistance)
                {
                    center = center + flock[i].transform.position;
                    divC++;
                }
            }
        }
        else
        {
            for (int i = 0; i < flockA.Count; i++)
            {
                if (flockA[i] != bird && Vector3.Distance(flockA[i].transform.position, bird.transform.position) < centerDistance)
                {
                    center = center + flockA[i].transform.position;
                    divC++;
                }
            }
        }

        if (divC > 0)
        {
            center = center / divC;
            center = center - bird.transform.position;
            center = center * centerMult;
        }

        //center = center - boid.transform.position;
        //center = center * centerMult;
        
        return center;
    }

    Vector3 repel(GameObject bird)
    {
        Vector3 repel = new Vector3();
        int divR = 0;

        for(int i = 0; i < flock.Count; i++)
        {
            if(flock[i] != bird && (Vector3.Distance(flock[i].transform.position, bird.transform.position) < repelDistance))
            {
                repel = repel - (flock[i].transform.position - bird.transform.position);
            }
        }

        for (int i = 0; i < flockA.Count; i++)
        {
            if (flockA[i] != bird && (Vector3.Distance(flockA[i].transform.position, bird.transform.position) < repelDistance))
            {
                repel = repel - (flockA[i].transform.position - bird.transform.position);
            }
        }
        if (divR > 0)
        {
            //repel = repel / divR;
            repel = repel * repelMult;
        }
        

        return repel;
    }

    Vector3 match(GameObject bird, int f)
    {
        Vector3 match = new Vector3();
        int divM = 0;

        if (f == 0)
        {
            for (int i = 0; i < flock.Count; i++)
            {
                if (flock[i] != bird && (Vector3.Distance(flock[i].transform.position, bird.transform.position) < matchDistance))
                {
                    match = match + flock[i].GetComponent<Rigidbody>().velocity;
                    divM++;

                }
            }
        }
        else
        {
            for (int i = 0; i < flockA.Count; i++)
            {
                if (flockA[i] != bird && (Vector3.Distance(flockA[i].transform.position, bird.transform.position) < matchDistance))
                {
                    match = match + flockA[i].GetComponent<Rigidbody>().velocity;
                    divM++;

                }
            }
        }

        if(divM > 0)
        {
            match = match / divM;
            match = match * matchMult;
        }

        return match;
    }

    Vector3 boundary(GameObject bird)
    {
        Vector3 repel = new Vector3(0,0,0);
        
        for(int i = 0; i < walls.Length; i++)
        {
            Vector3 ping = walls[i].ClosestPointOnBounds(bird.transform.position);
            if ( Vector3.Distance(ping, bird.transform.position) < bound)
            {
                Rigidbody rb = bird.GetComponent<Rigidbody>();
               // Debug.Log("Close to walls!");
                repel = repel - (ping - bird.transform.position);
            }
        }

        repel = repel * boundaryMult;


        return repel;
    }

    Vector3 fear(GameObject bird)
    {
        Vector3 repel = new Vector3(0, 0, 0);

        for (int i = 0; i < enemy.Count; i++)
        {
            Vector3 ping = enemy[i].ClosestPointOnBounds(bird.transform.position);
            if (Vector3.Distance(ping, bird.transform.position) < scareRange)
            {
                Rigidbody rb = bird.GetComponent<Rigidbody>();
                Debug.Log("Predator Spotted!");
                repel = repel - (ping - bird.transform.position);
            }
        }

        repel = repel * fearMult;


        return repel;
    }

}
