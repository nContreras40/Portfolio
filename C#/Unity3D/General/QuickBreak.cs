
//=============================================================================
// Author:  Nathan Contreras
// 

using UnityEngine;
using System.Collections;

public class QuickBreak : MonoBehaviour
{

    [Tooltip("How long to wait before breaking the hinge joints.")]
    public float timeToWait = 2.0f;
    [Tooltip("How much additional speed we will give the child objects before breaking.")]
    public float speed = 5.0f;

    private float timer = 0.0f;

    private bool beginBreaking = false;
    private bool hasPerformed = false;

    public GameObject[] crystals;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && hasPerformed == false)
        {
            beginBreaking = true;
            hasPerformed = true;
        }

        if (beginBreaking)
        {
            breakHinge();
        }
    }

    void breakHinge()
    {
        timer += Time.deltaTime;


        foreach (GameObject crystal in crystals)
        {
            HingeJoint2D[] myJoints = crystal.GetComponentsInChildren<HingeJoint2D>();
            foreach (HingeJoint2D joint in myJoints)
            {
                if (timer >= timeToWait)
                {
                    joint.enabled = false;
                    beginBreaking = false;
                    hasPerformed = true;
                }
                else
                {
                    JointMotor2D motor = joint.motor;
                    motor.motorSpeed += speed;

                    joint.motor = motor;
                }
            }
            Rotation[] childRots = crystal.GetComponentsInChildren<Rotation>();
            foreach (Rotation rot in childRots)
            {
                rot.deltaZ -= speed;
            }
        }

    }
}
