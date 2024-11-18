
# 1.	Chinook ma’lumotlar bazasidan Artist  jadvalidagi barcha musiqachilarni va ular bilan bog'liq albomlarni chiqaring.
select ar.Name, al.Title
from artist ar join album al 
on ar.ArtistId = al.AlbumId;


# 2.	Chinook ma’lumotlar bazasidagi Customer va Invoice jadvallarini qo‘shib, har bir mijozning ismi,
# familyasi,invoice sanasi va to‘lov ma’lumotlarini chiqaring.
select c.FirstName, c.LastName, i.InvoiceDate, i.Total 
from customer c inner join invoice i 
on c.CustomerId = i.CustomerId;

# 3.	Chinook ma'lumotlar bazasidagi Track va Genre jadvallarini qo‘shib, har bir trekning nomi va janrini chiqaring.
select t.Name as tname, g.Name as gname 
from track t inner join genre g 
on t.GenreId = g.GenreId;

# 4.	Chinook ma'lumotlar bazasidagi InvoiceLine va Track jadvallarini qo‘shib, har bir invoice chizig‘idagi trek nomini va ularning narxini chiqaring.
select * from genre;

# 5.	Chinook ma'lumotlar bazasidagi Album va Track jadvallarini qo‘shib, har bir albomdagi treklar ro‘yxatini chiqaring.
select * from track;

# 6.	Chinook ma'lumotlar bazasidagi Employee va Customer jadvallarini qo‘shib, har bir mijoz va unga tayinlangan xodimning ismini chiqaring.
select * from artist;

# 7.	Chinook ma'lumotlar bazasidagi PlaylistTrack va Track jadvallarini qo‘shib, har bir pleylistdagi treklar ro‘yxatini chiqaring.
select * from artist;

# 8.	Chinook ma'lumotlar bazasidagi MediaType va Track jadvallarini qo‘shib, har bir trekning nomi va uning media turini chiqaring.
select * from artist;

# 9.	Chinook ma'lumotlar bazasidagi Customer va Invoice jadvallarini qo‘shib, faqat 10 dollar yoki undan ortiq to‘lovni amalga oshirgan  mijozlarning ID raqamini,  ism va familyasini(bir ustunda, Fullname deb nomlang)  va to‘lov summalarini ko‘rsating.
select * from artist;

# 10.	Chinook ma'lumotlar bazasidan har bir qo'shiq (Track) va unga mos keladigan albom (Album) nomini toping.
select * from artist;

# 
select * from artist;

# 
select * from artist;

# JOINING MULTIPLE TABLES
select * from artist;

# 1.	Chinook ma'lumotlar bazasidagi Invoice, Customer, va Employee jadvallaridan har bir invoice uchun mijoz ismi va unga xizmat ko'rsatuvchi xodimning ismini ko'rsating.
select * from artist;

# 2.	Chinook ma'lumotlar bazasidagi Track, Album, va Artist jadvallaridan har bir trek nomini, uning albomini va albom ijrochisini toping.
select * from artist;

# 3.	Chinook ma'lumotlar bazasidagi Track, Album, va Genre jadvallaridan har bir albom uchun janr bo'yicha treklarni ko'rsating.
select * from artist;

# 4.	Chinook ma'lumotlar bazasidagi Track, PlaylistTrack, va Playlist jadvallaridan har bir trek va uning qaysi pleylisitga tegishli ekanini toping.
select * from artist;

# 5.	Chinook ma'lumotlar bazasidagi Track, MediaType, va Genre jadvallaridan har bir janr va media turining nomini ko'rsating.
select * from artist;

# 6.	Chinook ma'lumotlar bazasidagi PlaylistTrack, Track, va MediaType jadvallaridan har bir pleylisitdagi treklar va ularning media turini ko'rsating.
select * from artist;

# 7.	Chinook ma'lumotlar bazasidagi InvoiceLine, Track, Album, va Artist jadvallaridan har bir trekning albomi va uning ijrochisini ko'rsating.
select * from artist;

# 8.	Chinook ma'lumotlar bazasidagi Invoice, Customer jadvallari orqali har bir davlat bo'yicha umumiy invoice summasini hisoblang.
select * from artist;

# 9.	Chinook ma'lumotlar bazasidagi InvoiceLine, Track, va Genre jadvallaridan har bir janr bo'yicha sotilgan treklar sonini hisoblang.
select * from artist;

# 10.	Chinook ma'lumotlar bazasi asosida, har bir janr bo'yicha qo'shiqlar sonini va ularning umumiy vaqtini hisoblang.
select * from artist;

# 
select * from artist;

# 
select * from artist;

# 
select * from artist;

# 
select * from artist;

# 
select * from artist;

# 
select * from artist;

# INNER JOIN 
select * from artist;

# ENG
select * from artist;

# 
select * from artist;

# 
select * from artist;

# 1. Retrieve all musicians and their associated albums from the Artist table in the Chinook database.
select * from artist;

# 2. Retrieve each customer's first name, last name, invoice date, and payment information by joining the Customer and Invoice tables in the Chinook database.
select * from artist;

# 3. Retrieve each track's name and genre by joining the Track and Genre tables in the Chinook database.
select * from artist;

# 4. Retrieve each track's name and price on each invoice line by joining the InvoiceLine and Track tables in the Chinook database.
select * from artist;

# 5. Retrieve the list of tracks on each album by joining the Album and Track tables in the Chinook database.
select * from artist;

# 6. Retrieve each customer and the employee assigned to them by joining the Employee and Customer tables in the Chinook database.
select * from artist;

# 7. Retrieve each playlist's track list by joining the PlaylistTrack and Track tables in the Chinook database.
select * from artist;

# 8. Join the MediaType and Track tables in the Chinook database to retrieve the name of each track and its media type.
select * from artist;

# 9. Join the Customer and Invoice tables in the Chinook database to retrieve only the customer ID, first and last name (in one column, call it Fullname), and payment amounts of customers who have paid $10 or more.
select * from artist;

# 10. Retrieve each track (Track) and its corresponding album (Album) from the Chinook database.
select * from artist;

# 
select * from artist;

# JOINING MULTIPLE TABLES
select * from artist;

# 1. From the Invoice, Customer, and Employee tables in the Chinook database, display the customer name and the name of the employee who handled the invoice for each invoice.
select * from artist;

# 2. From the Track, Album, and Artist tables in the Chinook database, display the name of each track, its album, and the artist of the album.
select * from artist;

# 3. From the Track, Album, and Genre tables in the Chinook database, display the tracks by genre for each album.
select * from artist;

# 4. From the Track, PlaylistTrack, and Playlist tables in the Chinook database, display each track and which playlist it belongs to.
select * from artist;

# 5. From the Track, MediaType, and Genre tables in the Chinook database, display the name of each genre and media type.
select * from artist;

# 6. From the PlaylistTrack, Track, and MediaType tables in the Chinook database, display the tracks in each playlist and their media type.
select * from artist;

# 7. Display the album and artist of each track from the InvoiceLine, Track, Album, and Artist tables in the Chinook database.
select * from artist;

# 8. Calculate the total invoice amount for each country from the Invoice and Customer tables in the Chinook database.
select * from artist;

# 9. Calculate the number of tracks sold for each genre from the InvoiceLine, Track, and Genre tables in the Chinook database.
select * from artist;

# 10. Calculate the number of songs for each genre and their total playing time from the Chinook database.
select * from artist;

select * from artist;# 
