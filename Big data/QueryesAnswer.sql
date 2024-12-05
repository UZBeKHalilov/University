-- 1  World databesdan country tabledan Federal Republica hukumat shakliga ega davlatlarni ortacha umr davomiyligini korsating
select Name, LifeExpectancy from country where GovernmentForm like "Federal Republic";

-- 2  World databesdan city tabledan aholi soni oxirigi 2 ta raqami 77 bilan tugaydigan shaharlani toping
select * from city where Population like "%77";

-- 3  Sakila databesdan customer tabledan store_id 2 bolgan barcha odamlarni ism va famiylasi bilan chiqaring
select concat(first_name, ' ',last_name) as Name from customer where store_id = 2;

-- 4  Sakila databesdan city tabeldan country_id si bir xil bolgan davlatlarni ketma ket joylashtiring
select city, country_id from city order by country_id;

-- 5  Chinook databesdan track tabeldan composer null bolgan musiqalarni toping 
select * from track where Composer is null;

-- 6  Chinook databesdan customer tabledan telefon raqamlar +420 bilan boshlangan va oxirgi raqami 55 bilan tugaydigan odamlarni malumotlarni chiqarinh
select * from customer where Phone like "+420%55";


# 1-chinook databasedagi totalni billingcountry kesimida umumiy totalini 
# hisoblang va yangi hosil bo'lgan columnni TotalByCountry deb nomlang
select BillingCountry, sum(Total) as TotalByCountry from invoice group by BillingCountry;

# 2-chinook databasedagi shaharlar sonini har bir country kesimida 
# hisoblang va yangi hosil bo'lgan columnni 'NumberOfCities' deb nomlang
select Country as Countryes, (select count(city) from customer where Countryes = Country) as NumberOfCities from customer group by Country;
# needed just count(city)

# 3-sakila databasedagi country table'dan faqat 'tan' bilan tugagan davlatlarni olib ko'rsating.
select * from country;

# 4-sakila databasedagi country table'dan 'tan' bilan tugagan davlatlarni 'Central Asia Countries',
# united' bilan boshlangan davlatlarni 'United Countries', qolganlarni esa 'Other countries' va yangi columnni esa 'Categories' deb nomlang.

# 5-world database'dagi davlatlarni o'rtacha LifeExpectancy'ni 70 dan katta bo'lganlarini
# davlatlar kesimida yahlitlab chiqaring va yangi columnni 'AverageLifeEx' deb nomlang

# 6-world databasedagi countrylanguage table'dagi language columnida har bir tilni sonini hisoblang va har bir tilni va sonini chiqaring va alifbo bo'yicha tartiblang
