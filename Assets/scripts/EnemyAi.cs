using UnityEngine;

public class EnemyAi : MonoBehaviour
{
	// Velocidad del enemigo
	private float _speed = 4f;

	// Prefab de la explosión del enemigo (se asigna en el Inspector)
	[SerializeField]
	private GameObject _Enemy_ExplosionPrefab;

	// Sonido de la explosión del enemigo (se asigna en el Inspector)
	[SerializeField]
	private AudioClip _Enemy_ExplosionSound;

	// Referencia al UIManager para actualizar el puntaje
	private UiManager _uiManager;

	// Start is called once antes de que se ejecute el primer Update
	// Se usa para inicializar la referencia del UIManager.
	void Start()
	{
		// Encuentra el objeto "Canvas" en la escena y obtiene su componente UiManager
		_uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
	}

	// Update se llama una vez por frame
	// Es donde se maneja el movimiento del enemigo y las colisiones.
	void Update()
	{
		// Movimiento del enemigo hacia abajo en el eje Y (con velocidad _speed)
		transform.Translate(Vector2.down * _speed * Time.deltaTime);

		// Si el enemigo sale por la parte inferior de la pantalla, se reposiciona en la parte superior con una posición aleatoria en X
		if (transform.position.y < -7)
		{
			float randomX = Random.Range(-8.5f, 8.5f);  // Genera un valor aleatorio entre -8.5 y 8.5
			transform.position = new Vector3(randomX, 8.5f, 7);  // Reposiciona el enemigo en la parte superior con la posición X aleatoria
		}
	}

	// Método que se llama cuando el enemigo colisiona con otro objeto con un Collider2D
	private void OnTriggerEnter2D(Collider2D other)
	{
		// Si el enemigo colisiona con un objeto que tiene el tag "Laser" (posiblemente un disparo del jugador)
		if (other.CompareTag("Laser"))
		{
			// Si el "Laser" tiene un objeto padre, lo destruye (probablemente una nave o un objeto del disparo)
			if (other.transform.parent != null)
			{
				Destroy(other.transform.parent.gameObject);
			}
			// Destruye el objeto "Laser"
			Destroy(other.gameObject);

			// Instancia una explosión en la posición del enemigo
			Instantiate(_Enemy_ExplosionPrefab, transform.position, Quaternion.identity);

			// Actualiza el puntaje del jugador
			_uiManager.UpdateScore();

			// Reproduce el sonido de la explosión en la posición de la cámara
			AudioSource.PlayClipAtPoint(_Enemy_ExplosionSound, Camera.main.transform.position);

			// Destruye al enemigo
			Destroy(this.gameObject);
		}
		// Si el enemigo colisiona con el jugador
		else if (other.CompareTag("Player"))
		{
			// Obtiene el componente Player desde el objeto que colisionó
			Player player = other.GetComponent<Player>();

			// Si el jugador existe, le hace daño
			if (player != null)
			{
				player.Damage();
			}

			// Instancia una explosión en la posición del enemigo
			Instantiate(_Enemy_ExplosionPrefab, transform.position, Quaternion.identity);

			// Reproduce el sonido de la explosión en la posición de la cámara
			AudioSource.PlayClipAtPoint(_Enemy_ExplosionSound, Camera.main.transform.position);

			// Destruye al enemigo
			Destroy(this.gameObject);
		}
	}
}
