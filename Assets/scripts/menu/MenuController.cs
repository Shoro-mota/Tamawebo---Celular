using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;  // Asigna el panel del menú en el inspector

    void Start()
    {
        // Asegúrate de que el panel del menú está oculto al inicio
        menuPanel.SetActive(false);
    }

    public void ToggleMenu()
    {
        // Cambia la visibilidad del panel del menú
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void LoadScene(string sceneName)
    {
        // Carga la escena especificada
        SceneManager.LoadScene(sceneName);
    }
}