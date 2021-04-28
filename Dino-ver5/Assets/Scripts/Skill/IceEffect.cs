using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class IceEffect : MonoBehaviour
{
    public float speed = 20f;
    public float destroyTime = 40f;
    public bool shootLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(shootLeft)
            transform.Translate(Vector2.left * speed * Time.deltaTime); //Translation là sự di chuyển object trong trục tọa độ X,Y hoặc Z
        else
            transform.Translate(Vector2.right * speed * Time.deltaTime); //Translation là sự di chuyển object trong trục tọa độ X,Y hoặc Z
        //transform.Translate(Vector2.up * speed * Time.deltaTime); //Translation là sự di chuyển object trong trục tọa độ X,Y hoặc Z
    }

    [PunRPC]
    public void ChangeDirectionOfIce()
    {
        shootLeft = true;
    }
}
