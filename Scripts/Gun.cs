using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class Gun : MonoBehaviour
{
    
    [SerializeField]
    public Camera fpsCamera;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    public ParticleSystem muzzleFlash;
    [SerializeField]
    private GameObject holePrefab;
    [SerializeField]
    private GameObject ammoLeftText;
    [SerializeField]
    private GameObject scoreText;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ShootingAgent AgentScript;
    private GameObject bulletHole;
    Target target;
    public float ammoLeft = 8;
    public float score;
    TextMeshProUGUI ammoLeftTMPro;
    TextMeshProUGUI scoreTMPro;
    public bool reloading;
    public float nextFire = 0f;
    public float shotStreak = 0f;
    public float shotsMade = 0f;

    // Start is called before the first frame update
    void Start()
    {
        ammoLeftTMPro = ammoLeftText.GetComponent<TextMeshProUGUI>();
        scoreTMPro = scoreText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void HeuristicUpdate()
    {
        /*if (reloading) {
            Debug.Log("Reloading");
            return;
        }
        
        if (Input.GetButton("Fire1") && Time.time >= nextFire) {
            nextFire = Time.time + 1f/15f;
            Shoot();
        }

        if (Input.GetButtonDown("Fire2") && ammoLeft < 30) StartCoroutine(Reload());;

        /*
        if (reloading) {
            transform.Rotate(Vector3.left * 120 * Time.fixedDeltaTime);

            if (Math.Abs(transform.rotation.y) <= 0.001) {
                ammoLeft = 8;
                reloading = false;
                transform.Rotate(Vector3.zero);
            }
        }
        */
    }

    public void Shoot() {
        if (ammoLeft > 0) {
            //Debug.Log("Shhot 2");

            //Renable After Training Mode
            //muzzleFlash.Play();
            ammoLeft--;
            shotsMade++;
            ammoLeftTMPro.text = ammoLeft.ToString();

            RaycastHit hit;
            if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, 100f)) {
                //Debug.Log(hit.transform.name);
                //bulletHole = Instantiate(holePrefab, hit.point, Quaternion.identity, hit.transform);
                //bulletHole.transform.position += bulletHole.transform.forward / 100;
                //bulletHole.transform.parent = hit.transform;
                //bulletHole.transform.rotation = Quaternion.LookRotation(hit.normal);

                if (hit.transform.name == "Target(Clone)" || hit.transform.name == "Target") {
                    score++;
                    scoreTMPro.text = score.ToString(); 
                    target = hit.transform.GetComponent<Target>(); 
                    target.TakeDamage(2f);
                    shotStreak++;
                    AgentScript.AddReward(Mathf.Clamp01(0.2f * (shotStreak + 1)));
                } else {
                    score--;
                    scoreTMPro.text = score.ToString();
                    shotStreak = 0f;
                    AgentScript.AddReward(-0.1f);
                }
            }
        }
        else if (!reloading) {
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload() {
        animator.SetBool("Reloading", true);
        reloading = true;

        yield return new WaitForSeconds(1.75f);

        animator.SetBool("Reloading", false);

        yield return new WaitForSeconds(.25f);

        reloading = false;
        ammoLeft = 30;
        ammoLeftTMPro.text = ammoLeft.ToString();
    }

    public void ResetGame() {
        animator.SetBool("Reloading", false);
        ammoLeft = 30f;
        score = 0f;
        nextFire = 0f;
        shotsMade = 0f;
        reloading = false;
        ammoLeftTMPro.text = ammoLeft.ToString();
        scoreTMPro.text = score.ToString();
    }
}
