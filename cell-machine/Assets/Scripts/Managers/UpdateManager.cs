using System;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static event Action onUpdate;
    public static event Action onFixedUpdate;

    private void Update() => onUpdate?.Invoke();
    private void FixedUpdate() => onFixedUpdate?.Invoke();

}
