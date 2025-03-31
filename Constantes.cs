namespace projetBase
{
    public class Constantes
    {
        public static string BaseApiAddress => "http://213.130.144.159/";
        public static int UserId
        {
            get => Preferences.Get("UserId", 0);
            set => Preferences.Set("UserId", value);
        }
    }
}