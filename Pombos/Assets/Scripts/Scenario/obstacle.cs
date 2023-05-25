using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class objectDestruction : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected int obstacleHP = 1; 
    public Transform DropContainer;

    void obstacleDrop(){

    }
    void spawnParticles(){


    }
    void destroyObstacle(){
        //GameObject drop = GameObject.Instantiate(Particle_other, transform.position, Particle_other.transform.rotation, DropContainer);
        //spawnParticles();
        //obstacleDrop();
        
    }
}
