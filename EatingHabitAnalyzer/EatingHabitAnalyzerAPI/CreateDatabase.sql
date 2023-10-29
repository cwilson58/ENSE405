CREATE TABLE Foods 
(
	Barcode varchar(24) PRIMARY KEY,
	FoodName varchar(125) NOT NULL,
	ServingSizeInGrams int NOT NULL,
	CaloriesPerServing int NOT NULL,
	ProteinPerServing int NOT NULL,
	CarbsPerServing int NOT NULL,
	TotalSaturatedFatPerServing int NOT NULL,
	TotalUnSaturatedFatPerServing int NOT NULL,
	CholesterolPerServing int NOT NULL,
	SodiumPerServing int NOT NULL,
	CaffeinePerServing int NOT NULL,
	SugarPerServing int NOT NULL,
);

CREATE TABLE Users
(
	UserID int IDENTITY(1,1) PRIMARY KEY,
	Name varchar(50) NOT NULL,
	HeightInInches int NOT NULL,
	WeightInPounds decimal(5,2) NOT NULL,
	DateOfBirth datetime2 NOT NULL,
	GoalWeight decimal(5,2) NOT NULL,
	GoalDailyCalories int NOT NULL,
	Email varchar(50) NOT NULL,
	Pin varchar(4) NOT NULL,
); 

CREATE TABLE DiaryEntries
(
	EntryID int IDENTITY(1,1) PRIMARY KEY,
	UserID int FOREIGN KEY REFERENCES Users(UserID),
	EntryDateTime datetime2 NOT NULL,
	IsComplete bit NOT NULL,
);

CREATE TABLE Meals
(
	MealID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	EntryID int FOREIGN KEY REFERENCES DiaryEntries(EntryID),
	MealNumber tinyint NOT NULL,
);

CREATE TABLE MealFoods
(
	MealID int FOREIGN KEY REFERENCES Meals(MealID),
	Barcode varchar(24) FOREIGN KEY REFERENCES Foods(Barcode),
	NumberOfServings int,
	NumberOfGrams int NOT NULL,
);

CREATE TABLE FeelingSurveys
(
	SurveyID int IDENTITY(1,1) PRIMARY KEY,
	UserID int FOREIGN KEY REFERENCES Users(UserID),
	Blurb varchar(500),

);