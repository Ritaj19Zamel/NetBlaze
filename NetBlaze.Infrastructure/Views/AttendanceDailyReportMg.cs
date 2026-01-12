namespace NetBlaze.Infrastructure.Views
{
    public static class AttendanceDailyReportMg
    {
        public static string Up()
        {
            return @"CREATE OR REPLACE 
                VIEW `netblazedb`.`vw_attendancedailyreport` AS
                    SELECT 
                        `a`.`UserId` AS `UserId`,
                        `u`.`DisplayName` AS `DisplayName`,
                        `a`.`AttendDate` AS `AttendDate`,
                        MIN(`a`.`AttendTime`) AS `CheckIn`,
                        MAX(`a`.`AttendTime`) AS `CheckOut`
                    FROM
                        (`netblazedb`.`employeeattendences` `a`
                        JOIN `netblazedb`.`users` `u` ON ((`u`.`Id` = `a`.`UserId`)))
                    GROUP BY `a`.`UserId` , `u`.`DisplayName` , `a`.`AttendDate`";
        }
        public static string Down()
        {
            return @"DROP VIEW IF EXISTS `netblazedb`.`vw_attendancedailyreport`;";
        }
    }
}
