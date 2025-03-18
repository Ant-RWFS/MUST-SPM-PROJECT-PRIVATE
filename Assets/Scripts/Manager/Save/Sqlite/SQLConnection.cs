using UnityEngine;
using SQLite4Unity3d;
public class NewBehaviourScript : MonoBehaviour
{
    public SQLiteConnection connection; 
    private void Start()
    {
        string dataBasePath = Application.streamingAssetsPath + "/SqliteDatabase.db";
        connection = new SQLiteConnection(dataBasePath,SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }
}
