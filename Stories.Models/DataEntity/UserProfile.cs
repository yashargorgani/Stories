using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stories.Models
{
    public class UserProfile
    {
        public System.Guid Id { get; set; }

        [Required(ErrorMessage ="لطفا وارد نمایید")]
        public string Name { get; set; }
        public string MobileNumber { get; set; }

        //public int? EducationLevelId { get; set; }
        //public int? EducationFieldId { get; set; }
        //public int? WorkFieldId { get; set; }

        public string Bio { get; set; }
        public byte[] ProfileImage { get; set; }
        public System.DateTime RegisterDate { get; set; }


        #region Linked Entities

        //public string EducationLevelCaption { get; set; }
        //public string EducationFieldCaption { get; set; }
        //public string WorkFieldCaption { get; set; }
        #endregion
    }
}