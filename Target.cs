using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    public float health = 20f;
    
    [SerializeField]
    Spawn spawnScript;
    [SerializeField]
    ShootingAgent agentScript;
    [SerializeField]
    Material MidHealthMaterial;
    [SerializeField]
    Material LowHealthMaterial;
    GameObject PlayerAgent;
    Tutorial tutorialScript;

    private void Start() {
        spawnScript = this.transform.parent.GetComponent<Spawn>();
        PlayerAgent = GameObject.FindGameObjectWithTag("Agent");
        agentScript = PlayerAgent.GetComponent<ShootingAgent>();
        tutorialScript = PlayerAgent.GetComponent<Tutorial>();
    }

    public void TakeDamage(float damageTaken) {
        health -= damageTaken;
        if (health <= 0f) {
            spawnScript.Respawn();
            agentScript.AddReward(2f);
            SelfDestruct();
            Debug.Log(gameObject.transform.position);
            if (tutorialScript.tutorial) tutorialScript.TargetDown();
        } else if (health < 5f) {
            GetComponent<Renderer>().material = LowHealthMaterial;
        } else if (health < 14f) {
            GetComponent<Renderer>().material = MidHealthMaterial;
        }
    }

    public Vector3 GetTargetLocation() {
        Vector3 currentPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        return currentPos;
    }

    public Vector3 TargetUpVector {
        get {
            return transform.up;
        }
    }

    private void SelfDestruct() {
        Destroy(gameObject);
    }
}
