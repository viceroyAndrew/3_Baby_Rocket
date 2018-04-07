using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource; // initialising audio..
    [SerializeField] AudioClip[] fartThrusters;

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
        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (audioSource.isPlaying == false)
            {
                PlayRandomSound();
            }
        }
        else
        {
            audioSource.Stop();
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime *10); //corisponds to left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime *10); //opposite to left
        }

    }
}
