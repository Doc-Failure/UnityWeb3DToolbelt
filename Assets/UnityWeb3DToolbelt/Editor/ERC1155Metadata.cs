using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// using Unity.EditorCoroutines.Editor;

public class ERC1155Metadata
{
    public string name;
    public string description;
    public Texture2D image;
    public string attributes;


    public ERC1155Metadata()
    { this.attributes = "{ }"; }

}