# 1.	World database_dagi city jadvalidan Shahar nomlari va  mamlakat kodlari keltirilgan ustunlarni chiqaring.
select Name, CountryCode from city;

# 2.	World database_dagi country table_da mamlakatlar nomi, aholi soni va egallagan maydoni keltirilgan ustunlarni chiqarib bering.
select Name,Population,SurfaceArea from country;

# 3.	World database_dagi country table_da keltirilgan ma’lumotlardan foydalanib har bir mamlakat aholisining jon boshiga to’g’ri keladigan maydonning o’rtacha qiymatini toping.
select Name,SurfaceArea/Population from country;

# 4.	World database_dagi country table_dan mamalakat nomilari, aholi soni va o’rtacha umr ko’rish muddatining 5 ga kamaytirilgan ustunlarini chiqaring.
select Name,Population,LifeExpectancy-5 from country;

# 5.	World database_dagi country table_da qaysi continentlar keltirilgan?
select distinct Continent from country;

# 6.	World database_dagi country jadvalida nomlari keltirilgan mamlakatlarda qanday hukumat shallari mavjud.
select Name, GovernmentForm, HeadOfState from country;

# 7.	World database_dagi city jadvalida qanday turdagi  CountryCode_lar mavjud
select distinct Code from country;

# 
# WHERE CLAUSE


# 1.	World database_dagi city jadvalidan mamlakat kodi AFG bo’lgan shaharlarga tegishli barcha ma’lumotlarni chiqaring.
select * from city where CountryCode = "AFG";

# 2.	World database_dagi city jadvalidan Preston shahridagi aholi sonini toping (bu ma’lumot bizga aholining o’sish ko’rsatgichini baholash uchun zarur)
select Name,Population from city where Name = "Preston";

# 3.	World database_dagi city jadvalida ID raqami 45 bo‘lgan shaharning mamlakat kodi va aholi sonini aniqlang.
select CountryCode,Population from city Where ID = 45;

# 4.	World database_dagi city jadvalidan aholi soni 270000 ga teng bo’lgan shaharlar nomini aniqlang 
select Name from city where Population = 270000;

# 5.	World database_dagi countrylanguage jadvalidan AFG kodli mamlakatda rasmiy va norasmiy sanalgan tillar va ularning foiz ko’rsatgichini chiqaring
select Language, Percentage from countrylanguage where CountryCode = "AFG";

# ??? need to ask ???
# COMPARISON OPERATORS
# 1.	World database_dagi country jadvalida keltirilgan ma’lumotlardan foydalanib aholi soni 24 000 000  dan yuqori bo’lgan mamlakat nomlari va ularning kontinentlari ro’yaxatini chiqaring.
select Name,Continent from country where Population > 24000000;

# 2.	World database_dagi country jadvalidan foydalanib 1900 yildan oldin mustaqil bo’lgan davlatlarning nomlari va mustaqil bo’lgan yillari ro’yxatini chiqaring
select Name,IndepYear from country where IndepYear < 1900 order by IndepYear;

# 3.	World database_dagi country jadvalidan foydalanib aholisining o’rtacha umr ko’rish muddati 79 yosh va undan yuqori bo’lgan mamlakatlar ro’yxatini chiqaring
select Name,LifeExpectancy from country where LifeExpectancy >= 79 order by LifeExpectancy desc;

# 4.	World database_dagi country jadvalida keltirilgan ma’lumotlar orqali maydoni 15 000 000 km2 dan yuqori bo’lgan mamlakat nomi va aholi sonini aniqlang.
select Name,Population from country where SurfaceArea > 15000000;

# 5.	World database_dagi country jadvalida keltirilgan ma’lumotlar orqali mamlakat kodi AFG bo’lmagan shaharlar nomlarni chiqaring
select Name from country where not Code = "AFG";

