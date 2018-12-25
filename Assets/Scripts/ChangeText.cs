using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaveAsset.Input;
using CaveAsset.Joycon;
using TMPro;

public class ChangeText : MonoBehaviour
{
    private TextMeshPro signWalk;
    private TextMeshPro signPickUp;
    private TextMeshPro signThrow;
    private JoyconController input;
    public GameObject cave;

    // Start is called before the first frame update
    void Start()
    {
        input = cave.GetComponent<JoyconController>();
        signWalk = GameObject.Find("WalkText").GetComponent<TextMeshPro>();
        signPickUp = GameObject.Find("PickUpText").GetComponent<TextMeshPro>();
        signThrow = GameObject.Find("ThrowText").GetComponent<TextMeshPro>();


        if (input == null)
        {
            string notAvalible = "Kein Joy-Con angeschlossen";
            signWalk.text = notAvalible;
            signPickUp.text = notAvalible;
            signThrow.text = notAvalible;
        }
        else if (input.GetNumberOfJoycons() > 1)
        {
            signWalk.text = "Benutzen Sie bitte den linken Analog-Stick, um sich zu bewegen";
            signPickUp.text = "Mit der Taste 'ZR', kann der Ball aufgehoben werden";
            signThrow.text = "Mit der Taste 'R' kann der Ball geworfen werden";
        }
        else
        {
            signWalk.text = "Benutzen Sie bitte den\n Analog-Stick, um sich zu bewegen";
            signPickUp.text = "Mit der Taste 'ZL', kann der Ball aufgehoben werden";
            signThrow.text = "Mit der Taste 'L' kann der Ball geworfen werden";
        }
    }
}
