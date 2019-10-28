using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement 

public class Main : MonoBehaviour
{
    static public Main S; //a singleton for Main

    [Header ("Set in Inspector")]
    public GameObject[] prefabEnemies; //arrary of Enemy prefabs
    public float enemySpawnperSecond= 0.5f; //#enemies/second
    public float enemyDefaultPadding= 1.5f // padding for position

    private BoundsCheck bndCheck;

    void Awake(){
        S=this;
        //set bndCheck to reference the BoundsCheck component of this GameObject
        bndCheck=GetComponent<BoundsCheck>();
        //invoke SpawnEnemy() once (in 2 seconds, based on default values)
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);
    }

    public void SpawnEnemy(){
        //pick a random Enemy prefab to instantiate
        int ndx=Random.Range(0,prefabEnemies.length);
        GameObject go= Instatiate<GameObject>(prefabEnemies[ndx]);

        //position the enemy above the screen with a random x position
        float enemyPadding= enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>()!= null){
            enemyPaddding= Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //set the initial position for the spawned Enemy
        Vector3 pos= Vector3.zero; 
        float xMin= -bndCheck.camWidth+enemyPaddding;
        float xMax= bndCheck.camWidth-enemyPaddding;
        pos.x= Random.Range(xMin,xMax);
        pos.y= bndCheck.camHeight+enemyPaddding;
        go.transform.position= pos;

        //Invoke SpawnEnemy() again
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);
    }
    
}
