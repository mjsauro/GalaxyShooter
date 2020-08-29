using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private bool isGameOver;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver)
        {
            SceneManager.LoadScene(1); // current game scene
        }
    }
 
    public void GameOver()
    {
        isGameOver = true;
    }

}
