using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VilliaDTO> villaList = new List<VilliaDTO>()
        {
                new VilliaDTO {Id=1, Name="Pool side View",Sqft=150,Occupancy=4 },
                new VilliaDTO {Id=2, Name="Beach side View",Sqft=100, Occupancy =3},
                new VilliaDTO {Id=3, Name="Window side View",Sqft=90,Occupancy=2},
                new VilliaDTO {Id=4, Name="Shop side View",Sqft=200,Occupancy=6}
        };
    }
}
