using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using Photon.Pun;
using Steamworks;

public class LoadPreferences : MonoBehaviourPunCallbacks
{
    public AudioMixer audioMixer;
    Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("volume"));
        Screen.fullScreen = PlayerPrefs.GetInt("fullscreen") == 0 ? true : false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        resolutions = Screen.resolutions;
        Resolution resolution = resolutions[resolutions.Length-1];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

}
