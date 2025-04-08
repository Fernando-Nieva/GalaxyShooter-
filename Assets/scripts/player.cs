using UnityEngine;

public class player : MonoBehaviour
{
	public float moveSpeed = 5f;
	private Rigidbody2D rb;
	private Vector2 movement;

	// Límites verticales (Y) y horizontales (X)
	public float minY = -3.94f;
	public float maxY = 3.96f;
	public float minX = -8.5f;
	public float maxX = 8.5f;


	public GameObject laserPrefab; // Prefab del láser

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		playerMovement();
		ApplyBoundaries();
		laser();

	}



	private void playerMovement()
	{
		// Leer la entrada del teclado
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");

	
	}



	private void laser()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Instantiate(laserPrefab, transform.position + new Vector3(0,0.69f,0), Quaternion.identity);
			
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
		rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
	}
}
