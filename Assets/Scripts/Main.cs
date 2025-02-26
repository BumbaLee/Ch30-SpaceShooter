﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Main : MonoBehaviour
{
    static public Main S; //a singleton for Main
    static Dictionary<WeaponType,WeaponDefinition> WEAP_DICT;

    [Header ("Set in Inspector")]
    public GameObject[] prefabEnemies; //arrary of Enemy prefabs
    public float enemySpawnPerSecond= 0.5f; //#enemies/second
    public float enemyDefaultPadding= 1.5f; // padding for position
    public WeaponDefinition[] weaponDefinitions; 

    private BoundsCheck bndCheck;

    void Awake(){
        S=this;
        //set bndCheck to reference the BoundsCheck component of this GameObject
        bndCheck=GetComponent<BoundsCheck>();
        //invoke SpawnEnemy() once (in 2 seconds, based on default values)
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);
        WEAP_DICT= new Dictionary<WeaponType,WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions){
            WEAP_DICT[def.type]=def;
        }
       
    }

    public void SpawnEnemy(){
        //pick a random Enemy prefab to instantiate
        int ndx=Random.Range(0,prefabEnemies.Length);
        GameObject go= Instantiate<GameObject>(prefabEnemies[ndx]);

        //position the enemy above the screen with a random x position
        float enemyPadding= enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>()!= null){
            enemyPadding= Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //set the initial position for the spawned Enemy
        Vector3 pos= Vector3.zero; 
        float xMin= -bndCheck.camWidth+enemyPadding;
        float xMax= bndCheck.camWidth-enemyPadding;
        pos.x= Random.Range(xMin,xMax);
        pos.y= bndCheck.camHeight+enemyPadding;
        go.transform.position= pos;

        //Invoke SpawnEnemy() again
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay){
        //invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart(){
        //reload scene to restart the game
        SceneManager.LoadScene("Chs-30-31-SpaceShmup");
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt){
        if(WEAP_DICT.ContainsKey(wt)){
            return(WEAP_DICT[wt]);
        }
        return(new WeaponDefinition());
    }

    
}
