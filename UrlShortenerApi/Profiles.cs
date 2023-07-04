using AutoMapper;
using UrlShortenerApi.Models;
using UrlShortenerApi.Models.Data;
using UrlShortenerApi.Models.View;

namespace UrlShortenerApi.Profiles
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Account to AccountView
            CreateMap<Account, AccountView>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
                .ForMember(dest => dest.FallBackUrls, opt => opt.MapFrom(src => src.FallBackUrls))
                .ForMember(dest => dest.UrlExclusions, opt => opt.MapFrom(src => src.UrlExclusions))
                .ForMember(dest => dest.CustomDomains, opt => opt.MapFrom(src => src.CustomDomains))
                .ForMember(dest => dest.Urls, opt => opt.MapFrom(src => src.Urls));

            // Map AccountView to Account
            CreateMap<AccountView, Account>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
                .ForMember(dest => dest.FallBackUrls, opt => opt.MapFrom(src => src.FallBackUrls))
                .ForMember(dest => dest.UrlExclusions, opt => opt.MapFrom(src => src.UrlExclusions))
                .ForMember(dest => dest.CustomDomains, opt => opt.MapFrom(src => src.CustomDomains))
                .ForMember(dest => dest.Urls, opt => opt.MapFrom(src => src.Urls));


            // User Mappings
            CreateMap<User, UserView>();

            CreateMap<UserView, User>();

            // Custom Domain Mappings
            CreateMap<CustomDomain, CustomDomainView>();

            CreateMap<CustomDomainView, CustomDomain>();

            // FallbackUrls Mapping
            CreateMap<FallBackUrls, FallBackUrlsView>();

            CreateMap<FallBackUrlsView, FallBackUrls>();

            // Url Exclusions
            CreateMap<UrlExclusion, UrlExclusionView>();

            CreateMap<UrlExclusionView, UrlExclusion>();

            //  Urls
            CreateMap<Url, UrlView>();
            CreateMap<UrlView, Url>();


        }
    }
}
