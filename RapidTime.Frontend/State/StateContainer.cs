using System;

namespace RapidTime.Frontend.State
{
    public class StateContainer
    {
        private string UserId = "";

        public string Id
        {
            get => UserId;
            set
            {
                UserId = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}