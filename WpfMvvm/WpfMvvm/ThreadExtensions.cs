using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfMvvm
{
    public static class ThreadExtensions
    {
        public static void BeginInvokeInUiThread(Action action)
        {
            var dispatcher = GetUiDispatcher();
            if (dispatcher == null)
                return;

            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.BeginInvoke(action);
        }

        public static void BeginInvokeInUiThread<T>(Action<T> action, T param)
        {
            var dispatcher = GetUiDispatcher();
            if (dispatcher == null)
                return;

            if (dispatcher.CheckAccess())
                action(param);
            else
                dispatcher.BeginInvoke(action, param);
        }

        public static Dispatcher GetUiDispatcher()
        {
#if SILVERLIGHT
            return Deployment.Current.Dispatcher;
#else
            return Application.Current == null ? null : Application.Current.Dispatcher;
#endif
        }

        public static Task ContinueWithUi(this Task task, Action action)
        {
            if (action == null)
            {
                return task;
            }
            if (task == null)
            {
                BeginInvokeInUiThread(action);
                return null;
            }

            return task.ContinueWith(t =>
            {
                try
                {
                    BeginInvokeInUiThread(action);
                }
                catch (Exception)
                {

                }
            });
        }

        public static Task ContinueWithUi<T>(this Task<T> task, Action<T> action) where T : class
        {
            if (action == null)
            {
                return task;
            }
            if (task == null)
            {
                BeginInvokeInUiThread(action, null);
                return null;
            }

            return task.ContinueWith(t =>
            {
                try
                {
                    BeginInvokeInUiThread(action, t.IsCompleted ? t.Result : null);
                }
                catch (Exception)
                {

                }
            });
        }

    }
}
