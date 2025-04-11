using UnityEngine;

public class laser : MonoBehaviour
{


    public float speed = 10.0f;

    void Start()
    {
        
    }

    void Update()
    {
        
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if(transform.position.y >= 6.0f)
		{
			Destroy(this.gameObject);
		}
	}
}
