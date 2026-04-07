using UnityEngine;

public class PlayerDataInitializer : MonoBehaviour
{

    public void Start()
    {
        
    }
    public void Initialize()
    {
        // Access PlayerData to trigger initialization
        var _ = PlayerData.PlayerCurrentLevel;
    }
}