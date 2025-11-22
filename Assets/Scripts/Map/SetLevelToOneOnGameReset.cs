using UnityEngine;

public class SetLevelToOneOnGameRestart : MonoBehaviour{
    public CurrentLevel currentLevel;
    void Start(){
        currentLevel.currentLevel = 1;
    }
}
