using UnityEngine;
using UnityEngine.SceneManagement;

public static class GoToScene
{
    //public Scene startScreen;
    //public Scene level1;
    //public Scene level2;
    //public Scene level3;
    //public Scene level4;
    //public Scene level5;

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
            SceneManager.LoadScene(SceneManager.loadedSceneCount + 1);
        } 
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
}
