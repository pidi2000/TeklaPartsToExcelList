using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;

namespace MakeTeklaListWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        Model mod = new Model();

        public MainWindow()
        {         
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonGetAssembliesFromSelectedPArts_Click(object sender, RoutedEventArgs e)
        {         
        GetParts getParts = new GetParts(mod);
        Dictionary<string, Assembly> assemblies = getParts.GetAssemblysFromParts();

            List <AssemblyInfo> assemblyInfo = new List<AssemblyInfo>();
            foreach(var ass in assemblies)
                {
                    assemblyInfo.Add(new AssemblyInfo(ass.Value));
                }

            string s = "";
            foreach(var info in assemblyInfo)
                {
                    s=s+"\n"+info.ToString();
                } 
            labelInfoBox.Content = s;
        }

 
    }
}
