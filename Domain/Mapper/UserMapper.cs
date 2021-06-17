using AutoMapper;
using Domain.Authenticate;
using Domain.User;

namespace Domain.Mapper
{
    public static class UserMapper
    {
        private static readonly IMapper Mapper;

        static UserMapper()
        {
            var config = new MapperConfiguration(Init);

            Mapper = config.CreateMapper();
        }

        private static void Init(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserRegistrationRequestDto, UserModel>();
        }

        public static UserModel MapToDto(this UserRegistrationRequestDto userRegistrationRequestDto, string password)
        {
            var res = Mapper.Map<UserModel>(userRegistrationRequestDto);
            res.Password = password;
            return res;
        }
    }
}