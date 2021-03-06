﻿using System.Collections.Generic;
using AutoMapper;
using InsideMai.Models;

namespace InsideMai.ViewModels.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Department, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, Department>();

            CreateMap<User, UserViewModel>()
                .ForMember(dst => dst.FullName,
                    opts => opts.MapFrom(src => src.LastName + " " + src.FirstName))
                .ForMember(dst => dst.DepartmentName,
                    opts => opts.MapFrom(src => src.Department.Name));

            CreateMap<Post, PostViewModel>()
                .ForMember(dst => dst.Author,
                    opts => opts.MapFrom((src, dto, i, context) =>
                        src.IsAnonymous ?
                            new UserViewModel()
                                { FullName = "Анонимно", UserPic = "b91a76fd-168f-4302-99b1-ddf063485c58.png" }
                            : context.Mapper.Map<UserViewModel>(src.Author)))
                .ForMember(dst => dst.Department,
                    opts => opts.MapFrom((src, dto, i, context) =>
                        context.Mapper.Map<DepartmentViewModel>(src.Department)));

            CreateMap<Comment, CommentViewModel>()
                .ForMember(dst => dst.Author,
                    opts => opts.MapFrom((src, dto, i, context) =>
                        context.Mapper.Map<UserViewModel>(src.Author)));

            CreateMap<Post, UserPostViewModel>()
                .ForMember(dst => dst.Author,
                    opts => opts.MapFrom((src, dto, i, context) =>
                        src.IsAnonymous ? 
                            new UserViewModel() 
                                { FullName = "Анонимно", UserPic = "b91a76fd-168f-4302-99b1-ddf063485c58.png" } 
                            : context.Mapper.Map<UserViewModel>(src.Author)))
                .ForMember(dst => dst.Department,
                    opts => opts.MapFrom((src, dto, i, context) =>
                        context.Mapper.Map<DepartmentViewModel>(src.Department)))
                .ForMember(dst => dst.Comments, opts => opts.MapFrom((src, dto, i, context) =>
                    context.Mapper.Map<List<CommentViewModel>>(src.Comments)));
        }
    }
}
