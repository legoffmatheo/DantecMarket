using Newtonsoft.Json;

public class ImageProduit
{
    #region Attributs
    private string url;
    #endregion

    #region Constructeurs
    public ImageProduit(string url)
    {
        this.url = url;
    }
    #endregion

    #region Getters/Setters
    [JsonProperty("Url")]
    public string Url { get => url; set => url = value; }
    public string FullUrl => url.StartsWith("http") ? url : "http://213.130.144.159/" + url;
    #endregion
}