using System;

namespace PixelCrew.Utils.Disposable
{
    public class ActionDisposable :  IDisposable
    {

        private  Action _onDisponse;
        public ActionDisposable(Action onDisponse)
        {
            _onDisponse = onDisponse;
        }
        
        public void Dispose()
        {
            _onDisponse?.Invoke();
            _onDisponse = null;
        }
    }
}