//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Survey.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User_Vote
    {
        public int id { get; set; }
        public Nullable<int> QuestionOptionID { get; set; }
        public string user_name { get; set; }
        public Nullable<bool> isSelect { get; set; }
    }
}