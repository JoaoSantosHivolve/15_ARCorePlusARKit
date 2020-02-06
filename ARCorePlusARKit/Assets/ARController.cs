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
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                technology = ArTechnology.ArCore;
                break;
            case RuntimePlatform.IPhonePlayer:
                technology = ArTechnology.ArKit;
                break;
        }

        arCore.SetActive(technology == ArTechnology.ArCore);
        arKit.SetActive(technology == ArTechnology.ArKit);
    }
}
