//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsFormsApp8.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PositionID { get; set; }
        public Nullable<double> Salary { get; set; }
        public System.DateTime StartTime { get; set; }
        public bool Deleted { get; set; }
        public string Photo { get; set; }
        public int? SpecialityID { get; set; }
    
        public virtual Position Position { get; set; }
        public virtual Program Program { get; set; }
    }
}
