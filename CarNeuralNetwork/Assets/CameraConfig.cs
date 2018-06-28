using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Storage for a configuration for the SpringArm class.
/// </summary>
public class CameraConfig : MonoBehaviour {
    
    /// <summary>
    /// Target to follow.
    /// </summary>
    [SerializeField] private Transform target;


    [Header("Distance")]
    /// <summary>
    /// Arm Lenght.
    /// </summary>
    [SerializeField] private float armLenght;

    [Header("Offsets")]
    /// <summary>
    /// Offset of the target.
    /// </summary>
    [SerializeField] private Vector3 targetOffset;
    /// <summary>
    /// Offset of this object.
    /// </summary>
    [SerializeField] private Vector3 offset;

    [Header("Rotation")]
    /// <summary>
    /// Determines if the arm rotates by itself or uses the target rotation.
    /// </summary>
    [SerializeField] private bool useTargetRotation;
    /// <summary>
    /// Stores the actual rotation of the arm.
    /// </summary>
    [SerializeField] private Vector3 rotation;

    [Header("Camera speed")]
    /// <summary>
    /// Arm movement speed.
    /// </summary>
    [SerializeField] [Range(1,200)]public float speed;
    /// <summary>
    /// Arms gameobject rotation speed.
    /// </summary>
    [SerializeField] [Range(1, 200)] private float rotationSpeed;


    [Header("Enviroment collision")]
    /// <summary>
    /// Determines if the arm does a collision test.
    /// </summary>
    [SerializeField] private bool doCollisionTest;


    /// <summary>
    /// Struct of camera config.
    /// </summary>
    private cameraConfig config;

	void Start()
	{
        config = new cameraConfig(target, armLenght, targetOffset, offset, useTargetRotation,rotation, speed,rotationSpeed,doCollisionTest);
	}
    /// <summary>
    /// Returns this objects configuration.
    /// </summary>
    /// <returns></returns>
    public cameraConfig getConfig()
    {
        return config;
    }
}



