using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] sceneMusic;  // Array para almacenar la música de cada escena
    private AudioSource audioSource;

    void Awake()
    {
        // Asegurarse de que solo hay un MusicManager
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cambiar la música dependiendo de la escena
        AudioClip clip = sceneMusic[scene.buildIndex];
        if (clip != null && clip != audioSource.clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}