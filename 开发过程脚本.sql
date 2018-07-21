
drop table projectcontributors;
drop table projectproperties;
drop table projectviewers;
drop table projectvisiblerules;
drop table projects;
delete from __efmigrationshistory;


use finbook_metadata;


select * from Users;
insert into users (city,Company,Name,Phone,gender) values('hangzhou','google','creasypita','nophone',1);

update users set id = 1;