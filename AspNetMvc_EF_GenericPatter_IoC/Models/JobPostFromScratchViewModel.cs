using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetMvc_EF_GenericPattern_IoC.Models
{
    public class JobPostFromScratchViewModel
    {
        public JobPost JobPost { get; set; }
        public Employer Employer { get; set; }
        public string Tags { get; set; }
    }
}