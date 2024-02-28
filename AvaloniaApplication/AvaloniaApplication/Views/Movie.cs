using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaloniaApplication.Views;

using Newtonsoft.Json;
using System.Collections.Generic;

public class Movie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int id { get; set; }

    [JsonProperty("name")]
    public string name { get; set; }

    [JsonProperty("description")]
    public string description { get; set; }
        
    [JsonProperty("apiId")]
    public int apiId { get; set; }
}

public class MovieResponse
{
    [JsonProperty("docs")]
    public List<Movie> Docs { get; set; }
}

