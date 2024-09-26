using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
	[Header("File Storage Config")]
	[SerializeField] private string _fileName;
	[SerializeField] private bool _useEncryption;

	[Header("Auto Saving Configuration")]
	[SerializeField] private float autoSaveTimeSeconds = 60f;

	private GameData _gameData;
	private List<IDataPersistence> _dataPersistenceObjects;
	private FileHandler _dataHandler;
	private Coroutine autoSaveCoroutine;
	public static DataPersistenceManager instance { get; private set; }

	private void Awake() 
	{
		Debug.Log(Application.persistentDataPath);
		if (instance != null) 
		{
			Destroy(this.gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(this.gameObject);

		_dataHandler = new FileHandler(Application.persistentDataPath, _fileName, _useEncryption, "secretCodeWord");
	}

	private void OnEnable() 
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable() 
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
	{
		_dataPersistenceObjects = FindAllDataPersistenceObjects();
		LoadGame();

		// start up the auto saving coroutine
		if (autoSaveCoroutine != null) 
		{
			StopCoroutine(autoSaveCoroutine);
		}
		autoSaveCoroutine = StartCoroutine(AutoSave());
	}

	public void NewGame() 
	{
		_gameData = new GameData();
	}

	public void LoadGame()
	{
		// load any saved data from a file using the data handler
		_gameData = _dataHandler.Load();

		// start a new game if the data is null and we're configured to initialize data for debugging purposes
		if (_gameData == null) 
		{
			NewGame();
		}

		// if no data can be loaded, don't continue
		if (_gameData == null) 
		{
			Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
			return;
		}

		// push the loaded data to all other scripts that need it
		foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
		{
			dataPersistenceObj.LoadData(_gameData);
		}
	}

	public void SaveGame()
	{
		// if we don't have any data to save, log a warning here
		if (_gameData == null) 
		{
			Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
			return;
		}

		// pass the data to other scripts so they can update it
		foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
		{
			dataPersistenceObj.SaveData(_gameData);
		}
		Debug.Log("ContainerCount=" + _gameData.Containers.Count);

		// timestamp the data so we know when it was last saved
		_gameData.LastUpdated = System.DateTime.Now.ToString("O");

		// save that data to a file using the data handler
		_dataHandler.Save(_gameData);
	}

	private void OnApplicationQuit() 
	{
		SaveGame();
	}

	private List<IDataPersistence> FindAllDataPersistenceObjects() 
	{
		// FindObjectsofType takes in an optional boolean to include inactive gameobjects
		IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
			.OfType<IDataPersistence>();

		return new List<IDataPersistence>(dataPersistenceObjects);
	}

	public bool HasGameData() 
	{
		return _gameData != null;
	}

	private IEnumerator AutoSave() 
	{
		while (true) 
		{
			yield return new WaitForSeconds(autoSaveTimeSeconds);
			SaveGame();
			Debug.Log("Auto Saved Game");
		}
	}
}