using System.Collections.Concurrent;
using Microsoft.Windows.ApplicationModel.Resources;
using ByteSize = ByteSizeLib.ByteSize;

namespace PlatyPdfs.App.Extensions;

public static class StringExtensions
{

    private static readonly ResourceMap resourcesTree = new ResourceManager().MainResourceMap.TryGetSubtree("Resources");
    private static readonly ConcurrentDictionary<string, string> cachedResources = new();

    private static readonly Dictionary<string, string> abbreviations = new()
        {
            { "KiB", "KiloByteSymbol".GetLocalizedResource() },
            { "MiB", "MegaByteSymbol".GetLocalizedResource() },
            { "GiB", "GigaByteSymbol".GetLocalizedResource() },
            { "TiB", "TeraByteSymbol".GetLocalizedResource() },
            { "PiB", "PetaByteSymbol".GetLocalizedResource() },
            { "B", "ByteSymbol".GetLocalizedResource() },
            { "b", "ByteSymbol".GetLocalizedResource() }
        };

    public static string ConvertSizeAbbreviation(this string value)
    {
        foreach (var item in abbreviations)
        {
            value = value.Replace(item.Key, item.Value, StringComparison.Ordinal);
        }

        return value;
    }

    public static string ToSizeString(this double size) => ByteSize.FromBytes(size).ToSizeString();
    public static string ToSizeString(this long size) => ByteSize.FromBytes(size).ToSizeString();
    public static string ToSizeString(this ulong size) => ByteSize.FromBytes(size).ToSizeString();
    public static string ToSizeString(this decimal size) => ByteSize.FromBytes((double)size).ToSizeString();
    public static string ToSizeString(this ByteSize size) => size.ToBinaryString().ConvertSizeAbbreviation();

    public static string GetLocalizedResource(this string resourceKey)
    {
        if (cachedResources.TryGetValue(resourceKey, out var value))
        {
            return value;
        }

        value = resourcesTree?.TryGetValue(resourceKey)?.ValueAsString;

        return cachedResources[resourceKey] = value ?? string.Empty;
    }
}
