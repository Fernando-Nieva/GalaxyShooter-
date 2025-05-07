using UnityEngine;

public class PowerUp : MonoBehaviour
{
	[SerializeField]
	private float speed = 2f;

	[SerializeField]
	private AudioClip _powerUpSound;

	[SerializeField]
	private int powerupID; // 0 = TripleShoot, 1 = Speed, 2 = Shield

	void Update()
	{


		transform.Translate(Vector3.down * Time.deltaTime * speed);


		if (transform.position.y < -6f)
		{
			Destroy(this.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Player player = other.GetComponent<Player>();
			AudioSource.PlayClipAtPoint(_powerUpSound, Camera.main.transform.position);

			if (player != null)
			{
				switch (powerupID)
				{
					case 0:
						player.EnableTripleShoot();
						break;
					case 1:
						player.EnableSpeed();
						break;
					case 2:
						player.EnableShield();
						break;
				}
			}

			Destroy(this.gameObject);
		}
	}
}
