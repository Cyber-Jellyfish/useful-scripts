using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    #region UNITY METHODS

    public virtual void Awake()
    {
        DoOnAwake();
    }

    public virtual void OnEnable()
    {
        DoOnEnable();
    }

    public virtual void OnDisable()
    {
        DoOnDisable();
    }

    public virtual void OnDestroy()
    {
        DoOnDestroy();
    }

    #endregion

    #region STATE METHODS

    public virtual void Activate()
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public virtual void DestroyInstance()
    {
        Destroy(gameObject);
    }

    #endregion

    #region METHODS

    public virtual void DoOnAwake()
    {
    }

    public virtual void DoOnEnable()
    {
    }

    public virtual void DoOnDisable()
    {
    }

    public virtual void DoOnDestroy()
    {
    }

    #endregion
}