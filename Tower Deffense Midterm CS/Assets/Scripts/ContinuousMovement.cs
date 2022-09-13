using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class ContinuousMovement : MonoBehaviour
{

    [Header("Movement Speed")]
    public float movementSpeed = 1f;
    [Header("Controller Movement")]
    public XRNode inputSource;

    [Header("Right Controller ButtonSource")]
    public XRNode right_HandButtonSource;
  
    [Header("Left Controller ButtonSource")]
    public XRNode left_HandButtonSource;
  
    private XROrigin rig;

    [Header("Ground Floor & Gravity ")]
    public float gravity = -9.81f;
    public LayerMask  groundLayer;

    [Header("Character Height")]
    public float additionalHeight = .10f;

    private float fallingSpeed;

    private Vector2 inputAxis;

    [SerializeField] private bool primaryButtonPressed;

    [Header("Visual Right Hand Buttons Bool Check")]
    [Tooltip("Do not edit, just a visual refference to see that the button is pressed")]
    [SerializeField] private bool rightAButtonPressed;
    [SerializeField] private bool right_B_ButtonPressed;
    [SerializeField] private bool right_2DAxisClicked;
    [SerializeField] private bool right_2DAxisTouched;

    [Header("Visual Left Hand Buttons Bool Check")]
    [Tooltip("Do not edit, just a visual refference to see that the button is pressed")]

    [SerializeField] private bool left_X_ButtonPressed;
    [SerializeField] private bool left_Y_ButtonPressed;
    [SerializeField] private bool left_2DAxisClicked;
    [SerializeField] private bool left_2DAxisTouched;
    [SerializeField] private bool left_MenueButtonTouched;

    [Header("Jump Velocity")]
    public float jumpVelocity = 100f;
    public bool isJumping;

    [Header("Right Hand Button Events")]
    [SerializeField] private UnityEvent aButtonPressed;
    [SerializeField] private UnityEvent bButtonPressed;
    [SerializeField] private UnityEvent right_2DAxis_Click;
    [SerializeField] private UnityEvent right_2DAxisTouch;

    [Header("Left Hand Button Events")]
    [SerializeField] private UnityEvent xButtonPressed;
    [SerializeField] private UnityEvent yButtonPressed;
    [SerializeField] private UnityEvent left_2DAxis_Click;
    [SerializeField] private UnityEvent left_2DAxisTouch;
    [SerializeField] private UnityEvent left_MenuePressed;


    private CharacterController character; // this character controller will manage how we move the rig when we collide with an object

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {

        //accessing the device via node 
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        InputDevice jumpbutton = InputDevices.GetDeviceAtXRNode(right_HandButtonSource);
        jumpbutton.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed);

        // RIGHT HAND
        InputDevice AButton = InputDevices.GetDeviceAtXRNode(right_HandButtonSource);
        AButton.TryGetFeatureValue(CommonUsages.primaryButton, out rightAButtonPressed);

        InputDevice BButton = InputDevices.GetDeviceAtXRNode(right_HandButtonSource);
        BButton.TryGetFeatureValue(CommonUsages.secondaryButton, out right_B_ButtonPressed);

        InputDevice right_2DAxisClick = InputDevices.GetDeviceAtXRNode(right_HandButtonSource);
        right_2DAxisClick.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out right_2DAxisClicked);

        InputDevice right_2DAxisTouch = InputDevices.GetDeviceAtXRNode(right_HandButtonSource);
        right_2DAxisTouch.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out right_2DAxisTouched);

        // LEFT HAND

        InputDevice XButton = InputDevices.GetDeviceAtXRNode(left_HandButtonSource);
        XButton.TryGetFeatureValue(CommonUsages.primaryButton, out left_X_ButtonPressed);

        InputDevice YButton = InputDevices.GetDeviceAtXRNode(left_HandButtonSource);
        YButton.TryGetFeatureValue(CommonUsages.secondaryButton, out left_Y_ButtonPressed);

        InputDevice left_2DAxisClick = InputDevices.GetDeviceAtXRNode(left_HandButtonSource);
        left_2DAxisClick.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out left_2DAxisClicked);

        InputDevice left_2DAxisTouch = InputDevices.GetDeviceAtXRNode(left_HandButtonSource);
        left_2DAxisTouch.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out left_2DAxisTouched);


        InputDevice left_MenueTouch = InputDevices.GetDeviceAtXRNode(left_HandButtonSource);
        left_MenueTouch.TryGetFeatureValue(CommonUsages.menuButton, out left_MenueButtonTouched);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();

        Quaternion headYaw = Quaternion.Euler(0, y: rig.Camera.transform.eulerAngles.y, 0);

        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction * Time.fixedDeltaTime * movementSpeed);

        

        bool isGrounded = CheckIfGrounded();
        if(isGrounded)
        {
            fallingSpeed = 0;
            isJumping = false;
        }
        else
        {
            //Gravity
            fallingSpeed += gravity * Time.fixedDeltaTime;
            character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
        }
        handleJumping();

        // Invoking the RIGHT HAND Events when the button is pressed:

        if(rightAButtonPressed == true)
        {
            aButtonPressed.Invoke();
        }

        if(right_B_ButtonPressed == true)
        {
            bButtonPressed.Invoke();
        }

        if(right_2DAxisClicked == true)
        {
            Debug.Log("This event is being runed");
            right_2DAxis_Click.Invoke();
        }

        if(right_2DAxisTouched == true)
        {
            right_2DAxisTouch.Invoke();
        }

        // Invoking the LEFT HAND Events whn the button is pressed:

        if(left_X_ButtonPressed == true)
        {
            xButtonPressed.Invoke();
        }

        if(left_Y_ButtonPressed == true)
        {
            yButtonPressed.Invoke();
        }

        if(left_2DAxisClicked == true)
        {
            left_2DAxis_Click.Invoke();
        }

        if(left_2DAxisTouched == true)
        {
            left_2DAxisTouch.Invoke();
        }

        if(left_MenueButtonTouched == true)
        {
            left_MenuePressed.Invoke();
        }


    }

    void CapsuleFollowHeadset()
    {
        // changing the character height to the righ hight + additional height of our prefference
        character.height = rig.CameraInOriginSpaceHeight + additionalHeight;
        Vector3 capsulCenter = transform.InverseTransformPoint(rig.Camera.transform.position);
        character.center = new Vector3(capsulCenter.x, character.height / 2 + character.skinWidth, capsulCenter.z);
    }


    bool CheckIfGrounded()
    {
        // tells us if we are on the ground
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }

    // Todo make the jumping mechanism softer going up and quicker going down. 
    void handleJumping()
    {
        if(!isJumping && CheckIfGrounded() && primaryButtonPressed)
        {
            isJumping = true;
            character.Move(Vector3.up * jumpVelocity * Time.fixedDeltaTime);

        }
        else if(!isJumping && CheckIfGrounded() && primaryButtonPressed)
        {
            isJumping = false;
        }

    }

    public void Boost(float buff)
    {
        Debug.Log("Boost Ability Activated");
        movementSpeed = movementSpeed * buff;

    }


    public void ResetBoost(float buff)
    {
        movementSpeed = movementSpeed / buff;

    }

}
