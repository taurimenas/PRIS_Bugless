using PRIS.Web.Models;
using PRIS.Core.Library.Entities;

namespace PRIS.Web.Mappings
{
    public static class ProgramMappings
    {
        public static Core.Library.Entities.Program ToProgramEntity(ProgramViewModel model)
        {
            return new Core.Library.Entities.Program
            {
                Id = model.ProgramId,
                Name = model.ProgramName
            };
        }
        public static City ToCityEntity(ProgramViewModel model)
        {
            return new City
            {
                Id = model.CityId,
                Name = model.CityName
            };
        }
        public static ProgramViewModel ToProgramViewModel(Core.Library.Entities.Program programEntity)
        {
            return new ProgramViewModel
            {
                ProgramId = programEntity.Id,
                ProgramName = programEntity.Name,
            };
        }
        public static ProgramViewModel ToCityViewModel(City cityEntity)
        {
            return new ProgramViewModel
            {
                CityId = cityEntity.Id,
                CityName = cityEntity.Name
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
