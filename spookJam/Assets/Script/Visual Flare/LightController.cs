using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    [SerializeField]
    Light insideLight;
    [SerializeField]
    Color insideFlickerColor;

    [SerializeField]
    List<Light> lighting = new List<Light>();
    [SerializeField]
    Color outsideFickerColor;


    [SerializeField]
    float flickerPeriodmin;
    [SerializeField]
    float flickerPeriodmax;
    [SerializeField]
    int flickerCountMax;
    int FlickerTimes;
    float timerToFlicker;
    float timer = 0.0f;
    float flickerPeriod;


    Color insideLightColor;
    Color OutsideLightColor;

    bool toggle = true;


    void Start()
    {
        timerToFlicker = Random.Range(5.0f, 10.0f);
        insideLightColor = insideLight.color;
        OutsideLightColor = lighting[0].color;
        flickerPeriod = Random.Range(flickerPeriodmin, flickerPeriodmax);
    }
	
	// Update is called once per frame
	void Update ()
    {

        timer += Time.deltaTime;

        if(timer > timerToFlicker)
        {
            FlickerTimes = Random.Range(1, flickerCountMax);
            StartCoroutine("Flicker");
            timer = 0.0f;
        }

	}

    IEnumerator Flicker()
    {

        for (int count = 0; count < FlickerTimes; count++)
        {
            // Flash the light
            if(toggle)
            {
                insideLight.color = insideFlickerColor;

                foreach (var l in lighting)
                {
                    l.color = outsideFickerColor;
                }

            }
            else
            {
                insideLight.color = insideLightColor;

                foreach (var l in lighting)
                {
                    l.color = OutsideLightColor;
                }

            }

            toggle = !toggle;

            yield return new WaitForSeconds(flickerPeriod);

            flickerPeriod = Random.Range(flickerPeriodmin, flickerPeriodmax);

        }

        // Ensure the light goes back on

        insideLight.color = insideLightColor;
        foreach (var l in lighting)
        {
            l.color = OutsideLightColor;
        }

    }

}
