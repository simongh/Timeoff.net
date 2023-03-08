﻿namespace Timeoff.Entities
{
    public class Department
    {
        public string Name { get; set; }

        public int Allowance { get; set; } = 20;

        public bool IncludeBankHolidays { get; set; } = true;

        public bool IsAccrued { get; set; }

        public int DepartmentId { get; private set; }

        public int? ManagerId { get; set; }

        public User? Manager { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public ICollection<User> Users { get; set; } = new HashSet<User>();

        public ICollection<DepartmentSupervisor> Supervisors { get; set; } = new HashSet<DepartmentSupervisor>();
    }
}