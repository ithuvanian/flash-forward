use Flashcards
go

insert into users (Email, Password, IsAdmin, DisplayName)
	values ('admin@flashforward.co', 'admin', 1, 'Admin')

insert into cards (UserID, Front, Back)
	values (1, 'front of card', 'back of card')
insert into cards (UserID, Front, Back)
	values (1, 'sample question', 'sample answer')
insert into cards (UserID, Front, Back)
	values (1, 'What is the capital of Kansas?', 'Topeka')

insert into tags (TagName) values ('sample')
insert into tags (TagName) values ('geography')

insert into card_tag (TagID, CardID) values (1, 1)
insert into card_tag (TagID, CardID) values (1, 2)
insert into card_tag (TagID, CardID) values (1, 3)
insert into card_tag (TagID, CardID) values (2, 3)

select * from users;
select * from cards;
select * from tags;
select * from card_tag;