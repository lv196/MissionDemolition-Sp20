using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {
    //fields set in the Unity Inspector pane
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    //fields set dynamically
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;  //stores 3D world position of launchpoint
    public GameObject projectile; //reference to the new Projectile instance that is created
    public bool aimingMode;//normally false but set to true when the player presses mouse button 0 down over slingshot.

    private Rigidbody projectileRigidbody;

    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position; 
    }

    void OnMouseEnter() {
        // print( "Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
        }

    void OnMouseExit() {
        // print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
        }
    //OnMouseDown will only be called on the frame that the player presses the mouse button down over
    void OnMouseDown()
    {
        //The playeer has pressed the mouse button while over Slingshot
        aimingMode = true;
        //Instantiate a Projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //Start it at the launchpoint
        projectile.transform.position = launchPos;
        //Set it to isKinematic for now
      //  projectile.GetComponent<Rigidbody>().isKinematic = true;
        //set it to isKinematic for now
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }


    void Update()
    {
        //if slingshot is not in aimingMode, don't run this code
        if (!aimingMode) return;

        //Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //Limit mouseDelta to the radius of Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude>maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;


        if (Input.GetMouseButtonUp(0))
        {
            //The mouse has been released
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            projectile = null;
        }

    }


}
