using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public RigidBody rigid;
    [SerializeField]
    private WeaponType _type;

    //this public property masks the field _type and takes action when it is set
    public WeaponType type{
        get{
            return(_type);

        }set{
            SetType(value);
        }
    }

    void Awake(){
        bndCheck= GetComponent<BoundsCheck>();
        rend=GetComponent<Renderer>();
        rigid=GetComponent<RigidBody>();
    }

    void Update(){
        if(bndCheck.offUp){
            Destroy(gameObject);
        }
    }
}
