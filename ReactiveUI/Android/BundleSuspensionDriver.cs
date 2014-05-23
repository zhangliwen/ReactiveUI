using System;
using System.Reactive;
using System.Reactive.Linq;
using Newtonsoft.Json;

namespace ReactiveUI.Mobile
{
    public class BundleSuspensionDriver : ISuspensionDriver
    {
        public IObservable<T> LoadState<T>() where T : class, IApplicationRootState
        {
            try {
                return Observable.Return(JsonConvert.DeserializeObject<T>(
                    AutoSuspendActivityHelper.LatestBundle.GetString("__state")));
            } catch (Exception ex) {
                return Observable.Throw<T>(ex);
            }
        }

        public IObservable<Unit> SaveState<T>(T state) where T : class, IApplicationRootState
        {
            try {
                AutoSuspendActivityHelper.LatestBundle.PutString("__state", JsonConvert.SerializeObject(state));
                return Observable.Return(Unit.Default);
            
            } catch(Exception ex) {
                return Observable.Throw<Unit>(ex);
            }
        }

        public IObservable<Unit> InvalidateState()
        {
            try {
                AutoSuspendActivityHelper.LatestBundle.PutString("__state", "");
                return Observable.Return(Unit.Default);
            
            } catch(Exception ex) {
                return Observable.Throw<Unit>(ex);
            }
        }
    }
}