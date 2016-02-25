using UnityEngine;
using System.Collections;

public class AnimCNTRL : MonoBehaviour {

    Animator anim;
    int clip;
	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        clip = Random.Range(1, 10);
        Debug.Log(clip);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bird_Flight1") && anim.GetCurrentAnimatorStateInfo(0).IsName("Bird_Flight2"))
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
                anim.Play("Bird_Idle");
            }
        }
        
	}
}
