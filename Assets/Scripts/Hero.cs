using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; //Singleton
    [Header("Set in Inspector")]
    //controls movement of ship
    public float speed=30;
    public float rollMult=-45;
    public float pitchMult=30;
    public float gameRestartDelay= 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed= 40;

    [Header("Set Dynamically")]
    private GameObject lastTriggerGo= null;
    //declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //create a weaponfiredelegate field named firedelegate
    public WeaponFireDelegate fireDelegate;

    [SerializeField]
    private float _shieldLevel=1; 

    void Awake(){
        if(S==null){
            S=this; //Set the singleston
        }
        fireDelegate+=TempFire;
    }

    void Update(){
        //pull in information from the Input class
        float xAxis= Input.GetAxis("Horizontal");
        float yAxis= Input.GetAxis("Vertical");

        //change transformation.pos based on the axes 
        Vector3 pos= transform.position;
        pos.x+=xAxis*speed*Time.deltaTime;
        pos.y+=yAxis*speed*Time.deltaTime;
        transform.position=pos;

        //rotate the ship to make it feel more dynamic 
        transform.rotation= Quaternion.Euler(yAxis*pitchMult,xAxis*rollMult,0);

        //allow the ship to fire
        //if(Input.GetKeyDown(KeyCode.Space)){
        //   TempFire();
        // }
       
        //use the fireDelegate to fire Weapons
        //first,make sure the button is pressed: axis("jump")
        //then ensure that fireDelegate isnt null to avoid an error
        if (Input.GetAxis("Jump")==1&&fireDelegate!=null){
            fireDelegate();
        }
    }
    
    void TempFire(){
        GameObject projGO= Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position=transform.position;
        Rigidbody rigidB= projGO.GetComponent<Rigidbody>();
        //rigidB.velocity=Vector3.up*projectileSpeed;
        Projectile proj= projGO.GetComponent<Projectile>();
        proj.type=WeaponType.blaster;
        float tSpeed=Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity=Vector3.up*tSpeed;
    }

    void OnTriggerEnter(Collider other){
        Transform rootT= other.gameObject.transform.root;
        GameObject go= rootT.gameObject;
        
        //make sure it's not the same triggering go as last time
        if(go==lastTriggerGo){
            return;
        }
        lastTriggerGo= go;
        if(go.tag=="Enemy"){
            shieldLevel--;
            Destroy(go);
        }else{
            print("Triggered by non-Enemy: "+go.name);
        }
    }

    public float shieldLevel{
        get{
            return(_shieldLevel);
        }
        set{
            _shieldLevel=Mathf.Min(value,4);
            //if the shield is going to be set to less than zero
            if(value<0){
                Destroy(this.gameObject);
                //tell main.s to restart the game after a delay
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}

