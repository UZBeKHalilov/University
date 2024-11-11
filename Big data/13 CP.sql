# 1.	World ma’lumotlar bazasidagi Country jadvalidan nomlarida “land” so‘zi uchraydigan davlatlar sonini mintaqalar bo‘yicha chiqaring va
# ularning umumiy sonni ham aniqlang.
select Region, count(*) from country where Name like "%land" group by Region with rollup;

# 2.	World ma’lumotlar bazasidagi Country jadvalidan nomlari uzunligi 5 dan kam bo‘lgan davlatlarni sonini qita’lar bo‘yicha chiqaring.
# Bunday davlatlardan ja’mi nechta bor.
select Continent, count(*) from country where length(Name) < 5 group by Continent with rollup;

# 3.	World ma’lumotlar bazasidagi Country jadvalida keltirilgan davlatlar nomini teskari yozing va davlatlar sonini mintaqalar va
# teskari nomlanishi bo‘yicha chiqaring.
select Region, reverse(Name) as reversed, Count(*) from country group by Region, reversed order by Region, reversed;

# 4.	World ma’lumotlar bazasidagi Country jadvalida keltirilgan davlatlarni CASE statement yordamida quyidagicha toifalarga ajrating: 
# “United” bilan boshlansa – "Birlashgan", boshqalar esa – "Turli". Har bir toifa bo‘yicha davlat sonini mintaqalar bo‘yicha hisoblang.
select Region, 
	case 
    when Name like "United%" then "Birlashgan"
    else 'Turli'
    end as category,
    count(*) as count
from country group by Region, category order by count desc;

# 5.	World ma’lumotlar bazasidagi Country jadvalidan, nomining uzunligi 10 dan ortiq bo‘lgan yoki 1900 yildan keyin mustaqillikka
# erishgan davlatlar sonini regionlar bo‘yicha o‘sib borish tartibida chiqaring va bunday davlatlarning umumiy soniga ham aniqlik kiriting.
select Region, count(*) as count from country where length(Name)>10 or IndepYear>1900 group by Region with rollup order by Region,count;

# 6.	World ma’lumotlar bazasidagi Country jadvalidan nomining oxiri ‘stan’ bilan tugaydigan davlatlarni aniqlang va davlatlarni mintaqalar bo‘yicha sanang.
select Region, count(*) from country where Name like "%stan" group by Region;

# 7.	World ma’lumotlar bazasidagi Country jadvalidan davlatlarni nomining uzunligiga qarab qisqa (5 dan kam), o‘rtacha (5-10) va
# uzun (10 dan ortiq) kabi toifalarga ajrating. Har bir mintaqa va ajratilgan toifa bo‘yicha davlatlarni sanang.
select Region, Name, 
	case
		when length(Name) < 5 then "qisqa"
        when length(Name) < 10 then "o'rtacha"
        else "uzun"
    end as style
 from country group by Region, Name with rollup;

# 8.	Chinook ma’lumotlar bazasidagi invoice jadvalidan har bir mijoz bo‘yicha jami xarid summasini hisoblang va xarid
# summasi $45 dan yuqori bo‘lsa "Katta", $40–$45 oralig‘ida bo‘lsa "O‘rtacha", $40 dan past bo‘lsa "Kam" deb belgilab chiqing.
select CustomerId, sum(Total),
case
	when sum(Total) < 40 then "Kam"
	when sum(Total) < 45 then "O'rtacha"
    else "Katta"
end as style
 from invoice group by CustomerId;

# 9.	World ma’lumotlar bazasidagi country jadvalidan barcha davlatlarni region bo‘yicha guruhlang va har bir regiondagi davlatlar sonini hisoblang.
# Har bir davlatda yashovchi aholining o‘rtacha umri (lifeexpectancy) bo‘yicha quyidagi life_status toifalarini yarating:
# a.	Agar lifeexpectancy 80 dan katta bo‘lsa, “HighLife” deb belgilang.
# b.	Agar lifeexpectancy 60 va 80 orasida bo‘lsa, “MediumLife” deb belgilang.
# c.	Agar lifeexpectancy 60 dan kichik bo‘lsa, “LowLife” deb belgilang.
# Faqat aholi soni 10 milliondan ortiq bo‘lgan regionlardagi natijalarni chiqaring va tartibni davlatlar soni bo‘yicha kamayish tartibida ko‘rsating.
select Region, Count(*) as count,
case
	when avg(LifeExpectancy) < 60 then "LowLife"
    when avg(LifeExpectancy) < 80 then "MediumLife"
    else "HighLife"
end as style
from country group by Region having sum(Population)>10000000 order by count desc;

# 10.	Chinook ma’lumotlar bazasidagi customer jadvalidan shahar sonini hisoblang va country ustuni bo‘yicha guruhlang.
# Shuningdek, phone raqami AQSh uchun boshlanish qismi “+1%” bo‘lsa, “USAnumber” deb, Fransiya uchun “+33%” bo‘lsa, “Francenumber” deb,
# boshqa raqamlar uchun “others” deb belgilang. Faqat city_count qiymati 5 dan katta bo‘lgan natijalarni ko‘rsating.
select Country, count(distinct city) as cityCount,
case
	when Phone like "+1%" then "USAnumber"
    when Phone like "+33%" then "Francenumber"
    else "others"
end as style
 from customer group by Country, style having cityCount > 5;

# 11.	Chinook ma’lumotlar bazasidagi invoice jadvalidan har bir mijoz bo‘yicha xaridlar sonini hisoblang. Agar xaridlar soni 5 dan ko‘p bo‘lsa
# "yuqori xarid ", aks holda "Kam xarid" deb tasniflang.
select CustomerId, count(*),
case
	when count(*) > 5 then "yuqori xarid"
    else "Kam xarid"
end as style
 from invoice group by CustomerId;

# 12.	Chinook ma’lumotlar bazasidagi invoice jadvalidan har bir InvoiceId bo‘yicha umumiy xarid summasini hisoblab, $10 dan katta bo‘lsa "Yuqori to‘lov", $5–$10 oralig‘ida bo‘lsa "O‘rtacha to‘lov", $5 dan kichik bo‘lsa "Kam to‘lov" deb baholang.
select * from invoice;

# 13.	Chinook ma’lumotlar bazasidagi invoice jadvalidan har bir mijoz bo‘yicha xaridlar sonini hisoblang va xaridlar soni 10 dan katta bo‘lsa "Aktiv mijoz", 5 dan 10 orasida bo‘lsa "O‘rtacha aktiv mijoz", 5 dan kichik bo‘lsa "Past aktiv mijoz" deb belgilash.
select * from invoice;

# 14.	Sakila database_dagi film_category jadvalidan har bir toifa (category) bo‘yicha filmlarning umumiy sonini hisoblang va umumiy qiymatni ko‘rsating.
select * from film_category;