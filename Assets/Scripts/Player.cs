using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private int lives = 3;
    [SerializeField] private int score = 0;
    [SerializeField] private GameObject leftEngineDamage;
    [SerializeField] private GameObject rightEngineDamage;

    //play area dimensions
    //TODO: move these to a general game options class.  these are reused here and in other classes
    [SerializeField] private float speedMultiplier = 2f;
    [SerializeField] private float upperBoundary = 0f;
    [SerializeField] private float lowerBoundary = -3.8f;
    [SerializeField] private float sideBoundary = 11.3f;

    //laser weapon
    [SerializeField] private GameObject laser;
    [SerializeField] private float laserOffset = 0.8f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float nextFire = 0.0f;

    //power ups
    [SerializeField] private GameObject tripleShot;
    [SerializeField] private GameObject shieldVisualizer;
    [SerializeField] private bool isTripleShotActive = false;
    [SerializeField] private bool isSpeedBoostActive = false; // for visibility in inspector
    [SerializeField] private bool isShieldActive = false;

    

    // cached references
    private Spawner _spawner;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        shieldVisualizer.SetActive(false);

        //init cached references
        _spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        if (_spawner == null)
        {
            Debug.LogError("Spawner is null.");
        }

        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is null.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireWeapon();
    }

    private void CalculateMovement()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Time.deltaTime * playerSpeed * new Vector3(horizontalInput, verticalInput, 0));
        transform.position = new Vector3(transform.position.x,
                   Mathf.Clamp(transform.position.y, lowerBoundary, upperBoundary), 0);


        //this checks the side boundaries and wraps the player to the other side if the boundary is crossed
        var position = transform.position;
        if (position.x >= sideBoundary || position.x <= -sideBoundary)
        {
            transform.position = new Vector3(position.x * -1, position.y, 0);
        }
    }

    private void FireWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            if (isTripleShotActive)
            {
                Instantiate(tripleShot, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(laser, new Vector3(transform.position.x, transform.position.y + laserOffset, 0),
                    Quaternion.identity);
            }


        }
    }

    public void DamagePlayer()
    {

        if (isShieldActive)
        {
            isShieldActive = false;
            shieldVisualizer.SetActive(false);
            return;
        }
        lives--;

        _uiManager.UpdateLives(lives);

        if (lives < 1)
        {
            _spawner.OnPlayerDeath();
            Destroy(gameObject);
        }

        RandomizeEngineDamage();
    }

    private void RandomizeEngineDamage()
    {
        // surely there is a better way to do this!
        var engine = Random.Range(0, 2);
        
        if (engine == 0)
        {
            if (leftEngineDamage.activeInHierarchy == false)
            {
                leftEngineDamage.SetActive(true);
            }
            else
            {
                rightEngineDamage.SetActive(true);
            }
        }
        else
        {
            if (rightEngineDamage.activeInHierarchy == false)
            {
                rightEngineDamage.SetActive(true);
            }
            else
            {
                leftEngineDamage.SetActive(true);
            }
        }

    }

    public void EnableTripleShot()
    {
        isTripleShotActive = true;
        StartCoroutine(PowerDownTripleShot());
    }

    private IEnumerator PowerDownTripleShot()
    {
        yield return new WaitForSeconds(5f);
        isTripleShotActive = false;
    }

    public void EnableSpeedBoost()
    {

        isSpeedBoostActive = true;
        playerSpeed *= speedMultiplier;
        StartCoroutine(PowerDownSpeedBoost());

    }

    private IEnumerator PowerDownSpeedBoost()
    {
        yield return new WaitForSeconds(5f);
        isSpeedBoostActive = false;
        playerSpeed /= speedMultiplier;
    }

    public void EnableShields()
    {
        shieldVisualizer.SetActive(true);
        isShieldActive = true;
    }

    public void AddToScore(int points)
    {
        score += points;
        _uiManager.UpdateScoreText(score);
    }
}
