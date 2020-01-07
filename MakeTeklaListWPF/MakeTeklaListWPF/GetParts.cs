using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;

namespace MakeTeklaListWPF
{
    public class GetParts
    {
        /// <summary>
        ///  Connectio to the Tekal Model.
        /// </summary>
        public Model Mod { get; set; }

        public GetParts(Model _mod)
        {
            this.Mod = _mod;
        }

        public Dictionary<string, Assembly> GetAssemblysFromParts()
        {
            /// <summary>
            ///  Returns assemblies from parts that have been pre-selected in the model.
            /// </summary>          
            //1.)make object selector Class:
            Tekla.Structures.Model.UI.ModelObjectSelector selectedObjectsSelector = new Tekla.Structures.Model.UI.ModelObjectSelector();
            //2.)copy selected Object Enumeration to ModelObjectEnumerator:
            ModelObjectEnumerator selectedParts = selectedObjectsSelector.GetSelectedObjects();
            //3.)get all selected single Parts:
            List<Part> parts = new List<Part>();
            while (selectedParts.MoveNext())
            {
                Part ThisAssembly = selectedParts.Current as Part;
                if (ThisAssembly != null)
                {
                    parts.Add(ThisAssembly);
                }
            }
            //4.)make a dictinary with all assemblies from the single parts key= AssemblyPos Nr.
            //   delete duplicates by try-catch:
            Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
            foreach (var p in parts)
            {
                string s = "";
                p.GetAssembly().GetReportProperty("ASSEMBLY_POS", ref s);
                try
                {
                    assemblies.Add(s, p.GetAssembly());
                }
                catch
                {
                }
            }
            return assemblies;
        }
    }
}
