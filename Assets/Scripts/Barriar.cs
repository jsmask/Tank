using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barriar : MonoBehaviour
{
    public AudioClip HitClip;

    public void Hit()
    {   
        try
        {
            AudioSource.PlayClipAtPoint(HitClip, transform.position);
        }
        catch
        {

        }       
    }

}
