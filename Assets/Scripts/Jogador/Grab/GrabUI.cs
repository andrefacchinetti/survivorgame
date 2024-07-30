using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Image))]
public class GrabUI : MonoBehaviourPunCallbacks
{
    public Sprite DefaultAim, passiveAim, grabAim, interactionAim, driveAim;

    private Image img;
    public GrabObjects grabSys;

    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = null;
    }

    void Update()
    {
        
        if (grabSys.grabedObj != null)
        {
            img.sprite = grabAim;
        }
        else
        {
            if (grabSys.possibleGrab)
            {
                img.sprite = passiveAim;
            }
            else if (grabSys.possibleInteraction)
            {
                img.sprite = interactionAim;
            }
            else if (grabSys.possibleDrive)
            {
                img.sprite = driveAim;
            }
            else
            {
                img.sprite = DefaultAim;
            }   
        }
    }
}
