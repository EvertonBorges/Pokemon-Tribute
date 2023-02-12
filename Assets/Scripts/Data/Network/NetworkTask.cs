using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class NetworkTask<T>
{

    public static async Task<T> Get(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        await request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
                throw new System.Exception($"HTTP Error ({request.result}): {request.error} - {url}");
            default:
                break;
        }

        return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
    }

}
