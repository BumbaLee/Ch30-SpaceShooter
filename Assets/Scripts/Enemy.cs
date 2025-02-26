﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header ("Set in Inspector: Enemy")]
    public float speed= 10f; //speed in m/s
    public float fireRate= 0.3f; //seconds/shot (unused)
    public float health= 10;
    public int score= 100; //points earned for destroying this

    protected BoundsCheck bndCheck;

    void Awake(){
        bndCheck= GetComponent<BoundsCheck>();
    }

    //this is a Property: a method that acts like a field
    public Vector3 pos{
        get{
            return(this.transform.position);
        }
        set{
            this.transform.position=value;
        }
    }
    void Update(){
        Move();

        if(bndCheck!= null&&bndCheck.offDown){
                //were on the bottom, so destroy this gameobject
                Destroy(gameObject);
        }
    }
    public virtual void Move(){
        Vector3 tempPos= pos;
        tempPos.y-=speed*Time.deltaTime;
        pos=tempPos;
    }

    void OnCollisionEnter (Collision coll){
        GameObject otherGO= coll.gameObject;
        if(otherGO.tag=="ProjectileHero"){
            Destroy(otherGO);
            Destroy(gameObject);
        }else{
            print("enemy hit by non-projectilHero"+ otherGO.name);
        }
    }
}
