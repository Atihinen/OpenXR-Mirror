using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool triggerPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SetTrigger(float val)
    {
        if (val >= 0.8 && triggerPressed == false)
        {
            triggerPressed = true;
        }
        if (val <= 0.5 && triggerPressed == true)
        {
            triggerPressed = false;
        }
    }
}
