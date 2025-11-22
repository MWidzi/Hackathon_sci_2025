using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelLogic : MonoBehaviour{
    public CurrentLevel currentLevel;
    void OnTriggerEnter2D(Collider2D collidingObject){
        if(collidingObject.name == "NextLevelDoors"){
            currentLevel.currentLevel++;
            SceneManager.LoadScene("Level"+currentLevel.currentLevel.ToString());
        }
    }
}
