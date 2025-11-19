using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AngleSpriteEnvironment : MonoBehaviour
{
    [Header("Sprites por dirección")]
    [Tooltip("Orden recomendado: Frente, Frente-Derecha, Derecha, Atrás-Derecha, Atrás, Atrás-Izquierda, Izquierda, Frente-Izquierda")]
    public Sprite[] directionalSprites = new Sprite[8];

    [Header("Referencias")]
    public Transform viewCamera;

    private SpriteRenderer _sr;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();

        if (viewCamera == null && Camera.main != null)
        {
            viewCamera = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (viewCamera == null || directionalSprites.Length == 0)
            return;

        // 1. Vector hacia la cámara (en XZ)
        Vector3 toCamera = viewCamera.position - transform.position;
        toCamera.y = 0f;
        toCamera.Normalize();

        // 2. Forward del objeto (en XZ)
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        // 3. Ángulo entre el frente del objeto y la cámara
        float angle = Vector3.SignedAngle(forward, toCamera, Vector3.up);
        if (angle < 0f) angle += 360f;

        // 5. Buscar índice de sprite
        int numDirections = directionalSprites.Length; // general: 8 pero puede ser 4, 6...
        float sectorSize = 360f / numDirections;

        // 6. Índice de sprite
        int index = Mathf.FloorToInt(angle / sectorSize);
        if (index >= numDirections) index = numDirections - 1;

        // . Asignar sprite
        _sr.sprite = directionalSprites[index];
    }
}
