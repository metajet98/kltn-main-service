namespace main_service.Constants
{
    public class Role
    {
        public const string StaffDesk = "StaffDesk";
        public const string StaffMaintenance= "StaffMaintenance";
        public const string CenterManager = "CenterManager";
        public const string User = "User";

        public const string SystemUser = "User,StaffDesk,StaffMaintenance";
        public const string All = "User,StaffDesk,StaffMaintenance,CenterManager";
        public const string Staff = "StaffDesk,StaffMaintenance,CenterManager";
        
    }
}