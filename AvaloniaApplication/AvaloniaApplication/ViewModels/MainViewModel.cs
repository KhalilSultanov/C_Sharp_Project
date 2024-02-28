using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AvaloniaApplication.Data;
using AvaloniaApplication.Views;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;



namespace AvaloniaApplication.ViewModels;

public class MainViewModel : ViewModelBase, INotifyPropertyChanged
{
    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set { _searchQuery = value; }
    }
    

    private readonly HttpClient _httpClient;
    
    private string _jsonResponse;
    public string JsonResponse
    {
        get => _jsonResponse;
        set 
        { 
            _jsonResponse = value;
            OnPropertyChanged(nameof(JsonResponse));
        }
    }

    public MainViewModel()
    {
        _httpClient = new HttpClient();
    }

    public async Task MakeSearch()
    {
        try
        {
            string apiToken = "AW98YFQ-VK74B72-MP1KEGP-PH0QXJG";
            string apiUrl = "https://api.kinopoisk.dev/v1.4/movie/search";
            string fullUrl = $"{apiUrl}?token={apiToken}&query={Uri.EscapeDataString(SearchQuery)}";

            HttpResponseMessage response = await _httpClient.GetAsync(fullUrl);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            MovieResponse movieResponse = JsonConvert.DeserializeObject<MovieResponse>(responseBody);

            var stringBuilder = new System.Text.StringBuilder();
            foreach (var movie in movieResponse.Docs)
            {
                stringBuilder.AppendLine($"ID: {movie.id}\nName: {movie.name}\nDescription: {movie.description}\n");
            }

            JsonResponse = stringBuilder.ToString();

            using (var context = new AppDbContext())
            {
                foreach (var apiMovie in movieResponse.Docs)
                {
                    var movie = await context.movies.FirstOrDefaultAsync(m => m.apiId == apiMovie.id);
                    Console.WriteLine(movie?.ToString());
                    if (movie == null) {
                        context.movies.Add(new Movie
                        {
                            apiId = apiMovie.id,
                            name = apiMovie.name,
                            description = apiMovie.description
                        });
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        
        finally
        {
            Console.WriteLine("MakeSearch Completed");
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}