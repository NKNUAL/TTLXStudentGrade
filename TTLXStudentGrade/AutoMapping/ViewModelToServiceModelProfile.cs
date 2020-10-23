using AutoMapper;
using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTLXStudentGrade.Api.Models;

namespace TTLXStudentGrade.AutoMapping
{
    public class ViewModelToServiceModelProfile : Profile
    {
        public override string ProfileName => "ViewModelToServiceModelProfile";

        public ViewModelToServiceModelProfile()
        {
            CreateMap<UploadUser, UploadUserServiceModel>();
        }
    }
}