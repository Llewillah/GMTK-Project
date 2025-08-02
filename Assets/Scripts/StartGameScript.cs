using Unity.VisualScripting;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    public void StartGame() 
    { 
        GoToScene.GoToNextLevel();
    }
}
