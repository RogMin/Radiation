using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationCounter : MonoBehaviour
{
    [SerializeField] // debug
    private float ClicksInMinute;
    private AudioClip[] GeigerClickSound;
    private AudioClip[] GeigerSwitchSound;
    private AudioSource audioSource;
    public float RoentgenHour;
    [HideInInspector]
    public int GeigerClick;
    public GameObject Pivot;
    public GameObject Switch;
    private int SelectMode = 0;
    private bool CoroutineStart;
    private float LerpMinValue;
    private float LerpMaxValue;
    public int SwitchSpeedCoef= 10;
    private float ModeCoefficent;

    public void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        GeigerClickSound = Resources.LoadAll<AudioClip>("Audio/Geiger/SingleClick");
        GeigerSwitchSound = Resources.LoadAll<AudioClip>("Audio/Geiger/SwitchSound");
    }
    private AudioClip RandomClickSound(AudioClip[] GeigerClickSound)
    {
       var RandomSingleClick = GeigerClickSound[Random.Range(0, GeigerClickSound.Length)];
       return RandomSingleClick;
    }
    private AudioClip RandomSwitchSound(AudioClip[] GeigerSwitchSound)
    {
        var RandomSwitchSound = GeigerSwitchSound[Random.Range(0, GeigerSwitchSound.Length)];
        return RandomSwitchSound;
    }
    public void Update()
    {
        ClicksInMinute = Mathf.Lerp(2.4f, 0.001f, RoentgenInvLerp(RoentgenHour * ModeCoefficent)); //2.4
        if (Input.GetKeyDown("x"))
        {
            SelectMode++;
            audioSource.pitch = Random.Range(0.8f, 1f);
            audioSource.PlayOneShot(RandomSwitchSound(GeigerSwitchSound));
        }   
        switch (SelectMode)
        {
            case 0: //circle off mode

                StopAllCoroutines(); CoroutineStart = false;

                Pivot.transform.localRotation = Quaternion.Lerp(Pivot.transform.localRotation, Quaternion.Euler(0f, 283, 0f), Time.deltaTime * 2);
                Switch.transform.localRotation = Quaternion.Lerp(Switch.transform.localRotation, Quaternion.Euler(270f, -98f, 0f), Time.deltaTime* SwitchSpeedCoef);

                break;
            case 1: //triang. callibrate mode

                StopAllCoroutines(); CoroutineStart = false;

                    Pivot.transform.localRotation = Quaternion.Lerp(Pivot.transform.localRotation, Quaternion.Euler(0f, 346, 0f), Time.deltaTime * 2);
                    Switch.transform.localRotation = Quaternion.Lerp(Switch.transform.localRotation, Quaternion.Euler(270f, 289, 0f), Time.deltaTime * SwitchSpeedCoef);

                break;
            case 2: //200 - 0-200 roentgen/h

                Switch.transform.localRotation = Quaternion.Lerp(Switch.transform.localRotation, Quaternion.Euler(270f, 318, 0f), Time.deltaTime * SwitchSpeedCoef);
                GeigerPivotRotate();ModeCoefficent = 1f; LerpMinValue = 0f; LerpMaxValue = 200f;

                if (!CoroutineStart)
                {
                    CoroutineStart = true; StartCoroutine("GeigerCounter"); 
                }

                break;
            case 3: //x1000 500-5000 microroentgen/h

                Switch.transform.localRotation = Quaternion.Lerp(Switch.transform.localRotation, Quaternion.Euler(270f, 346, 0f), Time.deltaTime * SwitchSpeedCoef);
                GeigerPivotRotate();ModeCoefficent = 1000000f; LerpMinValue = 500f; LerpMaxValue = 5000f;

                if (!CoroutineStart)
                {
                    CoroutineStart = true; StartCoroutine("GeigerCounter");
                }

                break;
            case 4: //x100 50-500 microroentgen/h

                Switch.transform.localRotation = Quaternion.Lerp(Switch.transform.localRotation, Quaternion.Euler(270f, 17, 0f), Time.deltaTime * SwitchSpeedCoef);
                GeigerPivotRotate();ModeCoefficent = 1000000f; LerpMinValue = 50f; LerpMaxValue = 500f;

                if (!CoroutineStart)
                {
                    CoroutineStart = true; StartCoroutine("GeigerCounter");
                }
                break;
            case 5: //x10 5-50 microroentgen/h

                Switch.transform.localRotation = Quaternion.Lerp(Switch.transform.localRotation, Quaternion.Euler(270f, 49, 0f), Time.deltaTime * SwitchSpeedCoef);
                GeigerPivotRotate();ModeCoefficent = 1000000f; LerpMinValue = 5f; LerpMaxValue = 50f;

                if (!CoroutineStart)
                {
                    CoroutineStart = true; StartCoroutine("GeigerCounter");
                }

                break;
            case 6: //x1  0.5-5 microroentgen/h

                Switch.transform.localRotation = Quaternion.Lerp(Switch.transform.localRotation, Quaternion.Euler(270f, 84, 0f), Time.deltaTime * SwitchSpeedCoef);
                GeigerPivotRotate(); ModeCoefficent = 1000000f; LerpMinValue = .5f; LerpMaxValue = 5f;

                if (!CoroutineStart)
                {
                    CoroutineStart = true; StartCoroutine("GeigerCounter");
                }

                break;
            case 7: //x0.1 0.05-0.5 microroentgen/h

                Switch.transform.localRotation = Quaternion.Lerp(Switch.transform.localRotation, Quaternion.Euler(270f, 110, 0f), Time.deltaTime * SwitchSpeedCoef);
                GeigerPivotRotate();ModeCoefficent = 1000000f; LerpMinValue = .1f; LerpMaxValue = .5f; // 1000000f - roentgen to microroentgen

                if (!CoroutineStart)
                {
                    CoroutineStart = true; StartCoroutine("GeigerCounter");
                }

                break;
            case 8:
                SelectMode = 0;
                Switch.transform.localRotation = Quaternion.Lerp(Switch.transform.localRotation, Quaternion.Euler(270f, -262f, 0f), Time.deltaTime* SwitchSpeedCoef);
                break;
        }
    }
    public float RoentgenInvLerp(float RoentgenHour)
    {
        float RoentgenInvLerp = Mathf.InverseLerp(LerpMinValue, LerpMaxValue, RoentgenHour); 
        return RoentgenInvLerp;  //0-200 roent                            
    }
    void GeigerPivotRotate()
    {
            float pivotRotation = Mathf.Lerp(-72f, 58f, RoentgenInvLerp(RoentgenHour *ModeCoefficent)); // -72 min - 58 max
            if (RoentgenHour >= 210)
            {
                Pivot.transform.localRotation = Quaternion.Lerp(Pivot.transform.localRotation, Quaternion.Euler(0f, -71f, 0f), Time.deltaTime * 2); //2-coeff
            }
            else
            {
                Pivot.transform.localRotation = Quaternion.Lerp(Pivot.transform.localRotation, Quaternion.Euler(0f, Random.Range(pivotRotation, pivotRotation+9), 0f), Time.deltaTime * 5); //5 - coeff
            }   
    }
    void FixedUpdate()
    {
        Debug.LogWarning(RoentgenHour + "В мкрч:" + RoentgenHour * 1000000);
        RoentgenHour = 0; 
    }
    IEnumerator GeigerCounter()
    {
        while (CoroutineStart)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(RandomClickSound(GeigerClickSound));
            yield return new WaitForSeconds(ClicksInMinute);
        }
    }
   
}

