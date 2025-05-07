using UnityEngine;

public class EnemyAi : MonoBehaviour
{



    private float _speed = 4f;
	[SerializeField]
	private GameObject _Enemy_ExplosionPrefab;
	[SerializeField]
	private AudioClip _Enemy_ExplosionSound;
	private UiManager _uiManager;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		_uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

	}

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector2.down * _speed * Time.deltaTime);

		if (transform.position.y < -7)
		{   
            float randomX = Random.Range(-8.5f, 8.5f);
			transform.position = new Vector3 (randomX, 8.5f, 7);
		}

	}




    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.CompareTag("Laser"))
		{
			if (other.transform.parent != null)
			{
				Destroy(other.transform.parent.gameObject);
			}
			Destroy(other.gameObject);
			Instantiate(_Enemy_ExplosionPrefab, transform.position, Quaternion.identity);
			_uiManager.UpdateScore();
			AudioSource.PlayClipAtPoint(_Enemy_ExplosionSound, Camera.main.transform.position);
			Destroy(this.gameObject);
		}
		else if (other.CompareTag("Player"))
		{
			Player player = other.GetComponent<Player>();
			if (player != null)
			{
				player.Damage();
			}
			Instantiate(_Enemy_ExplosionPrefab, transform.position, Quaternion.identity);
			AudioSource.PlayClipAtPoint(_Enemy_ExplosionSound,Camera.main. transform.position);
			Destroy(this.gameObject);
		}


	}
}
