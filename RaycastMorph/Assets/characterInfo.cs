﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterInfo : MonoBehaviour {
    public PlayerControlStickyGaze playerScript;
    public bool isSally;
    AudioSource audio;

    private void OnCollisionEnter(Collision collision)
    {
    
        Debug.Log("FUCK");
        if(collision.gameObject.tag == "COMBO")
        {
            //check if this is the right person
            if (collision.gameObject.GetComponent<myInfo>().sallyObject)
            {
                //if this is a sally object
                if (isSally)
                {
                    //correctly gave to right person.
                    Debug.Log("Got here.");
                    audio.Play();
                    //destroy combo object and gain a point
                    //Destroy(collision.gameObject);
                   
                    playerScript.detachItems();
                    playerScript.cleanCam();
                    playerScript.nextTask();
                }
                if (!playerScript.gameObject.GetComponent<AudioSource>().isPlaying)
                {
                    playerScript.gameObject.GetComponent<AudioSource>().Play();
                }

            }
            else if(!collision.gameObject.GetComponent<myInfo>().sallyObject)
            {
                //if this is not a sally object
                if (!isSally)
                {
                    //correctly gave to right person (Bob).
                    Debug.Log("Got here.");
                    audio.Play();
                    //destroy combo object and gain a point
                    playerScript.detachItems();
                    playerScript.cleanCam();
                   // Destroy(collision.gameObject);
                    playerScript.nextTask();
                }
                else
                {
                    //wrong person!
                    if (!playerScript.gameObject.GetComponent<AudioSource>().isPlaying)
                    {
                        playerScript.gameObject.GetComponent<AudioSource>().Play();
                    }
                    
                }

            }

        }
        else
        {
            if (!playerScript.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                playerScript.gameObject.GetComponent<AudioSource>().Play();
            }

        }
    }
    // Use this for initialization
    void Start () {
        playerScript = GameObject.Find("Main Camera").GetComponent<PlayerControlStickyGaze>();
        audio = this.gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
