

# If functions


# 1.	chinook database_dagi invoice jadvalida keltirilgan total ustunidagi qiymatlarning 2 dan kichik bo’lganlarini Amazing, 
# qolganlarini Great deb nomlangan ustinni chiqaring 
select Total, if(Total < 2, 'Amazing', 'Great') as 'Total_status' from invoice;

# 2.	chinook database_dagi invoice jadvalida keltirilgan total ustunidagi qiymatlarning 10 yoki undan katta bo‘lganlarini yaxshi, qolganlarini yomon deb 
# nomlangan ustinni chiqaring va bu ustundagi ma’lumotlarni total ustunidagi ma’lumotlarning kamiyish tartibida chiqaring
select Total, if(Total >= 10, 'yaxshi', 'yomon') as 'Total_st' from invoice order by Total desc;

# 3.	world database_dagi country jadvalidan foydalanib mamlakatlarning nomlari shu bilan birga mustaqillikka erishgan yillari 1900 yildan oldin bo’lganlarini 
# OLD, qolganlarini NEW deb nomlangan datalari keltirilgan ustunlarni(bu ustunni indep_status deb nomlang) chiqaring
select Name, if(IndepYear < 1900, 'Old', 'New') as indep_status from country;

# 4.	chinook database_dagi invoice jadvalining total ustunida keltirilgan datalarning 5 yoki undan kichik bo’lganlarini 1.25 ga, 
# qolganlarini 0.75 ga ko’paytirish natijasida hosil bo’lgan ustunni chiqaring. 
select Total, if(Total <= 5, Total*1.25, Total*0.75) from invoice;

# 5.	chinook database_dagi invoice jadvalining total ustunida keltirilgan datalarning 10 yoki undan kichik bo’lganlarini 1.5 ga, qolganlarini 0.85 ga ko’paytirish
# natijasida hosil bo’lgan ustunni chiqaring va bu ustunga result_total deb nom bering.
select Total, if(Total <= 10, Total*1.5, Total * 0.85) as 'result_total'  from invoice;


# Case Statements

# 1.	sakila database_dagi film jadvalining rental_rate ustunida keltirilgan datalarning 1 dan kichiklarini Stern,
# 1 dan 3 gacha bo’lganlarini Midst va 3 dan kattalarini Prime deb nomlangan ustunni chiqaring.
select rental_rate,
case
when rental_rate < 1 then 'Stern'
when rental_rate between 1 and 3 then 'Midst'
when rental_rate > 3 then 'Prime'
end as "rate status"
from film;

# 2.	sakila database_dagi film jadvalining length ustunida keltirilgan datalarning 40 dan kichiklarini Satisified, 40 dan 50 gacha bo’lganlarini Normal 
# va 50 dan kattalarini Classic deb nomlangan ustunni chiqaring.
select title,
case
when length < 40 then "Satisified"
when length between 40 and 50 then "Normal"
when length > 50 then "Classic"
end
 from film;

# 3.	World database_dagi country jadvalida GNP nomli ustun mavjud. Ushbu ustundagi datalarining 1000 dan kichiklarini Perilous, 
# 1000 dan boshlab 10000 gacha bo’lganlarini Well, 10000 dan kattalarini Great deb nomlangan ustunni chiqaring va bu ustunga GNP_status deb nom bering.
select GNP, 
case
when GNP < 1000 then "Perilous"
when GNP between 1000 and 10000 then "Well"
when GNP > 10000 then "Great"
end as "GNP_status"
 from country;

# 4.	World database_dagi country jadvalida lifeexpectancy nomli ustun mavjud. Ushbu ustundagi datalarining 50 dan kichiklarini Dangerous,
# 50 va undan katta biroq 60 dan kichik bo’lganlarini Normal, 60 va undan kattalarini Amazing deb nomlangan ustunni chiqaring va bu ustunga Age_lavel deb nom bering.
# Natijada Name va Age_lavel ustunlarini chiqaring.
select Name,
case
when LifeExpectancy < 50 then "Dangerous"
when LifeExpectancy between 50 and 60 then "Normal"
when LifeExpectancy > 60 then "Amazing"
end as "Age_level"
 from country;

# 5.	World database_dagi country jadvalida mamlakatlarning mustaqil bo’lgan yillari keltirilgan ustun mavjud. Ushbu ustundan foydalanib 1900 yildan oldin
# mustaqil bo’lgan mamlakatlarni Classical, 1900 yildan 1945 yilgacha oraliqda mustaqil bo’lganlarini Old, 1946 yildan 1990 yilgacha oraliqda mustaqil bo’lganlarini
# Federal va 1990 yildan keyin mustaqil bo’lganlarni Modern deb nomlangan ustunni chiqaring va bu ustunga indep_name deb nom bering. Shu bilan birga bu ustunga
# qo’shimcha ravishda mamalakat nomlari va mustaqil bo’lgan yillari keltirilgan ustunlarni ham taqdim eting.
select Name, IndepYear,
case
when IndepYear < 1900 then "CLassical"
when IndepYear between 1900 and 1945 then "Old"
when IndepYear between 1946 and 1990 then "Federal"
when IndepYear > 1990 then "Modern"
end as "Status"
from country order by IndepYear desc;