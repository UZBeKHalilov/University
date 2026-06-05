

# If functions


# 1.	Sakila database_dagi payment jadvalining amount ustunida keltirilgan ma’lumotlardan foydalanib 5 va undan ko’p to’lov qilganlarni Premium,
# qolganlarini Standart toifasiga kiritilgan ustunni chiqaring va bu ustunni amount bo’yicha, kamayib borishda tartiblang hamda payment_status deb nomlang.
select *, if(amount >= 5, "Premium", "Standart") as "payment_status" from payment order by amount desc;

# 2.	sakila database_dagi film jadvalining length ustunida keltirilgan datalarning 80 yoki undan kattalarini normal, qolganlarini not_normal deb ifodalangan 
# ustunni chiqaring. Shu bilan birga bu ustunni biror bir nom bilan atab, undagi datalarni rental_rate ustunidagi ma’lumotlarning kamayib borishida tartiblang.
select *, if(length >= 80, "normal", "not_normal") as "Status" from film order by rental_rate desc;

# 3.	sakila database_dagi film jadvalida keltirilgan ma’lumotlardan foydalanib, rental_rate ni kamayib borishi tartibida, G yoki NC-17 reytingida turuvchi 
# filmlarning title, reyting va rental_rate 4 dan katta bo’lganda Original,
# qolganlari Common ko’rinishida nomlangan ustunlarni chiqaring(bu ustunni StandartValue deb nomlaysiz).
select title, rating,  if(rental_rate > 4, "Orginal", "Common") as "StandartValue" from film where rating in ("G", "NC-17") order by rental_rate desc;

# 4.	world database_dagi country jadvalidan foydalanib Osiyo qit’asida joylashgan mamlakatlarning nomlari, aholisining o’rtacha umr ko’rish
# davomiyligi va bu o’rtacha umr ko’rish davomiyligining 7 raqami bilan boshlanadiganlarini climb qolganlarini low deb nomlangan
# ustunni(Bu ustunga live_status deb nom bering) chiqaring.
select Name, LifeExpectancy, if (LifeExpectancy like "7%" , "climb", "low") as "live_status" from country where Continent = "Asia";

# 5.	world database_dagi country jadvalidan foydalanib mamlakatlarning nomlari keltirilgan ustunni va Southern and Central Asia
# regionidagi mamlakatlar Turkestan, qolganlari esa Others deb nomlangan ustunni (bu ustunga Geoname nomini bering) chiqaring.
select Name, if(Region = "Southern and Central Asia", "Turkestan", "Others") as "Geoname" from country;


# Case Statements


# 1.	Chinook database_dagi invoice jadvalida InvoiceDate nomli ustunda invoice sanalari keltirilgan. Invoice sanalari 2022-12-31 gacha bo’lganlarini
# ‘oldyears’, 2023-01-01 dan boshlab 2023-12-31 gacha bo’lganlarini 'lastyear' , 2024-01-01 va undan keying sanlarni esa ‘this year’ deb nomlangan
# ustunni chiqaring va unga InvoicePeriod deb nom bering.
select * from invoice;

# 2.	World database_dagi country jadvalida mamlakatlarning mustaqil bo’lgan yillari keltirilgan ustun mavjud. Ushbu ustundan foydalanib mustaqillik yili 20 soni bilan boshlanadiganlarini 21century, 19 soni bilan boshlanadiganlarini 20century, 18 soni bilan boshlanadiganlarini 19century, 17 soni bilan boshlanadiganlarini 18century va qolganlarini oldcentury deb nomlangan ustunni chiqaring va bu ustunga indepcentury deb nom bering. Shu bilan birga bu ustunga qo’shimcha ravishda mamalakat nomlari keltirilgan ustunni ham chiqaring
select * from country;

# 3.	World database_dagi countrylanguage jadvalidan foydalanib, mamlakatlarda rasmiy sanalgan tillarning foiz ko’rsatkichlarini bu ko’rsatkich 10 dan kichik bo’lganda Yomon, 10 dan 20 gacha bo’lganlarini Qoniqarli, 20.1 dan 30 gacha bo’lganlarini Yaxshi va 30 dan katta bo’lganlarini Ajoyib deb nomlangan ustunni foiz ko’rsatgichlarining kamayib borishi tartibida chiqaring hamda bu ustunga Languagestatus deb nom bering. Natijada Languagestatus ustuni bilan birgalikda CountryCode va IsOfficial ustunlarini ham chiqaring.
select * from countrylanguage;
