﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControlStickyGaze : MonoBehaviour
{

    public Camera cam;
    public Collider testCollider;
    public Collider floor;
    public GameObject player;
    int timer;
    bool timeme;
    bool played = false;
    bool moved = false;
    public AudioSource testAudio1;
    public AudioSource testAudio2;
    public MeshRenderer testswap;
    public Text evil;
    public Text good;
    public Text taskDisplay;
    GameObject greenCube;
    public GameObject TEMPNEWOBJ;
    List<GameObject> MyObjects = new List<GameObject>();
    public Text WorldLabel;
    public bool inSallysRoom;
    public Text task;

    //Public Tasks
    int taskNum = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("my obj" + MyObjects.Count);
        Cursor.lockState = CursorLockMode.Locked;
        ///Restart Scene on Escape Press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(0);
        }

        float z = Input.GetAxis("Vertical") * Time.deltaTime;
        gameObject.transform.position += z * transform.forward * 2f;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime;
        gameObject.transform.position += x * transform.right * 2f;
        
        this.gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.93f, gameObject.transform.position.z);

        player.transform.localEulerAngles = new Vector3(player.transform.localEulerAngles.x, player.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 2f, player.transform.localEulerAngles.z);
        player.transform.localEulerAngles = new Vector3(player.transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * -2f, player.transform.localEulerAngles.y, player.transform.localEulerAngles.z);

        // transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
        /* if (Input.GetAxis("Mouse X") < 0)
         {
             player.transform.localEulerAngles = player.transform.Rotate(Vector3.up);
         }

         if (Input.GetAxis("Mouse X") > 0)
         {
             player.transform.Rotate(Vector3.up) * -speed;
         }*/


        //Cast a ray from screen center into space
        Ray ray = cam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);//show the debug ray
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f)) //the 10f is the length the ray extends in distance
        {
            //A collision occured between the ray and a thing
            if (hit.collider != null && hit.collider != floor && hit.collider.gameObject != cam && Input.GetKeyDown(KeyCode.LeftShift))
            {
                hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = false;
                hit.collider.transform.parent = cam.transform;
                MyObjects.Add(hit.collider.gameObject);
                if (hit.collider.GetComponent<myInfo>() != null)
                {
                    hit.collider.GetComponent<myInfo>().grabbed = true;
                    
                }
               
                //hit.collider.transform.GetComponent<Rigidbody>().velocity = cam.transform.GetComponent<Rigidbody>().velocity;
                // Debug.Log("YOU DOOD IT");
              
              
            }
            else if(hit.collider != null && hit.collider != floor && hit.collider.gameObject != cam)
            {
                //display the label
                //showLabel();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //click to release
                    if(hit.collider.tag == "combined")
                    {
                        //this is a combined object, break it it
                    }
                }
               
                //isabella: UI LABEL STUFF
                if (hit.collider.gameObject.GetComponent<myInfo>() != null)
                {
                    hit.collider.gameObject.GetComponent<myInfo>().watched = true;
                    if (hit.collider.gameObject.GetComponent<myInfo>().label != null)
                    {
                        Debug.Log("WOOO");
                        //WorldLabel.enabled = true;
                        if (hit.collider.gameObject.GetComponent<myInfo>().wrongCombine) {
                            WorldLabel.enabled = true;
                        }
                        else {
                            WorldLabel.enabled = false;
                        }
                        WorldLabel.text = hit.collider.gameObject.GetComponent<myInfo>().label;
                    }
                }
                
            }
           
        }
        else
        {
            //didnt catch anything on ray
            turnOffAllLabels();
        }
        if (Input.GetKey(KeyCode.R))
        {
            //push r to launch stuff in the direction of the camera
          
            Transform[] grabbed = cam.GetComponentsInChildren<Transform>();
            cam.transform.DetachChildren();
            detachItems();
            foreach (Transform t in grabbed)
            {
                if(t.GetComponent<Rigidbody>() != null)
                {
                    t.GetComponent<Rigidbody>().gameObject.GetComponent<myInfo>().grabbed = false;
                    t.GetComponent<Rigidbody>().AddRelativeForce(cam.transform.forward * 300f);
                    t.GetComponent<Rigidbody>().useGravity = true;
                    /*  t.GetComponent<Rigidbody>().gameObject.GetComponent<myInfo>().grabbed = false;
                      t.GetComponent<Rigidbody>().isKinematic = false;
                      t.GetComponent<Rigidbody>().useGravity = true;
                      t.GetComponent<Rigidbody>().AddExplosionForce(500f, t.position, 10f, 3.0F);*/
                }
                
            }
            
        }

        if (Input.GetKey(KeyCode.Space))
        {
            //push space to release stuff
            //this.transform.parent = null;
            detachItems();
            cam.transform.DetachChildren();
            //Remove from the MyObjects list
            MyObjects.Clear();
           
            
            //Debug.Log("YOU DOOD IT");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //press q to bulk stuff (green cubes)
            //this is super not good code lol
            /* float tally = 0;
             GameObject[] cubes = GameObject.FindGameObjectsWithTag("greencube");
             List<GameObject> cubesIGot = new List<GameObject>();
             GameObject targetCube = null;
             foreach (GameObject cube in cubes)
             {
                 //if is picked up?
                 if (cube.GetComponent<myInfo>().grabbed)
                 {
                     tally++;
                     if(tally == 1)
                     {
                         //first cube I got
                         //put the bulk cube here
                         targetCube = cube;
                     }
                     else
                     {   //cubesIGot.Add(cube);
                         Destroy(cube);//if this works Ill shit

                     }


                     }
             }
             if(targetCube != null)
             {
                 targetCube.transform.localScale += new Vector3(tally/10, tally/10, tally/10);
             }*/

            //this is to combine objects with tags
            
                switch (taskNum)
                {
                    case 1:
                        {
                            //find cute and smart
                            if (checkMatchingTags("clean", "dirty"))
                            {
                                //success
                                Debug.Log("YOU COMBINED CORRECTLY");
                                
                                //taskNum++;
                                // taskDisplay.text = getTaskText(); 
                                //Remove old objects for new one
                                Vector3 pos = MyObjects[0].transform.position;//standardize this to be a uniform location infront of camera
                                GameObject temp = Instantiate(TEMPNEWOBJ, transform.position + (transform.forward * 2), transform.rotation);//move this to infront of camera
                                //MyObjects[0].GetComponent<myInfo>().label = "Puppy That Knows 17 Digits of Pi";
                                temp.name = "Clean and dirty item.";
                                temp.GetComponent<myInfo>().sallyObject = true;
                                detachItems();
                                cleanCam();
                                Debug.Log(MyObjects.Count);
                                /*foreach (GameObject go in MyObjects)
                                {
                                    Destroy(go);
                                }*/
                                //MyObjects.Add(TEMPNEWOBJ);

                            }
                            //isabella: DIDN'T WORK - SHOW THE ADJECTIVES NAO (all the else statements in this switch)
                            else
                            {
                                Debug.Log("COMBO DIDN'T WORK");
                                MyObjects[0].GetComponent<myInfo>().wrongCombine = true;
                                this.gameObject.GetComponent<AudioSource>().Play();
                            }
                            //detachItems();
                           // cam.transform.DetachChildren();
                            //MyObjects.Clear();
                            break;
                        }
                    case 2:
                        {
                            Debug.Log("ENTERING CASE 2");
                            //find cute and smart
                            if (checkMatchingTags("tasty", "explosive"))
                            {
                                //success
                                Debug.Log("YOU COMBINED CORRECTLY");
                                // taskNum++;
                                //taskDisplay.text = getTaskText(); ;
                                //Remove old objects for new one
                                Vector3 pos = MyObjects[0].transform.position;
                                GameObject temp = Instantiate(TEMPNEWOBJ, transform.position + (transform.forward * 2), transform.rotation);//move this to infront of camera
                                temp.GetComponent<myInfo>().label = "Tasty Explosive Food";
                                temp.name = "TasyExplosiveFood";
                                temp.GetComponent<myInfo>().sallyObject = false;

                                detachItems();
                                cleanCam();
                                /*foreach (GameObject go in MyObjects)
                                {
                                    Destroy(go);
                                }*/
                                //MyObjects.Add(TEMPNEWOBJ);

                            }
                            else
                            {
                                Debug.Log("COMBO DIDN'T WORK");
                                MyObjects[0].GetComponent<myInfo>().wrongCombine = true;
                                this.gameObject.GetComponent<AudioSource>().Play();
                            }
                           // detachItems();
                          //  cam.transform.DetachChildren();
                          //  MyObjects.Clear();
                            break;
                        }
                    case 3:
                        {
                            //find cute and smart
                            if (checkMatchingTags("ball", "gown"))
                            {
                                //success
                                Debug.Log("YOU COMBINED CORRECTLY");
                                //  taskNum++;
                                // taskDisplay.text = getTaskText(); ;
                                //Remove old objects for new one
                                Vector3 pos = MyObjects[0].transform.position;
                                GameObject temp = Instantiate(TEMPNEWOBJ, transform.position + (transform.forward * 2), transform.rotation);//move this to infront of camera
                                temp.GetComponent<myInfo>().label = "Ball Gown";
                                temp.name = "BallGown";
                                temp.GetComponent<myInfo>().sallyObject = true;

                                detachItems();
                                cleanCam();
                                /*foreach (GameObject go in MyObjects)
                                {
                                    Destroy(go);
                                }*/
                                //MyObjects.Clear();
                                //MyObjects.Add(TEMPNEWOBJ);

                            }
                            else
                            {
                                Debug.Log("COMBO DIDN'T WORK");
                                MyObjects[0].GetComponent<myInfo>().wrongCombine = true;
                                this.gameObject.GetComponent<AudioSource>().Play();
                            }
                            //detachItems();
                           // cam.transform.DetachChildren();
                            //MyObjects.Clear();
                            break;
                        }
                }
            
           


   
        }
    }

    public void cleanCam()
    {
        //safely eject items from the camera load
        //deleting
        foreach (GameObject go in MyObjects)
        {
            if (gameObject.name != "Player")
            {
                Debug.Log("im deleting:" + go.name);
                Destroy(go);
            }
        }
        MyObjects.Clear();
        cam.transform.DetachChildren();
        //MyObjects.Add(GameObject.Find("Main Camera/Player"));
    }
   public void nextTask()
    {
        //sets up next task
        taskNum++;
        taskDisplay.text = getTaskText();
    }

    string getTaskText()
    {
        switch (taskNum)
        {
            case 1:
                {
                    return "ITEM REQUEST: Sally - My honey, the executive, is coming over. Bring me something dirty to get me in the mood, but also clean to keep it classy.";
                    break;
                }
            case 2:
                {
                    return "MAKE REQUEST: Bob - About to live tweet the fireworks show! Make me something tasty and explosive to eat during the show.";
                    break;
                }
            case 3:
                {
                    return "COMFORT REQUEST: Petunia - Meet me in the home theatre to watch my student film. I got a B- sooooooo.";
                    break;
                }
        }
        return "";
    }
    void turnOffAllLabels()
    {
        //Text[] labels = WorldLabels.GetComponentsInChildren<Text>();
        //turn off all labels
        /* foreach(Text t in labels)
         {
             t.enabled = false;
         }*/
        WorldLabel.text = "";
    }

    bool checkMatchingTags(string key1, string key2)
    {

        //compare tags and return if the game objects grabbed match those tags.
        bool got1 = false;
        bool got2 = false;
        //check if one object is from each side
        bool gotSally = false;
        bool gotBob = false;
        Debug.Log("ITS ME" + MyObjects.Count);
        foreach (GameObject g in MyObjects)
        {
           
            Debug.Log("ITS ME" + g.name);
            Debug.Log("ITS ME " + g.tag);//problem: i think its going through every childNOPE
            if (!got1 || !got2)
            { 
                if (g.tag == key1)
                {
                    got1 = true;
                    Debug.Log("My tag is xxxxxx " + g.tag);
                    if (g.GetComponent<myInfo>().sallyObject)
                    {
                        gotSally = true;
                    }
                    else
                    {
                        gotBob = true;
                    }
                }
                else
                {
                    Debug.Log("My tag is " + g.tag);
                }
            }
            if (!got2)
            {
                if (g.tag == key2)
                {
                    got2 = true;
                    if (g.GetComponent<myInfo>().sallyObject)
                    {
                        gotSally = true;
                    }
                    else
                    {
                        gotBob = true;
                    }
                }
                else
                {
                    Debug.Log("My tag is " + g.tag);
                }
            }
        }
        if(got1 && got2)
        {
            if (gotBob && gotSally) return true;
            else return false;
        }
        else
        {
            return false;
        }
        
    }

    public void detachItems()
    {
        //remove items from sticky gaze
        Rigidbody[] items = cam.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody i in items)
        {
            i.useGravity = true;
            if (i.gameObject.GetComponent<myInfo>() != null)
            {

                i.gameObject.GetComponent<myInfo>().grabbed = false;
            }
            //Vector3 force = 7f * Time.deltaTime * transform.forward;
            //i.AddForce(force);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "reset") {
            Debug.Log("this worked");
            List<myInfo> AllObjs = new List<myInfo>();
            for (int i = 0; i < FindObjectsOfType<myInfo>().Length; i++) {
                AllObjs[i] = FindObjectsOfType<myInfo>()[i];
            }
            
            for (int i = 0; i < AllObjs.Count; i++) {
                //MyObjects[i].GetComponent<Transform>().position = MyObjects[i].GetComponent<myInfo>().startPos;
                AllObjs[i].gameObject.GetComponent<Transform>().position = AllObjs[i].startPos;
            }
        }
    }
}
