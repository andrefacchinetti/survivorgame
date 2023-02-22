using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Image))]
public class GrabUI : MonoBehaviourPunCallbacks
{
    public Sprite DefaultAim, passiveAim, grabAim, interactionAim;

    private Image img;
    private GrabObjects grabSys;

    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = null;

        grabSys = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabObjects>(); ;
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
            else
            {
                img.sprite = DefaultAim;
            }   
        }
    }
}
