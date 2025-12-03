using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
  [Header("Weapon Settings")]
  [SerializeField] Camera shootCamera;
  [SerializeField] float range = 1000f;
  public float shootDamage;

  [Header("Particle Settings")]
  public ParticleSystem muzzleFlash;

  private void Shoot()
  {
    RaycastHit hit;
    Physics.Raycast(shootCamera.transform.position, shootCamera.transform.forward, out hit, range);
    Debug.Log("Hit : " + hit.transform.name);

    if (hit.transform.tag.Equals("Enemy")) {
      EnemyLogic target = hit.transform.GetComponent<EnemyLogic>();
      target.TakeDamage(shootDamage);
    }
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Vector3 direction = shootCamera.transform.TransformDirection(Vector3.forward) * range;
    Gizmos.DrawRay(shootCamera.transform.position, direction);
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Mouse0)) {
      muzzleFlash.Play();
      Shoot();
    }
  }
}