

## (AND, OR, NOT, BETWEEN, IS NULL, ROUND, CEIL, FLOOR, ABS, POWER, IF, CASE)


#1.	 Customer jadvalidan AQShda yashovchi yoki elektron pochta manzilida ".com" bo'lgan mijozlarni tanlang.
select * from Customer;

#2.	 Employee jadvalidan lavozimi "Sales Support Agent" bo'lmagan xodimlarni toping.
select * from Employee;

#3.	Invoice jadvalidan mamlakati "Canada" va umumiy summasi $10 dan katta bo'lgan fakturalarni toping.
select * from Invoice;

#4.	Track jadvalidan davomiyligi 300,000 millisekunddan katta yoki narxi $0.99 dan yuqori bo'lgan treklarni qidiring.
select * from Track;

#5.	Album jadvalidan AlbumId qiymati 5 va 10 oralig'ida bo'lmagan albomlarni qidiring.
select * from Album;

#6.	Invoice jadvalidan umumiy narxni 2 xonagacha yaxlitlab qaytaring.
select * from Invoice;

#7.	Track jadvalidan trek narxini yuqoriga qarab yaxlitlang va trekning nomi bilan birga qaytaring.
select * from Track;

#8.	Track jadvalidan trekning davomiyligini daqiqalarda hisoblab, pastga qarab yaxlitlab qaytaring.
select * from Track;

#9.	Invoice jadvalidan umumiy qiymati $20 dan katta bo'lgan fakturalarning umumiy summasidan $20 ni ayirib, ularning mutlaq qiymatini qaytaring.
select * from Invoice;

#10.	Track jadvalidan trek narxining kvadratini hisoblab, natijani qaytaring.
select * from Track;

#11.	Customer jadvalidan Kanadada yashovchi yoki "Gmail" elektron pochta manziliga ega bo'lgan, ammo ismi "John" bo'lmagan mijozlarni tanlang.
select * from Customer;

#12.	Invoice jadvalidan shartnoma sanasi 2010-yilning 1-yanvaridan 2021-yilning 31-dekabriga qadar bo'lgan va mamalakati AQSh yoki Kanada bo'lgan fakturalarni tanlang.
select * from Invoice;

#13.	Employee jadvalidan ReportsTo qiymati mavjud bo'lmagan (ya'ni, NULL) yoki lavozimi "Manager" bo'lmagan xodimlarni tanlang
select * from Employee;

#14.	Track jadvalidan davomiyligi 5 daqiqadan kam yoki 10 daqiqadan ko'p bo'lgan va narxi $0.99 dan katta bo'lmagan treklarni toping.
select * from Track;

#15.	Album jadvalidan nomi "The" so'zi bilan boshlanmaydigan va ArtistId qiymati 5, 8 yoki 10 bo'lgan albomlarni qidiring.
select * from Album;

#16.	Invoice jadvalida umumiy summasi $10 dan katta bo'lsa, “Qimmat”, aks holda “Arzon” deb belgilang.
select * from Invoice;

#17.	Customer jadvalida mijozning telefon raqami mavjud bo'lmasa, uni “Telefon raqamsiz” deb belgilang.
select * from Customer;

#18.	Invoice jadvalidan foydalanib, umumiy summa 20 dan katta bo’lsa “qimmat”, 10 va 20 orasida bo’lsa “o’rtacha” va 10 dan kichik bo’lsa “arzon” kabi darajalarni belgilang.
select * from Invoice;

#19.	Customer jadvalida xaridorlar mamlakati asosida turli tavsiflarni qaytaring. Masalan, agar xaridor AQSh yoki Kanada mamlakatidan bo‘lsa, uni “Asosiy Mijoz” sifatida, aks holda “Xorijiy Mijoz” sifatida belgilang va ustunga customer_description deb nom bering.
select * from Customer;

#20.	Customer jadvalidan foydalanib, mijozning Country ustunidagi qiymati mavjud bo‘lmasa, u holda uni "Mamlakat ko‘rsatilmagan" deb belgilang.
select * from Customer;

#21.	Invoice jadvalidan foydalanib, har bir hisob-kitob uchun Total qiymatiga asoslanib, quyidagi darajalardan birini qaytaring:
#Agar Total $15 dan katta bo'lsa – "Qimmat",
#Agar Total $10 va $15 orasida bo'lsa – "O‘rtacha",
#Agar Total $10 dan kichik bo'lsa – "Arzon".

select * from Invoice;

#22.	Track jadvalidan foydalanib, har bir qo'shiqning davomiyligi (Milliseconds ustuni) bo‘yicha uning "Qisqa" yoki "Uzun" ekanini belgilang. Agar qo‘shiq davomiyligi 3 daqiqadan (180000 milliseconds) ortiq bo‘lsa, uni "Uzun" deb, aks holda "Qisqa" deb belgilang.
select * from Track;

#23.	Track jadvalidan foydalanib, qo'shiqning davomiyligi (Milliseconds) va GenreId ga asoslanib quyidagicha ichki janr toifalarini belgilang:
#Agar Milliseconds 300000 dan ortiq va GenreId 1 yoki 2 (Rock yoki Jazz janrida) bo'lsa – "Uzun va Klassik",
#Agar Milliseconds 180000 va 300000 orasida bo'lsa va GenreId 3 yoki 4 (Metal yoki Classical janrida) bo'lsa – “O’rtacha Davomiylik”,
#Boshqa holatlar uchun – “Qisqa”.
select * from Track;

#24.	Mijozlarning Customer jadvalidagi SupportRepId ustuniga asoslanib, quyidagicha bonus darajasini aniqlang:
#Agar SupportRepId 3 yoki 4 ga teng bo'lsa – “Yuqori Bonus”,
#Agar SupportRepId 5 dan katta bo'lsa – “normal Bonus”,
#Aks holda – “Bonussiz”.
select * from Customer;

#25.	Track jadvalidan foydalanib, UnitPrice va Milliseconds qiymatlarini hisobga olib quyidagi toifalarni belgilang:
#Agar UnitPrice 1 dan katta va Milliseconds 200000 dan ortiq bo'lsa – "Qimmat va Uzun",
#Agar UnitPrice 0.5 dan kichik va Milliseconds 100000 dan kichik bo'lsa – "Arzon va Qisqa",
#Aks holda – "O’rtacha".
select * from Track;

#26.	Invoice jadvalidan foydalangan holda BillingCountry va Total ustunlariga qarab quyidagi stavkalarni belgilang:
#Agar BillingCountry "USA" yoki "UK" va Total 10 dan katta bo‘lsa – "Yuqori Stavka",
#Agar BillingCountry "Canada" va Total 5 dan kichik bo‘lsa – "Past Stavka",
#Aks holda – "O’rtacha Stavka".
select * from Invoice;

#27.	Customer jadvalidan foydalanib, Country va SupportRepId qiymatlariga qarab quyidagi toifalarni belgilang:
#Agar mijozning Country "Brazil" va SupportRepId 5 dan katta bo‘lsa – "Qo’shimcha yordam kerak",
#Agar Country "France" va SupportRepId 3 dan kichik bo‘lsa – "Yuqori xizmat ko’rsatilgan",
#Boshqa hollarda – "Yordam yo’q".
select * from Customer;

