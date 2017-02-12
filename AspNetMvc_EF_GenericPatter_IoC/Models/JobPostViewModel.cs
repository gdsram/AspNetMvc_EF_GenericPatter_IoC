using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNetMvc_EF_GenericPattern_IoC.Models
{
    public class JobPostViewModel
    {
        public JobPost JobPost { get; set; }
        public IEnumerable<SelectListItem> AllJobTags { get; set; }

        private List<int> _selectedJobTags;
        public List<int> SelectedJobTags
        {
            get
            {
                if (_selectedJobTags == null)
                {
                    try
                    {
                        _selectedJobTags = JobPost.JobTags.Select(m => m.Id).ToList();
                    }
                    catch (NullReferenceException)
                    {
                        return new List<int>();
                    }
                }
                return _selectedJobTags;
            }
            set { _selectedJobTags = value; }
        }
    }
}