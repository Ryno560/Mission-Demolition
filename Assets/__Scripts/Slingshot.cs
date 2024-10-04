using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject[] projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;
    public Transform leftBandPoint; 
    public Transform rightBandPoint;
    public AudioClip shootSound;
    public Dropdown dropdown;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    void Awake() {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 0.25f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;

        audioSource = GetComponent<AudioSource>();
    }

    void UpdateLineRenderer() {
            lineRenderer.enabled = true;
            Vector3 projPos = projectile ? projectile.transform.position : launchPos;

            lineRenderer.SetPosition(0, leftBandPoint.position);
            lineRenderer.SetPosition(1, projPos);
            lineRenderer.SetPosition(2, rightBandPoint.position);
        }
    void OnMouseEnter() {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit() {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown() {
        aimingMode = true;
        projectile = Instantiate(projectilePrefab[dropdown.value]) as GameObject;
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;

        UpdateLineRenderer();
    }

    void Update() {
        if (!aimingMode) {
            lineRenderer.enabled = false;
            return;
        }

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        UpdateLineRenderer();

        if (Input.GetMouseButtonUp(0)) {
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;

            if (shootSound != null && audioSource != null) {
                audioSource.PlayOneShot(shootSound);
            }

            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
            FollowCam.POI = projectile;

            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            MissionDemolition.SHOT_FIRED();

            lineRenderer.enabled = false;
        }

    }
}
