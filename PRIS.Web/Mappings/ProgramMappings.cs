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
            };
        }
        public static City ToCityEntity(CityCreateModel model)
        {
            return new City
            {
                Name = model.CityName,
                Id = model.CityNameId
            };
        }
        public static ProgramViewModel ToProgramViewModel(List<ProgramCreateModel> program)
        {
            return new ProgramViewModel
            {
                ProgramNames = program,
            };
        }

        public static ProgramViewModel ToCityViewModel(List<CityCreateModel> city)
        {
            return new ProgramViewModel
            {
                CityNames = city
            };
        }
        public static CityCreateModel ToCityListViewModel(City city)
        {
            return new CityCreateModel
            {
                CityName = city.Name,
                CityNameId = city.Id
            };
        }
        public static ProgramCreateModel ToProgramListViewModel(PRIS.Core.Library.Entities.Program program)
        {
            return new ProgramCreateModel
            {
                ProgramName = program.Name,
                ProgramNameId = program.Id
            };
        }
    }
}
