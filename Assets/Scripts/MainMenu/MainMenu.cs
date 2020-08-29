using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public void LoadGame()
        {
            SceneManager.LoadScene(1); //main game scene
        }
    }
}
