-- STRING FUNCTIONS (Length, Upper, lower, left, right, substring, replace, locate, concat;)

-- 1.	chinook database_dagi customer jadvalida keltirilgan mijozlarning
-- ismi va ularga tegishli PostalCode ustunidagi ma’lumotlarning uzunliklari keltirilgan ustunlarni yarating 
select FirstName,  length(PostalCode) from employee;

-- 2.	chinook database_dagi invoice jadvalida keltirilgan BillingPostalCode ustunidagi 
-- barcha ma’lumotlarning uzunliklari keltirilgan ustunni yarating va bu ustunga BPC deb nom bering
select length(BillingAddress) as BPC from invoice;

-- 3.	chinook database_dagi invoice jadvalida keltirilgan BillingCity va BillingCountry ustunidagi ma’lumotlarning barchasini katta harflarda chiqaring
select Upper(BillingCity) ,Upper(BillingCountry) from invoice;

-- 4.	chinook database_dagi invoice jadvalida keltirilgan BillingAddress ustunidagi ma’lumotlarning barchasini kichkina harflarda chiqaring 
select lower(BillingAddress) from invoice;

-- 5.	Ma’lumotlari chinook database_dagi customer jadvalida keltirilgan va telefon raqamlari +3 bilan boshlanadigan mijozlarning 
-- ism va familyalarini katta harflarda, addresslarini kichik harflarda va telefon raqamlarini o’z holaticha keltirilgan ustunlarni chiqaring.
select upper(FirstName), upper(LastName), lower(Address ) from customer where Phone like '+3%';

-- 6.	chinook database_dagi customer jadvalida ma’lumotlari keltirilgan, Email addresslari .com bilan tugaydigan mijozlarning,
--  ismi va mamlakat nomlari katta harflarda, telefon raqamlari, Email addresslari va supportrepid lari o’z holaticha
--  hamda supportrepid lari kamayib borish tartibida keltirilgan ustunlarni chiqaring.
select upper(FirstName), upper(Country), Phone, Email, SupportRepId from customer where Email like '%.com' order by SupportRepId desc;

-- 7.	chinook database_dagi employee jadvalidan ishchilarning ishga yollangan oylarini chiqaring
select substring(HireDate, 6, 2) from employee;

-- 8.	chinook database_dagi employee jadvalidan ishchilarning tug’ilgan yillarini chiqaring
select left(BirthDate, 4) from employee;

-- 9.	chinook database_dagi customer jadvalidan mijozlarning Emailiga tegishli fr domenlarini com ga o’zgartiring
select Email, replace(Email, '.fr', '.com') from customer;

-- 10.	chinook database_dagi employee jadvalidan ishchilarning  ism va familyalarini bir ustunga joylashtiring bunda
-- familya katta harflarda bo’lishi zarur.
select concat(FirstName, upper(LastName)) from employee;

-- 11.	chinook database_dagi employee jadvalida keltirilgan ishchilarning  ismi va tug’ilgan yillarini 
-- bir ustunga joylashtiring bunda ularning ismlari katta harflarda bo’lishi zarur.
select concat(upper(FirstName), BirthDate) from employee;

-- 12.	chinook database_dagi employee jadvalida keltirilgan ishchilarning  ismi va tug’ilgan yillarini bir ustunga joylashtirilishidan
-- hosil bo’lgan ma’lumotlarning uzunliklarini toping. (ma’lumot na’munasi: Azamat 2004)
select length(concat(FirstName,left(BirthDate,4))) from employee;

-- 13.	Chinook database_dagi invoice jadvalidagi ma’lumotlardan invoicedate ustunidan yillarini ajtratib chiqaring va unga invoice_year deb nom bering
select left(InvoiceDate,4) as 'invoice_year' from invoice;

-- 14.	Chinook database_dagi invoice jadvalidagi ma’lumotlardan BillingPostalCode larning oxirgi 3 ta belgisi keltirilgan ustunni chiqaring 
-- va uni importantcode deb nomlang 
select right(BillingPostalCode,3) as 'importantcode' from invoice;

