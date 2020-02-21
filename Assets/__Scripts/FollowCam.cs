﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; //The static point of interest that the camera should follow (projectile)

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;


    [Header("Set Dynamically")]
    public float camZ; //The desired z pos of the camera

    private void Awake()
    {
        camZ = this.transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if there's only one line following an if, it doesn't need braces
        //-- if (POI == null) return; //return if there is no poi, the rest of code is not executed

        //Get the position of the poi
        //-- Vector3 destination = POI.transform.position;

        Vector3 destination;
        //if there is no poi, return to P:[0,0,0]
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //Get the position of the POI
            destination = POI.transform.position;
            //If poi is a Projectile, check to see if its at rest

            if (POI.tag == "Projectile")
            {
                //if it is sleeping (that is, not moving)
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    //return to default view
                    POI = null;
                    //in the next update
                    return;
                }
            }
        }

        //Limit the X & Y to minimum values 
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //Interpolate from the current camera Position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        //Force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;
        //set the camera to the destination
        transform.position = destination;
        //Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10;
    }
}
