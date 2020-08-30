using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 20.0f;

    //Cached references
    [SerializeField] private GameObject explosionPrefab;
    private Spawner _spawner;
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawner = GameObject.Find("Spawner").GetComponent<Spawner>();

        if (_spawner == null)
        {
            Debug.LogError("Spawner is null");
        }

        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("The Audio Manager is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            _spawner.StartSpawning();
            Destroy(other.gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            _audioManager.PlayExplosion();
            Destroy(gameObject, 0.25f);
        }
    }
}
