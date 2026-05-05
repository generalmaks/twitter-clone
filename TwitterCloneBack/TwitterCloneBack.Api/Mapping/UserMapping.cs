using AutoMapper;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Model.User.Contracts;
using TwitterCloneBack.Model.User.Model;
using TwitterCloneBack.Post.Contracts;
using TwitterCloneBack.User.Contracts;

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