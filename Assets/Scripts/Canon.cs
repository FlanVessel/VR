using UnityEngine;

public enum TypeShoot
{
    Normal, Fast
}
public class Canon : MonoBehaviour
{
    public TypeShoot typeShoot = TypeShoot.Normal;
    private GameObject _canon;
    public Transform spawn;

    public void ConstantShoot()
    {
        switch (typeShoot)
        {
            case TypeShoot.Normal:
                spawn.position += Vector3.forward * Time.deltaTime;
                break;
            case TypeShoot.Fast:
                break;
        }
    }

}
