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
            GetParts getPArts = new GetParts(mod);
            Dictionary<string, Assembly> assemblies = getPArts.GetAssemblysFromParts();

            Dictionary<string, List<InfoSinglePart>> assembliePartInfo = new Dictionary<string, List<InfoSinglePart>>(); //string: AssemblyPos  valus: List of SinglePart Info

            foreach (var a in assemblies)
            {
                assembliePartInfo.Add(a.Key, getPartInfo.GetSinglePartInfoFromAssemblies(a.Value));
            }
            


            string s = "";
            
            foreach (var pi in assembliePartInfo)
                
            {
                string partinfo = "";
                s =  s+pi.Key.ToString()+"\n";
                foreach(var p in pi.Value)
                {
                    partinfo = p.ToString(); 
                    s= s+partinfo + "\n";
                }

            }
           labelInfoBox.Content = s;


        }

 
    }
}
