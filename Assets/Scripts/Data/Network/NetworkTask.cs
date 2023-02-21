using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
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

    public static async Task<Texture2D> GetTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        await request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError($"HTTP Error ({request.result}): {request.error} - {url}");

                return await GetTexture(url);
            default:
                break;
        }

        return ((DownloadHandlerTexture) request.downloadHandler).texture;
    }

}
