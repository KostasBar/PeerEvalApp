-- Inserting data into the Group table
INSERT INTO Groups (GroupName)
VALUES ('Admins');

-- Inserting data into the Question table
INSERT INTO Questions (Text)
VALUES ('Self-Confidence'),
       ('Dedication'),
       ('Job Knowledge'),
       ('Quality and Accuracy of Work'),
       ('Ability to Meet Deadlines'),
       ('Independence'),
       ('Commitment'),
       ('Attention to Detail'),
       ('Ability to Work with Others'),
       ( 'Communication Skills'),
       ( 'Performs Assigned Duties Under Pressure');

-- Inserting data into the User table
INSERT INTO Users (FirstName, LastName, Email, Password, Role, GroupId)
VALUES ('Admin', 'User', 'admin@example.com', '$2a$10$nGFvso1JF7oXU6u61Ky3o.XV7Y9q6MJgfKzVylBn5MkfbFYu8gndy', 'Admin', 1);