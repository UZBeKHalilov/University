# 1 - World databasedan foydalanib, har bir davlat turidagi (governmentForm) o'rtacha aholisini va o'rtacha umr ko'rishini tarzini (LifeExpentancy) yuqoriga yaxlitlab ko'rsating va aholi soni bo'yicha tartiblang
select GovernmentForm, ceil(avg(Population)) as Population, ceil(avg(LifeExpectancy)) as LifeExp from country group by GovernmentForm;

# 2 - World databasedan foydalanib, har bir qit'a (Continent) dagi o'rtacha aholi soni va o'rtacha umr ko'rish tarzini ko'rsating, ikkala sonni pastga qarab yaxlitlab ko'rsating va axoli soni bo'yicha tartiblang
# 3 - World databasedan foydalanib, har bir regiondagi davlatlarning o'rtacha egallagan maydonini ko'rsating (SurfaceArea) va o'rtacha Umr ko'rish tarzini ko'rsating (albatta Region uchun) va Life expentancy bo'yicha tartiblang
# 4 - World databasedan foydalanib, Harbir davlatdagi o'rtacha umr ko'rish tarzini va eng ko'p aholi soniga ega bo'lgan shaharini aholisi bilan ko'rsating. (Inner join)
# 5 - World databasedan foydalanib, har bir Industry bo'yicha umumiy Net Worth ni ko'rsating.
# 6 - World databasedan foydalanib, Eng katta daromadga ega 5ta kompaniyani qaysi davlatga tegililigini va o'sha davlatning o'rtacha aholisi va yashash tarzini ko'rsating, kompaniyani daromadini kamayish bo'yicha tartiblang
