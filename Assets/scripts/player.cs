using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[Header("Movimiento")]
	[SerializeField] private float baseSpeed = 5f; // Velocidad base del jugador
	private float currentSpeed; // Velocidad actual (puede aumentar con power-up)
	private Rigidbody2D _rb; // Componente Rigidbody2D para movimiento físico
	private Vector2 _movement; // Dirección del movimiento del jugador

	public int lives = 3; // Vidas del jugador

	public bool shieldActive = false; // ¿Está el escudo activo?
	[SerializeField] private GameObject shieldVisualizer; // Objeto visual del escudo
	private bool isInvulnerable = false; // Protege de daño repetido inmediato

	[SerializeField] private float minY = -3.94f; // Límite inferior en Y
	[SerializeField] private float maxY = 3.96f;  // Límite superior en Y
	[SerializeField] private float minX = -8.5f;  // Límite izquierdo en X
	[SerializeField] private float maxX = 8.5f;   // Límite derecho en X

	private UiManager _uiManager; // Referencia al UI manager

	private AudioSource _audioSource; // Componente de audio para efectos de sonido
	[Header("Disparo")]
	[SerializeField] private GameObject laserPrefab; // Prefab del láser normal
	[SerializeField] private GameObject tripleShootPrefab; // Prefab del triple disparo

	[SerializeField] private float fireRate = 0.5f; // Tiempo entre disparos
	private float nextFireTime = 0.0f; // Control de tiempo para el siguiente disparo

	private bool isSpeedPowerUpActive = false; // ¿Power-up de velocidad activo?
	private bool isTripleShootActive = false;  // ¿Triple disparo activo?

	private Game_Manager _gameManager; // Referencia al Game Manager
	[SerializeField] private GameObject _explosionprefab; // Prefab de explosión al morir
	private SpanwManager _spawnManager; // Referencia al Spawn Manager

	[SerializeField]
	private GameObject[] _engines; // Array de motores para el efecto de explosión

	private int hitCount = 0;

	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>(); // Obtenemos Rigidbody2D
		currentSpeed = baseSpeed; // Velocidad inicial

		// Buscamos el UI Manager
		_uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
		if (_uiManager != null)
		{
			_uiManager.UpdateLives(lives); // Mostramos vidas en pantalla
		}

		// Buscamos GameManager y SpawnManager
		_gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
		//_spawnManager = GameObject.Find("Spanw_Manager").GetComponent<SpanwManager>();

		//if (_spawnManager != null)
		//{
		//	_spawnManager.StartSpawnRoutines(); // Iniciamos spawn de enemigos/power-ups
		//}
		//else
		//{
		//	Debug.LogError("No se encontró el SpawnManager");
		//}


		_audioSource = GetComponent<AudioSource>(); // Obtenemos el componente de audio
		hitCount = 0; // Inicializamos contador de impactos
	}

	private void Update()
	{
		HandleInput();       // Movimiento por entrada
		ApplyBoundaries();   // Limita la posición del jugador
		HandleShooting();    // Maneja los disparos
	}

	private void FixedUpdate()
	{
		// Movimiento físico con Rigidbody
		_rb.MovePosition(_rb.position + _movement * currentSpeed * Time.fixedDeltaTime);
	}

	private void HandleInput()
	{
		// Movimiento básico WASD o flechas
		_movement.x = Input.GetAxisRaw("Horizontal");
		_movement.y = Input.GetAxisRaw("Vertical");

		// Aplica velocidad aumentada si hay power-up
		currentSpeed = isSpeedPowerUpActive ? baseSpeed * 2f : baseSpeed;
	}

	private void HandleShooting()
	{

		// Disparo con espacio o clic izquierdo, respetando el fireRate
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Time.time > nextFireTime)
		{
			nextFireTime = Time.time + fireRate;
			_audioSource.Play(); // Reproduce sonido de disparo
			if (isTripleShootActive && tripleShootPrefab != null)
			{
				// Triple disparo activo
				Instantiate(tripleShootPrefab, transform.position + new Vector3(0.25f, 0.30f, 0), Quaternion.identity);
			}
			else if (laserPrefab != null)
			{
				// Disparo normal
				Instantiate(laserPrefab, transform.position + new Vector3(0, 0.69f, 0), Quaternion.identity);
			}
		}
	}

	private void ApplyBoundaries()
	{
		// Limita la posición vertical
		float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

		// Teletransporte horizontal (si se sale por izquierda entra por derecha y viceversa)
		float wrappedX = transform.position.x;
		if (transform.position.x < minX)
		{
			wrappedX = maxX;
		}
		else if (transform.position.x > maxX)
		{
			wrappedX = minX;
		}

		transform.position = new Vector3(wrappedX, clampedY, transform.position.z);
	}

	// === MÉTODOS DE POWER UPS ===

	public void EnableTripleShoot()
	{
		isTripleShootActive = true;
		StartCoroutine(TripleShootPowerDownRoutine()); // Apaga después de 5 segundos
	}

	public void EnableSpeed()
	{
		isSpeedPowerUpActive = true;
		StartCoroutine(SpeedPowerDownRoutine()); // Apaga después de 5 segundos
	}

	public void EnableShield()
	{
		shieldActive = true;

		if (shieldVisualizer != null)
		{
			shieldVisualizer.SetActive(true); // Muestra visual del escudo
		}
		else
		{
			Debug.LogWarning("¡El visualizador del escudo no está asignado!");
		}

		StartCoroutine(ShieldPowerDownRoutine()); // Apaga escudo a los 5 segundos
		Debug.Log("¡Escudo activado!");
	}
	public void Damage()
	{
		if (isInvulnerable) return; // No recibe daño si está invulnerable

		if (shieldActive)
		{
			shieldActive = false;

			if (shieldVisualizer != null)
				shieldVisualizer.SetActive(false);

			StartCoroutine(TemporaryInvulnerability());
			return;
		}

		hitCount++;

		// Activación aleatoria de motores al recibir daño
		if (hitCount == 1 || hitCount == 2)
		{
			// Buscar motores inactivos para evitar repetir
			List<int> inactiveEngines = new List<int>();
			for (int i = 0; i < _engines.Length; i++)
			{
				if (!_engines[i].activeSelf)
					inactiveEngines.Add(i);
			}

			if (inactiveEngines.Count > 0)
			{
				int randomIndex = inactiveEngines[Random.Range(0, inactiveEngines.Count)];
				_engines[randomIndex].SetActive(true);
			}
		}

		lives--;
		_uiManager.UpdateLives(lives);

		if (lives <= 0)
		{
			Instantiate(_explosionprefab, transform.position, Quaternion.identity);
			_gameManager.gameOver = true;
			_uiManager.ShowTitleScreen();
			Destroy(this.gameObject);
		}
		else
		{
			Debug.Log("Vidas restantes: " + lives);
		}
	}



	private IEnumerator TemporaryInvulnerability()
	{
		isInvulnerable = true;
		yield return new WaitForSeconds(0.5f); // Protección corta tras perder escudo
		isInvulnerable = false;
	}

	private IEnumerator ShieldPowerDownRoutine()
	{
		yield return new WaitForSeconds(5f); // Dura 5 segundos
		shieldActive = false;

		if (shieldVisualizer != null)
			shieldVisualizer.SetActive(false);

		Debug.Log("Escudo desactivado por tiempo.");
	}

	private IEnumerator TripleShootPowerDownRoutine()
	{
		yield return new WaitForSeconds(5f);
		isTripleShootActive = false;
	}

	private IEnumerator SpeedPowerDownRoutine()
	{
		yield return new WaitForSeconds(5f);
		isSpeedPowerUpActive = false;
	}
}
