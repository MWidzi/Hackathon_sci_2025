using UnityEngine;
using UnityEngine.Tilemaps;

public class MapWallCollision : MonoBehaviour{
    Tilemap map;
    GridLayout grid;
    string activeGridName;
    void Start(){
        map = GetComponent<Tilemap>();
        grid = map.transform.root.GetComponent<GridLayout>();
        activeGridName = map.transform.parent.GetComponent<GridLayout>().name;
    }
    void OnTriggerStay2D(Collider2D collider){
        BulletScript collidingObject = collider.GetComponent<BulletScript>();
        Vector3 collidingPosition = new Vector3(collider.transform.position.x,collider.transform.position.y,0);
        Vector3Int collidingCell = grid.WorldToCell(collidingPosition);
        if(collidingObject.firedByPlayer){
            if(map.GetTile(collidingCell) != null){
                if(map.GetTile(collidingCell).name == "breakable_barrier"){
                    map.SetTile(collidingCell,null);
                }    
            }
        }
        Destroy(collidingObject.gameObject);
    }
}
