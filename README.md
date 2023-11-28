<h1 align="center">Quiz Blazor WASM ✅🕒🏆 </h1>

<p align = center>
by <a href="https://github.com/Cecilia-Coutinho">Cecilia Coutinho</a>
</p>

## 🌍 Overview

The development of this project was required by Chas Academy, and as such, it followed the specified requirements and deadline. 

This project involves the creation of an interactive quiz application. Users can create quizzes with varied components like titles, multimedia questions, set time limits for answering, and customizable answers. Each question possesses its link, enabling users to easily share specific questions among themselves.

The web app provides a dashboard to manage created quizzes and view participant scores. Additional features include quiz publishing and filters for organizing quizzes.

## 🚀 Features

✅ User Authentication: Implementation of user authentication via Identity.

✅ Intuitive Navigation: Integration of an intuitive and user-friendly navigation.

✅ Quiz Creation: Enable users to create quizzes.

✅ Quiz Components: Inclusion of a title, questions (text, images, videos), and optional time limits.

✅ Answer Types: Support for multiple-choice or free-text answers.

✅ Quiz Sharing: Quizzes have unique links for easy sharing.

✅ User Dashboard: Provision of a dashboard for users to create, view created quizzes and participants' scores.

Optional Features:

✅ Quiz Publishing: Ability for users to publish their quizzes.

✅Filter Functionality: Creation of filters to differentiate between published and unpublished quizzes.

## 💻 Technology Stack

* ASP.NET Core

* Blazor WebAssembly

* C#

* SQL Server Management Studio (SSMS)

* Entity Framework

* Visual Studio

* GitHub


## 📋 Additional Information

### SQL Design

The SQL design follows a relational database model, with tables representing entities such as Users, Media Type, Media File, Question, Answer, and Quiz Item. These tables handle tasks such as user authentication, media uploads, question creation, storing answers, and keeping track of participants' scores for quizzes. Relationships are established using foreign keys to maintain data integrity.

[IMAGE ER MODEL]
![ER Model](/BlazorQuizWASM.Client/wwwroot/images/image.jpg)


### Code Structure

This project is set up to work as a Single Page Application (SPA) using ASP.NET Core on the server side and Blazor on the client side. The structure keeps the code organized and makes it easier to work on different parts of the project.

On the server-side, tasks include managing user accounts, transferring SQL data, and handling client requests.

The logic is structured around four main controllers: MediaFiles, Questions, Answers and Quiz Items. There is also a Repository folder that handles the database context and the implementation of repository pattern.

![Code Structure 1](/BlazorQuizWASM/Client/wwwroot/images/code-structure_server-side.PNG)

The client-side manages the creation of an interactive and dynamic user interface while initiating requests to the server-side.

![Code Structure 2](/BlazorQuizWASM/Client/wwwroot/images/code-structure_client-side.PNG)

Additionally, there is a shared library used by both server and client sides. These components manage functionalities such as data transfer (DTO), services for API requests, and state management.

![Code Structure 3](/BlazorQuizWASM/Client/wwwroot/images/code-structure_shared.PNG)

#### Seed Data

Seed Data was implemented to provide a set of information that is automatically inserted into the database during application initialization or migration. This ensures a starting point for testing and application functionality. The implementation utilizes Entity Framework Core's ModelBuilder to seed data into MediaType entity.

```
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaType>().HasData(new[]
            {
                new MediaType{ MediaId = Guid.NewGuid(), Mediatype = "image"},
                new MediaType{ MediaId = Guid.NewGuid(), Mediatype = "video"}
            });
        }
```

### Conclusion

In conclusion, this project uses Blazor WASM to create an interactive Single Page Application (SPA). The design incorporates distinct server-side controllers, a repository pattern implementation, and shared components, ensuring efficient data handling and an engaging user experience.

 There are areas for potential improvements, such as refactoring to implement better DRY principles to avoid code redundancy. Additionally, enhancing the validation model would strengthen data integrity. On the client-side, further enhancements, such as hiding questions already answered by users and preventing users from seeing their uploaded questions during gameplay, were not part of the project requirements and were left unimplemented due to time constraints. These aspects present potential areas for future development to improve user experience and functionality.

