using System.Collections;
using System.Collections.Generic;
using JoyconAPI;
using CaveAsset.Input;
using CaveAsset.Joycon;
using UnityEngine;

public class PickUpThrow : MonoBehaviour
{
    float throwForce = 3000;
    Vector3 objectPosition;
    float distance;
    Vector3 offset = new Vector3(5f,5f,5f);
    private JoyconController joyconController = null;
    private Joycon joyconLeft = null;
    private Joycon joyconRight = null;
    public bool canHold = true;
    private GameObject centerCamera;
    public GameObject item;
    public GameObject tempParent;
    public GameObject cave;
    public bool isHolding = false;
    public bool wasPushed = false;
    private Vector3 rotate;
    private Vector3 initPos;
    [Header("Set frequency band and amplitude for throw")]
    [Header("Set the lowest frequency of the frequency band")]
    [Range(41.0f,1251.0f)]
    public float lowerLimit = 0.0f;
    [Header("Set the upper frequency of the frequency band")]
    [Range(41.0f,1253.0f)]
    public float upperLimit = 1.0f;
    [Header("Set the amplitude")]
    [Range(0.0f,1.003f)]
    public float amplitude = 0.1f;

    private void Start()
    {
        centerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        initPos = item.transform.position;
        joyconController = cave.GetComponent<JoyconController>();
        joyconLeft = joyconController.GetLeftJoycon();
        joyconRight = joyconController.GetRightJoycon();
    }

    // Update is called once per frame
    void Update()
    {
        checkLimitsIntegrity();
        CorrectPosition();
        ResetObject();
        //Checks distance between Object and Player 
        distance = Vector3.Distance(item.transform.position, tempParent.transform.position);
        if(distance <= 5f)
            PushedButton();
        //Checks if currently holding something
        if (isHolding)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);
            RotateObject();
        }
        else
        {
            objectPosition = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPosition;
        }


    }

    ///<summary>
    ///Checks if a shoulder button was pressed and on which controller
    ///</summary>
    private void PushedButton()
    {
        if (joyconLeft != null && joyconRight != null)
        {
            if (!wasPushed && joyconRight.GetButton(Joycon.Button.SHOULDER_2))
            {
                PickUp();
                wasPushed = true;
                joyconLeft.SetRumble(160.0f, 320.0f, 0.6f, 1000);
                joyconRight.SetRumble(160.0f, 320.0f, 0.6f, 1000);
            }
            if (wasPushed && isHolding && joyconRight.GetButton(Joycon.Button.SHOULDER_1))
            {
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                joyconLeft.SetRumble(lowerLimit, upperLimit, amplitude, 1000);
                joyconRight.SetRumble(lowerLimit, upperLimit, amplitude, 1000);
                isHolding = false;
                wasPushed = false;
            }

        }
        else if (joyconLeft != null)
        {
            if (!wasPushed && joyconLeft.GetButton(Joycon.Button.SHOULDER_2))
            {
                PickUp();
                wasPushed = true;
                joyconLeft.SetRumble(160.0f, 320.0f, 0.6f, 1000);
            }
            if (wasPushed && isHolding && joyconLeft.GetButton(Joycon.Button.SHOULDER_1))
            {
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                joyconLeft.SetRumble(lowerLimit, upperLimit, amplitude, 1000);
                isHolding = false;
                wasPushed = false;
            }

        }
    }
    ///<summary>
    ///Resets object to origin position when the down button on the d-pad is pressed
    ///</summary>
    private void ResetObject()
    {
        if (joyconLeft != null && joyconRight != null)
        {
            if (joyconRight.GetButton(Joycon.Button.DPAD_DOWN))
            {
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                item.transform.position = initPos;
            }
        }
        else if(joyconLeft != null)
        {
            if (joyconLeft.GetButton(Joycon.Button.DPAD_DOWN))
            {
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                item.transform.position = initPos;
            }
        }
    }

    ///<summary>
    ///Picks object and places it in front of the main camera
    ///</summary>
    private void PickUp()
    {
        isHolding = true;
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().detectCollisions = true;
        item.transform.rotation = centerCamera.transform.rotation;
        item.transform.position = centerCamera.transform.position;
        item.transform.Translate(Vector3.up * 0.5f);
        item.transform.Translate(Vector3.forward * 1.5f);
    }

    ///<summary>
    ///Maps the rotation of the joycon to the object
    ///</summary>
    private void RotateObject()
    {
        if(joyconLeft != null && joyconRight != null)
        {
            rotate = joyconRight.GetGyro();
            transform.Rotate(rotate);
        } else if(joyconLeft != null)
        {
            rotate = joyconLeft.GetGyro();
            transform.Rotate(rotate);
        }
    }

    ///<summary>
    ///Corrects the position of the object if it is underneath the floor
    ///</summary>
    private void CorrectPosition()
    {
        if (item.transform.position.y < 0)
        {
            Vector3 tmp = item.transform.position;
            tmp.y = 0.1f;
            item.transform.position = tmp;
        }
    }

    private void checkLimitsIntegrity()
    {
        if (lowerLimit >= upperLimit)
            upperLimit = lowerLimit + 2.0f;
    }
}