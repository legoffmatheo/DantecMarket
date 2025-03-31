using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.ObjectModel;
using System.Text;

namespace projetBase.Services
{
    public class Apis
    {
        public readonly HttpClient _httpClient = new HttpClient();
        public async Task<bool> LoginAsync(string email, string password)
        {
            await Task.Delay(1000);

            return email == "admin@example.com" && password == "password";
        }
        public async Task<ObservableCollection<T>> GetAllAsync<T>(string url)
        {
            try
            {
                var json = await _httpClient.GetStringAsync(Constantes.BaseApiAddress + url);
                var result = JsonConvert.DeserializeObject<List<T>>(json, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                return new ObservableCollection<T>(result);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed.
                throw;
            }
        }
        public async Task<T> GetOneAsync<T>(string endpoint, T requestDataObj)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(requestDataObj);
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(Constantes.BaseApiAddress + endpoint, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle or log the error based on the response status
                    return default(T);
                }

                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                return result;
            }
            catch (Exception ex)
            {

                return default(T);
            }
        }
        public async Task<T> PostAsync<T>(string endpoint, object requestDataObj)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(requestDataObj);
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                Console.WriteLine($"[POST] {Constantes.BaseApiAddress + endpoint}");
                Console.WriteLine($"Données envoyées: {jsonString}");

                var response = await _httpClient.PostAsync(Constantes.BaseApiAddress + endpoint, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[ERREUR] Code HTTP: {response.StatusCode}");
                    return default;
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Réponse reçue: {json}");

                return JsonConvert.DeserializeObject<T>(json, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur PostAsync: {ex.Message}");
                return default;
            }
        }
    
}
}
