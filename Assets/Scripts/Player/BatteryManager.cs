using UnityEngine;
using Utils;

namespace Player
{
  public class BatteryManager : ResetableMonoBehaviour, Utils.IDevSerializable
  {
    [Header("Game Manager")]
    [SerializeField] private PlayerLifeManager lifeManager;

    [Header("Energy Stats")]
    [Range(0, 100)] public float batteryPercentage = 100f;
    public float chargeRate = 5f;
    public float drainRate = 10f;

    [Header("Dev mod")]
    public bool debug = false;

    private bool _inCharge = false;

    public void Charge(float deltaTime, float chargeSpeed = 1) => batteryPercentage = Mathf.Min(100, batteryPercentage + chargeRate * chargeSpeed * deltaTime);
    public void Drain(float deltaTime) => batteryPercentage = Mathf.Max(0, batteryPercentage - drainRate * deltaTime);

    public bool GetInCharge() => _inCharge;
    public void SetInCharge(bool value) => _inCharge = value;

    private float _baseChargeRate;
    private float _baseDrainRate;

    public void Start()
    {
      _baseChargeRate = chargeRate;
      _baseDrainRate = drainRate;
      if (lifeManager == null)
      {
        Debug.LogError("No PlayerLifeManager assigned to BatteryManager");
      }
    }

    private void Update()
    {
      if (debug)
        Debug.Log("Battery Level : " + batteryPercentage + " Charge state : " + _inCharge);

      // Kill the player if battery is empty
      if (batteryPercentage <= 0)
      {
        lifeManager.Die(Death.Energy);
      }
    }

    public void refill(float percentage = 100f)
    {
      batteryPercentage = percentage;
    }

    public string DevSerialize()
    {
      return $"Bat Level : {batteryPercentage} | Charge state : {_inCharge}";
    }

    public override void ResetProperty(PlayerResetType resetType = PlayerResetType.Medium)
    {
      if (resetType != PlayerResetType.Light)
      {
        batteryPercentage = 100f;
        chargeRate = _baseChargeRate;
        drainRate = _baseDrainRate;
        _inCharge = false;
      }
    }
  }
}