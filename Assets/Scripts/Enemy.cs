﻿using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;



public class Enemy : MonoBehaviour
{

    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float lowerBoundary = -5.0f;
    [SerializeField] private float upperBoundary = 7.0f;
    [SerializeField] private float sideBoundary = 8.0f;
    
    //Cached variables
    private Player _player;
    private Animator _animator;
    private static readonly int OnEnemyDeath = Animator.StringToHash("OnEnemyDeath");
    private Collider2D _collider2D;
    private AudioManager _audioManager;

    //Start is called before the first frame update
    void Start()
    {
        _collider2D = gameObject.GetComponent<Collider2D>();
        //init cached references
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is null.");
        }

        _animator = gameObject.GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator is null");
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
        transform.Translate(Time.deltaTime * speed * Vector3.down);
        
        if (transform.position.y <= lowerBoundary)
        {
            float randomX = Random.Range(-sideBoundary, sideBoundary);
            transform.position = new Vector3(randomX, upperBoundary, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                player.DamagePlayer();
            }

            StartCoroutine(EnemyDeathRoutine());
            
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddToScore(10);
            }

            StartCoroutine(EnemyDeathRoutine());
        }

    }

    private IEnumerator EnemyDeathRoutine()
    {
        _collider2D.enabled = false; // stops the enemy from damaging player before the enemy object is destroyed
        speed /= 2;
        _animator.SetTrigger(OnEnemyDeath); // 0 is on enemy death
        _audioManager.PlayExplosion();
        yield return new WaitForSeconds(2.8f);
        Destroy(gameObject);
        
    }
}
