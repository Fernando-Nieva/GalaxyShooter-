using System.Collections;
using UnityEngine;

public class SpanwManager : MonoBehaviour // Se encarga de generar enemigos y power-ups
{
	[SerializeField]
	private GameObject enemyShipPrefab; // Prefab del enemigo que se va a instanciar

	[SerializeField]
	private GameObject[] powerUps; // Array de power-ups diferentes que se pueden spawnear

	private Game_Manager _gameManeger; // Referencia al Game Manager

	void Start()
	{
		// Busca el GameManager en la escena y accede a su componente
		_gameManeger = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
		//StartCoroutine(EnemySpawnRoutine());
		//StartCoroutine(PowerUpSpawnRoutine());
	}

	public void StartSpawnRoutines()
	{
		// Inicia las dos rutinas de spawn (enemigos y power-ups)
		StartCoroutine(EnemySpawnRoutine());
		StartCoroutine(PowerUpSpawnRoutine());
	}

	IEnumerator EnemySpawnRoutine()
	{
		// Mientras el juego no haya terminado (gameOver = false)
		while (_gameManeger.gameOver == false)
		{
			// Instancia un enemigo en una posición X aleatoria, siempre en la parte superior de la pantalla (y=7)
			Instantiate(enemyShipPrefab, new Vector3(Random.Range(-7f, 7f), 7, 0), Quaternion.identity);

			// Espera 5 segundos antes de instanciar el siguiente enemigo
			yield return new WaitForSeconds(5.0f);
		}
	}

	IEnumerator PowerUpSpawnRoutine()
	{
		// Mientras el juego no haya terminado (gameOver = false)
		while (_gameManeger.gameOver == false)
		{
			// Elige un power-up aleatorio del array
			int randomPowerUp = Random.Range(0, 3);

			// Instancia el power-up en una posición aleatoria en la parte superior
			Instantiate(powerUps[randomPowerUp], new Vector3(Random.Range(-7f, 7f), 7, 0), Quaternion.identity);

			// Espera 5 segundos antes del próximo power-up
			yield return new WaitForSeconds(5.0f);
		}
	}
}
