namespace UseCases.Profiles;

[ExcludeFromCodeCoverage]
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SessionUserDto, LoggedUserDetails>().ReverseMap();
        // CreateMap<DriverDetailsDto, Driver>().ReverseMap();
   
    }
}
