
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
//fit landing bug


public class rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource; // initialising audio..
    enum State { Alive, Dying, Trancending, Debug };
    State state = State.Alive;
    bool CollisionsDisabled = false;
    [SerializeField] float levelLoadDelay = 3f;

    [SerializeField] AudioClip impact;
    [SerializeField] AudioClip laugh;
    [SerializeField] AudioClip levelComplete;
    [SerializeField] AudioClip[] fartThrusters;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 50f;

    [SerializeField] ParticleSystem impactParticles;
    [SerializeField] ParticleSystem fartParticles;
    [SerializeField] ParticleSystem levelCompleteParticles;

    // Use this for initialization
    void Start() {
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
    // somewhere stop sound on death
    {
        if (state == State.Alive)
        {
            ProcessInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Level skipped");
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            print("collisions toggled");
            CollisionsDisabled = !CollisionsDisabled; // toggle collisions
        }
    }
        
    void OnCollisionEnter(Collision collision)
    {
        
        if (state != State.Alive || CollisionsDisabled) return; // ignore collisions when dead
        switch (collision.gameObject.tag)
        {
            case "friendly":
                print("friendly"); // TODO delete later
                break;
            case "fuel":
                print("fuel tank collected");
                break;
            case "Finish":
                StartLevelCompleteSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
           
    }

    private void StartDeathSequence()
    {
        print("dying");
        state = State.Dying;
        Invoke("RestartGame", 3f); // parameterise time
        audioSource.Stop();
        audioSource.PlayOneShot(impact);
        audioSource.PlayOneShot(laugh);
        impactParticles.Play();
    }

    private void StartLevelCompleteSequence()
    {
        print("level complete");
        state = State.Trancending;
        Invoke("LoadNextLevel", levelLoadDelay);
        audioSource.Stop();
        audioSource.PlayOneShot(levelComplete);
        levelCompleteParticles.Play();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; // loop back to start
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void ProcessInput()
    {
        RespondToRotateInput();
        RespondToThrustInput();    
    }

    private void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {

            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            fartParticles.Play();
            if (audioSource.isPlaying == false)
            {
                PlayRandomSound();
            }
        }
        else
        {
            audioSource.Stop();
            fartParticles.Stop();
        }
    }


}
