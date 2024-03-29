using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField] protected Slider _uiHealth;
    protected float _health;
    protected float _armor;
    private Camera _camera;
    private Quaternion _previousRotationCamera;

    private string _sfxDead;
    [HideInInspector] public bool HasDied;
    protected abstract float GetHealthData();

    protected abstract float GetArmorData();

    protected abstract string GetSfxDeadName();
    protected abstract void AddPool(GameObject gameObject);
   
    protected void Start()
    {
        _sfxDead = GetSfxDeadName();
        _health = GetHealthData();
        _armor = GetArmorData();
        Initialize();
    }

    private void Initialize()
    {
        _uiHealth.maxValue = _health;
        _uiHealth.value = _health;
        _camera = Camera.main;
        _previousRotationCamera = _camera.transform.rotation;
        StartCoroutine(UpdatetheRotationOfUiHealth());
        HasDied = false;
    }

    private void Update()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
        if (_camera.transform.rotation != _previousRotationCamera)
        {
            if (!_uiHealth.gameObject.activeInHierarchy) return;
            _uiHealth.transform.LookAt(_camera.transform);
            _uiHealth.transform.Rotate(0, 180, 0);
            _previousRotationCamera = _camera.transform.rotation;
        }
    }
    public abstract void Death();
    private IEnumerator OnDeath()
    {
        SoundManager.Instance.PlaySound(_sfxDead);
        _uiHealth.gameObject.SetActive(false);
        GetComponent<Animator>().SetBool("Died",true);
        Death();
        yield return new WaitForSeconds(2);
        _uiHealth.gameObject.SetActive(true);
        AddPool(gameObject);
    }
    IEnumerator UpdatetheRotationOfUiHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            _uiHealth.transform.LookAt(_camera.transform);
            _uiHealth.transform.Rotate(0, 180, 0);
            _previousRotationCamera = _camera.transform.rotation;
        }
    }
    public void TakeDamage(float damage)
    {
        _uiHealth.gameObject.SetActive(true);
        float damageMultiplier = _armor != 0 ? (100 - _armor) / 100 : 1;
        _health -= damage * damageMultiplier;
        _uiHealth.value = _health;
        if (_health <= 0)
        {
            HasDied = true;
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
            StartCoroutine(OnDeath());
        }
    }
    protected void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        GetComponent<Animator>().SetBool("Died", false);
        _health = GetHealthData();
        _uiHealth.maxValue = _health;
        _uiHealth.value = _health;
        _camera = Camera.main;
        _previousRotationCamera = _camera.transform.rotation;
        StartCoroutine(UpdatetheRotationOfUiHealth());
        HasDied = false;
    }

    protected void OnDisable()
    {
        StopCoroutine(UpdatetheRotationOfUiHealth());
    }
}
