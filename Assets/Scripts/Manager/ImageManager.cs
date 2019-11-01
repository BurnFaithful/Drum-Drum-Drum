using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoSingleton<ImageManager>
{
    private Dictionary<string, string> imageData;

    public Dictionary<string, string> ImageData { get { return imageData; } set { imageData = value; } }

    protected override void Initialize()
    {
        imageData = new Dictionary<string, string>();
    }

    private void Awake()
    {
        this.Initialize();
    }
}
