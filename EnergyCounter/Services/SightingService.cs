using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace EnergyCounter.Services;

public class SightingService
{
    private List<EnergySighting> energySightings;
    private readonly string filePath;

    public SightingService(string filePath)
    {
        this.filePath = filePath;
        energySightings = [.. LoadFromFile(filePath)];
    }

    public int SightingCount => energySightings.Count;

    private JsonSerializerOptions options = new() { WriteIndented=true };

    private void SaveSightings()
    {
        string dir = Path.GetDirectoryName(filePath);
        Directory.CreateDirectory(dir);

        string json = JsonSerializer.Serialize(energySightings, options);
        File.WriteAllText(filePath, json);
    }

    private EnergySighting[] LoadFromFile(string filePath)
    {
        if (File.Exists(filePath) == false)
        {
            return [];
        }
        return JsonSerializer.Deserialize<EnergySighting[]>(File.ReadAllText(filePath)) ?? [];
    }

    public EnergySighting[] GetEnergySightings()
    {
        return [..energySightings.OrderByDescending(e => e.SpottedOn)];
    }

    public void AddSighting(EnergySighting sighting)
    {
        energySightings.Add(sighting);
        SaveSightings();
    }
}

public record EnergySighting(DateTime SpottedOn, string spotterName, int ml);