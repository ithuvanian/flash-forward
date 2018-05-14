This is a C# .NET MVC application that can be run from MS Visual Studio with IIS. Database queries have been prepared for SQL Server. First, create a database titled "HotelFlashcards", then run "db/schema.sql" and "db/data.sql".

# Final Capstone

## What Projects Are Included w/Repository

- **Capstone.Web** - ASP.NET MVC Project
- **Capstone.Web.Tests** - Unit Tests and Integration Tests

## Database Folder

The database folder contains two files: `schema.sql` and `data.sql`.

- `schema.sql` should contain all of your `CREATE` statements should the database ever need to be rebuilt.
- `data.sql` should contain all of your `INSERT` seed data that is necessary to initially using of the database.

## NuGet Packages Installed
 
- **Capstone.Web**
    - Ninject
    - Ninject.MVC5
    - jQuery
    - jQuery.Validation
    - jQuery.Validation.Unobtrusive
    - Bootstrap

**## User Stories**
---
- Register
	- As an anonymous guest, I can register for a new account on the website.

---
- Login
	- As a registered user, I can login to the website using my email address and password.

---
- Search Flashcards
	- As a user I can search for cards (by tag/keyword) to add to my deck so that I can add them to my deck for a study session.

---
- Create Flashcard
	- As a user, I can create a new flash card that is text-based so that it can be added to a deck for studying.
	- A flashcard has the following properties:
		- front / back (e.g. question / answer)
		- tags/keywords for searching
		- user that created it

---
- Modify Flashcard
	- As a user, I can edit a flash card that I have created.
	- Editable properties include:
		- front / back
		- tags

---
- View Flashcard Decks
	- As a user, I can view all of the flash card decks that I have created.

---
- Create Flashcard Deck
	- As a user, I can create a new flash card deck so that I can use it in my study sessions.

---
- Modify Deck
	- As a user, I can modify one of the existing flashcard decks that I have created.
	- A modification includes:
		- adding new flash cards
		- removing existing flash cards
		- changing the name and description of the deck

---
- Begin Study Session
	- As a user, I can begin a study session. A study session requires the user to select a deck that they wish to study first.

---
- View Card During Study Session
	- As a user, I need to be able to see the flash card during a study session. A flashcard can be "flipped" so that I can see the front or the back, but never both at the same time.

---
- Mark Right/Wrong
	- As a user, I need to be able to mark a flashcard right or wrong during a study session so that I can see my final progress at the end.

---
- Complete Study Session 
	- As a user, I need to be able mark a study session "complete" either by stopping in the middle or reaching the end of the deck. The final screen shows me how many right/wrong questions I had.

---
- Search for Admin Cards
	- As a user, I can include cards created/owned by the administrator so that they can be added to my Flash Card deck.

