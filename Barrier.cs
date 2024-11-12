using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameObject tiggerObject;
    public Renderer myObject;
    public int hitpoints;
    public GameObject bigExplosion;
    // Start is called before the first frame update
    void Start()
    {
        
        if (!tiggerObject.GetComponent<RingTrigger>().isBarrierActive)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shell"))
        {
            hitpoints--;
            if (hitpoints == 0)
            {
                Destroy(gameObject);
                GameObject explosion = Instantiate(bigExplosion,transform.position,transform.rotation);
                Destroy(explosion, 3);
                tiggerObject.GetComponent<RingTrigger>().isBarrierActive = false;
                myObject.materials[1].SetColor("_EmissionColor", Color.green * 2);
            }
        }
    }
}
