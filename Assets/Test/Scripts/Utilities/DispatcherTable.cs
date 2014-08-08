using System;
using System.Collections.Generic;

public class DispatcherTable<T> {

    private Dictionary<T, Delegate> dispatcher;

    public DispatcherTable() {
        dispatcher = new Dictionary<T, Delegate>();
    }

    public DispatcherTable(int size) {
        dispatcher = new Dictionary<T, Delegate>(size);
    }

    public void AddAction(T key, Delegate action) {
        dispatcher[key] = action;
    }

    public void Dispatch(T key) {
        if (dispatcher.ContainsKey(key))
            dispatcher[key].DynamicInvoke();
        else
            ;//LogError("dispatcher table action does not exists for key: " + key);
    }

    public void Dispatch<K>(T key, K arg) {
        if (dispatcher.ContainsKey(key))
            dispatcher[key].DynamicInvoke(arg);
        else
            ;//LogError("dispatcher table action does not exists for key: " + key);
    }
}