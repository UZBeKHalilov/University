-- LIKE 


-- 1.	World database_dagi country jadvalidan foydalanib mamlakat kodi A harfi bilan boshlanadigan mamlakatlarning kodlari va nomlarini chiqaring.
select Code, Name from country where Code like 'A%';

-- 2.	World database_dagi country jadvalidan foydalanib mustaqil bo’lgan yili 1 raqami bilan tugaydigan va A yoki B harfi bilan boshlanuvchi regionlarga ega mamlakatlarning ro’yxatini chiqaring.
select * from country where IndepYear like '%1' and Region like 'A%' or Region like 'B%';

-- 3.	World database_dagi country jadvalida o’rtacha umr ko’rish davomiyligi keltirilgan ustundan 7 raqimi bilan boshlandiganlarini ajratib bering
select * from country where LifeExpectancy like '7%';

-- 4.	World database_dagi country jadvalidan foydalanib capital raqami ikki xonali son bo’lgan mamlakatlarning nomi va mustaqil bo’lgan yillarini chiqaring
select Name, IndepYear from country where Capital like '__';

-- 5.	Sakila database_dagi address jadvalidan foydalanib, postal kodi 5 bilan boshlanib, telefon raqami 5 bilan tugaydigan mijozlarga tegishli address_id larni chiqaring
select * from address;

-- 

-- ORDER BY CLAUSE 

-- 1.	World database_dagi country jadvalidan foydalanib mamlakatlarning nomlarini va aholi sonini, ulardagi aholi sonining ortib borishi tartibida chiqaring.
select Name, Population from country order by Population;

-- 2.	World database_dagi country jadvalidan foydalanib mamlakatlarning kodlarini va aholi sonini, ulardagi aholi sonining ortib borishi tartibida chiqaring.
select Code, Population from country order by Population;

-- 3.	World database_dagi city jadvalida keltirilgan barcha ma’lumotlarni bu shaharlardagi aholi sonining kamayib borishi tartibida keltiring
select * from city order by Population desc;

-- 4.	World database_dagi country jadvalidan foydalanib Osiyo qit’asida joylashgan mamlakatlarning nomlari va o’racha umr ko’rish davomiyligi keltirilgan ustunlarni ulardagi aholi sonining kamayishi tartibida chiqaring
select Name, LifeExpectancy from country where Continent = 'Asia' order by Population desc;

-- 5.	World database_dagi country jadvalidan foydalanib Yevropa  qit’asida joylashgan, uch xonali capital raqamga ega mamlakatlarning nomlari va mustaqil bo’lgan yillari keltirilgan ustunlarni aholi sonining kamayishi tartibida chiqaring
select Name, IndepYear from country where Continent = 'Europe' and Capital like '___' order by Population desc;


-- 

-- LIMIT 


-- 1.	World database_dagi country jadvalidan barcha ustunlarga ega, faqatgina dastlabki 7 qator ma’lumotni chiqaring
select * from country limit 7;

-- 2.	World database_dagi country jadvalidan foydalanib, o’rtacha umr ko’rish davomiyligi bo’yicha yetakchi 5 ta mamlakatni ko’rsating
select Name,Region,LifeExpectancy from country order by LifeExpectancy desc limit 5;

-- 3.	World database_dagi country jadvalidan foydalanib Osiyo  qit’asida joylashgan va aholisining soni bo’yicha top 3 talikda turuvchi mamlakatlarning nomlarini chiqaring. 
select * from country where Continent = 'Asia' order by Population desc limit 3;

-- 4.	World database_dagi country jadvalidan foydalanib, eggalagan maydoni bo’yicha reytingda eng quyi 10 talikda turuvchi  va Africa qit’asida joylashgan mamlakatlar ro’yxatini chiqaring.
select * from country where Continent = 'Africa' order by SurfaceArea limit 10;

-- 5.	World database_dagi country jadvalidan foydalanib, Yevropa qit’asida joylashgan, 1950 yildan so’ng mustaqillikka erishgan, aholisining o’racha umr ko’rish davomiyligi 70 yosh va undan yuqori bo’lgan va GNP reytingida egallagan o’rni bo’yicha top 5 talik turuvchi mamlakatlar ro’yxatini chiqaring. 
select * from country where Continent = 'Europe' and IndepYear > 1950 and LifeExpectancy >= 70 order by GNP desc limit 5;

-- 

-- ALIASING


-- 1.	Sakila database_dagi category jadvalidagi category_id ustunini ID ga va name ustunini names ga o‘zgrtirib oling   
select * from country;

-- 2.	Sakila database_dagi category jadvalidagi category_id ni 100 ga ortiring va bu  ustunini new_id  deb nomlab oling. Last_update ni update ga o’zgartiring
select * from country;

-- 3.	Chinook database_dagi genre jadvalida keltirilgan GenreId nomli ustunni ID deb, Name ustunini esa Genre_name deb nomlagan holda taqdim eting.
select GenreId as ID, Name as Genre_name from genre;

-- 4.	Chinook database_dagi invoiceline jadvalidan foydalanib, maxsulot sotuvidan bo’ladigan kirimning miqdorlari keltirilgan ustunni revenue deb nomlang va bu ustunni InvoiceId ustuni bilan birgalikda chiqaring
select UnitPrice * Quantity as Revenue, InvoiceId from invoiceline order by revenue desc;

-- 5.	Chinook database_dagi customer jadvalida keltirilgan CustomerId nomli ustundagi ID larni 100 ga ortirib va ustun nomini  ID deb, FirstName ustunini Forename deb, lastname ustunini esa surename deb nomlagan holda taqdim eting.
select CustomerId * 100 as ID, FirstName as Forename, LastName as Surename from customer;