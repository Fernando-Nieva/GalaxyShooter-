using UnityEngine;

public class player : MonoBehaviour
{
	public float _moveSpeed = 5f;
	private Rigidbody2D _rb;
	private Vector2 _movement;

	// Límites verticales (Y) y horizontales (X)
	public float minY = -3.94f;
	public float maxY = 3.96f;
	public float minX = -8.5f;
	public float maxX = 8.5f;


	public GameObject laserPrefab; // Prefab del láser
	public float fireRate = 0.5f; // Tasa de disparo (en segundos)
	public float cantfire = 0.0f; // Cantidad de disparos por segundo	

	public bool canTripleShoot = false; // Variable para controlar el disparo triple
	[SerializeField]
	private GameObject _triplshootPrefab;



	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		playerMovement();
		ApplyBoundaries();
		Shoot();

	}



	private void playerMovement()
	{
		// Leer la entrada del teclado
		_movement.x = Input.GetAxisRaw("Horizontal");
		_movement.y = Input.GetAxisRaw("Vertical");

	
	}



	private void Shoot()
	{

		if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
		{
			if (Time.time > cantfire)
			{

				if (canTripleShoot )
			{
				Instantiate(_triplshootPrefab, transform.position + new Vector3(0.25f, 0.30f, 0), Quaternion.identity);
				}
			else
			{
				
				Instantiate(laserPrefab, transform.position + new Vector3(0, 0.69f, 0), Quaternion.identity);	
			}
				cantfire = Time.time + fireRate;
		}

		}

	}



	public void ApplyBoundaries()
	{
		// ✅ Limitar la posición Y del jugador (arriba/abajo)
		// Si el jugador intenta ir más allá de los límites en Y, lo dejamos justo en el borde
		float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

		// ✅ Comportamiento envolvente en el eje X (izquierda/derecha)
		// Si el jugador se va más allá del borde izquierdo, aparece en el derecho, y viceversa
		float wrappedX = transform.position.x;

		if (transform.position.x < minX)
		{
			wrappedX = maxX;
		}
		else if (transform.position.x > maxX)
		{
			wrappedX = minX;
		}

		// Aplicar la nueva posición corregida
		transform.position = new Vector3(wrappedX, clampedY, transform.position.z);
	}


	private void FixedUpdate()
	{
		// Mover al jugador con física
		_rb.MovePosition(_rb.position +_movement * _moveSpeed * Time.fixedDeltaTime);
	}
}
