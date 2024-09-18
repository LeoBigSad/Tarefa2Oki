using System;
using System.Text.Json;
using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json.Serialization;
using MinhaBiblioteca;
public class ExemploSerialização
{
    public static string SerializeToJson()
    {
        Person person = new Person("Leonardo", 48, "Piracicaba");
        string jsonString = JsonSerializer.Serialize(person);
        Console.WriteLine("JSON serializado: " + jsonString);
        return jsonString;
    }
}

public class ExemploDeserialização
{
    public static Person DeserializeFromJson(string jsonString)
    {
        Person person = JsonSerializer.Deserialize<Person>(jsonString);
        Console.WriteLine($"Person deserializado: Nome={person.Nome}, Idade={person.Idade}, Cidade={person.Cidade}");
        return person;
    }
}

public class PersonDTO
{
    public string NomeCompleto { get; set; }
    public string IdadeEmAnos { get; set; }
}

public class ExemploMapper
{
    public static void MapToDto(Person person)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Person, PersonDTO>()
                .ForMember(dest => dest.NomeCompleto, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.IdadeEmAnos, opt => opt.MapFrom(src => src.Idade.ToString()));
        });

        var mapper = config.CreateMapper();
        PersonDTO dto = mapper.Map<PersonDTO>(person);
        Console.WriteLine($"DTO mappeado: Nome Completo={dto.NomeCompleto}, Idade em Anos={dto.IdadeEmAnos}");
    }
}

public class ApiResponse
{
    [JsonPropertyName("logradouro")]
    public string Logradouro { get; set; }

    [JsonPropertyName("bairro")]
    public string Bairro { get; set; }

    [JsonPropertyName("localidade")]
    public string Localidade { get; set; }
}

public class ExemploRestClient
{
    public static async Task CallRestApi()
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = "https://viacep.com.br/ws/13024050/json/";
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonString);

                if (apiResponse != null)
                {
                    Console.WriteLine($"Resposta da API : Logradouro={apiResponse.Logradouro}, Bairro={apiResponse.Bairro}, Localidade={apiResponse.Localidade}");
                }
            }
            else
            {
                Console.WriteLine("Falha em achar dados da API.");
            }
        }
    }
}

public class ExemploRestSharp
{
    public static async Task CallRestApiWithRestClient()
    {
        var client = new RestClient("https://viacep.com.br/ws/13024050/json/");
        var request = new RestRequest();
        var response = await client.ExecuteAsync<ApiResponse>(request);

        if (response.IsSuccessful)
        {
            ApiResponse apiResponse = response.Data;
            Console.WriteLine($"Resposta do RestClient: Logradouro={apiResponse.Logradouro}, Bairro={apiResponse.Bairro}, Localidade={apiResponse.Localidade}");
        }
        else
        {
            Console.WriteLine("Falha ao pegar dados RestClient.");
        }
    }
}

public class ConversãoData
{
    public static void ConvertDate()
    {
        DateTime now = DateTime.Now;
        string formattedDate = $"{now.Day}@{now.Year}@{now.Month}#{now.Hour}#{now.Minute}";
        Console.WriteLine("Data Formatada: " + formattedDate);
    }
}

public class CalcularData
{
    public static void CalculateDateDifference()
    {
        DateTime now = DateTime.Now;
        DateTime futureDate = now.AddDays(45);
        double differenceInSeconds = (futureDate - now).TotalSeconds;
        Console.WriteLine($"Diferença em Segundos: {differenceInSeconds}");
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        // Serializar para JSON
        string jsonString = ExemploSerialização.SerializeToJson();

        // Deserializar JSON para classe
        Person person = ExemploDeserialização.DeserializeFromJson(jsonString);

        // Mapear para DTO usando AutoMapper
        ExemploMapper.MapToDto(person);

        // Fazer chamada REST usando HttpClient
        await ExemploRestClient.CallRestApi();

        // Fazer chamada REST usando RestClient
        await ExemploRestSharp.CallRestApiWithRestClient();

        // Converter Data
        ConversãoData.ConvertDate();

        // Calcular diferença de datas
        CalcularData.CalculateDateDifference();
    }
}
