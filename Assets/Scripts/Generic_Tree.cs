using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Generic_Tree<T, U> where U : ScriptableObject
{

    public List<Generic_Tree<T, U>> nodes = new();
    public T path;
    public bool isExpanded;
    public Dictionary<U, SerializedObject> objs = new();

    public bool HasNode => !nodes.IsEmpty();

    public Generic_Tree(T path)
    {
        this.path = path;
    }

    public void AddObj(U obj)
    {
        objs.Add(obj, new(obj));
    }

    public void AddNode(Generic_Tree<T, U> child)
    {
        nodes.Add(child);
    }

    public Generic_Tree<T, U> FindNode(T value)
    {
        if (path.Equals(value))
            return this;

        foreach (var node in nodes)
            if (node.FindNode(value) != null)
                return node;

        return null;
    }

    public Generic_Tree<T, U> FindObj(string value)
    {
        foreach (var item in objs)
            if (item.Key.name.Equals(value))
                return this;

        foreach (var node in nodes)
            if (node.FindObj(value) != null)
                return node;

        return null;
    }

    public void PrintTree()
    {
        Debug.Log($"Node: {this}");

        foreach (var obj in objs)
            Debug.Log($"Node: {this} -> {obj.Key.name}");

        foreach (var node in nodes)
            node.PrintTree();
    }

    public override string ToString()
    {
        return path.ToString();
    }

}
