# Subquery Basics (World Database)

# 1.	World ma’lumotlar bazasidan foydalanib, eng ko‘p aholiga ega mamlakatni aniqlang.
select Name, Population from country where Population = (select max(Population) from country);

# 2.	World ma’lumotlar bazasidan foydalanib, aholi soni o'rtacha qiymatdan yuqori bo‘lgan shaharlar va ularning aholisini ko‘rsating.
select Name, Population from city where Population > (select avg(Population) from city);

# 3.	World ma’lumotlar bazasidan foydalanib, aholisi o‘rtacha qiymatdan past bo‘lgan mamlakatlarni aniqlang.
select Name, Population from country where Population < (select avg(Population) from country);

# 4.	World ma’lumotlar bazasidan foydalanib, maydoni dunyodagi mamlakatlarning o'rtacha yer maydonidan yuqori bo'lgan mamlakatlarni ko'rsating.
select Name, SurfaceArea from country where SurfaceArea > (select avg(SurfaceArea) from country);

# 5.	World ma’lumotlar bazasidan foydalanib, aholisining umr ko‘rish davomiyligi barcha mamlakatlar aholisining
# o’rtacha umr ko’rish davomiyligidan yuqori bo‘lgan mamlakatlar ro'yxatini chiqarib bering.

select Name, LifeExpectancy from country where LifeExpectancy > (select avg(LifeExpectancy) from country);



# World Database: Any and All Operators

# 1.	World ma’lumotlar bazasidan foydalanib, aholisi barcha shaharlarining aholisidan katta bo‘lgan mamlakatlarni toping.
select Name, Population from country where Population > (select max(Population) from city);

# 2.	World ma’lumotlar bazasidan foydalanib, eng kam aholiga ega mamlakatlarni toping.
select Population, name from country where Population <=all (select Population from country where Population >0);

# 3.	World ma’lumotlar bazasidan foydalanib, Osiyo qit’asidagi eng yuqori umr ko‘rish ko‘rsatkichini aniqlang.
select Continent, max(LifeExpectancy) from country where Continent = "Asia" group by Continent;

# 4.	World ma’lumotlar bazasidan foydalanib, har bir qit’a uchun minimal umr ko‘rish qiymatini toping.
select Continent, min(LifeExpectancy) from country group by Continent;

# 5.	World ma’lumotlar bazasidan foydalanib, har bir qit’adagi minimal Yerni maydoniga ega mamlakatlarni toping.
select Continent, Name, min(SurfaceArea) from country group by Continent, Name;

# 6.	World ma’lumotlar bazasidan foydalanib, aholisi har bir mintaqadagi o‘rtacha aholiga teng yoki ko‘proq bo‘lgan mamlakatlarni ko‘rsating.
select * from country where Population > (select * from country);

# 7.	World ma’lumotlar bazasidan foydalanib, eng kam Yerni maydoniga ega bo'lgan mamlakatni toping.
select * from country where Population > (select * from country);

# 8.	World ma’lumotlar bazasidan foydalanib, o'rtacha umr ko‘rish qiymati butun dunyo mamlakatlarining o’rtacha umr ko’rish qiymatidan yuqori bo‘lgan mamlakatlarni ko'rsating.
select * from country where Population > (select * from country);

# 9.	World ma’lumotlar bazasidan foydalanib, har bir mintaqadagi eng kam aholi soniga ega shaharlarni toping.
select * from country where Population > (select * from country);

# 10.	World ma’lumotlar bazasidan foydalanib, mintaqalardagi har qanday mamlakatning maydonidan kichik mamlakatlarni ko‘rsating.
select * from country where Population > (select * from country);