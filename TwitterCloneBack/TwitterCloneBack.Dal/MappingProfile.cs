using AutoMapper;
using TwitterCloneBack.Dal.Post.Dao;
using TwitterCloneBack.Dal.User.Dao;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Model.User.Model;

namespace TwitterCloneBack.Dal;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, UserDao>().ReverseMap();
        CreateMap<PostDto, PostDao>().ReverseMap();
    }
}
