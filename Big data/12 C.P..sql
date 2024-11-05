

# Group by, Having and Rollup

# Group by

# 1.	World database_dagi country jadvalidagi ma’lumotlardan foydalanib, Continent va GovernmentForm bo'yicha davlatlar sonini aniqlang.
SELECT Continent, GovernmentForm, COUNT(*) AS CountryCount
FROM country
GROUP BY Continent, GovernmentForm
order by Continent, GovernmentForm;

# 2.	World database_dagi CountryLanguage jadvalidan har bir CountryCode bo'yicha davlatda gaplashiladigan tillar sonini toping.
select CountryCode, Count(*) from countryLanguage group by CountryCode;

# 3.	World database_dagi CountryLanguage jadvalidan har bir CountryCode bo'yicha davlatda gaplashiladigan norasmiy tillar sonini toping.
select CountryCode, count(IsOfficial) from countrylanguage  where IsOfficial = 'F' group by CountryCode;

# Having

# 1.	World database_dagi country jadvalidan har bir Continent bo‘yicha davlat soni 20 dan katta bo‘lgan qit'alarni ko'rsating.
select Continent, count(*) as countCountry from country group by Continent having countCountry > 20;

# 2.	World database_dagi country jadvalidan har bir Continent bo‘yicha aholisi 100 milliondan ortiq regionlarni aniqlang.
select Continent, Region, sum(Population) as sumPopulation from country group by Continent, Region having sumPopulation > 100000000 order by Continent;

# 3.	World database_dagi city jadvalidan har bir davlat bo‘yicha shaharlar soni 15 yoki ortiq bo‘lgan davlatlarni ko‘rsating.
select CountryCode, count(*) as countEverything from city group by CountryCode having countEverything > 15 order by countEverything desc;

# 4.	World database_dagi city jadvalidan shaharlar soni 17 bo‘lgan davlatni ko‘rsating.
select CountryCode, count(*) as countEverything from city group by CountryCode having countEverything = 17;

# 5.	World database_dagi country jadvalidan har bir Continent bo‘yicha o'rtacha LifeExpectancy 70 dan undan yuqori bo‘lgan qit'alarni ko‘rsating.
select Continent, Region, avg(LifeExpectancy) as avgLife from country group by Continent, Region having avgLife > 70 order by Continent, Region;

# 6.	World database_dagi country jadvalidan Region bo‘yicha eng katta aholi soni 100 milliondan yuqori bo‘lganlarni aniqlang.
select Region, sum(Population) as sumPop from country group by Region having sumPop> 100000000 order by Region;

# Rollup

# 1.	World database_dagi country jadvalidan har bir mintaqa (Region) bo‘yicha davlatlar sonini hisoblang va umumiy qiymatni ko‘rsating.
select Region, count(*) from country group by Region with rollup order by Region;

# 2.	World database_dagi country jadvalidan har bir qit'a (Continent) bo‘yicha umumiy aholi sonini hisoblang va umumiy qiymatni ko‘rsating.
select Continent, sum(Population) as sumPop from country group by Continent with rollup order by Continent;

# 3.	World database_dagi country jadvalidan har bir davlat turi (GovernmentForm) bo‘yicha davlatlar sonini ko‘rsating.
select GovernmentForm, count(*) as everything from country group by GovernmentForm with rollup order by everything desc;
# Is it true???


# 4.	World database_dagi country jadvalidan har bir qit’a(continent) va mintaqa(region) bo‘yicha o‘rtacha maydonni (SurfaceArea) hisoblang
# va umumiy qiymatni ko‘rsating.
select Continent, Region, sum(SurfaceArea) from country group by Continent, Region with rollup order by Continent, Region;

# 5.	World database_dagi country jadvalidan har bir mintaqa bo‘yicha hayot davomiyligining (LifeExpectancy) o‘rtacha qiymatini ko‘rsating.
select Region, avg(LifeExpectancy) as avgLife from country group by Region with rollup order by region;

# 6.	World database_dagi country jadvalidan har bir mintaqa bo‘yicha davlatlarning umumiy maydonini hisoblang va umumiy qiymatni ko‘rsating.
select Region, sum(SurfaceArea) as area from country group by Region with rollup order by area desc;

# 7.	World database_dagi country jadvalidan har bir qit'a bo‘yicha davlatlarning o‘rtacha aholisini hisoblang va umumiy qiymatni ko‘rsating.
select Continent, avg(LifeExpectancy) from country group by Continent with rollup;

# 8.	Sakila database_dagi film jadvalidan har bir film davomiyligi (length) bo‘yicha filmlar sonini hisoblang va umumiy qiymatni ko‘rsating.
select length, count(*) as films from film group by length with rollup order by films desc;

# 9.	Sakila database_dagi film jadvalidan har bir toifa (category_id) bo‘yicha filmlarning o‘rtacha davomiyligini hisoblang va umumiy qiymatni ko‘rsating.
select * from film;