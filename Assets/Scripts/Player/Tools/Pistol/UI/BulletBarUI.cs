using UnityEngine;
using UnityEngine.UI;

public class BulletBarUI : MonoBehaviour
{
    [SerializeField] private Pistol _pistol;
    [SerializeField] private PistolReloadState _pistolReloadState;

    [SerializeField] private Sprite _fullBulletSprite;
    [SerializeField] private Sprite _hollowBulletSprite;
    [SerializeField] private Slider _delayBar; 
    [SerializeField] private Slider _reloadBar; 
    [SerializeField] private GameObject _delayBarUI; 
    [SerializeField] private GameObject _reloadBarUI; 
    [SerializeField] private Image _spinningReloadIcon; 
    [SerializeField] private GameObject _ammoSlotHolder;

    private Image[] _bulletIcons;
    
    private float _maxBarProgress;
    private float _shotDelay;
    private float _delayProgress;

    private float _maxReloadBarProgress;
    private float _reloadSpeed;
    private float _reloadProgress;

    private bool _isDelayActive;
    private bool _isReloading;

    public void Awake()
    {
        _reloadBarUI.SetActive(false);
        _delayBarUI.SetActive(true);
        _bulletIcons = _ammoSlotHolder.GetComponentsInChildren<Image>();
    }
    public void Start()
    {
        _shotDelay = _pistol.AttackDelay;
        _reloadSpeed = _pistol.ReloadSpeed;
        _reloadProgress = 0f;
        _isDelayActive = false;
        _isReloading = false;
        _maxBarProgress = _delayBar.maxValue;
        _maxBarProgress = _reloadBar.maxValue;
        ResetDelayBar();
    }

    private void OnEnable()
    {
        // Subscribe to the OnPlayerDamaged event
        _pistol.OnAmmoChanged += UpdateUI;
        _pistolReloadState.OnReload += StartReload;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event to prevent memory leaks
        _pistol.OnAmmoChanged -= UpdateUI;
        _pistolReloadState.OnReload -= StartReload;
    }

    void Update()
    {
        // Reduce current delay timer if it's active
        if (_isDelayActive)
        {
            if (_delayProgress >= 0)
            {
                _delayProgress -= Time.deltaTime;
                UpdateDelayBar();
            }
            else
            {
                _isDelayActive = false;
                ResetDelayBar();
            }
        }
        else if (_isReloading)
        {
            _reloadProgress += Time.deltaTime;
            UpdateReloadBar();

            _spinningReloadIcon.transform.Rotate(Vector3.forward, -200 * Time.deltaTime);

            if (_reloadProgress >= _pistol.ReloadSpeed)
            {
                _spinningReloadIcon.transform.rotation = Quaternion.identity;
                FinishReload();
            }
        }
    }

    void UpdateDelayBar()
    {
        // get a representation of the sliders progress because it 
        // has a max of 100f and delay is 3f
        _delayBar.value = (_delayProgress / _shotDelay) * _delayBar.maxValue;
    }

    void TriggerShotDelay()
    {
        _isDelayActive = true;
        _delayProgress = _shotDelay;
    }

    void ResetDelayBar()
    {
        _delayBar.value = _maxBarProgress;
    }

    public void UpdateUI(int currentAmmo)
    {
        // This is checking if the pistol was just shot
        // don't do this if it was a reload or no ammo left
        // perhaps this should be an event listener instead
        if (currentAmmo >= 0 && currentAmmo < _pistol.MaxAmmo)
        {
            TriggerShotDelay();
        }

        for (int i = 0; i < _bulletIcons.Length; i++)
        {
            _bulletIcons[i].sprite = i < currentAmmo ? _fullBulletSprite : _hollowBulletSprite;
        }
    }

    public void FillAmmo()
    {
        for (int i = 0; i < _bulletIcons.Length; i++)
        {
            _bulletIcons[i].sprite = _fullBulletSprite;
        }
    }

    public void StartReload()
    {
        ResetDelayBar();
        FillAmmo();
        _isDelayActive = false;

        _isReloading = true;
        _reloadProgress = 0f;
        _delayBarUI.SetActive(false);
        _reloadBarUI.SetActive(true);
    }

    private void FinishReload()
    {
        _isReloading = false;
        _reloadBarUI.SetActive(false);
        _delayBarUI.SetActive(true);
    }

    private void UpdateReloadBar()
    {
        _reloadBar.value = (_reloadProgress / _reloadSpeed) * _reloadBar.maxValue;
    }
}
