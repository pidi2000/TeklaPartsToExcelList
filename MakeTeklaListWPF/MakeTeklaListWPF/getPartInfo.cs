using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;
using System.Collections;


namespace MakeTeklaListWPF
{
    class getPartInfo
    {
        /// <summary>
        /// Split Assemblies in Singleparts and returns Properties
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static List<InfoSinglePart> GetSinglePartInfoFromAssemblies(Assembly assembly)
        {

            List<InfoSinglePart> infoSingleParts = new List<InfoSinglePart>();
            //1.) Get Array Parts = seondaryParts + Main Part:
            ArrayList parts = assembly.GetSecondaries();
            parts.Add(assembly.GetMainPart());


            //2.) Get Properties from Part:
            foreach (var p in parts)
            {
                InfoSinglePart info = new InfoSinglePart("", "", "", 0, 0, 0, 0);
                ModelObject mo = p as ModelObject;
                string posiNr = "";
                string profile = "";
                string material = "";

                double length = 0;
                double weight = 0;

                mo.GetReportProperty("PART_POS", ref posiNr);
                mo.GetReportProperty("PROFILE", ref profile);
                mo.GetReportProperty("MATERIAL", ref material);
                mo.GetReportProperty("LENGTH", ref length);
                mo.GetReportProperty("WEIGHT", ref weight);
                info.PosNr = posiNr;
                info.Profile = profile;
                info.Material = material;
                info.Length = length;
                info.Weight = weight;
                infoSingleParts.Add(info);

            }

            //3.) group assembly by pos Nr.
            var sortAssembly = infoSingleParts.GroupBy(x => x.PosNr)
             .ToList();
            List<InfoSinglePart> groupedAssembly = new List<InfoSinglePart>();
            foreach (var group in sortAssembly)
            {
                foreach (var v in group)
                {
                    groupedAssembly.Add(v);
                }
            }

            //4.) delete duplicates, find quantity of parts:

            List<InfoSinglePart> groupedQuantityAssembly = new List<InfoSinglePart>();
            string posNr = "startPosNr";
            int count = -1;
            foreach (var part in groupedAssembly)
            {
                if (posNr != part.PosNr)
                {
                    groupedQuantityAssembly.Add(part);
                    posNr = part.PosNr;
                    count++;
                }
            //Quantity:
                groupedQuantityAssembly[count].Quantity++;
             //Total Weight:
                groupedQuantityAssembly[count].WeightTotal =  groupedQuantityAssembly[count].Weight* groupedQuantityAssembly[count].Quantity;
            }
            return groupedQuantityAssembly;
        }
    }


    class InfoSinglePart
    {
        public string PosNr { get; set; }
        public string Profile { get; set; }
        public string Material { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }
        public double WeightTotal {get; set;}
        

        public InfoSinglePart(string _posNr, string _profile, string _material, double _length, double _weight, int _quantity, double _weightTotal)
        {
            this.PosNr = _posNr;
            this.Profile = _profile;
            this.Material = _material;
            this.Length = _length;
            this.Weight = _weight;
            this.Quantity = _quantity;
            this.WeightTotal = _weightTotal;
        }

        public override string ToString()
        {
            return $" PosNr:{PosNr}   Profil:{Profile}   Material:{Material}   Length:{Length}   Weight:{Weight}   Quantity:{Quantity}   WeightTotat:{WeightTotal} ";
        }
    }


     class AssemblyInfo
      {
        private List<InfoSinglePart> InfoSiglePartList {get;set;}
        public double AssemblyWeight{get;set;}
        public string AssemblyPosNr{get;set;}
        public int AssemblyQuantity{get;set;}

        public AssemblyInfo(Assembly _assembly)
            {
          
            this.InfoSiglePartList= getPartInfo.GetSinglePartInfoFromAssemblies(_assembly);
            string _posNr="";
            _assembly.GetReportProperty("ASSEMBLY_POS", ref _posNr);
            int _quantity=0;
            _assembly.GetReportProperty("MODEL_TOTAL", ref _quantity);
            this.AssemblyPosNr = _posNr;
            this.AssemblyQuantity = _quantity;

            //Calculate Weight
            double _assemblyWeight = 0;
            foreach(var part in InfoSiglePartList)
             {
                _assemblyWeight = _assemblyWeight+part.WeightTotal;
             }
            this.AssemblyWeight=_assemblyWeight;
         }

           public override string ToString()
        {
            return $" PosNr:{AssemblyPosNr}    Weight:{AssemblyWeight}   Quantity:{AssemblyQuantity} ";
        }

     }
}
