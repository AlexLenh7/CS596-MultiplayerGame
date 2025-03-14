using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
    {
        private VisualElement rootVisualElement;
        private Label statusLabel;

        void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            rootVisualElement = uiDocument.rootVisualElement;

            statusLabel = CreateLabel("StatusLabel", "Not Connected");

            rootVisualElement.Clear();
            rootVisualElement.Add(statusLabel);
        }

        void Update()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            if (NetworkManager.Singleton == null)
            {
                SetStatusText("NetworkManager not found");
                return;
            }

            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                SetStatusText("Not connected");
            }
            else
            {
                UpdateStatusLabels();
            }
        }

        void SetStatusText(string text) => statusLabel.text = text;

        void UpdateStatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
            string transport = "Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name;
            string modeText = "Mode: " + mode;
            SetStatusText($"{transport}\n{modeText}");
        }

        private Label CreateLabel(string name, string content)
        {
            var label = new Label();
            label.name = name;
            label.text = content;
            label.style.color = Color.black;
            label.style.fontSize = 18;
            return label;
        }
    }
}
