# LMS
readme in progress.. as well the system itself

## Table of contents
* [Introduction](#introduction)
* [Technologies](#technologies)
* [Features](#features)
* [Coded mechanisms](#coded-mechanisms)
* [Status](#the-project-status)

## Introduction
Web application without a specified name, created as part of an engineering thesis using Razor Pages model offered by ASP.NET Framework.
It is learning management system with implemented specific for this type of system functionalities. 
The user interface is minimalistic and quickly created with bootstrap library, css selectors and js functions.
System has three main user types: Student, Teacher, Admin:
- Student is able to participate in courses and submitting responses to activities
- Teacher is responsible for managing the course they have created, evaluation of users responses
- Admin is responsible for creating user accounts and has permission to perform any actions.

## Technologies
- C#, .NET 6.0, ASP.NET 6.0 Razor Pages
- MSSQL
- Bootstrap 5
- JQuery
- Microsoft Visual Studio

##Features
- Easy course management in form of adding new activities
- Three possible activities to use: Information, Quiz, Task
- Possibility of including files to your activities, in case of student also to theirs responses
- Easily access to student reponses in certain activities and simple grading system

### To Do
- Improve UI interface
- Add more activities
- Expansion of the evaluating system
- Fix multiple choice question type and add more question types

## Coded mechanisms
- CRUD for basic entities
- data validation for every form
- database seeder filling database with random data for testing
- authorization based on role
- authorization to resources based on user identifier
- user interface created by bootstrap classes
- asynchronous searching
- using external text editor
- evaluation of students activities
- grouping users
- creating users account by administrator - created account credentials are sent via e-mail
- 3 main types of activities that can be added to course: information, task, quiz
- quizes offer to add 2 different types of questions
- course participants can respond to activities and their work can be evaluated by course owner
- sending files to server and downloading from
- panel with incoming tasks
- enrollment to courses can be protected by password

## The project status
There is still much work to do in order to use this system fully and seamlessly. Thankfully it is 
easily scalable and allows to add quickly new functionalities. Soon I'm going to develop this project.
When i learn react the ui will be rebuilt, there will be new activities, better evalutation system and more.. stay tuned.
