using UnityEngine;

public class Fade : MonoBehaviour
{
    private Light myLight;
    public float maxIntensity;
    public float timeFactor;
    private float t = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        myLight = gameObject.GetComponent<Light>();
        myLight.intensity = maxIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        myLight.intensity = Mathf.Lerp(maxIntensity, 0, t);
        t = Mathf.Clamp01(t + timeFactor * Time.deltaTime);
    }
}
