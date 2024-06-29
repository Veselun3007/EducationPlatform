# Education platform

The microservice web application is built using C# ASP .Net and React TS

## Table of Contents
- [Introduction](#introduction)
    - [Overview](#overview)
    - [Opportunities](#opportunities)
    - [Authorization and roles](#authorization-and-roles)
- [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
- [Usage](#usage)
- [Testing](#testing)
- [AWS Resources](#aws-resources)
- [Gallery](#gallery)
- [Licence](#licence)

## Introduction

### Overview:

The service allows you to organize training in a distance format. TEACHERS can provide materials and assignments for Students, evaluate assignments completed by students, and write feedback on these assignments. STUDENTS have access to materials and can submit assignments. All users have access to group chats within a specific course.

### Opportunities:

- Creation and administration of courses.
- Course content administration.
- Group chats.
- Evaluation of submitted works.
- Support for viewing different types of files, including different types of images, videos and MS Office files

### Authorization and roles

USERS receive authorization tokens from the Cognito pool. User roles depend on their status in a particular course. There are three main roles:
- ADMIN - the creator of the course. Admins can perform the functions of a teacher and manage the roles within the course.
- TEACHER - a teacher can fill the course with content and evaluate students' works, without ability to change roles. 
- STUDENT - a student participating in the course.

## Getting Started

### Prerequisites

To run the project you need the following tools:
- aws sdk v2
- aws system manager
- aws cognito
- aws rds
- aws s3

The following tools are additionally required to run the tests:
- localstack
- aws local
- docker

### Installation:

1. Clone the repository.
2. Deploy the necessary AWS services.
3. Set up basic credentials.
4. Run service in development

## Usage

Project consists of several main parts:
- `EducationPlatform.Identity` is designed to process user data, 
registration, authentication and authorization.
- `CourseService` is intended for the implementation of the main ones operations with courses.
- `EducationPlatform.CourseContent` is intended for the implementation of the main ones operations with course content.
- `EducationPlatform.Chat` is intended to ensure communication between chat participants in real time.
- `EducationPlatform.StudentResult` is designed to record the success of the participants course
- `education-platform.client` - client application.

## Testing

- The project does not contain complex logic, so there are no unit tests.
- Integration tests are designed for only the core methods of the microservice administration of filling courses.

>[!NOTE]
>If you need to replace scripts, keep the following points in mind:
>- .sh files must have LF (Line Feed) line endings.
>- the file must be UTF-8 encoded without BOM. 
>```powershell
>### Check if file has UTF-8 BOM
>### If the first flag is true - you need to recode.
>(Get-Content file-name.sh -Encoding Byte -TotalCount 4) -contains 0xEF, 0xBB, 0xBF
>
>### Set file encoding to UTF-8 (use with caution)
>Set-Content -Path file-name.sh -Encoding UTF8 -Value (Get-Content file-name.sh)
>```

## AWS Resources

- System manager Parameter Store - for environment variables.
- S3 - for storing of user avatars, files of materials and tasks, files of submitted works.
- AWS Cognito - for user authentication and authorization.
- RDS - used for to store business data necessary for the operation of the platform.

## Gallery

<img alt="sign-in" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/427f1f30-0b13-4fa1-9be6-e1f7f89b9684" 
  width="47.5%"></img> 
<img alt="sign-in" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/db7568bd-6aca-4357-ae7a-e9323351e444" 
  width="47.5%"></img>
<img alt="main-course" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/03fa957c-6cb0-42ff-a0ee-92dd43995df3" 
  width="47.5%"></img> 
<img alt="main-in-course" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/f459e929-3261-4699-9fc4-32ac77e486eb" 
  width="47.5%"></img>
<img alt="form" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/c52c0358-6333-48d1-9a2d-f5614d23d5a9" 
  width="47.5%"></img> 
<img alt="chat" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/6139a20a-6932-49bc-95da-d495be3d0406" 
  width="47.5%"></img>

## Licence

GitHub Changelog Generator is released under the [MIT License](https://opensource.org/license/MIT).
