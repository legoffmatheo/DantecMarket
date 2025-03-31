using AP4DantecMarket.Vues;

namespace AP4DantecMarket
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new VueLogin();

        }
    }
}
