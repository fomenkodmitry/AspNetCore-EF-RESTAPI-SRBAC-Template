using AutoMapper;

namespace Domain.DomainToModelProfile
{
    /// <summary>
    /// Mapper profile
    /// </summary>
    public class DomainToModelProfile : Profile
    {
        public DomainToModelProfile()
        {
            int userId = 0;

            #region -- Test --
            // CreateMap<DbModel, ForeignModel>()
            //     .ForMember(dst => dst.Owner, src => src.MapFrom(x => x.ToPage))
            //     .ForMember(dst => dst.Photos, src => src.MapFrom(x => x))
            //     ;
            #endregion
        }
    }
}