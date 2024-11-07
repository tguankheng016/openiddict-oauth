using AutoMapper;
using OpenIddictOAuth.Web.Models.Login;

namespace OpenIddictOAuth.Web.Models;

public class ModelMappings : Profile
{
    public ModelMappings()
    {
        CreateMap<LoginViewModel, Features.Login.Login>();
    }
}