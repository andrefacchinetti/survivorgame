using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundsController : MonoBehaviourPunCallbacks
{

    [SerializeField] AudioSource soundYourTurn, soundDistribuiuTanque, soundPreparandoAtkMissil, soundTerritorioConquistado;
    [SerializeField] AudioSource sountTiroSoldado, sountTiroTanque, sountTiroMissil, soundTanqueFalhou, soundMissilFalhou;
    [SerializeField] AudioSource soundCardsTroca, soundVictory, soundGameover;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public void TocarSoundYourTurn()
    {
        if (!soundYourTurn.isPlaying) soundYourTurn.Play();
    }

    public void TocarSoundConquistado()
    {
        soundTerritorioConquistado.Play();
    }

    public void TocarSoundDistribuiuTanque()
    {
        PV.RPC("RPC_TocarSoundDistribuiuTanque", RpcTarget.All);
    }

    public void TocarSoundPreparandoAtkMissil()
    {
        if(!soundPreparandoAtkMissil.isPlaying) soundPreparandoAtkMissil.Play();
    }

    public void TocarSoundTiroSoldado()
    {
        sountTiroSoldado.Play();
    }

    public void TocarSoundTiroTanque()
    {
        sountTiroTanque.Play();
    }

    public void TocarSoundTiroMissil()
    {
        sountTiroMissil.Play();
    }

    public void TocarSoundTanqueFalhou()
    {
        soundTanqueFalhou.Play();
    }

    public void TocarSoundMissilFalhou()
    {
        soundMissilFalhou.Play();
    }

    [PunRPC]
    void RPC_TocarSoundDistribuiuTanque()
    {
        soundDistribuiuTanque.Play();
    }

    public void TocarSoundCartasTroca()
    {
        soundCardsTroca.Play();
    }

    public void TocarSoundVictory()
    {
        soundVictory.Play();
    }

    public void TocarSoundGameover()
    {
        soundGameover.Play();
    }

}
