using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Task_3_Steam_top_10
{
  internal static class Program
  {
    private static async Task Main()
    {
      try
      {
        var topSales = await GetTopSalesAsync();
        if (topSales != null)
        {
          Console.WriteLine("Топ-10 продаж Steam (Россия):");
          for (int i = 0; i < topSales.Count; i++)
          {
            JObject game = topSales[i];
            Console.WriteLine($"{i + 1}. {game["name"]} - {int.Parse(game["final_price"]!.ToString()) / 100} рублей");
          }
        }
        else
        {
          Console.WriteLine("Ошибка при получении данных");
        }
      }
      catch (HttpRequestException)
      {
        Console.WriteLine("Неудалось выполнить запрос к серверу");
      }
      catch (JsonReaderException)
      {
        Console.WriteLine("Ошибка при чтении данных");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Произошла ошибка: {ex.Message}");
      }
    }

    private static async Task<List<JObject>?> GetTopSalesAsync()
    {
      using HttpClient client = new();
      string response = await client.GetStringAsync("https://store.steampowered.com/api/featuredcategories?l=russian");
      var categories = JObject.Parse(response)["top_sellers"]?["items"]?.ToObject<List<JObject>>();
      return categories;
    }
  }
}