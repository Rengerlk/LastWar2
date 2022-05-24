using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Utils.Disposable;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace PixelCrew.Model.Data
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        private PlayerData _save;
        private readonly CompositeDisposable _trash  = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }

        private void Awake()
        {
            LoadUIs();
            if (IsSessionExit())
            {
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
              DontDestroyOnLoad(this);  
            }
        }

        private void InitModels()
        {
           QuickInventory = new QuickInventoryModel(_data);
           _trash.Retain(QuickInventory);
        }

        private void LoadUIs()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
            LoadOnScreenControls();
        }
        [Conditional("USE_ONSCREEN_CONTROLS")]
        private void LoadOnScreenControls()
        {
            SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
        }
        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return true;
            }

            return false;
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
            _trash.Dispose();
            InitModels();
            
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}