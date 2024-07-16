using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class WheelController : MonoBehaviour
{
    public WheelCollider frontRightCollider;
    public WheelCollider frontLeftCollider;
    public WheelCollider backRightCollider;
    public WheelCollider backLeftCollider;

    public Transform frontRightMesh;
    public Transform frontLeftMesh;
    public Transform backRightMesh;
    public Transform backLeftMesh;

    public BoxCollider rightLever;
    public BoxCollider leftLever;

    public InputActionProperty rightHandGrip;
    public InputActionProperty leftHandGrip;

    public InputActionProperty rightHandPosition;
    public InputActionProperty leftHandPosition;

    public float breakingForce;
    public float acceleration;
    public float accelerationFactor;

    private float zr0;
    private float zl0;
    private float dzr;
    private float dzl;
    private float tr0;
    private float dtr;
    private float tl0;
    private float dtl;
    private bool rightWheelWasGrabbed;
    private bool leftWheelWasGrabbed;
    private bool rightArrowIsPressed;
    private bool leftArrowIsPressed;

    // Start is called before the first frame update
    void Start()
    {
        zr0 = 0;
        zl0 = 0;
        dzr = 0;
        dzl = 0;
        rightWheelWasGrabbed = false;
        leftWheelWasGrabbed = false;
        rightArrowIsPressed = false;
        leftArrowIsPressed = false;
    }

    void UpdateWheel(WheelCollider collider, Transform mesh)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        mesh.position = position;
        mesh.rotation = rotation;
    }

    void OnRightGripPressEnter(){
        float zr = rightHandPosition.action.ReadValue<Vector3>()[2]; 
        zr0 = zr; 
        tr0 = Time.time;
    }

    void OnRightGripStay(){
        float zr = rightHandPosition.action.ReadValue<Vector3>()[2]; 
        dzr = zr - zr0;
        dtr = Time.time - tr0;
        backRightCollider.motorTorque = accelerationFactor * dzr / dtr;
    }

    void OnRightGripExit(){
        backRightCollider.motorTorque = 0;
    }

    void OnLeftGripPressEnter(){
        float zl = leftHandPosition.action.ReadValue<Vector3>()[2]; 
        zl0 = zl; 
        tl0 = Time.time;
    }

    void OnLeftGripStay(){
        float zl = leftHandPosition.action.ReadValue<Vector3>()[2]; 
        dzl = zl - zl0;
        dtl = Time.time - tl0;
        backLeftCollider.motorTorque = accelerationFactor * dzl / dtl;
    }

    void OnLeftGripExit(){
        backLeftCollider.motorTorque = 0;
    }

    void OnRightArrowEnter (){
        backRightCollider.motorTorque = acceleration;
    }

    void OnRightArrowExit (){
        backRightCollider.motorTorque = 0;
    }

    void OnLeftArrowEnter (){
        backLeftCollider.motorTorque = acceleration;
    }

    void OnLeftArrowExit (){
        backLeftCollider.motorTorque = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool rightGripIsPressed = rightHandGrip.action.ReadValue<float>() > 0.5? true : false ; 
        bool leftGripIsPressed  = leftHandGrip.action.ReadValue<float>()  > 0.5? true : false ; 

        if (rightGripIsPressed && !rightWheelWasGrabbed){
            rightWheelWasGrabbed = true;
            OnRightGripPressEnter();
        } else if (rightGripIsPressed && rightWheelWasGrabbed) {
            OnRightGripStay();
        } else if (!rightGripIsPressed && rightWheelWasGrabbed){
            OnRightGripExit();
            rightWheelWasGrabbed = false;
        }

        if (leftGripIsPressed && !leftWheelWasGrabbed){
            leftWheelWasGrabbed = true;
            OnLeftGripPressEnter();
        } else if (leftGripIsPressed && leftWheelWasGrabbed) {
            OnLeftGripStay();
        } else if (!leftGripIsPressed && leftWheelWasGrabbed){
            OnLeftGripExit();
            leftWheelWasGrabbed = false;
        }

        if (Input.GetKey(KeyCode.RightArrow) && !rightArrowIsPressed){
            OnRightArrowEnter();
            rightArrowIsPressed = true;
        } else if (!Input.GetKey(KeyCode.RightArrow) && rightArrowIsPressed) {
            OnRightArrowExit();
            rightArrowIsPressed = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !leftArrowIsPressed){
            OnLeftArrowEnter();
            leftArrowIsPressed = true;
        } else if (!Input.GetKey(KeyCode.LeftArrow) && leftArrowIsPressed) {
            OnLeftArrowExit();
            leftArrowIsPressed = false;
        }

    }
}
