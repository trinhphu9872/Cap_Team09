using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapstoneProjectTeam09.model
{
    public  class User:DbContext
    {
        // GET: User
      
        public string MSSV { get; set; }
        public string id_Class { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public int id { get; set; }
        public int id_Role { get; set; }
        public string email { get; set; }
        public string image { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        public Nullable<int> id_Status { get; set; }

        

    }
}