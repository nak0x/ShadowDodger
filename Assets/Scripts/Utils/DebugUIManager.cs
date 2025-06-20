using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace Utils
{
    
    public class DebugUIManager : MonoBehaviour
    {
        [SerializeField] private DebugRuntimeRegistry runtimeRegistry;
        [SerializeField] private Transform contentContainer;
        [SerializeField] private GameObject debugTextPrefab;

        private readonly List<(IDevSerializable devObject, TextMeshProUGUI uiText)> registered = new();

        private void Start()
        {
            foreach (var target in runtimeRegistry.debugTargets)
            {
                if (target == null) continue;

                if (target is IDevSerializable devSerializable)
                {
                    GameObject textGO = Instantiate(debugTextPrefab, contentContainer);
                    TextMeshProUGUI textComp = textGO.GetComponent<TextMeshProUGUI>();

                    if (textComp == null)
                    {
                        Debug.LogWarning("Prefab is missing a Text component.");
                        continue;
                    }

                    registered.Add((devSerializable, textComp));
                }
                else
                {
                    Debug.LogWarning($"Target {target.name} does not implement IDevSerializable.");
                }
            }
        }

        private void Update()
        {
            foreach (var (devObject, uiText) in registered)
            {
                if (devObject == null || uiText == null) continue;

                uiText.text = devObject.DevSerialize();
            }
        }
    }
}