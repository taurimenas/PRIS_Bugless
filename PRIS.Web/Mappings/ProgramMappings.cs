using PRIS.Web.Models;
using PRIS.Core.Library.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PRIS.Web.Mappings
{
    public static class ProgramMappings
    {
        public static Core.Library.Entities.Program ToProgramEntity(ProgramCreateModel model)
        {
            return new Core.Library.Entities.Program
            {
                Name = model.ProgramName,
                Id = model.ProgramNameId
                //Id = model.CityNames.LastOrDefault().Id
                
            };
        }
        public static City ToCityEntity(CityCreateModel model)
        {
            return new City
            {
                Name = model.CityName,
                Id = model.CityNameId
                
                //Id = model.CityNames.LastOrDefault().Id
            };
        }
        public static ProgramViewModel ToProgramViewModel(List<Core.Library.Entities.Program> programEntity)
        {
            return new ProgramViewModel
            {
                ProgramNames = programEntity,
                
            };
        }


        public static ProgramViewModel ToCityViewModel(List<City> cityEntity)
        {
            return new ProgramViewModel
            {
                CityNames = cityEntity,
                
            };
        }
    }
}
