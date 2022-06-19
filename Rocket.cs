using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotSpeed = 100f;

    [SerializeField] float flySpeed = 100f;

    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] AudioClip finishSound;

    [SerializeField] ParticleSystem flyPart;
    [SerializeField] ParticleSystem boomPart;
    [SerializeField] ParticleSystem finishPart;

    Rigidbody rigidBody;
    AudioSource audioSource;

    /// <summary>
    /// ������������ ��������� ����
    /// </summary>
    enum State
    {
        Playing,
        Dead,
        NextLevel
    };

    State state = State.Playing;

    void Start()
    {
        state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == State.Playing)
        {
            Launch();
            Rotation();
        }
        
    }

    /// <summary>
    /// �������������� ��������
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (state!= State.Playing)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
           
            case "Friendly":

                break;

            case "Battery":

                break;
            case "Finish":

                Finish();

                break;

            default:

                Lose();

                break;
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }


    void LoadFirfsLevel()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// ����� ������
    /// </summary>
    void Lose()
    {
        state = State.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(boomSound);
        boomPart.Play();
        Invoke("LoadFirfsLevel", 1.5f);
    }
    /// <summary>
    /// ������ ���������� ��������
    /// </summary>
    void Finish ()
    {
        state = State.NextLevel;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        finishPart.Play();
        Invoke("LoadNextLevel", 2.0f);
    }

    /// <summary>
    /// ������ ������ � ����
    /// </summary>
    void Launch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed);
            if (!audioSource.isPlaying)
            {   
                audioSource.PlayOneShot(flySound);
                flyPart.Play();
            }
            
        }
        else
        {
            audioSource.Pause();
            flyPart.Stop();
        }
        
    }

    /// <summary>
    /// ���������� �������
    /// </summary>
    void Rotation()
    {   float rotationSpeed = rotSpeed * Time.deltaTime;

        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }
}
