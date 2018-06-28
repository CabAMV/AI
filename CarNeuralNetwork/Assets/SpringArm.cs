using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that acts as an automatic spring arm by the given configuration.
/// </summary>
public class SpringArm : MonoBehaviour
{
    private Vector3 desiredPosition;

    [Header("Target and distance")]
    /// <summary>
    /// Target to follow.
    /// </summary>
    [SerializeField]
    private Transform target;
    /// <summary>
    /// Arm Lenght.
    /// </summary>
    [SerializeField] private float armLenght;
    /// <summary>
    /// Length calculated after collision tests 
    /// </summary>
    private float desiredArmLenght;

    [Header("Offsets")]
    /// <summary>
    /// Offset of the target.
    /// </summary>
    [SerializeField]
    private Vector3 targetOffset;
    /// <summary>
    /// Offset of this object.
    /// </summary>
    [SerializeField] private Vector3 offset;

    [Header("Rotation")]
    /// <summary>
    /// Determines if the arm rotates by itself or uses the target rotation.
    /// </summary>
    [SerializeField]
    private bool useTargetRotation;
    /// <summary>
    /// Stores the actual rotation of the arm.
    /// </summary>
    [SerializeField] private Vector3 rotation;

    [Header("Rotation Limits")]
    [SerializeField]
    bool useXAngleLimit;
    [SerializeField] bool useYAngleLimit;
    [SerializeField] bool useZAngleLimit;
    [SerializeField] private Vector3 minAngleLimit;
    [SerializeField] private Vector3 maxAngleLimit;

    [Header("Camera speed")]
    /// <summary>
    /// Arm movement speed.
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float speed;
    /// <summary>
    /// Arms gameobject rotation speed.
    /// </summary>
    [SerializeField] [Range(0, 100)] private float rotationSpeed;

    private float finalSpeed;

    [Header("Enviroment collision")]
    /// <summary>
    /// Determines if the arm does a collision test.
    /// </summary>
    [SerializeField]
    private bool doCollisionTest;
    /// <summary>
    /// List of corner points required to do the collision test. 
    /// </summary>
    private List<Vector3> lista = new List<Vector3>();
    /// <summary>
    /// Reference of the velocity required in the movement interpolation process.
    /// </summary>
    Vector3 velocity;


    private void Start()
    {
        desiredArmLenght = armLenght;
        finalSpeed = speed;
    }


    void FixedUpdate()
    {

        updateTransform();
        if (doCollisionTest)
        {
            CollisionTest(desiredPosition);
            return;
        }
        else
        {
            desiredArmLenght = armLenght;
            finalSpeed = speed;
        }

    }

    /// <summary>
    /// Checks if something is in between the target and the gameobject this script is associated.
    /// </summary>
    /// <param name="origin"></param>
    private void CollisionTest(Vector3 origin)
    {
        RaycastHit hit;
        Debug.DrawRay(origin, -transform.forward * armLenght * Mathf.Abs(offset.z), Color.red);
        //center
        if (Physics.Raycast(origin, -transform.forward, out hit, desiredArmLenght + 0.5f))
        {
            if (hit.collider.gameObject != target.gameObject)
            {

                desiredArmLenght = Vector3.Distance(origin, hit.point) - 0.2f;
                finalSpeed = 15;
                return;
            }

        }

        else
        {
            desiredArmLenght = armLenght;
            finalSpeed = speed;
        }
        lista.Clear();

        lista.Add(transform.position + (transform.right * 4) + (transform.up * 4) - transform.forward * 1.5f);
        lista.Add(transform.position + (transform.right * 4) + (-transform.up * 4) - transform.forward * 1.5f);
        lista.Add(transform.position + (-transform.right * 4) + (transform.up * 4) - transform.forward * 1.5f);
        lista.Add(transform.position + (-transform.right * 4) + (-transform.up * 4) - transform.forward * 1.5f);


        foreach (Vector3 element in lista)
        {

            if (Physics.Raycast(origin, element - origin, out hit, desiredArmLenght))
            {
                if (hit.collider.gameObject != target.gameObject)
                {
                    desiredArmLenght = Vector3.Distance(origin, hit.point) - 0.2f;
                    finalSpeed = 20;
                    return;
                }
            }

            else
            {
                desiredArmLenght = armLenght;
                finalSpeed = speed;
            }

        }
    }

    /// <summary>
    /// Updates the transform loaction and rotation.
    /// </summary>
    private void updateTransform()
    {
        //if collision test is done shouldnt use offset
        if (useTargetRotation)
        {
            Vector3 rotX = target.right * targetOffset.x;
            Vector3 rotY = target.up * targetOffset.y;
            Vector3 rotZ = target.forward * targetOffset.z;

            Vector3 vectorX = target.right * offset.x;
            Vector3 vectorY = target.up * offset.y;
            Vector3 vectorZ = target.forward * offset.z * desiredArmLenght;


            transform.position = Vector3.SmoothDamp(transform.position, target.position + vectorX + vectorY + vectorZ, ref velocity, 1 / finalSpeed);
            if (desiredArmLenght != 0)
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation((target.position + rotX + rotY + rotZ) - this.transform.position), rotationSpeed * Time.deltaTime);

            if (doCollisionTest)
            {
                desiredPosition = target.position + rotX + rotY + rotZ;
            }


        }

        else
        {
            Vector3 rotX = target.right * targetOffset.x;
            Vector3 rotY = target.up * targetOffset.y;
            Vector3 rotZ = target.forward * targetOffset.z;
            Vector3 vectorX = target.right * offset.x;
            Vector3 vectorY = target.up * offset.y;
            Quaternion eulerRot = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

            Vector3 desiredPos = target.position + eulerRot * new Vector3(0, 0, -1 * desiredArmLenght);
            desiredPos = desiredPos + vectorX + vectorY;

            transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, 1 / finalSpeed);
            if (desiredArmLenght != 0)
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation((target.position + rotX + rotY + rotZ) - this.transform.position), rotationSpeed * Time.deltaTime);
            if (doCollisionTest)
            {
                desiredPosition = target.position + rotX + rotY + rotZ;
            }

        }




    }


    /// <summary>
    /// Changes the target to follow.
    /// </summary>
    /// <param name="newTarget"></param>
    public void changeTarget(cameraConfig newTarget)
    {
        speed = newTarget.speed;
        rotationSpeed = newTarget.rotationSpeed;
        desiredArmLenght = armLenght = newTarget.armLenght;
        targetOffset = newTarget.targetOffset;
        offset = newTarget.offset;
        useTargetRotation = newTarget.useTargetRotation;
        rotation = newTarget.rotation;
        target = newTarget.target;
        doCollisionTest = newTarget.doCollisionTest;

    }

    /// <summary>
    /// Returns the actual configuration of the arm.
    /// </summary>
    /// <returns></returns>
    public cameraConfig getConfig()
    {
        return new cameraConfig(target, armLenght, targetOffset, offset, useTargetRotation, rotation, speed, rotationSpeed, doCollisionTest);
    }
    /// <summary>
    /// Increments the actual rotation by the rotation given.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void addRotation(float x, float y, float z)
    {
        if (useXAngleLimit)
        {
            if (rotation.x + x >= minAngleLimit.x && rotation.x + x <= maxAngleLimit.x)
                rotation.x += x;
        }
        else
            rotation.x += x;
        if (useYAngleLimit)
        {
            if (rotation.y + y >= minAngleLimit.y && rotation.y + y <= maxAngleLimit.y)
                rotation.y += y;
        }
        else
            rotation.y += y;
        if (useZAngleLimit)
        {
            if (rotation.z + z >= minAngleLimit.z && rotation.z + z <= maxAngleLimit.z)
                rotation.z += z;
        }
        else
            rotation.z += z;

    }

    /// <summary>
    /// Returns the actual rotation of the arm.
    /// </summary>
    /// <returns></returns>
    public Vector3 getRotation()
    {
        return rotation;
    }
    /// <summary>
    /// Sets the length of the arm.
    /// </summary>
    /// <param name="lenght"></param>
    public void setArmLenght(float lenght)
    {
        armLenght = lenght;
    }
    /// <summary>
    /// Returns the arm length.
    /// </summary>
    /// <returns></returns>
    public float getArmLenght()
    {
        return armLenght;
    }
    /// <summary>
    /// Sets the rotation limits are in use.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void setUseRotationLimits(bool x, bool y, bool z)
    {
        useXAngleLimit = x;
        useYAngleLimit = y;
        useZAngleLimit = z;

    }
    /// <summary>
    /// Sets the minimum angle limits 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void setMinLimits(float x, float y, float z)
    {
        minAngleLimit = new Vector3(x, y, z);

    }
    /// <summary>
    /// Sets the maximum angle limits.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void setMaxLimits(float x, float y, float z)
    {
        maxAngleLimit = new Vector3(x, y, z);
    }



}


public struct cameraConfig
{
    public Transform target;
    public float armLenght;
    public Vector3 targetOffset;
    public Vector3 offset;
    public bool useTargetRotation;
    public Vector3 rotation;
    public float speed;
    public float rotationSpeed;
    public bool doCollisionTest;

    public cameraConfig(Transform target, float armLenght, Vector3 targetOffset, Vector3 offset, bool useTargetRotation, Vector3 rotation, float speed, float rotationSpeed, bool doCollisionTest)
    {
        this.target = target;
        this.armLenght = armLenght;
        this.targetOffset = targetOffset;
        this.offset = offset;
        this.useTargetRotation = useTargetRotation;
        this.rotation = rotation;
        this.speed = speed;
        this.rotationSpeed = rotationSpeed;
        this.doCollisionTest = doCollisionTest;
    }
}



