using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour {

    //GameObject m_Mon;
    public void Quit()
    {
        
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
