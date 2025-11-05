using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Configuración del Objeto Clave")]
    public string itemID = "Key_01";

    [Header("Zona de entrega asociada")]
    public DropZone linkedDropZone; // DropZone asignada desde el editor
}
