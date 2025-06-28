using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Utils
{
    [System.Serializable]
    public sealed class GameTimer
    {
        [SerializeField, MinValue(0f)] private float _targetTime;
        [SerializeField] private bool _loop;

        private CancellationTokenSource _cts;
        public UnityEvent OnElapsed;

        public GameTimer(float targetTime, bool loop)
        {
            _targetTime = targetTime;
            _loop = loop;
        }

        public void StartElapsing()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            ElapseAsync(_cts.Token);
        }

        public void StopElapsing()
        {
            _cts?.Cancel();
        }

        private async void ElapseAsync(CancellationToken token)
        {
            if (await UniTask.WaitForSeconds(_targetTime, cancellationToken: token)
                .SuppressCancellationThrow()) return;
            
            OnElapsed?.Invoke();
            if (_loop) StartElapsing();
        }
    }
}
