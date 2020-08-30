using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float lowerBoundary = -5f;
    [SerializeField] private int powerUpTypeId;

    private AudioManager _audioManager;
    private enum PowerUpType
    {
        TripleShot = 0, SpeedBoost = 1, Shields = 2
    }

    // Update is called once per frame
    public void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("The Audio Manager is null.");
        }
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.down);

        if (transform.position.y < lowerBoundary)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                _audioManager.PlayPowerUp();
                switch (powerUpTypeId)
                {
                    case (int)PowerUpType.TripleShot:
                        player.EnableTripleShot();
                        break;
                    case (int)PowerUpType.SpeedBoost:
                        player.EnableSpeedBoost();
                        break;
                    case (int)PowerUpType.Shields:
                        player.EnableShields();
                        break;
                    default:
                        Debug.LogError("PowerUpType not found.");
                        break;

                }

                Destroy(gameObject);
            }
        }
    }
}
