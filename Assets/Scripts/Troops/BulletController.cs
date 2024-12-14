using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletController : MonoBehaviour
{
    public GameObject target;
    public bool followTarget;
    Vector3 positionTarget;
    public float speed = 10f;
    public int damage;
    public float alturaMax = 5f;
    public float tempsDespl = 1f;
    //public float rangeDamage = 0f;
    //public LayerMask capaEnemics;

    Vector3 initialPos;

    public enum TrajectoryType{
        Direct,
        UpCurve
    };

    public TrajectoryType trajectoryType;

    void Start(){
        if(target != null) positionTarget = target.transform.position;
        initialPos = transform.position;

        if(trajectoryType == TrajectoryType.UpCurve){
            Vector3 vel = ObtenirVelocitatsInicials();
            GetComponent<Rigidbody>().velocity = vel;
        }
    }

    void Update(){

        transform.LookAt(positionTarget);

        if(followTarget && target != null){
            positionTarget = target.transform.position;
        }

        if(trajectoryType == TrajectoryType.Direct){
            Vector3 dir = positionTarget - transform.position;
            float distFrame = speed * Time.deltaTime;

            transform.Translate(dir.normalized * distFrame, Space.World);                
        }
        else if(trajectoryType == TrajectoryType.UpCurve){
            //Potser no cal implementar (TOT a l'Start)
        }
    }

    void OnCollisionEnter(Collision coll){

        /*if(LayerMask.LayerToName(coll.gameObject.layer) == "Ground"){
            if(rangeDamage > 0) TreureVidaEnemicsVoltant();
            Destroy(gameObject);
        }*/

        if(coll.gameObject.tag == "Enemic"){
            //if(rangeDamage == 0){
                if (target != null && target.GetComponent<Health>() != null){
                    target.GetComponent<Health>().TakeDamage(damage);
                }
            /*}
            else{
                //TreureVidaEnemicsVoltant();
            }*/
            Destroy(gameObject);
        }
        else Destroy(gameObject);
    }

    Vector3 ObtenirVelocitatsInicials(){
        Vector3 targetPos = target.transform.position;

        //Obtenim dades del target
        Vector3 targetforward = transform.InverseTransformDirection(target.transform.forward);
        float targetVel = target.GetComponent<NavMeshAgent>().speed;
        if(target.GetComponent<NavMeshAgent>().isStopped == true) targetVel = 0;
        Vector3 targetEndPos = targetPos + targetforward * targetVel * tempsDespl;

        //Obtenim dades del seguent punt que anira el target
        /*int currentPointIndex = target.GetComponent<NavMeshAgent>().pointIndex;
        Vector3 nextPointWayPos = MapGenerator.Instance.wayPoints[currentPointIndex];

        float distPointTarget = Vector3.Distance(target.transform.position, nextPointWayPos);
        float distTargetEndPos = Vector3.Distance(target.transform.position, targetEndPos);

        if(distPointTarget < distTargetEndPos){
            Vector3 dir = MapGenerator.Instance.wayPoints[currentPointIndex+1] - MapGenerator.Instance.wayPoints[currentPointIndex];
            dir = dir.normalized;
            targetEndPos = MapGenerator.Instance.wayPoints[currentPointIndex] + dir * (distTargetEndPos-distPointTarget);
        }*/

        //float yDiff = transform.position.y - targetPos.y;

        //float velY = alturaMax + 1/2 * 9.8f * (tempsDespl);
        //Vector3 velocitats = (targetEndPos - initialPos) / tempsDespl;    

        //float velY = (targetEndPos.y - initialPos.y+1)/tempsDespl;
        float velX = (targetEndPos.x - initialPos.x)/tempsDespl;
        float velZ = (targetEndPos.z - initialPos.z)/tempsDespl;
        
        float gravity = Physics.gravity.y;
        float velY = ((targetEndPos.y+1) - initialPos.y - 0.5f * gravity * Mathf.Pow(tempsDespl, 2)) / tempsDespl;


        //return new Vector3(velocitats.x, velY, velocitats.z);   
        return new Vector3(velX, velY, velZ);  
    }

    /*void TreureVidaEnemicsVoltant(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, rangeDamage, capaEnemics);

        foreach (Collider col in colliders)
        {
            if (col.GetComponent<Health>() != null){
                col.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }*/
}
