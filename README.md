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

### Overview

The service allows you to organize training in a distance format. TEACHERS can provide materials and assignments for Students, evaluate assignments completed by students, and write feedback on these assignments. STUDENTS have access to materials and can submit assignments. All users have access to group chats within a specific course.

### Opportunities

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

### Installation

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

```powershell
### Check if file has UTF-8 BOM
### If the first flag is true - you need to recode.
(Get-Content file-name.sh -Encoding Byte -TotalCount 4) -contains 0xEF, 0xBB, 0xBF

### Set file encoding to UTF-8 (use with caution)
Set-Content -Path file-name.sh -Encoding UTF8 -Value (Get-Content file-name.sh)
```

## AWS Resources

- System manager Parameter Store - for environment variables.
- S3 - for storing of user avatars, files of materials and tasks, files of submitted works.
- AWS Cognito - for user authentication and authorization.
- RDS - used for to store business data necessary for the operation of the platform.

## Gallery

<div align=center>   
    
   <img alt="index" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/c429b2bc-54a7-429f-9de2-f6d5cf9edfea" 
      height=24% width="47.5%"></img> <span><img width=2% /></span>
   <img alt="sign-in" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/89100e23-f3aa-4180-9211-f8a0eefb442f" 
      height=24% width="47.5%"></img>

   <img alt="main-course" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/11430263-4499-4b2a-84d1-f0bbed543d5c" 
      height=24% width="47.5%"></img> <span><img width=2% /></span>
   <img alt="main-in-course" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/fbb65e37-1300-4af4-aeab-69b1775a821d" 
      height=24% width="47.5%"></img>

   <img alt="form" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/cad22622-18d3-4cf9-83dc-7b3ea45a5f48" 
      height=24% width="47.5%"></img> <span><img width=2% /></span>
   <img alt="chat" src="https://github.com/Veselun3007/Education_Platform/assets/70714177/356057e9-cbab-4115-98df-a23d4cdaa1c9" 
      height=24% width="47.5%"></img>

</div>

## Licence

GitHub Changelog Generator is released under the [MIT License](https://opensource.org/license/MIT).
