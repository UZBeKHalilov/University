# World Database: Subqueries in SELECT and FROM Clauses


#1.	World ma’lumotlar bazasidagi country jadvalidan foydalanib, mamlakatlararo insonlarning o‘rtacha umr ko‘rish ko‘rsatkichini ko‘rsating.
select Name, (select avg(LifeExpectancy) from country) as global_lifeExp, LifeExpectancy from country;

#2.	World ma’lumotlar bazasidan foydalanib, har bir mamlakat uchun shaharlar sonini hisoblang.
select Name, (select count(*) from city where country.Code = city.CountryCode) as city_count from country;

#3.	World ma’lumotlar bazasidan foydalanib, shaharlar o‘rtacha aholisini har bir mamlakat bo‘yicha ko‘rsating.
select Name, (select avg(Population) from city where country.Code = city.CountryCode) as avg_population from country;

#4.	World ma’lumotlar bazasidan foydalanib, har bir mintaqada o'rtacha aholi sonini ko'rsating.
select Region, avg(Population) avg_population from country group by Region;

#5.	World ma’lumotlar bazasidan foydalanib, har bir mamlakatda davlat tillari sonini hisoblang.
select Name, (select count(*) from countrylanguage where country.Code = countrylanguage.CountryCode) as Language_count 
from country order by Language_count desc;

#6.	World ma’lumotlar bazasidan foydalanib, mamlakatlarning qit’alari bo‘yicha eng yuqori aholi ko‘rsatkichini ko‘rsating.
select Continent, max(Population) max_population from country group by Continent;

#7.	World ma’lumotlar bazasidan foydalanib, har bir mamlakatda ishlatiladigan tillarning ro‘yxatini tuzing.
select * from countrylanguage;

#8.	World ma’lumotlar bazasidan foydalanib, qit’alar bo‘yicha umumiy mamlakatlar sonini ko'rsating.
select Continent as Continents, (select count(*) from country where Continents = country.Continent) as country_count 
from country group by Continent order by country_count desc;

#9.	World ma’lumotlar bazasidan foydalanib, har bir mamlakatda faqat eng ko'p aholiga ega bo'lgan shaharni ko'rsating.
select Name, (select max(Population) from city where city.CountryCode = country.Code order by Population desc limit 1) as max_pop from country;


#10.	World ma’lumotlar bazasidan foydalanib, mamlakatlar va ulardagi o’rtacha umr ko’rish ko’rsatgichi, 
# shu bilan birga mamlakatlar aholisining o'rtacha umr ko'rishidan yuqori ko'rsatkichga ega mamlakatlarning o'rtacha
# umr ko'rish ko’rsatgichini chiqaring.select * from country;
select * from country;
