using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileHandler
{
	private readonly string _dataDirectoryPath;
	private readonly string _dataFileName;
	private readonly bool _useEncryption;
	private readonly string _encryptionCodeWord;
	
	public FileHandler(string dataDirectoryPath, string dataFileName, bool useEncryption, string encryptionCodeWord)
	{
		_dataDirectoryPath = dataDirectoryPath;
		_dataFileName = dataFileName;
		_useEncryption = useEncryption;
		_encryptionCodeWord = encryptionCodeWord;
	}
	
	public GameData Load()
	{
		var path = Path.Combine(_dataDirectoryPath, _dataFileName);
		GameData loadedData = null;
		if(File.Exists(path))
		{
			try
			{
				string data;
				using (FileStream stream = new FileStream(path, FileMode.Open))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						data = reader.ReadToEnd();
					}
				}
				
				if(_useEncryption)
				{
					data = EncryptDecrypt(data);
				}
				
				loadedData = JsonUtility.FromJson<GameData>(data);
			}
			catch(Exception e)
			{
				Debug.LogError("Error occured when trying to load file at path: " 
					+ path  + " and backup did not work.\n" + e);
			}
		}
		return loadedData;
	}

    public void Save(GameData data) 
    {
        string path = Path.Combine(_dataDirectoryPath, _dataFileName);
        try 
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (_useEncryption) 
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream)) 
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e) 
        {
            Debug.LogError("Error occured when trying to save data to file: " + path + "\n" + e);
        }
    }

	// the below is a simple implementation of XOR encryption
	private string EncryptDecrypt(string data) 
	{
		string modifiedData = "";
		for (int i = 0; i < data.Length; i++) 
		{
			modifiedData += (char) (data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
		}
		return modifiedData;
	}
}
