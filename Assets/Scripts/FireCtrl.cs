using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    public GameObject rightWeaponAmmo;
    public GameObject leftWeaponAmmo;

    public Transform rightFirePos;
    public Transform leftFirePos;

    int RAmmo = 30;
    int LAmmo = 30;

    public Text RAmmoText;
    public Text LAmmoText;

    AudioSource reloading;

    bool isReloading;
    // Start is called before the first frame update
    void Start()
    {
        reloading = GetComponent<AudioSource>();
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
    void RightWeaponFire()
    {
        if (RAmmo > 0 && !isReloading)
        {
            Instantiate(rightWeaponAmmo, rightFirePos.position, rightFirePos.rotation);
            RAmmo--;
            RAmmoText.text = RAmmo + " / 30";
        }
        else return;
    }
    void LeftWeaponFire()
    {
        if (LAmmo > 0 && !isReloading)
        {
            Instantiate(leftWeaponAmmo, leftFirePos.position, leftFirePos.rotation);
            LAmmo--;
            LAmmoText.text = LAmmo + " / 30";
        }
        else return;
    }
    void Reload()
    {
        isReloading = true;
        if (RAmmo == 30 && LAmmo == 30) return;
        RAmmo = 30;
        LAmmo = 30;
        RAmmoText.text = RAmmo + " / 30";
        LAmmoText.text = LAmmo + " / 30";
        reloading.Play();
        Invoke("ReloadOut", 1f);
    }
    void ReloadOut()
    {
        isReloading = false;
    }

}
