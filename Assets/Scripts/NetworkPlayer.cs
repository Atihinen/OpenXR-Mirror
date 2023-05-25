using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayer : NetworkBehaviour
{
    #region serializables
    [SerializeField] private Transform rigTransform;
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform leftHandTransform;
    #endregion
    #region SyncVars
    [SyncVar(hook = nameof(OnHeadRotationChanged))]
    private Quaternion headRotation = Quaternion.identity;
    [SyncVar(hook = nameof(OnHeadPositionChanged))]
    private Vector3 headPosition = Vector3.zero;
    [SyncVar(hook = nameof(OnRightHandRotationChanged))]
    private Quaternion rightHandRotation = Quaternion.identity;
    [SyncVar(hook = nameof(OnRightHandPositionChanged))]
    private Vector3 rightHandPosition = Vector3.zero;
    [SyncVar(hook = nameof(OnLeftHandRotationChanged))]
    private Quaternion leftHandRotation = Quaternion.identity;
    [SyncVar(hook = nameof(OnLeftHandPositionChanged))]
    private Vector3 leftHandPosition = Vector3.zero;
    #endregion
    public VRRig vrRig;
    public GameObject XRPlayerPrefab;
    private GameObject xrPlayer;
    private ExtendedNetworkManager enm;
    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer && isOwned)
        {
            // Setup local open XR client
            enm = GameObject.Find("ExtendedNetworkManager").GetComponent<ExtendedNetworkManager>();
            vrRig.localRig = false;
            rigTransform = vrRig.transform;
            rigTransform.SetParent(transform);
            xrPlayer = Instantiate(XRPlayerPrefab);
            xrPlayer.transform.SetParent(transform);
            xrPlayer.transform.localPosition = new Vector3(0, 0, 0);
            ClientXRRig cxrRig = xrPlayer.GetComponent<ClientXRRig>();
            vrRig.head.vrTarget = cxrRig.Head.transform;
            vrRig.leftHand.vrTarget = cxrRig.LeftController.transform;
            vrRig.rightHand.vrTarget = cxrRig.RightController.transform;
            //Setup camera
            Transform cameraTransform = cxrRig.Head.gameObject.transform;  //Find main camera which is part of the scene instead of the prefab
            cameraTransform.parent = cxrRig.CameraOffSet.transform;  //Make the camera a child of the mount point
            cameraTransform.position = cxrRig.CameraOffSet.transform.position;  //Set position/rotation same as the mount point
            cameraTransform.rotation = cxrRig.CameraOffSet.transform.rotation;
            // Prepare for server binding
            headTransform = vrRig.head.vrTarget;
            rightHandTransform = vrRig.rightHand.vrTarget;
            leftHandTransform = vrRig.leftHand.vrTarget;
            CmdSetupPlayer(headTransform.position,
                headTransform.rotation, 
                rightHandTransform.position, 
                rightHandTransform.rotation,
                leftHandTransform.position,
                leftHandTransform.rotation);
            if(cxrRig.Head.transform.position.y < 0.1)
            {
                enm.ReplacePlayer(connectionToClient);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        UpdateAvatar();
        

    }
    // Trigger syncvars
    private void UpdateHeadForServer()
    {
        headTransform = vrRig.head.rigTarget;
        headRotation = headTransform.rotation;
        headPosition = headTransform.position;
    }

    private void UpdateRightHandForServer()
    {
        rightHandTransform = vrRig.rightHand.rigTarget;
        rightHandPosition = rightHandTransform.position;
        rightHandRotation = rightHandTransform.rotation;
    }

    private void UpdateLeftHandForServer()
    {
        leftHandTransform = vrRig.leftHand.rigTarget;
        leftHandPosition = leftHandTransform.position;
        leftHandRotation = leftHandTransform.rotation;
    }

    void UpdateAvatar()
    {
        rigTransform.position = vrRig.headConstraint.position + vrRig.headBodyOffset;
        rigTransform.forward = Vector3.ProjectOnPlane(vrRig.headConstraint.up, Vector3.up).normalized;
        vrRig.head.Map();
        vrRig.leftHand.Map();
        vrRig.rightHand.Map();
        UpdateHeadForServer();
        UpdateRightHandForServer();
        UpdateLeftHandForServer();
    }



    //Syncvar methods
    #region Syncvar methods
    private void OnHeadRotationChanged(Quaternion oldRotation, Quaternion newRotation)
    {
        if (!isLocalPlayer)
        {
            vrRig.head.rigTarget.rotation = headRotation;
        }
    }
    
    private void OnHeadPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        if (!isLocalPlayer)
        {
            vrRig.head.rigTarget.position = headPosition;
        }
    }

    private void OnRightHandRotationChanged(Quaternion oldRot, Quaternion newRot)
    {
        if (!isLocalPlayer)
        {
            vrRig.rightHand.rigTarget.rotation = rightHandRotation;
        }
    }
    private void OnRightHandPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        if (!isLocalPlayer)
        {
            vrRig.rightHand.rigTarget.position = rightHandPosition;
        }
    }

    private void OnLeftHandRotationChanged(Quaternion oldRot, Quaternion newRot)
    {
        if (!isLocalPlayer)
        {
            vrRig.leftHand.rigTarget.rotation = leftHandRotation;
        }
    }
    private void OnLeftHandPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        if (!isLocalPlayer)
        {
            vrRig.leftHand.rigTarget.position = leftHandPosition;
        }
    }
    #endregion

    //Commands
    [Command]
    public void CmdSetupPlayer(Vector3 hPosition, Quaternion hRotation, Vector3 rhPosition, Quaternion rhRotation, Vector3 lhPosition, Quaternion lhRotation)
    {
        headPosition = hPosition;
        headRotation = hRotation;
        rightHandPosition = rhPosition;
        rightHandRotation = rhRotation;
        leftHandPosition = lhPosition;
        leftHandRotation = lhRotation;
    }

}
