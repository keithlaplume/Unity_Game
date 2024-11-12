using UnityEngine;

public class RingTrigger : MonoBehaviour
{
    public Renderer myObject;
    public bool isBarrierActive;
    public AudioSource passthrough_sound;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogWarning("GameManager not found in the scene.");
        }

        if (isBarrierActive)
        {
            myObject.materials[1].SetColor("_EmissionColor", Color.red * 3);
        }
        else
        {
            myObject.materials[1].SetColor("_EmissionColor", Color.green * 2);
        }
    }
    void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.CompareTag("Player") && !isBarrierActive)
        {
            myObject.materials[1].SetColor("_EmissionColor", Color.yellow * 2);
            gameManager.FlyThroughRing(this.gameObject);
            passthrough_sound.Play();
        }
    }
}
