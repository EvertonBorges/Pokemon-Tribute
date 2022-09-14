using System;
using System.Collections.Generic;

public static class Observer
{

    public interface IEvent
    {
        void Add(Action callback, int order = 0);
    }

    public class Event
    {
        private readonly SortedList<int, List<Action>> _callbackDic = new();

        public Event Add(Action callback, int order = 0)
        {
            if (_callbackDic.ContainsKey(order))
                _callbackDic[order].Add(callback);
            else
                _callbackDic.Add(order, new List<Action>() { callback });

            return this;
        }

        public Event Remove(Action callback, int order = 0)
        {
            if (_callbackDic.ContainsKey(order))
                _callbackDic[order].Remove(callback);

            return this;
        }

        public Event Notify()
        {
            foreach (var keyPair in _callbackDic)
                foreach (var callback in keyPair.Value)
                    callback?.Invoke();

            return this;
        }

        public static Event operator +(Event ev, Action callback) => ev.Add(callback);

        public static Event operator -(Event ev, Action callback) => ev.Remove(callback);
    }

    public class Event<T>
    {
        private readonly SortedList<int, List<Action<T>>> _callbackDic = new();

        public Event<T> Add(Action<T> callback, int order = 0)
        {
            if (_callbackDic.ContainsKey(order))
                _callbackDic[order].Add(callback);
            else
                _callbackDic.Add(order, new List<Action<T>>() { callback });

            return this;
        }

        public Event<T> Remove(Action<T> callback, int order = 0)
        {
            if (_callbackDic.ContainsKey(order))
                _callbackDic[order].Remove(callback);

            return this;
        }

        public Event<T> Notify(T obj)
        {
            foreach (var keyPair in _callbackDic)
                foreach (var callback in keyPair.Value)
                    callback?.Invoke(obj);

            return this;
        }

        public static Event<T> operator +(Event<T> ev, Action<T> callback) => ev.Add(callback);

        public static Event<T> operator -(Event<T> ev, Action<T> callback) => ev.Remove(callback);
    }

    public class Event<T1, T2>
    {
        private readonly SortedList<int, List<Action<T1, T2>>> _callbackDic = new();

        public Event<T1, T2> Add(Action<T1, T2> callback, int order = 0)
        {
            if (_callbackDic.ContainsKey(order))
                _callbackDic[order].Add(callback);
            else
                _callbackDic.Add(order, new List<Action<T1, T2>>() { callback });

            return this;
        }

        public Event<T1, T2> Remove(Action<T1, T2> callback, int order = 0)
        {
            if (_callbackDic.ContainsKey(order))
                _callbackDic[order].Remove(callback);

            return this;
        }

        public Event<T1, T2> Notify(T1 obj1, T2 obj2)
        {
            foreach (var keyPair in _callbackDic)
                foreach (var callback in keyPair.Value)
                    callback?.Invoke(obj1, obj2);

            return this;
        }

        public static Event<T1, T2> operator +(Event<T1, T2> ev, Action<T1, T2> callback) => ev.Add(callback);

        public static Event<T1, T2> operator -(Event<T1, T2> ev, Action<T1, T2> callback) => ev.Remove(callback);
    }

    public class Event<T1, T2, T3>
    {
        private readonly SortedList<int, List<Action<T1, T2, T3>>> _callbackDic = new();

        public Event<T1, T2, T3> Add(Action<T1, T2, T3> callback, int order = 0)
        {
            if (_callbackDic.ContainsKey(order))
                _callbackDic[order].Add(callback);
            else
                _callbackDic.Add(order, new List<Action<T1, T2, T3>>() { callback });

            return this;
        }

        public Event<T1, T2, T3> Remove(Action<T1, T2, T3> callback, int order = 0)
        {
            if (_callbackDic.ContainsKey(order))
                _callbackDic[order].Remove(callback);

            return this;
        }

        public Event<T1, T2, T3> Notify(T1 obj1, T2 obj2, T3 obj3)
        {
            foreach (var keyPair in _callbackDic)
                foreach (var callback in keyPair.Value)
                    callback?.Invoke(obj1, obj2, obj3);

            return this;
        }

        public static Event<T1, T2, T3> operator +(Event<T1, T2, T3> ev, Action<T1, T2, T3> callback) => ev.Add(callback);

        public static Event<T1, T2, T3> operator -(Event<T1, T2, T3> ev, Action<T1, T2, T3> callback) => ev.Remove(callback);
    }

}
