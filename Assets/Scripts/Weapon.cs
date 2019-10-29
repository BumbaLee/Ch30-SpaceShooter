using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///this is an enum of the various possible weapon types
///it also includes a "shield" type to allow a shield power-up
///items marked [NI] below are Not Implemented in the IGDPD book
///<summary>

public enum WeaponType{
    none, //the default/no weapon
    blaster, //a simple blaster
    spread, // two shots simultaneously
    phaser, //[NI] shots that move in waves
    missile, //[NI] homing missles
    laser, //[NI] damage over time
    shield // raise shield level
}
[System.Serializable]
public class WeaponDefinition{
    public WeaponType type=WeaponType.none;
    public string letter;
    public Color color=Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor=Color.white;
    public float damageOnHit=0;
    public float continuousDamage=0;
    public float delayBetweenShots=0;
    public float velocity=20;
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")] [SerializeField]
    private WeaponType _type=WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; //time last shot was fired

    private Renderer collarRend;

    void Start(){
        collar=transform.Find("Collar").gameObject;
        collarRend=collar.GetComponent<Renderer>();

        //collar settype() for the default _type of weapontype.none
        SetType(_type);

        //dynamically create an anchor fr all projectiles
        if(PROJECTILE_ANCHOR==null){
            GameObject go=new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR=go.transform;
        }
        GameObject rootGO=transform.root.gameObject;
        if(rootGO.GetComponent<Hero>()!=null){
            rootGO.GetComponent<Hero>().fireDelegate+=Fire;
        }
    }

    public WeaponType type{
        get{
            return(_type);
        }set{
            SetType(value);
        }
    }

    public void SetType(WeaponType wt){
        _type=wt;
        if(type==WeaponType.none){
            this.gameObject.SetActive(fale);
            return;
        }else{
            this.gameObject.SetActive(true);

        }
        def=Main.GetWeaponDefinition(_type);
        collarRend.material.color=def.color;
        lastShotTime=0;
    }

    public void Fire(){
        //if this.gameObject is inactive, return
        if(!gameObject.activeInHierarchy){
            return;
        }
        //if it hasnt been enough time between shots, return
        if(Time.time-lastShotTime<def.delayBetweenShots){
            return;
        }
        Projectile p;
        Vector3 vel=Vector3.up*def.velocity;
        if(transform.up.y<0){
            vel.y=-vel.y;
        }
        switch(type){
            case WeaponType.blaster:
                p=MakeProjectile();
                p.rigid.velocity=vel;
                break;
            case WeaponType.spread:
                p=MakeProjectile();//make middle projectile
                p.rigid.velocity=vel;
                p=MakeProjectile();//make right projectile
                p.transform.rotation=Quaternion.AngleAxis(10,Vector3.back);
                p.rigid.velocity=p.transform.rotation*vel;
                p=MakeProjectile();//make left projectile
                p.transform.rotation=Quaternion.AngleAxis(-10,Vector3.back);
                p.rigid.velocity=p.transform.rotation*vel;
                break;
        }
    }

    public Projectile MakeProjectile(){
        GameObject go=Instantiate<GameObject>(def.projectilePrefab);
        if(transform.parent.gameObject.tag=="Hero"){
            go.tag="ProjectileHero";
            go.layer=LayerMask.NameToLayer("ProjectileHero");
        }else{
            go.tag="ProjectileEnemy";
            go.layer=LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position=collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR,true);
        Projectile p=go.GetComponent<Projectile>();
        p.type=type;
        lastShotTime=Time.time;
        return (p);
    }

}
