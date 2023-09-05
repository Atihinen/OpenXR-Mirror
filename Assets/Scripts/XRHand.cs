using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRHand : MonoBehaviour
{
    public InputActionProperty triggerAction;
    private float triggerValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        triggerValue = triggerAction.action.ReadValue<float>();
        Debug.Log("Trigger from "+gameObject.name+": " + triggerValue);
    }
}
