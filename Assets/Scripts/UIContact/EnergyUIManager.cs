using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUIManager : MonoBehaviour {

    private Slider energySlider;
    public float maxEnergy = 100.0f;

	// Use this for initialization
	void Start () {
        energySlider = GetComponent<Slider>();
        energySlider.maxValue = maxEnergy;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUIEnergy(float energy)
    {
        energySlider.maxValue = maxEnergy;
        energySlider.value = energy;
    }

}
