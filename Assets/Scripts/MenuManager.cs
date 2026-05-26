using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;

    void Start()
    {
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);

        Time.timeScale = 1f;

        GameManager.Instance.currentState = GameManager.GameState.Playing;
    }

    public void ExitGame()
    {
        Debug.Log("Game Closed");

        Application.Quit();
    }
}