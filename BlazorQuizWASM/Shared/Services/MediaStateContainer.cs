using BlazorQuizWASM.Shared.DTO;

namespace BlazorQuizWASM.Shared.Services
{
    public class MediaStateContainer
    {
        public MediaFileResponseDto? Value { get; set; }
        public event Action? OnStateChange;
        public void SetValue(MediaFileResponseDto? value)
        {
            this.Value = value;
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
