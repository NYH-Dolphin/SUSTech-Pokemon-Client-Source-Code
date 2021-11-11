using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpManager : MonoBehaviour
{
    public void PassOP()
    {
        SceneManager.LoadScene("Intro");
    }
    
}
