using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Jetpack : MonoBehaviour
{

    //Check to see whether we can continue flying up
    public bool fuel = true;

    //Float to track how much time has passed since we took off
    private float time = 0;

    //How much time we have to fly before we "run out of fuel"
    public float flyTime = 3f;

    //Modifier to change how fast we descend after running out of fuel
    public float fallVelocity = -5f;


    //Reference our Character Controller on the Oculus prefab
    public CharacterController character;

    public Rigidbody rb;

    public ContinuousMovement ab;

    //Declare our new upwards velocity for the Jetpack
    public float liftVelocity = 100f;

    //Create a new Vector3 to set our new Jetpack velocity
    private Vector3 moveDirection = Vector3.zero;

    //Flag to determine if we should descend slowly or if we should be affected by gravity
    public bool slowFall = false;

    private Handness m_hand = Handness.Right;
    private Handness m_hand2 = Handness.Left;

    //Declare two floats to reference the float value of the Oculus hand triggers
    private float JetpackRight;
    private float JetpackLeft;

    public AudioSource jetpackSource;
    public AudioClip jetpackclip;

    // Start is called before the first frame update
    void Start()
    {

        JetpackRight = Input.GetAxis("XRI_" + m_hand + "_GripButton");
        JetpackLeft = Input.GetAxis("XRI_" + m_hand2 + "_GripButton");


        //Set character to our Character Controller component
        character = character.GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
            flyTime = 5f;
        //Continually re-declare our float values for JetpackLeft and JetpackRight
        JetpackRight = Input.GetAxis("XRI_" + m_hand + "_GripButton");
        JetpackLeft = Input.GetAxis("XRI_" + m_hand2 + "_GripButton");

        //Call Jetpack function
        newJetpack();

        //Calculate our new Character Controller move velocity
        character.Move(moveDirection * Time.deltaTime);

    }

    public void newJetpack()
    {
        //Set moveDirection back to 0
        moveDirection = Vector3.zero;

        if (time > flyTime)
        {
            //Run out of fuel after our designated fly time
            fuel = false;
        }

        //Check to see if both hand triggers are grabbed
        if (JetpackLeft > 0.9 && JetpackRight > 0.9 && fuel)
        {
            //Negate FallSpeed calculated in OVRPlayerController script
            ab.GetComponent<DeviceBasedContinuousMoveProvider>().useGravity = false;
            rb.GetComponent<Rigidbody>().useGravity = false;

            //Increment y velocity on our Vector3 to create upward velocity
            moveDirection.y += liftVelocity;

            //Set slowFall to true 
            slowFall = true;
            if(!jetpackSource.isPlaying)
            {
                jetpackSource.PlayOneShot(jetpackclip);
            }

        }

        if(JetpackLeft < 0.9 && fuel && slowFall|| JetpackRight < 0.9 && fuel && slowFall)
        {
            ab.GetComponent<DeviceBasedContinuousMoveProvider>().useGravity = true;
            rb.GetComponent<Rigidbody>().useGravity = true;
            moveDirection.y += fallVelocity;
        }


        //If character is back on the ground, set slowFall back to false.  
        if (character.isGrounded)
        {
            slowFall = false;
            Debug.Log("slowFall value set to: " + slowFall);

            time = 0.0f;
            fuel = true;
            jetpackSource.Stop();
        }

        //If slowFall is still true (meaning we're still in the air) then negate the gravity equation from OVRPlayerController
        if (slowFall)
        {
            //If you just add this line, you'll slowly drift down to the ground
            ab.GetComponent<DeviceBasedContinuousMoveProvider>().useGravity = false;
            rb.GetComponent<Rigidbody>().useGravity = false;

            //Add this if you want to fall faster once you run out of fuel
            if (!fuel)
            {
                moveDirection.y += fallVelocity; //Fall velocity has to be a negative number
               ab.GetComponent<DeviceBasedContinuousMoveProvider>().useGravity = true;
                rb.GetComponent<Rigidbody>().useGravity = true;
            }

        }

    }

}
