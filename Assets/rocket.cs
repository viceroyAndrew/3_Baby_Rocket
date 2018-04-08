using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource; // initialising audio..
    [SerializeField] AudioClip[] fartThrusters;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 50f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); // links audioSource variable to gameObject
    }

    private void PlayRandomSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, fartThrusters.Length);
        audioSource.clip = fartThrusters[randomIndex];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
	}

    private void ProcessInput()
    {
        Rotate();
        Thrust();    
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) //take manual control of rotation
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; //resume physics control
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {

            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (audioSource.isPlaying == false)
            {
                PlayRandomSound();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}
