using System.Collections.Generic;
using System;


namespace Utils
{
    /// <summary>
    /// �¼���
    /// </summary>
    public interface ISignal
    {
        string Hash { get; }
    }

    public static class Signals
    {
        private static readonly SignalHub hub = new SignalHub();

        public static SType Get<SType>() where SType : ISignal, new()
        {
            return hub.Get<SType>();
        }
    }

    /// <summary>
    /// �¼�����
    /// </summary>
    public class SignalHub
    {
        private Dictionary<Type, ISignal> signals = new Dictionary<Type, ISignal>();

        /// <summary>
        /// �������ͻ�ȡ�¼�
        /// </summary>
        public SType Get<SType>() where SType : ISignal, new()
        {
            Type signalType = typeof(SType);
            ISignal signal;

            if (signals.TryGetValue(signalType, out signal))
            {
                return (SType)signal;
            }

            return (SType)Bind(signalType);
        }

        /// <summary>
        /// �ֶ��ṩһ���¼��Ĺ�ϣ��������󶨵������ļ�����
        /// /// </summary> 
        public void AddListenerToHash(string signalHash, Action handler)
        {
            ISignal signal = GetSignalByHash(signalHash);
            if (signal != null && signal is ASignal)
            {
                (signal as ASignal).AddListener(handler);
            }
        }

        public void RemoveListenerFromHash(string signalHash, Action handler)
        {
            ISignal signal = GetSignalByHash(signalHash);
            if (signal != null && signal is ASignal)
            {
                (signal as ASignal).RemoveListener(handler);
            }
        }

        private ISignal Bind(Type signalType)
        {
            ISignal signal;
            if (signals.TryGetValue(signalType, out signal))
            {
                UnityEngine.Debug.LogError(string.Format("Signal already registered for type {0}",
                    signalType.ToString()));
                return signal;
            }

            signal = (ISignal)Activator.CreateInstance(signalType);
            signals.Add(signalType, signal);
            return signal;
        }

        private ISignal Bind<T>() where T : ISignal, new()
        {
            return Bind(typeof(T));
        }

        private ISignal GetSignalByHash(string signalHash)
        {
            foreach (ISignal signal in signals.Values)
            {
                if (signal.Hash == signalHash)
                {
                    return signal;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// �ź��¼��ĳ�����
    /// </summary>
    public abstract class ABaseSignal : ISignal
    {
        protected string _hash;

        /// <summary>
        /// ����Ĺ�ϣֵ
        /// </summary>
        public string Hash
        {
            get
            {
                if (string.IsNullOrEmpty(_hash))
                {
                    _hash = this.GetType().ToString();
                }

                return _hash;
            }
        }
    }

    /// <summary>
    /// �¼��ľ���ʵ��
    /// </summary>
    public abstract class ASignal : ABaseSignal
    {
        private Action callback;

        /// <summary>
        /// �¼��Ӽ���
        /// </summary>
        public void AddListener(Action handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(
                handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// �¼����Ƴ�����
        /// </summary>
        public void RemoveListener(Action handler)
        {
            callback -= handler;
        }

        /// <summary>
        /// �㲥�¼�
        /// </summary>
        public void Dispatch()
        {
            if (callback != null)
            {
                callback();
            }
        }
    }

    /// <summary>
    /// ��һ���������¼�
    /// </summary>
    public abstract class ASignal<T> : ABaseSignal
    {
        private Action<T> callback;

        /// <summary>
        /// �¼��Ӽ���
        /// </summary>
        public void AddListener(Action<T> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(
                handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// �Ƴ�����
        /// </summary>
        public void RemoveListener(Action<T> handler)
        {
            callback -= handler;
        }

        /// <summary>
        /// �㲥�¼�����һ������
        /// </summary>
        public void Dispatch(T arg1)
        {
            if (callback != null)
            {
                callback(arg1);
            }
        }
    }

    /// <summary>
    /// ͬ��2���������¼�
    /// </summary>
    public abstract class ASignal<T, U> : ABaseSignal
    {
        private Action<T, U> callback;

        public void AddListener(Action<T, U> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(
                handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        public void RemoveListener(Action<T, U> handler)
        {
            callback -= handler;
        }

        public void Dispatch(T arg1, U arg2)
        {
            if (callback != null)
            {
                callback(arg1, arg2);
            }
        }
    }

    /// <summary>
    /// ͬ�������������¼�
    /// </summary>
    public abstract class ASignal<T, U, V> : ABaseSignal
    {
        private Action<T, U, V> callback;

        public void AddListener(Action<T, U, V> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(
                handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        public void RemoveListener(Action<T, U, V> handler)
        {
            callback -= handler;
        }

        public void Dispatch(T arg1, U arg2, V arg3)
        {
            if (callback != null)
            {
                callback(arg1, arg2, arg3);
            }
        }
    }
}
