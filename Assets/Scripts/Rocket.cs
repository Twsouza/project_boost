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
  bool isTransitioning =  false;
  bool collisionEnabled = true;

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
    if (!isTransitioning)
    {
      ThrustInput();
      RotateInput();
    }
    if (Debug.isDebugBuild)
    {
      RespondToDebugKeys();
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    // only check collision if its alive
    if (isTransitioning || !collisionEnabled) { return; }

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
    isTransitioning = true;

    audioSource.Stop();
    audioSource.PlayOneShot(deathSound);

    deathParticles.Play();

    Invoke("LoadCurrentLevel", levelLoadDelay);
  }

  private void StartSuccessSequence()
  {
    isTransitioning = true;

    audioSource.Stop();
    audioSource.PlayOneShot(nextLevelSound);

    nextLevelParticles.Play();

    Invoke("LoadNextLevel", levelLoadDelay);
  }

  private void LoadNextLevel()
  {
    int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    if (nextScene >= SceneManager.sceneCountInBuildSettings)
    {
      nextScene = 0;
    }
    SceneManager.LoadScene(nextScene);
  }

  private void LoadCurrentLevel()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  private void ThrustInput()
  {
    if (Input.GetKey(KeyCode.Space))
    {
      ApplyThrust();
    }
    else
    {
      StopThrust();
    }
  }

  private void StopThrust()
  {
    audioSource.Stop();
    mainEngineParticles.Stop();
  }

  private void ApplyThrust()
  {
    rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
    if (!audioSource.isPlaying)
    {
      audioSource.PlayOneShot(mainEngine);
    }
    mainEngineParticles.Play();
  }

  private void RotateInput()
  {
    // remove rotation due to physics
    rigidBody.angularVelocity = Vector3.zero;

    float rotationThisFrame = rcsThrust * Time.deltaTime;
    // its separeted because it can rotate while thrust
    // but cannot rotate to left and right at same time
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    {
      transform.Rotate(Vector3.forward * rotationThisFrame);
    }
    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    {
      transform.Rotate(-Vector3.forward * rotationThisFrame);
    }

  }

  private void RespondToDebugKeys()
  {
    if (Input.GetKeyDown(KeyCode.L))
    {
      LoadNextLevel();
    }
    else if (Input.GetKeyDown(KeyCode.C))
    {
      collisionEnabled = !collisionEnabled;
    }
  }
}
