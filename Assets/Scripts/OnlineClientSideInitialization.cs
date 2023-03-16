using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OnlineClientSideInitialization : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject XRPrefab;
    private GameObject _XRRig;
    private GameObject _avatar;
    private readonly string playerTag = "Player";
    void Start()
    {
        if(_avatar == null)
        {
            FindOwnAvatar();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupAvatar()
    {
        VRRig vrRig = _avatar.GetComponent<NetworkPlayer>().vrRig;
        ClientXRRig cxrRig = _XRRig.GetComponent<ClientXRRig>();
        vrRig.head.vrTarget = cxrRig.Head.transform;
        vrRig.leftHand.vrTarget = cxrRig.LeftController.transform;
        vrRig.rightHand.vrTarget = cxrRig.RightController.transform;
    }

    private void FindOwnAvatar()
    {
        GameObject[] avatars = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (GameObject avatar in avatars)
        {
            NetworkBehaviour netId = avatar.GetComponent<NetworkBehaviour>();
            if (netId != null && netId.isOwned)
            {
                _avatar = avatar;
                _XRRig = Instantiate(XRPrefab);
                _XRRig.transform.SetParent(_avatar.transform);
                SetupAvatar();
                return;
            }
        }
    }
}
