using UnityEngine;

public static class BombSystem
{
    public static void ClearAllEnemies()
    {
        foreach (var e in Object.FindObjectsOfType<Enemy>())
            Object.Destroy(e.gameObject);
    }
}
