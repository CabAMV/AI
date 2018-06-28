using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxRotation : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private Skybox skybox;

    void Update()
    {
        skybox.material.SetFloat("_Rotation", Time.time * speed);
    }
}
