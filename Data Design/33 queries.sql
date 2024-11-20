select * from salesman;

select "This is SQL Exercise, Practice and Solution";

select 1,2,3;

select 10+15;

# 5
select 56+4-10/5*10;

select name, comission from salesman;

select ord_date, salesman_id, ord_no, purch_amt from orders;

select distinct salesman_id from orders;

select name, city from salesman where city = "Paris";

# 10

select * from customer where grade = 200;

select ord_no, ord_date, purch_amt from orders where salesman_id = 5001;

select YEAR, SUBJECT, WINNER from nobel_win where YEAR = 1970;

select WINNER from nobel_win where YEAR = 1971 and subject = 'Literature';

select year, subject from nobel_win where winner = "Dennis Gabor";

# 15

select winner from nobel_win where year >= 1950 and subject = "Physics";

select year, subject, winner, country where category= "Chemistry" and year between 1956 and 1975;





