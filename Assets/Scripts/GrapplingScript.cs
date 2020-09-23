using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingScript : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 GrapplingTo;
    public LayerMask whatIsGrapplable;
    public Transform grapplePointOnPlayer;
    private SpringJoint joint;
    public float maxDistanceMultiplierForJoint = 0.8f;
    public float minDistanceMultiplierForJoint = 0.25f;
    public float JointSpring = 4.5f;
    public float JointDamper = 7f;
    public float JointMassScale = 4.5f; 
    public float DeleteJointThreshold = 0.6f;
    float DistanceToPoint;
    bool DestroyJoint;
    bool LeftClick;
    bool RightClick;
    bool CanLeftClick = true;
    bool CanRightClick = true;
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update(){
        DestroyJoint = DistanceToPoint < DeleteJointThreshold;
        if (CanLeftClick){
            LeftClick = Input.GetMouseButtonDown(0);
        }
        if (CanRightClick){
            RightClick = Input.GetMouseButton(1);
        }
    }
    void FixedUpdate()
    {
        DistanceToPoint = Vector3.Distance(transform.position, GrapplingTo);
        if (LeftClick){
            GrappleToPoint();
            CanLeftClick = false;
            LeftClick = false;
        }
        else if (RightClick){
            PullObject();
            CanRightClick = false;
            RightClick = false;
        }
        if (DestroyJoint){
            Destroy(joint);
            lr.positionCount = 0;
            CanLeftClick = true;
        }
    }

    void LateUpdate(){
        DrawRope();
    }


    void GrappleToPoint(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maxDistance: Mathf.Infinity, layerMask: 1<<8)){
            GrapplingTo = hit.point;
            joint = transform.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = GrapplingTo;

            joint.maxDistance = DistanceToPoint * maxDistanceMultiplierForJoint;
            joint.minDistance = DistanceToPoint * minDistanceMultiplierForJoint;

            joint.spring = JointSpring;
            joint.damper = JointDamper;
            joint.massScale = JointMassScale;
        }
    }

    void DrawRope(){

        if (!joint) return;
        lr.positionCount = 2;
        lr.SetPosition(0, grapplePointOnPlayer.position);
        lr.SetPosition(1, GrapplingTo);
    }
    void DeleteRope(){
        lr.positionCount = 0;
    }

    void PullObject(){

    }
}