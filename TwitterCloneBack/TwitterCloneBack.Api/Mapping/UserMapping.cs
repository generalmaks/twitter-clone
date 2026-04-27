using AutoMapper;
using TwitterCloneBack.Api.Post.Contracts;
using TwitterCloneBack.Api.User.Contracts;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Model.User.Contracts;
using TwitterCloneBack.Model.User.Model;

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
