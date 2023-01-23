using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

[CreateAssetMenu(fileName = "New Stream", menuName = "custom/Stream", order = 1)]
public class Stream : ScriptableObject
{
    private List<Action<Object>> outputs = new List<Action<object>>();

    public void AddOutputStream<T>(Action<T> output) => outputs.Add(obj =>
    {
        if (obj.GetType().IsSubclassOf(typeof(T)) || obj.GetType() == typeof(T))
            output.Invoke((T)obj);
        else
        {
            try
            {
                output.Invoke((T)obj);
            }
            catch (InvalidCastException e)
            {
                Debug.LogWarning("Tried to do an invalid cast in a stream. Cancelled the invocation! Exception: " +
                                 e.Message);
            }
        }
    });

    public void StreamData(object data) => outputs.ForEach(x => x.Invoke(data));
}