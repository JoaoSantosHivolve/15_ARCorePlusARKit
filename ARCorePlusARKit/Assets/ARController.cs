using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArTechnology
{
    ArCore, ArKit
}

public class ARController : MonoBehaviour
{
    public GameObject arCore;
    public GameObject arKit;

    public ArTechnology technology;

    public void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            technology = ArTechnology.ArCore;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            technology = ArTechnology.ArKit;
        }

        arCore.SetActive(technology == ArTechnology.ArCore);
        arKit.SetActive(technology == ArTechnology.ArKit);
    }
}
