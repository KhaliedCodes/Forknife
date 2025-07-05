using UnityEngine;

public class JsonReader : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;



    public EnemyData GetEnemyData()
    {
        return JsonUtility.FromJson<EnemyData>(jsonFile.text);
    }
}
