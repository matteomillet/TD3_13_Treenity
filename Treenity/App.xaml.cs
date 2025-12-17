using System.Configuration;
using System.Data;
using System.Windows;

namespace Treenity
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool ModeZQSD = false;

        public static Dictionary<String, double> chanceEnnemis = new Dictionary<String, double>()
        {
            {"citrouille", 0.08},{"vers", 0.2},{"fantome", 0.4},{"lapin", 0.7},{"volatile", 1}
        };

        public static Dictionary<String, int[]> statsEnnemis = new Dictionary<string, int[]>()
        {
            {"volatile", new int[] {10, 2, 1} },
            {"lapin", new int[] {20, 4, 3} },
            {"fantome", new int[] {20, 7, 2} },
            {"vers", new int[] {30, 7, 3} },
            {"citrouille", new int[] {50, 12, 1}}
        };
    }

}
