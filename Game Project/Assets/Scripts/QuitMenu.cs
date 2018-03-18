using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMenu : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveQuit()
    {
        //save game
        Application.Quit();
    }

}