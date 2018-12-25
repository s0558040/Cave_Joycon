using JoyconAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    public GameObject gravityArrow;
    public GameObject joyConLeft;

    private GameObject gravityArrowInstance;
    private GameObject joyConLeftInstance;
    private JoyconManager manager;
    private List<Joycon> joyconList;
    private Joycon[] joycons;
    // Start is called before the first frame update
    void Start()
    {
        manager = JoyconManager.Instance;
        joyconList = manager.joycons;
        joycons = new Joycon[joyconList.Count];
        for (int i = 0; i < joyconList.Count; i++)
        {
            joycons[i] = joyconList[i];
            gravityArrowInstance = GameObject.Instantiate(gravityArrow);
            gravityArrowInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            joyConLeftInstance = GameObject.Instantiate(joyConLeft);
            joyConLeftInstance.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1, 1, 0, 0.5f));
            joyConLeftInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Joycon joy in joycons)
        {
            //Show the direction of gravity of the left joy con
            //Vector3 up = -joy.GetAccel();
            Vector3 up = joy.GetAccel();
            //up = new Vector3(up.z, up.y, -up.x);
            up = up.normalized;
            gravityArrowInstance.transform.position = transform.position + new Vector3(-0.05f, 1.8f, 0.3f);
            gravityArrowInstance.transform.up = up;//-joy.GetAccel();
            gravityArrowInstance.transform.Rotate(new Vector3(1, 0, 0), 90);
            
            //Show the rotation of the left joy con
            joyConLeftInstance.transform.position = transform.position + new Vector3(-0.05f, 1.8f, 0.3f);
            Vector3 rotation = joy.GetGyro();
            //rotation = new Vector3(rotation.z, rotation.y, -rotation.x);
            joyConLeftInstance.transform.Rotate(rotation);

            if (joy.GetButtonDown(Joycon.Button.DPAD_RIGHT))
            {
                Reset(up);
            }
            if (joy.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                gravityArrowInstance.SetActive(!gravityArrowInstance.activeSelf);
            }
            if (joy.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                joyConLeftInstance.SetActive(!joyConLeftInstance.activeSelf);
            }
        }
    }

    private void Reset(Vector3 up)
    {
        joyConLeftInstance.transform.up = up;
    }
}
