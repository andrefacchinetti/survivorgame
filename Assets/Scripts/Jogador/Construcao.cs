using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construcao : MonoBehaviour {
    public bool doesDisablePlaceHolder;
    public GameObject placeHolder;

    public void disativarPlaceHolder(GameObject target){
        placeHolder = target;
        if(doesDisablePlaceHolder) placeHolder.SetActive(false);
    }

    public void quebrar(){
        if(doesDisablePlaceHolder) placeHolder.SetActive(true);
        Destroy(gameObject);
    }

}