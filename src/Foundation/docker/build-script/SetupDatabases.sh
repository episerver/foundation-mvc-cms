#!/bin/bash

export cms_db=foundation.cms
export user=netcoreUser
export password=epi#Server7Local

export sql="/opt/mssql-tools/bin/sqlcmd -S . -U sa -P ${SA_PASSWORD}"
echo @Wait MSSQL server to start
export STATUS=1
i=0

while [[ $STATUS -ne 0 ]] && [[ $i -lt 60 ]]; do
    sleep 5s
	i=$i+1
	$sql -Q "select 1" >> /dev/null
	STATUS=$?
    echo "***Starting MSSQL server..."
done

if [ $STATUS -ne 0 ]; then
	echo "Error: MSSQL SERVER took more than 3 minute to start up."
	exit 1
fi
echo Dropping databases...
$sql -Q "EXEC msdb.dbo.sp_delete_database_backuphistory N'$cms_db'"
$sql -Q "if db_id('$cms_db') is not null ALTER DATABASE [$cms_db] SET SINGLE_USER WITH ROLLBACK IMMEDIATE"
$sql -Q "if db_id('$cms_db') is not null DROP DATABASE [$cms_db]"

echo Dropping user...
$sql -Q "if exists (select loginname from master.dbo.syslogins where name = '$user') EXEC sp_droplogin @loginame='$user'"

echo Creating databases...
$sql -Q "CREATE DATABASE [$cms_db] COLLATE SQL_Latin1_General_CP1_CI_AS"

echo Creating user...
$sql -Q "EXEC sp_addlogin @loginame='$user', @passwd='$password', @defdb='$cms_db'"
$sql -d $cms_db -Q "EXEC sp_adduser @loginame='$user'"
$sql -d $cms_db -Q "EXEC sp_addrolemember N'db_owner', N'$user'"

echo Done creating databases.
