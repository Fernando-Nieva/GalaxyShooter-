using UnityEngine;

public class Game_Manager : MonoBehaviour
{
	public bool gameOver = true; // Indica si el juego está terminado o no

	public GameObject playerPrefab; // Prefab del jugador para instanciar cuando se reinicie

	private UiManager _uiManager; // Referencia al manejador de UI

	private SpanwManager _spawnManager; // Referencia al manejador de spawns

	private void Start()
	{
		// Busca el UiManager en el Canvas
		_uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

		// Busca el SpawnManager en el objeto llamado "Spanw_Manager"
		_spawnManager = GameObject.Find("Spanw_Manager").GetComponent<SpanwManager>();

		// Si encontró el UI, muestra la pantalla del título
		if (_uiManager != null)
		{
			_uiManager.ShowTitleScreen();
		}
		else
		{
			Debug.LogError("No se encontró el UiManager");
		}

		// Verifica que el prefab del jugador esté asignado
		if (playerPrefab == null)
		{
			Debug.LogError("No se asignó el prefab del jugador");
		}
	}

	private void Update()
	{
		// Si el juego está terminado y se presiona espacio
		if (gameOver && Input.GetKeyDown(KeyCode.Space))
		{
			// Si el prefab del jugador está asignado
			if (playerPrefab != null)
			{
				// Instancia el jugador en el centro de la pantalla
				Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

				// Marca que el juego está activo
				gameOver = false;

				// Oculta la pantalla de título
				_uiManager.HideTitleScreen();

				// Comienza el spawn de enemigos y power-ups
				_spawnManager.StartSpawnRoutines();
			}
		}
	}
}
