using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private AudioSource[] audioSources;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayLaserShot()
    {
        audioSources[0].Play();
    }

    public void PlayExplosion()
    {
        audioSources[1].Play();
    }

    public void PlayPowerUp()
    {
        audioSources[2].Play();
    }
}
