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
    #endregion
    public VRRig vrRig;
    public GameObject XRPlayerPrefab;
    private GameObject xrPlayer;
    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer && isOwned)
        {
            vrRig.localRig = false;
            rigTransform = vrRig.transform;
            rigTransform.SetParent(transform);
            xrPlayer = Instantiate(XRPlayerPrefab);
            xrPlayer.transform.SetParent(transform);
            ClientXRRig cxrRig = xrPlayer.GetComponent<ClientXRRig>();
            vrRig.head.vrTarget = cxrRig.Head.transform;
            vrRig.leftHand.vrTarget = cxrRig.LeftController.transform;
            vrRig.rightHand.vrTarget = cxrRig.RightController.transform;
            rightHandTransform = vrRig.rightHand.vrTarget;
            leftHandTransform = vrRig.leftHand.vrTarget;
            CmdSetupPlayer(vrRig.head.rigTarget.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        UpdateAvatar();

    }
    private void UpdateHeadForServer()
    {
        headTransform = vrRig.head.rigTarget;
        headRotation = headTransform.rotation;
    }

    void UpdateAvatar()
    {
        rigTransform.position = vrRig.headConstraint.position + vrRig.headBodyOffset;
        rigTransform.forward = Vector3.ProjectOnPlane(vrRig.headConstraint.up, Vector3.up).normalized;
        vrRig.head.Map();
        vrRig.leftHand.Map();
        vrRig.rightHand.Map();
        UpdateHeadForServer();
    }

    //Syncvar methods
    private void OnHeadRotationChanged(Quaternion oldRotation, Quaternion newRotation)
    {
        if (!isLocalPlayer)
        {
            vrRig.head.rigTarget.rotation = headRotation;
        }
    }

    //Commands
    [Command]
    public void CmdSetupPlayer(Quaternion hRotation)
    {
        headRotation = hRotation;
    }
}
