using UnityEngine;
using UnityEngine.SceneManagement;

public static class GoToScene
{
    public static Scene StartScreen;
    public static Scene Level1;
    public static Scene Level2;
    public static Scene Level3;
    public static Scene Level4;
    public static Scene Level5;

    public static void GoToStartScreen()
    {
        SceneManager.LoadScene(0);
    }

    public static void GoToNextLevel()
    {
        if (SceneManager.loadedSceneCount == 0)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.loadedSceneCount);
        } 
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
}
