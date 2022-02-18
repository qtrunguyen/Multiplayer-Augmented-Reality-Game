using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
public class ScaleControl : MonoBehaviour
{
    ARSessionOrigin m_ARSessionOrigin;

    public Slider scaleSlider;
    private void Awake()
    {
        m_ARSessionOrigin = GetComponent<ARSessionOrigin>();
    }
    // Start is called before the first frame update
    void Start()
    {
        scaleSlider.onValueChanged.AddListener(OnSliderValueChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSliderValueChange(float value)
    {
        if (scaleSlider != null)
        {
            m_ARSessionOrigin.transform.localScale = Vector3.one / value;
        }
    }
}
