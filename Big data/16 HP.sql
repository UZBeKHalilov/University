# LEFT JOIN (chinook database
# UZ

# 1.	Har bir trek va u tegishli bo‘lgan albomning nomini ko‘rsating, albomsiz treklarni ham qo‘shing.
select t.Name as Track, a.Title as album from track t left join album a on t.AlbumId = a.AlbumId;

# 2.	Har bir trek va uning janri nomini ko‘rsating, janrsiz treklarni ham qo‘shing.
select t.Name as track, g.Name as genre from track t left join genre g on g.GenreId = t.GenreId;

# 3.	Har bir mijoz va uning invoyslardagi umumiy xarajatini ko‘rsating, invoyssiz mijozlarni ham qo‘shing.
select c.CustomerId, c.FirstName, sum(i.Total) as total from customer c left join invoice i on i.CustomerId = c.CustomerId group by c.CustomerId;

# 4.	Har bir albom va uning treklar sonini ko‘rsating, trekka ega bo‘lmagan albomlarni ham qo‘shing.
select a.Title as album, sum(t.TrackId) as Sum_of_track from album a left join track t on a.AlbumId = t.AlbumId group by a.Title;

# 5.	Har bir janr va u kiritilgan treklarning umumiy narxini ko‘rsating, trekka ega bo‘lmagan janrlarni ham qo‘shing.
select g.Name as genre, sum(t.UnitPrice) as Track_Sum_Price
from genre g left join track t on g.GenreId = t.GenreId
group by g.Name;

# 6.	Har bir mijoz va uning eng qimmat invoysini ko‘rsating, invoyssiz mijozlarni ham qo‘shing.
select c.CustomerId, c.FirstName, max(i.Total) as Max_total
from customer c left join invoice i
on c.CustomerId = i.CustomerId
group by c.CustomerId;

# 7.	Har bir mijozning ismi va oxirgi invoysning sanasini ko‘rsating, invoyssiz mijozlarni ham qo‘shing.
select * from track join a on b;

# 8.	Har bir trekning nomi va uning invoyslardagi umumiy sotilgan miqdorini ko‘rsating, sotilmagan treklarni ham qo‘shing.
select * from track join a on b;

# 9.	Har bir invoysning ismi va u ichidagi treklar sonini ko‘rsating, trekka ega bo‘lmagan invoyslarni ham qo‘shing.
select * from track join a on b;

# 10.	Har bir sotuvchining nomi va uning xizmat qilgan mijozlar sonini ko‘rsating, mijozsiz sotuvchilarni ham qo‘shing.
select * from track join a on b;

# LEFT JOIN (chinook database)
# ENG

# 1. Show each track and the name of the album it belongs to, including tracks without an album.
select * from track join a on b;

# 2. Show each track and its genre name, including tracks without a genre.
select * from track join a on b;

# 3. Show each customer and their total invoice cost, including customers without an invoice.
select * from track join a on b;

# 4. Show each album and its number of tracks, including albums without a track.
select * from track join a on b;

# 5. Show each genre and the total cost of the tracks it includes, including genres without a track.
select * from track join a on b;

# 6. Show each customer and their most expensive invoice, including customers without an invoice.
select * from track join a on b;

# 7. Show each customer name and the date of their last invoice, including customers without an invoice.
select * from track join a on b;

# 8. Show the name of each track and the total amount sold on the invoices, including unsold tracks.
select * from track join a on b;

# 9. Show the name of each invoice and the number of tracks in it, including invoices that do not have a track.
select * from track join a on b;

# 10. Show the name of each salesperson and the number of customers they have served, including salespeople without customers.
select * from track join a on b;