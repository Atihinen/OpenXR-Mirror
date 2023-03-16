using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
    public void Map(Transform transform)
    {
        rigTarget.position = transform.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = transform.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}
public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;
    public bool localRig = true;
    public Transform vrRigTransform;

    public Transform headConstraint;
    public Vector3 headBodyOffset;
    // Start is called before the first frame update
    void Start()
    {
        vrRigTransform = transform;
        headBodyOffset = transform.position - headConstraint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((head.vrTarget != null && leftHand.vrTarget != null && rightHand.vrTarget != null) && localRig)
        {
            transform.position = headConstraint.position + headBodyOffset;
            transform.forward = Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized;
            head.Map();
            leftHand.Map();
            rightHand.Map();
        }
    }
}
