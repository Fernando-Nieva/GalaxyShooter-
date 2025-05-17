using UnityEngine;

public class cube_enemy : MonoBehaviour
{
	public float minRotationSpeed = -180f;
	public float maxRotationSpeed = 360f;

	[SerializeField] private GameObject _ExplosionPrefab;
	[SerializeField] private ParticleSystem _trailPrefab; // Nuevo campo para usar un prefab de partículas

	private float _speed = 4f;
	private Vector3 rotationSpeeds;
	private ParticleSystem trailInstance;

	void Start()
	{
		rotationSpeeds = new Vector3(
			Random.Range(minRotationSpeed, maxRotationSpeed),
			Random.Range(minRotationSpeed, maxRotationSpeed),
			0f // SIN rotación en Z
		);

		if (_trailPrefab != null)
		{
			// Instanciamos el sistema como objeto separado, no hijo
			trailInstance = Instantiate(_trailPrefab, transform.position, Quaternion.identity);
			trailInstance.Play();
		}
		else
		{
			Debug.LogWarning("⚠️ No se asignó un prefab de partículas en el Inspector.");
		}
	}

	void Update()
	{
		transform.Rotate(rotationSpeeds * Time.deltaTime);
		transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);

		// Actualiza posición de las partículas para seguir al cubo
		if (trailInstance != null)
		{
			trailInstance.transform.position = transform.position;
		}

		if (transform.position.y < -7)
		{
			float randomX = Random.Range(-8.5f, 8.5f);
			transform.position = new Vector3(randomX, 8.5f, 7);
		}
	}

	private void OnDestroy()
	{
		if (trailInstance != null)
		{
			trailInstance.Stop();
			Destroy(trailInstance.gameObject, 2f); // Dejamos que termine el efecto
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Laser"))
		{
			Debug.Log("Enemigo impactado por un láser.");
			if (other.transform.parent != null)
				Destroy(other.transform.parent.gameObject);
			Destroy(other.gameObject);
			Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}
		else if (other.CompareTag("Player"))
		{
			Debug.Log("Colisión con el jugador detectada.");
			Player player = other.GetComponent<Player>();
			if (player != null)
			{
				Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
				player.Damage();
			}
			Destroy(this.gameObject);
		}
	}
}
