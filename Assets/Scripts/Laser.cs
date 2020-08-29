using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float laserSpeed = 8.0f;
    [SerializeField] private float upperBoundary = 8.0f;
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * laserSpeed * Vector3.up);

        if (transform.position.y > upperBoundary)
        {
            
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }


        }
    }
}
