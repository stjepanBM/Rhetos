using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoteli.Concepts
{
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("Sifarnik")]
    public class SifarnikInfo : IConceptInfo
    {
        [ConceptKey]
        public EntityInfo Entity { get; set; }

    }

    [Export(typeof(IConceptMacro))]
    public class SifarnikMacro : IConceptMacro<SifarnikInfo>
    {
        public IEnumerable<IConceptInfo> CreateNewConcepts(SifarnikInfo conceptInfo, IDslModel existingConcepts)
        {
            var newConcepts = new List<IConceptInfo>();
            var poljeOznaka =  new ShortStringPropertyInfo {
                Name = "Oznaka",
                DataStructure = conceptInfo.Entity
            };
            var poljeNaziv = new ShortStringPropertyInfo
            {
                Name = "Naziv",
                DataStructure = conceptInfo.Entity
            };

            newConcepts.Add(poljeNaziv);

            newConcepts.Add(new AutoCodePropertyInfo
            {
                Property = poljeOznaka   
            });
            newConcepts.Add(poljeOznaka);

            return newConcepts;
        }
    }


}
