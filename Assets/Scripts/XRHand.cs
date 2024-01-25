using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRHand : MonoBehaviour
{
    public InputActionProperty triggerAction;
    private float triggerValue;
    private InputAction indexTriggerAction;
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


    private void OnEnable()
    {
        // Enable the index trigger action
        indexTriggerAction = new InputAction("trigger", InputActionType.Value, "/input/trigger/value");
        indexTriggerAction.Enable();

        // Subscribe to the index trigger input event
        indexTriggerAction.performed += ctx => OnIndexTriggerPressed(ctx.ReadValue<float>());
        indexTriggerAction.canceled += ctx => OnIndexTriggerReleased(ctx.ReadValue<float>());
    }

    private void OnDisable()
    {
        // Unsubscribe from the index trigger input event and disable the action
        indexTriggerAction.performed -= ctx => OnIndexTriggerPressed(ctx.ReadValue<float>());
        indexTriggerAction.canceled -= ctx => OnIndexTriggerReleased(ctx.ReadValue<float>());
        indexTriggerAction.Disable();
    }

    private void OnIndexTriggerPressed(float value)
    {
        // Handle the index trigger press here
        Debug.Log($"Index trigger pressed with value: {value}");
    }

    private void OnIndexTriggerReleased(float value)
    {
        // Handle the index trigger release here
        Debug.Log($"Index trigger released with value: {value}");
    }
}
