using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ResetGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        Time.timeScale = 1;
        SceneManager.LoadScene(scene.name);
    }
}
