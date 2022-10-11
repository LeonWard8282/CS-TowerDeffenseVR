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

    public ActionBasedContinuousTurnProvider RegularActionTurnController;
    public ActionBasedContinuousTurnProvider SouthPawActionTurnController;

    public AbilityDash abilityDashButton;


    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        inputSource = GetComponent<PlayerMovement>().inputSource;
        buttonSourceInput = GetComponent<PlayerMovement>().inputSource;

        RegularActionTurnController = GetComponent<ActionBasedContinuousTurnProvider>();
        SouthPawActionTurnController = GetComponent<ActionBasedContinuousTurnProvider>();
        abilityDashButton = GetComponent<AbilityDash>();
    }
    
    public void RegularControllerInput()
    {
        inputSource = XRNode.LeftHand; // left hand moves
        buttonSourceInput = XRNode.RightHand; // Jump Button?

        RegularActionTurnController.leftHandTurnAction.DisableDirectAction();
        RegularActionTurnController.rightHandTurnAction.EnableDirectAction();

        //abilityDashButton.rightThumstickPress;

        //playerMovement.inputSource =
        //playerMovement.right_HandButtonSource =  //InputActionReference.ReferenceEquals(XRIDefaultInputActions.XRIRightHandButtonPressActions());

    }

    public void SouthPawControllerInput()
    {

        inputSource = XRNode.RightHand;
        buttonSourceInput = XRNode.LeftHand;
        RegularActionTurnController.leftHandTurnAction.EnableDirectAction();
        RegularActionTurnController.rightHandTurnAction.DisableDirectAction();

        playerMovement.inputSource = inputSource;
        playerMovement.right_HandButtonSource = buttonSourceInput;
    }

}
