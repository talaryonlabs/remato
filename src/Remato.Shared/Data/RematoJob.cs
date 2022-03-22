using System;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoJob
    {
        [JsonProperty("id")] public string JobId { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("date")] public DateTime Date { get; set; }
        [JsonProperty("type")] public RematoJobType Type { get; set; }
        [JsonProperty("is_done")] public bool IsDone { get; set; }
        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("changed_at")] public DateTime ChangedAt { get; set; }
    }

    public enum RematoJobType
    {
        SingleRun,
        Daily,
        Weekly,
        Monthly
    }
}