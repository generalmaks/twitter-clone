using AutoMapper;
using TwitterCloneBack.Dal.Dao;
using TwitterCloneBack.Model.Model;

namespace TwitterCloneBack.Dal;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, UserDao>().ReverseMap();
        CreateMap<PostDto, PostDao>().ReverseMap();
    }
}