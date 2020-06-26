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
                Name = model.Name
                
                //Id = model.CityNames.LastOrDefault().Id
                
            };
        }
        public static City ToCityEntity(ProgramViewModel model)
        {
            return new City
            {
                Name = model.CityNames.LastOrDefault().Name
                
                //Id = model.CityNames.LastOrDefault().Id
            };
        }
        public static ProgramViewModel ToProgramViewModel(List<Core.Library.Entities.Program> programEntity)
        {
            return new ProgramViewModel
            {
                ProgramNames = programEntity
            };
        }
        
        
        public static ProgramViewModel ToCityViewModel(List<City> cityEntity)
        {
            return new ProgramViewModel
            {
                CityNames = cityEntity
            };   
        }
        //public static ProgramViewModel ToCityViewModel(City entity)
        //{
        //    return new ProgramViewModel
        //    {
        //        CityName = entity.Name,
        //        CityId = entity.CityId
        //    };
        //}

    }
}
