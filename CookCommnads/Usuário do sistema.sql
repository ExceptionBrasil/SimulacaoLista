
--Login e senha do ColorobbiaPlataform

create login ColorobbiaPlataform With Password='Imperador'

Grant connect SQL to ColorobbiaPlataform;
Grant IMPERSONATE ANY LOGIN to ColorobbiaPlataform;
Grant AUTHENTICATE SERVER to ColorobbiaPlataform;
Grant  CREATE ANY DATABASE          to ColorobbiaPlataform;
Grant  VIEW ANY DATABASE			to ColorobbiaPlataform;
Grant  ADMINISTER BULK OPERATIONS	to ColorobbiaPlataform;
Grant  ALTER ANY DATABASE			to ColorobbiaPlataform;
Grant  CONNECT ANY DATABASE			to ColorobbiaPlataform;
Grant  CREATE ANY DATABASE			to ColorobbiaPlataform;
Grant VIEW SERVER STATE	            To ColorobbiaPlataform;


use ColorobbiaPlataform

create user ColorobbiaPlataform for login ColorobbiaPlataform

grant Alter to  ColorobbiaPlataform;
grant Select to ColorobbiaPlataform;
grant Delete to ColorobbiaPlataform;
grant Insert to ColorobbiaPlataform;
grant Update to ColorobbiaPlataform;



--Connect
grant CONNECT to ColorobbiaPlataform;;

--Backup
grant BACKUP DATABASE to ColorobbiaPlataform;;
grant BACKUP LOG to ColorobbiaPlataform;;

--Control
grant control to ColorobbiaPlataform;;


--Create
grant Create role  to ColorobbiaPlataform;;
grant Create Table  to ColorobbiaPlataform;;
grant Create Schema to ColorobbiaPlataform;;
grant CREATE SYNONYM to ColorobbiaPlataform;;
grant CREATE FUNCTION to ColorobbiaPlataform;;
grant CREATE DEFAULT to ColorobbiaPlataform;;
grant CREATE PROCEDURE to ColorobbiaPlataform;;
grant CREATE VIEW	   to ColorobbiaPlataform;;

--Execute
grant EXECUTE		   to ColorobbiaPlataform;;

--Show
grant SHOWPLAN		   to ColorobbiaPlataform;;