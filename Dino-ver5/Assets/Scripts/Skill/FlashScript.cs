using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class FlashScript : MonoBehaviour
{
    public bool isDestroy = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void ChangeDirectionOfFlash()
    {
        this.GetComponent<SpriteRenderer>().flipX = true;
    }
}
