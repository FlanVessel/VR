using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Configuración de LLave")]
    public string itemID;

    [Header("Zona de entrega")]
    public DropZone linkedDropZone; 
}
