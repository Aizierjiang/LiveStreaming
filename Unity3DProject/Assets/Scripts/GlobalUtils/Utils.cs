using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System.IO.Compression;

/*by Aizierjiang*/

public class Utils
{

    public static byte[] ConvertColor32ArrayToByteArray(Color32[] colors)
    {
        if (colors == null || colors.Length == 0)
            return null;

        int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
        int length = lengthOfColor32 * colors.Length;
        byte[] bytes = new byte[length];

        GCHandle handle = default(GCHandle);
        try
        {
            handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(ptr, bytes, 0, length);
        }
        finally
        {
            if (handle != default(GCHandle))
                handle.Free();
        }

        return bytes;
    }

    public static Color32[] ConvertByteArrayToColor32Array(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
            return null;

        int length = bytes.Length;
        int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
        Color32[] colors = new Color32[length / lengthOfColor32];

        GCHandle handle = default(GCHandle);
        try
        {
            handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(bytes, 0, ptr, length);
        }
        finally
        {
            if (handle != default(GCHandle))
                handle.Free();
        }

        return colors;
    }



    private Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }

    private Texture2D RenderTextureToTexture2D(RenderTexture texture)
    {
        RenderTexture RT = RenderTexture.active;
        RenderTexture.active = texture;
        Texture2D texture2D = new Texture2D(texture.width, texture.height);
        texture2D.ReadPixels(new Rect(0, 0, texture2D.width, texture2D.height), 0, 0);
        return texture2D;
    }



    //压缩字节
    //1.创建压缩的数据流 
    //2.设定compressStream为存放被压缩的文件流,并设定为压缩模式
    //3.将需要压缩的字节写到被压缩的文件流
    public static byte[] CompressBytes(byte[] bytes)
    {
        using (MemoryStream compressStream = new MemoryStream())
        {
            using (var zipStream = new GZipStream(compressStream, CompressionMode.Compress))
                zipStream.Write(bytes, 0, bytes.Length);
            return compressStream.ToArray();
        }
    }

    //解压缩字节
    //1.创建被压缩的数据流
    //2.创建zipStream对象，并传入解压的文件流
    //3.创建目标流
    //4.zipStream拷贝到目标流
    //5.返回目标流输出字节
    public static byte[] Decompress(byte[] bytes)
    {
        using (var compressStream = new MemoryStream(bytes))
        {
            using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
            {
                using (var resultStream = new MemoryStream())
                {
                    zipStream.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
        }
    }

    public static Texture2D BytesToTexture2D(byte[] bytes, int w = 1920, int h = 1080)
    {
        Texture2D texture2D = new Texture2D(w, h);
        texture2D.LoadImage(bytes);
        return texture2D;
    }

    /// <summary>
    /// color 转换hex
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255.0f);
        int g = Mathf.RoundToInt(color.g * 255.0f);
        int b = Mathf.RoundToInt(color.b * 255.0f);
        int a = Mathf.RoundToInt(color.a * 255.0f);
        string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }

    /// <summary>
    /// hex转换到color
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color HexToColor(string hex)
    {
        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f;
        float g = bg / 255f;
        float b = bb / 255f;
        float a = cc / 255f;
        return new Color(r, g, b, a);
    }
}
