namespace NetBlaze.Infrastructure.Views
{
    public static class EmployeeCheckinViolationsMg
    {
        public static string Up()
        {
            return @"CREATE 
                    VIEW `netblazedb`.`vw_employeecheckinviolations` AS
                        SELECT 
                            `ea`.`Id` AS `AttendanceId`,
                            `ea`.`UserId` AS `UserId`,
                            `u`.`UserName` AS `UserName`,
                            `p`.`Id` AS `PolicyId`,
                            `p`.`PolicyName` AS `PolicyName`,
                            `p`.`PolicyCode` AS `PolicyCode`,
                            `p`.`PolicyType` AS `PolicyType`,
                            CONCAT('Arrived at ',
                                    DATE_FORMAT(`ea`.`AttendTime`, '%h:%i %p')) AS `Clarification`,
                            `p`.`ActionValue` AS `ViolationValue`
                        FROM
                            ((((`netblazedb`.`employeeattendences` `ea`
                            JOIN (SELECT 
                                `netblazedb`.`employeeattendences`.`UserId` AS `UserId`,
                                    `netblazedb`.`employeeattendences`.`AttendDate` AS `AttendDate`,
                                    MIN(`netblazedb`.`employeeattendences`.`AttendTime`) AS `FirstCheckIn`
                            FROM
                                `netblazedb`.`employeeattendences`
                            GROUP BY `netblazedb`.`employeeattendences`.`UserId` , `netblazedb`.`employeeattendences`.`AttendDate`) `firstcheck` ON (((`firstcheck`.`UserId` = `ea`.`UserId`)
                                AND (`firstcheck`.`AttendDate` = `ea`.`AttendDate`)
                                AND (`firstcheck`.`FirstCheckIn` = `ea`.`AttendTime`))))
                            JOIN `netblazedb`.`users` `u` ON ((`u`.`Id` = `ea`.`UserId`)))
                            JOIN `netblazedb`.`policies` `p` ON ((`p`.`PolicyCode` = 'CHECKIN')))
                            LEFT JOIN `netblazedb`.`attendencepolicyactions` `apa` ON (((`apa`.`PolicyId` = `p`.`Id`)
                                AND `apa`.`AttendenceId` IN (SELECT 
                                    `ea2`.`Id`
                                FROM
                                    `netblazedb`.`employeeattendences` `ea2`
                                WHERE
                                    ((`ea2`.`UserId` = `ea`.`UserId`)
                                        AND (`ea2`.`AttendDate` = `ea`.`AttendDate`))))))
                        WHERE
                            ((`ea`.`AttendTime` NOT BETWEEN `p`.`WorkStartTime` AND `p`.`WorkEndTime`)
                                AND (`apa`.`Id` IS NULL))";
        }
        public static string Down()
        {
            return @"DROP VIEW IF EXISTS `netblazedb`.`vw_employeecheckinviolations`;";
        }
    }
}
