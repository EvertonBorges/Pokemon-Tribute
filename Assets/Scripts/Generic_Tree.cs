using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Generic_Tree<U> where U : ScriptableObject
{

    public string path;
    public Generic_Tree<U> parent = null;
    public List<Generic_Tree<U>> nodes = new();
    public Dictionary<U, SerializedObject> objs = new();
    public bool isExpanded;

    public Generic_Tree(string path)
    {
        this.path = path;
    }

    public void AddNode(KeyValuePair<string, U> item, bool toExpand = false)
    {
        string[] paths;

        if (item.Key.Contains("/"))
            paths = item.Key[..item.Key.LastIndexOf("/")].Split("/");
        else
            paths = new string[] { "" };

        var path = "";

        var actualNode = this;

        for (int i = 0; i < paths.Length; i++)
        {
            path += $"{(path.IsEmpty() ? "" : "/")}{paths[i]}";

            var node = actualNode.FindNode(path);

            if (node == null)
            {
                node = new Generic_Tree<U>(path);

                actualNode.nodes.Add(node);
            }

            if (node != actualNode)
                node.parent = actualNode;

            actualNode = node;

            if (i == paths.Length - 1)
                node.objs.Add(item.Value, new(item.Value));
        }

        if (!toExpand)
            return;

        while(actualNode.parent != null)
        {
            actualNode.isExpanded = true;

            actualNode = actualNode.parent;
        }
    }

    public void Clear()
    {
        nodes.Clear();

        objs.Clear();
    }

    public Generic_Tree<U> FindNode(string value)
    {
        if (path.Equals(value))
            return this;

        foreach (var node in nodes)
            if (node.FindNode(value) != null)
                return node;

        return null;
    }

    public Generic_Tree<U> FindObj(string value)
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
        var parentPath = "Null";

        if (parent != null)
            parentPath = parent.path;

        return $"{path} ({parentPath})";
    }

}
