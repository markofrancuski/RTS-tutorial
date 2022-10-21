using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public void Show()
    {
        if (_meshRenderer.enabled)
        {
            return;
        }
        _meshRenderer.enabled = true;
    }

    public void Hide()
    {
        if (!_meshRenderer.enabled)
        {
            return;
        }
        _meshRenderer.enabled = false;
    }
}
