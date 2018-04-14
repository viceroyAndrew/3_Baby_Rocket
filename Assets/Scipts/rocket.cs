
using UnityEngine;
using UnityEngine.SceneManagement;
//fit landing bug


public class rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource; // initialising audio..
    enum State { Alive, Dying, Trancending };
    State state = State.Alive;

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
        // somewhere stop sound on death
    {   
        if (state == State.Alive)
            ProcessInput();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) return; // ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "friendly":
                print("friendly"); // TODO delete later
                break;
            case "fuel":
                print("fuel tank collected");
                break;
            case "Finish":
                print("level complete");
                state = State.Trancending;
                Invoke("LoadNextLevel", 1f); // parameterise time
                break;
            default:
                print("dying");
                state = State.Dying;
                Invoke("RestartGame", 3f); // parameterise time
                break;
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel() // in future allow for more than 2 levels
    {
        SceneManager.LoadScene(1);
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
