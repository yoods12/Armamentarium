using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMenu : MonoBehaviour
{
    GameObject wheelGear;
    GameObject trackGear;

    // Start is called before the first frame update
    void Start()
    {
        wheelGear = GameObject.Find("WheelGear");
        trackGear = GameObject.Find("TrackGear");
    }

    public void WheelChange()
    {
        trackGear.SetActive(false);
        wheelGear.SetActive(true);
    }
    public void TrackChange()
    {
        wheelGear.SetActive(false);
        trackGear.SetActive(true);
    }
}
