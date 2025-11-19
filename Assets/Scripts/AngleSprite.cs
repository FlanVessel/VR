using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AngleSprite : MonoBehaviour
{
    [Header("Sprites en orden (8 direcciones)")]
    [Tooltip("0: Frente, 1: Frente-Derecha, 2: Derecha, 3: Atrás-Derecha, 4: Atrás, 5: Atrás-Izquierda, 6: Izquierda, 7: Frente-Izquierda")] //como una nota si pones encima el cursor
    public Sprite[] directionalSprites = new Sprite[8];

    [Header("Referencias")]
    public Transform vrCamera;          
    public Transform catRoot;           

    private SpriteRenderer _sr;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();

        if (vrCamera == null && Camera.main != null)
        {
            vrCamera = Camera.main.transform;
        }

        if (catRoot == null)
        {
            catRoot = transform.parent;
        }
    }

    void LateUpdate()
    {
        if (vrCamera == null || catRoot == null || directionalSprites == null || directionalSprites.Length == 0)
            return;

        // 1. El visual mira a la cámara (solo en Y)
        Vector3 lookPos = vrCamera.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        // 2. Vector hacia la cámara (en XZ)
        Vector3 toCamera = vrCamera.position - catRoot.position;
        toCamera.y = 0f;
        if (toCamera.sqrMagnitude < 0.0001f)
            return;
        toCamera.Normalize();

        // 3. Forward del gato (root) en XZ
        Vector3 forward = catRoot.forward;
        forward.y = 0f;
        forward.Normalize();

        // 4. Ángulo [0,360)
        float angle = Vector3.SignedAngle(forward, toCamera, Vector3.up);
        if (angle < 0f) angle += 360f;

        // 5. Elegir el índice de sprite
        int numDirections = directionalSprites.Length; // idealmente 8
        float sectorSize = 360f / numDirections;

        // 6. Índice del sprite basado en el ángulo
        int index = Mathf.FloorToInt(angle / sectorSize);
        if (index >= numDirections) index = numDirections - 1;

        // 7. Asignamos el sprite
        Sprite chosen = directionalSprites[index];
        if (chosen != null)
        {
            _sr.sprite = chosen;
        }
    }
}
