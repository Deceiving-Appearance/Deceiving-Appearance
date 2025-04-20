using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace LRS
{
    public class ScannerGrenade : MonoBehaviour
    {
        [Header("VFX Settings")]
        [SerializeField] private GameObject vfxContainer;
        [SerializeField] private VisualEffect vfxPrefab;

        [Header("Explosion Settings")]
        [SerializeField] private float explosionRadius = 10f;
        [SerializeField] private int pointsPerExplosion = 500;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float fadeDuration = 1.5f;

        [Header("Texture Settings")]
        [SerializeField] private int resolution = 100;

        private bool _createNewVFX = true;
        private List<Vector3> positions = new List<Vector3>();
        private VisualEffect currentVFX;
        private Texture2D texture;
        private Color[] positionsAsColors;

        private void Start()
        {
            Explode();
        }

        public void Explode()
        {
            positions.Clear();

            if (_createNewVFX)
            {
                currentVFX = NewVisualEffect(out texture, out positionsAsColors);
            }

            for (int i = 0; i < pointsPerExplosion; i++)
            {
              
                Vector3 randomDirection = Random.onUnitSphere;
                Vector3 targetPoint = transform.position + randomDirection * explosionRadius;

      
                Vector3 direction = (targetPoint - transform.position).normalized;

                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, explosionRadius, layerMask))
                {
                    if (hit.collider.CompareTag("PointReject")) continue;

                  
                    positions.Add(hit.point);

                 
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    if (rb != null && rb.velocity.magnitude > 0.1f)
                    {
                        StartCoroutine(FadePoint(hit.point));
                    }
                }
            }

            ApplyPositions(positions, currentVFX, texture, positionsAsColors);
        }

        private IEnumerator FadePoint(Vector3 point)
        {
            float time = 0f;
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                yield return null;
            }
            positions.Remove(point);
            ApplyPositions(positions, currentVFX, texture, positionsAsColors);
        }

        private void ApplyPositions(List<Vector3> positionsList, VisualEffect vfx, Texture2D texture, Color[] positions)
        {
            int loopLength = texture.width * texture.height;
            int posListLen = positionsList.Count;

            for (int i = 0; i < loopLength; i++)
            {
                Color data = (i < posListLen)
                    ? new Color(positionsList[i].x, positionsList[i].y, positionsList[i].z, 1)
                    : new Color(0, 0, 0, 0);

                positions[i] = data;
            }

            texture.SetPixels(positions);
            texture.Apply();
            vfx.SetTexture("PositionsTexture", texture);
            vfx.Reinit();
        }

        private VisualEffect NewVisualEffect(out Texture2D texture, out Color[] positions)
        {
            if (!_createNewVFX)
            {
                texture = null;
                positions = new Color[] { };
                return null;
            }

            VisualEffect newVFX = Instantiate(vfxPrefab, transform.position, Quaternion.identity, vfxContainer.transform);
            newVFX.SetUInt("Resolution", (uint)resolution);

            texture = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false);
            positions = new Color[resolution * resolution];

            _createNewVFX = false;
            return newVFX;
        }
    }
}
