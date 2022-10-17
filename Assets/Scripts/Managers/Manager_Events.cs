using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Observer;
using Event = Observer.Event;

public static class Manager_Events
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Setup()
    {
        foreach (var item in EventsExtensions.Fields)
        {
            var obj = Activator.CreateInstance(item.Value.FieldType);
            item.Value.SetValue(null, obj);
            ((IEvent) obj).SetEventName(item.Key);
        }
    }

    public static class Player
    {
        public static Event<Vector2> OnMovement;
        public static Event OnButtonA;
        public static Event OnButtonB;
        public static Event OnButtonPause;
        public static Event OnButtonSelect;
    }

    public static class GameManager
    {
        public static Event Pause;
        public static Event Unpause;
    }

    public static bool TryGetEvent(EventInspector eventInspector, out IEvent ev)
    {
        ev = null;

        if (!EventsExtensions.TryGetField(eventInspector, out FieldInfo field))
            return false;

        ev = (IEvent) field.GetValue(null);

        return ev != null;
    }


    public static class EventsExtensions
    {
        private static readonly Type MainType = typeof(Manager_Events);
        
        private static readonly Dictionary<string, FieldInfo> m_fields = new();

        public static Dictionary<string, FieldInfo> Fields
        {
            get
            {
                if (m_fields == null || m_fields.Count <= 0)
                    Setup();

                return m_fields;
            }
        }

        private static void Setup()
        {
            GetFieldsRecursively(MainType);

            // foreach (var item in m_fields.Keys)
            //     Debug.Log($"Path: {item}");
        }

        private static void GetFieldsRecursively(Type type)
        {
            foreach (var item in type.GetFields())
                m_fields.Add(GetBasePath(type, item), item);

            foreach (var item in type.GetNestedTypes())
                if (IsValidEventType(item))
                    GetFieldsRecursively(item);
        }

        private static bool IsValidEventType(Type type)
        {
            if (typeof(IEvent).IsAssignableFrom(type)) return false;
            if (type == typeof(EventsExtensions)) return false;
            return true;
        }

        private static string GetBasePath(Type type, FieldInfo field)
        {
            string result = $"{type}/{field.Name}";

            result = result.Remove(0, typeof(Manager_Events).ToString().Length + 1);

            result = result.Replace("+", "/");

            return result;
        }

        public static bool TryGetField(EventInspector eventInspector, out FieldInfo field)
        {
            field = null;

            if (!m_fields.ContainsKey(eventInspector.Event))
                return false;

            field = m_fields[eventInspector.Event];

            return true;
        }

    }

}
