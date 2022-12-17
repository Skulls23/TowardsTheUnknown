using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // V�rifie si la touche F5 a �t� appuy�e
        if (Input.GetKeyDown(KeyCode.F5))
        {
            // Charge la sc�ne d'index 0 (premi�re sc�ne dans la liste de sc�nes du projet)
            SceneManager.UnloadSceneAsync(1);
            SceneManager.LoadScene(0);
        }
    }

}
