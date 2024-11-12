using UnityEngine;

public class HideUI : MonoBehaviour
{
    public GameObject elementToHide;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.X))
        {
            elementToHide.SetActive(true);
        }
        else
        {
            elementToHide.SetActive(false);
        }
    }
}
