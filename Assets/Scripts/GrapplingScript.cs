using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingScript : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 GrapplingTo;
    public LayerMask whatIsGrapplable;
    GameObject ObjectBeingPulled;
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
    bool pulling = false;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();

        Debug.Log("I'm Dead!");
    }


    void Update()
    {
        if (CanLeftClick){
            LeftClick = Input.GetMouseButtonDown(0);
        }
        if (CanRightClick){
            RightClick = Input.GetMouseButtonDown(1);
        }
        if (pulling){
            DistanceToPoint = Vector3.Distance(ObjectBeingPulled.transform.position, GrapplingTo);
        }
        else{
            DistanceToPoint = Vector3.Distance(transform.position, GrapplingTo);
        }
        DestroyJoint = DistanceToPoint <= DeleteJointThreshold;

        if (LeftClick){
            GrappleToPoint();
        }
        else if (RightClick){
            PullObject();
        }
        if (DestroyJoint){
            Destroy(joint);
            lr.positionCount = 0;
            CanLeftClick = true;
            CanRightClick = true;
            pulling = false;
            ObjectBeingPulled = null;
        }
    }

    void LateUpdate(){
        DrawRope();
    }


    void GrappleToPoint(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maxDistance: Mathf.Infinity, layerMask: 1<<8)){
            GrapplingTo = hit.transform.position;
            joint = transform.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = GrapplingTo;

            joint.maxDistance = DistanceToPoint * maxDistanceMultiplierForJoint;
            joint.minDistance = DistanceToPoint * minDistanceMultiplierForJoint;

            joint.spring = JointSpring;
            joint.damper = JointDamper;
            joint.massScale = JointMassScale;
            CanLeftClick = false;
            LeftClick = false;
        }
    }

    void PullObject(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maxDistance: Mathf.Infinity, layerMask: 1<<8)){
            pulling = true;
            ObjectBeingPulled = hit.transform.gameObject;
            GrapplingTo = transform.position;
            joint = ObjectBeingPulled.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = transform.position;

            joint.maxDistance = DistanceToPoint * maxDistanceMultiplierForJoint;
            joint.minDistance = DistanceToPoint * minDistanceMultiplierForJoint;

            joint.spring = JointSpring;
            joint.damper = JointDamper;
            joint.massScale = JointMassScale;

            CanRightClick = false;
            RightClick = false;
        }
    }

    void DrawRope(){

        if (!joint) return;
        lr.positionCount = 2;
        if (pulling) {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, ObjectBeingPulled.transform.position);
        } 
        else {
            lr.SetPosition(0, grapplePointOnPlayer.position);
            lr.SetPosition(1, GrapplingTo);
        }
    }
    void DeleteRope(){
        lr.positionCount = 0;
    }
}