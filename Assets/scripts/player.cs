using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("Movimiento")]
	[SerializeField] private float baseSpeed = 5f;
	private float currentSpeed;
	private Rigidbody2D _rb;
	private Vector2 _movement;

	public int lives = 3;

	public bool shieldActive = false; // para activar el escudo
	[SerializeField] private GameObject shieldVisualizer; // para mostrar visual del escudo
	private bool isInvulnerable = false; // para evitar daño múltiple cuando se desactiva el escudo

	[SerializeField] private float minY = -3.94f;
	[SerializeField] private float maxY = 3.96f;
	[SerializeField] private float minX = -8.5f;
	[SerializeField] private float maxX = 8.5f;

	[Header("Disparo")]
	[SerializeField] private GameObject laserPrefab;
	[SerializeField] private GameObject tripleShootPrefab;

	[SerializeField] private float fireRate = 0.5f;
	private float nextFireTime = 0.0f;

	private bool isSpeedPowerUpActive = false;
	private bool isTripleShootActive = false;

	[SerializeField] private GameObject _explosionprefab;

	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		currentSpeed = baseSpeed;
	}

	private void Update()
	{
		HandleInput();
		ApplyBoundaries();
		HandleShooting();
	}

	private void FixedUpdate()
	{
		_rb.MovePosition(_rb.position + _movement * currentSpeed * Time.fixedDeltaTime);
	}

	private void HandleInput()
	{
		_movement.x = Input.GetAxisRaw("Horizontal");
		_movement.y = Input.GetAxisRaw("Vertical");

		currentSpeed = isSpeedPowerUpActive ? baseSpeed * 2f : baseSpeed;
	}

	private void HandleShooting()
	{
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Time.time > nextFireTime)
		{
			nextFireTime = Time.time + fireRate;

			if (isTripleShootActive && tripleShootPrefab != null)
			{
				Instantiate(tripleShootPrefab, transform.position + new Vector3(0.25f, 0.30f, 0), Quaternion.identity);
			}
			else if (laserPrefab != null)
			{
				Instantiate(laserPrefab, transform.position + new Vector3(0, 0.69f, 0), Quaternion.identity);
			}
		}
	}

	private void ApplyBoundaries()
	{
		float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

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
		StartCoroutine(TripleShootPowerDownRoutine());
	}

	public void EnableSpeed()
	{
		isSpeedPowerUpActive = true;
		StartCoroutine(SpeedPowerDownRoutine());
	}

	public void EnableShield()
	{
		shieldActive = true;

		if (shieldVisualizer != null)
		{
			shieldVisualizer.SetActive(true);
		}
		else
		{
			Debug.LogWarning("¡El visualizador del escudo no está asignado!");
		}

		StartCoroutine(ShieldPowerDownRoutine());
		Debug.Log("¡Escudo activado!");
	}

	public void Damage()
	{
		if (isInvulnerable) return;

		if (shieldActive)
		{
			shieldActive = false;

			if (shieldVisualizer != null)
				shieldVisualizer.SetActive(false);

			StartCoroutine(TemporaryInvulnerability()); // pequeña protección extra
			return;
		}

		lives--;

		if (lives <= 0)
		{
			Instantiate(_explosionprefab, transform.position, Quaternion.identity);
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
		yield return new WaitForSeconds(0.5f); // evita daño doble justo al perder escudo
		isInvulnerable = false;
		
	}
	private IEnumerator ShieldPowerDownRoutine()
	{
		yield return new WaitForSeconds(5f); // tiempo que dura el escudo

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
