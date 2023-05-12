using System.Collections.ObjectModel;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corzinha : MonoBehaviour
{
    public Color newColor;
    private SpriteRenderer rend; 
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = newColor;
        
    }


}
