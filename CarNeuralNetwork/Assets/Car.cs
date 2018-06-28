using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    Rigidbody rb;


    [SerializeField] private float carAcc,carDeAcc,maxSpeed;
    private float VerticalInput,HorizontalInput;

    //%frente   % derecha,  % izquierda,
    //OUTPUT: verticalMovement, horizontalRight, horizontalLeft
    private float[] inputs;

    int freezeCounter=0;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

        inputs = new float[3];
  
    }

    private void FixedUpdate()
    {
        //VerticalInput = Input.GetAxis("Vertical");
        //HorizontalInput = Input.GetAxis("Horizontal");
        rb.velocity += transform.forward * carAcc * VerticalInput;


        rb.velocity = Vector3.ClampMagnitude(rb.velocity,maxSpeed);


        Quaternion quat = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + HorizontalInput, transform.rotation.eulerAngles.z);
        rb.MoveRotation(quat);

        Vector3 velocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.velocity = transform.forward * velocity.magnitude;

        GenerateNeuralInputs();
    }
    Vector3 z;
    Vector3 x;

    Vector3 pointTodraw;
    Vector3 pointTodrawE;
    Vector3 pointTodrawW;

    private void GenerateNeuralInputs()
    {
        RaycastHit Fronthit,Westhit, Easthit;
         z = transform.forward * 1;
         x = transform.right * -1;

        Physics.Raycast(transform.position, z+x, out Westhit, 100);
        Physics.Raycast(transform.position, z-x, out Easthit, 100);
        Physics.Raycast(transform.position+transform.forward * 10, z, out Fronthit, 90);


        if (Westhit.collider)
        {
            //print("West     " + Westhit.collider.name);
            pointTodrawW = Westhit.point;
        }
        else
        {
            pointTodrawW = transform.position + z * 50 + x * 50;
        }
        if (Easthit.collider)
        {
            //print("East     " + Westhit.collider.name);
            pointTodrawE = Easthit.point;
        }
        else
        {
            pointTodrawE = transform.position + z * 50 - x * 50;
        }

        if (Fronthit.collider)
        {
            //print("Front     " + Fronthit.collider.name);
            pointTodraw = Fronthit.point;
        }
        else
        {
            pointTodraw = transform.position + z * 50 ;
        }

        inputs[0] = (pointTodraw - transform.position).magnitude / Const.MAX_FRONTAL_DISTANCE;
        inputs[1] = (pointTodrawE - transform.position).magnitude / Const.MAX_LATERAL_DISTANCE;
        inputs[2] = (pointTodrawW - transform.position).magnitude / Const.MAX_LATERAL_DISTANCE;

        float [] outputs= RedNeuronalNaves.instance.CheckAction(inputs);

        print(outputs[0]+"  "+ outputs[1] +"  "+ outputs[2]);

        VerticalInput = outputs[0];
        HorizontalInput = outputs[1] - outputs[2];
        if (inputs[0] == 1) 
        {
            float[] trainnigOut = { 1, 0, 0 };
            RedNeuronalNaves.instance.ReentrenarRed(inputs, trainnigOut);

        }
    }


    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
        Gizmos.DrawLine(transform.position, transform.position + z * 50 );
        Gizmos.DrawLine(transform.position, transform.position + z*50 +x*50);
        Gizmos.DrawLine(transform.position, transform.position + z*50 -x*50);

        Gizmos.DrawCube(pointTodrawE,Vector3.one*20);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(pointTodrawW, Vector3.one*20);
        Gizmos.DrawCube(pointTodraw, Vector3.one * 20);

    }

}
