using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject rightWeaponAmmo;
    public GameObject leftWeaponAmmo;

    public Transform rightFirePos;
    public Transform leftFirePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftWeaponFire();
        }
        if (Input.GetMouseButtonDown(1))
        {
            RightWeaponFire();
        }
    }
    void RightWeaponFire()
    {
        Instantiate(rightWeaponAmmo, rightFirePos.position, rightFirePos.rotation);
    }
    void LeftWeaponFire()
    {
        Instantiate(leftWeaponAmmo, leftFirePos.position, leftFirePos.rotation);
    }
}
