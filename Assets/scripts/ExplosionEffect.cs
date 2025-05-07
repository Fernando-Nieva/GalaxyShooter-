using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Destroy(this.gameObject, 1.5f); // Destroy the explosion effect after 1.5 seconds

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
