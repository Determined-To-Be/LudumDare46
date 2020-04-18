﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public PlayerController player;

    public float offsetPhi = .68f;
    float theta, phi = .68f;

    public float distance = 12;

    public float LookSmoothness = 10;
    public float TurnSmoothness = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 goalPos = Vector3.zero;

    // Update is called once per frame
    void FixedUpdate()
    {

        //We always want to look at the player
        Quaternion lookAt = Quaternion.LookRotation(player.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookAt, LookSmoothness * Time.deltaTime);

        //The camera is free to move in a circle around the player
        //But the distance should be a fixed distance
        //Also the angle should be relative to the relative back of the vehicle
        //float _theta = theta * Mathf.Deg2Rad;
        //float _phi = phi * Mathf.Deg2Rad;
        theta = Mathf.LerpAngle(theta, -(player.transform.rotation.eulerAngles.y), TurnSmoothness * Time.deltaTime);
        float _theta = (theta - 90) * Mathf.Deg2Rad ;

        phi = Mathf.LerpAngle(phi, (-player.GetSpeedNormalized() * .1f) + offsetPhi, TurnSmoothness * Time.deltaTime);
        float _phi = phi * Mathf.Deg2Rad;
        //TODO phi move the camera down a bit proportional to speed

        goalPos = angle2Vec3(theta, phi) * distance;

        this.transform.position = player.transform.position + angle2Vec3(_theta, phi) * distance;
        //this.transform.position = player.position;

    }

    Vector3 angle2Vec3(float theta, float phi){
        float x = Mathf.Cos (theta) * Mathf.Cos (phi);
        float y = Mathf.Sin (phi);
        float z = Mathf.Sin (theta) * Mathf.Cos (phi);

        return new Vector3(x, y, z);
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(player.transform.position + goalPos, .5f);
    }
}
