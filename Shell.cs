using UnityEngine;

public class Shell : MonoBehaviour

{
    public GameObject ricochet;
    public float lifetime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }


   void OnCollisionEnter(Collision collision)
   {
       GameObject explosion = Instantiate(ricochet,transform.position,transform.rotation);
       Destroy(gameObject);
       Destroy(explosion, 1);
   }
}
