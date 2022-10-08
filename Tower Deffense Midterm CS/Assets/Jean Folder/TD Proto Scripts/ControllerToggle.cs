using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerToggle : MonoBehaviour
{

    [SerializeField]private XRNode inputSource;
    [SerializeField]private XRNode buttonSourceInput;
    private PlayerMovement playerMovement;
    public ActionBasedContinuousTurnProvider ControllerTurning;


    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        inputSource = GetComponent<PlayerMovement>().inputSource;
        buttonSourceInput = GetComponent<PlayerMovement>().inputSource;
        ControllerTurning = GetComponent<ActionBasedContinuousTurnProvider>();
    }
    
    public void RegularControllerInput()
    {
        inputSource = XRNode.LeftHand; // left hand moves
        buttonSourceInput = XRNode.RightHand; // Jump Button?

        ControllerTurning.leftHandTurnAction.DisableDirectAction();
        ControllerTurning.rightHandTurnAction.EnableDirectAction();

        playerMovement.inputSource = inputSource;
        playerMovement.right_HandButtonSource = buttonSourceInput;

    }

    public void SouthPawControllerInput()
    {

        inputSource = XRNode.RightHand;
        buttonSourceInput = XRNode.LeftHand;
        ControllerTurning.leftHandTurnAction.EnableDirectAction();
        ControllerTurning.rightHandTurnAction.DisableDirectAction();

        playerMovement.inputSource = inputSource;
        playerMovement.right_HandButtonSource = buttonSourceInput;
    }

}
