using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlterarCorLetras : MonoBehaviour
{

    TMP_Text text;
    float r;
    float g;
    float b;
    bool subindo;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        ReiniciarCores();
    }

    // Update is called once per frame
    void Update()
    {
        if (subindo)
        {
            if (r < 110 && g >= 110)
            {
                r = r + 1;
            }
            else if (r >= 110)
            {
                g = g - 1;

            }
            if (g <= 0)
            {
                subindo = false;
            }
        }
        else
        {
            if(r >= 110 && g < 110)
            {
                g = g + 1;
            }else if(r > 0 && g >= 110)
            {
                r--;
            }
            if(r <=0 && g >= 110)
            {
                subindo = true;
            }
        }
        
        text.color = new Color32(((byte)r),((byte)g),((byte)b), 255);
    }

    private void ReiniciarCores()
    {
        text.color = new Color(0,110,0);
        r = text.color.r;
        g = text.color.g;
        b = text.color.b;
        subindo = true;
    }

}
