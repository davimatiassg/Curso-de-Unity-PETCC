using UnityEngine;
public interface I_HitableObj
{
    /// Interface para objetos atacáveis

    void TakeHit(int damage, Vector2 hitPos);
}
