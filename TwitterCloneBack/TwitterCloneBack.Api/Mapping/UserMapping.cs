using AutoMapper;
using TwitterCloneBack.Contracts;
using TwitterCloneBack.Model.Contracts;
using TwitterCloneBack.Model.Model;

namespace TwitterCloneBack.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<UserDto, GetUser>();
        CreateMap<UserDto, UpdateUser>().ReverseMap();

        CreateMap<PostDto, GetPost>();
    }
}
