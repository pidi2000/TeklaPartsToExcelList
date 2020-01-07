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
                InfoSinglePart info = new InfoSinglePart("", "", "", 0, 0, 0);
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

            //4.) delete duplicates:
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
                groupedQuantityAssembly[count].Quantity++;
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

        public InfoSinglePart(string _posNr, string _profile, string _material, double _length, double _weight, int _quantity)
        {
            this.PosNr = _posNr;
            this.Profile = _profile;
            this.Material = _material;
            this.Length = _length;
            this.Weight = _weight;
            this.Quantity = _quantity;
        }

        public override string ToString()
        {
            return $" PosNr:{PosNr}   Profil:{Profile}   Material:{Material}   Length:{Length}   Weight:{Weight}   Quantity:{Quantity}  ";
        }
    }
}
