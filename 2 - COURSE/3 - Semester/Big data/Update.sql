alter table data rename column п»їsweepstake_id to sweepstake_id;

select * from data;

select phone, regexp_replace(phone, '[()/<>]', '') from data;

update data
set phone=regexp_replace(phone, '[()/-<>]', '');

select phone, concat(substring(phone, 1, 3), '-', substring(phone, 4, 3), '-', substring(phone, 4, 3)) from data;

update data
set phone=regexp_replace(phone, '[()/<>]', '');

drop table data;




select * from cleandata;

select favorite_color, concat(upper(substring(favorite_color, 1, 1)), lower(substring(favorite_color, 2)))  from cleandata;

update cleandata set  favorite_color = concat(upper(substring(favorite_color, 1, 1)), lower(substring(favorite_color, 2)));

alter table cleandata rename column п»їsweepstake_id to sweepstake_id;

select `Are you over 18?`, case 
when `Are you over 18?` = 'yes' then "Y"
when `Are you over 18?` = 'No' then "N"
else `Are you over 18?`
end as type
 from cleandata;

update cleandata set `Are you over 18?`= case 
when `Are you over 18?` = 'yes' then "Y"
when `Are you over 18?` = 'No' then "N"
else `Are you over 18?`
end;

