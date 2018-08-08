using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
  [SerializeField] float rcsThrust = 100f;
  [SerializeField] float mainThrust = 100f;
  [SerializeField] float levelLoadDelay = 1f;

  [SerializeField] AudioClip mainEngine;
  [SerializeField] AudioClip deathSound;
  [SerializeField] AudioClip nextLevelSound;

  [SerializeField] ParticleSystem mainEngineParticles;
  [SerializeField] ParticleSystem deathParticles;
  [SerializeField] ParticleSystem nextLevelParticles;

  Rigidbody rigidBody;
  AudioSource audioSource;

  enum State
  {
    Alive, Dying, Transcending
  };
  State state = State.Alive;

  // Use this for initialization
  void Start()
  {
    rigidBody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
    // somewhere stop sound on death
    if (state == State.Alive)
    {
      ThrustInput();
      RotateInput();
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    // only check collision if its alive
    if (state != State.Alive) { return; }

    switch (collision.gameObject.tag)
    {
      case "Friendly":
        // print("Friendly");
        break;
      case "Finish":
        StartSuccessSequence();
        break;
      default:
        StartDeathSequence();
        break;
    }
  }

  private void StartDeathSequence()
  {
    state = State.Dying;

    audioSource.Stop();
    audioSource.PlayOneShot(deathSound);

    deathParticles.Play();

    Invoke("LoadFirstLevel", levelLoadDelay);
  }

  private void StartSuccessSequence()
  {
    state = State.Transcending;

    audioSource.Stop();
    audioSource.PlayOneShot(nextLevelSound);

    nextLevelParticles.Play();

    Invoke("LoadNextLevel", levelLoadDelay);
  }

  private void LoadNextLevel()
  {
    SceneManager.LoadScene(1);
  }

  private void LoadFirstLevel()
  {
    SceneManager.LoadScene(0);
  }

  void ThrustInput()
  {
    if (Input.GetKey(KeyCode.Space))
    {
      ApplyThrust();
    }
    else
    {
      audioSource.Stop();
      mainEngineParticles.Stop();
    }
  }

  private void ApplyThrust()
  {
    rigidBody.AddRelativeForce(Vector3.up * mainThrust);
    if (!audioSource.isPlaying)
    {
      audioSource.PlayOneShot(mainEngine);
    }
    mainEngineParticles.Play();
  }

  void RotateInput()
  {
    float rotationThisFrame = rcsThrust * Time.deltaTime;

    rigidBody.freezeRotation = true; // take manual control of rotation
    // its separeted because it can rotate while thrust
    // but cant rotate to left and right at same time
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    {
      transform.Rotate(Vector3.forward * rotationThisFrame);
    }
    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    {
      transform.Rotate(-Vector3.forward * rotationThisFrame);
    }

    rigidBody.freezeRotation = false; // resume physic control of rotation
  }
}
