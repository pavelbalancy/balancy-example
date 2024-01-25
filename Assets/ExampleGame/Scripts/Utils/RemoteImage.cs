using Balancy;
using UnityEngine;
using UnityEngine.UI;

public class RemoteImage : Image
{
    public void LoadFromUrl(string url, Vector4 border, int pixelsPerUnit)
    {
        var cachedColor = MakeImageTransparent();
        Balancy.ObjectsLoader.GetSprite(url, border, pixelsPerUnit, loadedSprite =>
        {
            if (!IsDestroyed())
            {
                sprite = loadedSprite;
                color = cachedColor;
            }
        });
    }

    public void LoadObject(UnnyObject unnyObject)
    {
        var cachedColor = MakeImageTransparent();
        unnyObject?.LoadSprite(loadedSprite =>
        {
            if (!IsDestroyed())
            {
                sprite = loadedSprite;
                color = cachedColor;
            }
        });
    }

    private Color MakeImageTransparent()
    {
        var cachedColor = color;
        var tempColor = color;
        tempColor.a = 0;
        color = tempColor;

        return cachedColor;
    }
}