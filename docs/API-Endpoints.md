### Contents:
- [/api/User](#apiuser)
  - [/Login](#login)
  - [/Register](#register)
  - [/GetUser](#getuser)
  - [/ProfilePicture](#profilepicture)
  - [Get User By Id](#get-user-by-id)
  - [Update User By Id](#update-user-by-id)
  - [Delete User By Id](#delete-user-by-id)
- [/api/Role](#apirole)
  - [Create Role](#create-role)
  - [Get Role By Id](#get-role-by-id)
  - [Update Role By Id](#update-role-by-id)
  - [Delete Role By Id](#delete-role-by-id)
- [/api/Feed](#apifeed)
  - [/GetPosts](#getposts)
  - [/GetUserPosts](#getuserposts)
- [/api/Post](#apipost)
  - [Create Post](#create-post)
  - [Get Post By Id](#get-post-by-id)
  - [Update Post By Id](#update-post-by-id)
  - [Delete Post By Id](#delete-post-by-id)
- [/api/Comment](#apicomment)
  - [Create Comment](#create-comment)
  - [Get Comment By Id](#get-comment-by-id)
  - [Update Comment By Id](#update-comment-by-id)
  - [Delete Comment By Id](#delete-comment-by-id)
- [/api/Language](#apilanguage)
  - [/GetLanguages](#getlanguages)
  - [Create Language](#create-language)
  - [Get Language By Id](#get-language-by-id)
  - [Update Language By Id](#update-language-by-id)
  - [Delete Language By Id](#delete-language-by-id)
- [/api/Technology](#apitechnology)
  - [/GetTechnologies](#gettechnologies)
  - [Create Technology](#create-technology)
  - [Get Technology By Id](#get-technology-by-id)
  - [Update Technology By Id](#update-technology-by-id)
  - [Delete Technology By Id](#delete-technology-by-id)

***

# /api/User

## /Login

|||
|---|---|
|Description|Get a JWT for an existing user|
|Type|POST|
|URL structure|`http://localhost:5000/api/User/login`|
|Authentication|None|
|Body|JSON|
|Returns|JSON; The JWT|
|||

Sample body:
```json
{
  "userName": "string",
  "password": "string"
}
```
Sample response:
```json
{
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJRCI6IjFiMTU2ZTY3LTVhZmItNGZmMi1hYzRmLTY4NjVmZWI5NzFiYiIsIlVzZXJuYW1lIjoieW9yZ3VzIiwicm9sZSI6IlVzZXIiLCJuYmYiOjE2MTIzMzg5NzIsImV4cCI6MTYxMjkwODAwMCwiaWF0IjoxNjEyMzM4OTcyfQ.dneQidggMu9FD7UXBzn5td3phX3OIgp7y4BygHTqq5Un5D67xH1jZTRQpi9Zqcq76mODvUToAo7j4PFdJtIdtg"
}
```

## /Register

|||
|---|---|
|Description|Add an account to the database and get a JWT|
|Type|POST|
|URL structure|`http://localhost:5000/api/User/register`|
|Authentication|None|
|Body|JSON|
|Returns|JSON; The JWT|

Sample body:
```json
{
  "userName": "string",
  "email": "user@example.com",
  "firstName": "string",
  "lastName": "string",
  "password": "string"
}
```
Sample response:
```json
{
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJRCI6IjFiMTU2ZTY3LTVhZmItNGZmMi1hYzRmLTY4NjVmZWI5NzFiYiIsIlVzZXJuYW1lIjoieW9yZ3VzIiwicm9sZSI6IlVzZXIiLCJuYmYiOjE2MTIzMzg5NzIsImV4cCI6MTYxMjkwODAwMCwiaWF0IjoxNjEyMzM4OTcyfQ.dneQidggMu9FD7UXBzn5td3phX3OIgp7y4BygHTqq5Un5D67xH1jZTRQpi9Zqcq76mODvUToAo7j4PFdJtIdtg"
}
```

## /GetUser

|||
|---|---|
|Description|Get a user via his UserName|
|Type|GET|
|URL structure|`http://localhost:5000/api/User/GetUser?UserName=test`|
|Authentication|None|
|Body|None|
|Returns|JSON; The object of the user|
|||

Sample response:
```json
{
  "profilePictureURL": "https://avatars.githubusercontent.com/u/75525529?s=60&v=4",
  "roles": [
    {
      "name": "User"
    }
  ],
  "friends": [
  ],
  "languages": [
    {
      "id": "cf40034d-75d7-4792-821d-c16220a5b928"
    }
  ],
  "technologies": [
    {
      "id": "907421d9-1b60-411b-b780-85e65a004b56"
    }
  ],
  "posts": [
    {
      "id": "850d0655-72cb-4477-b69b-35e8645db266"
    }
  ],
  "userName": "test",
  "email": "test@bg.com",
  "firstName": "Test",
  "lastName": "Test"
}
```

## /ProfilePicture

|||
|---|---|
|Description|Update the profile picture of the given User|
|Type|PUT|
|URL structure|`http://localhost:5000/api/User/ProfilePicture?UserId=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 2](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|Multipart Form|
|Returns|JSON; The link to the uploaded profile picture|
|||

Sample body:
|Name|Value|
|---|---|
|Picture|`new-profile-picture.png` (this is the actual file)|

Sample response:
```json
{
  "profilePictureURL": "https://avatars.githubusercontent.com/u/75525529?s=60&v=4"
}
```

## Get User By Id

|||
|---|---|
|Description|Get a user via his Id|
|Type|GET|
|URL structure|`http://localhost:5000/api/User?Id=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 2](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|None|
|Returns|JSON; The object of the user|
|||

Sample response:
```json
{
  "profilePictureURL": "https://avatars.githubusercontent.com/u/75525529?s=60&v=4",
  "roles": [
    {
      "name": "User"
    }
  ],
  "friends": [
  ],
  "languages": [
    {
      "id": "cf40034d-75d7-4792-821d-c16220a5b928"
    }
  ],
  "technologies": [
    {
      "id": "907421d9-1b60-411b-b780-85e65a004b56"
    }
  ],
  "posts": [
    {
      "id": "850d0655-72cb-4477-b69b-35e8645db266"
    }
  ],
  "userName": "test",
  "email": "test@bg.com",
  "firstName": "Test",
  "lastName": "Test"
}
```

## Update User By Id

|||
|---|---|
|Description|Modify the values in an existing user (account)|
|Type|PUT|
|URL structure|`http://localhost:5000/api/User?Id=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 2](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|JSON|
|Returns|JSON; The updated user object|
|||

Sample body:
```json
{
  "userName": "string",
  "email": "user@example.com",
  "firstName": "string",
  "lastName": "string",
  "password": "string",
  "friends": [
    {
      "userName": "string"
    }
  ],
  "roles": [
    {
      "name": "string"
    }
  ],
  "languages": [
    {
      "name": "string"
    }
  ],
  "technologies": [
    {
      "name": "string"
    }
  ]
}
```
Sample response:
```json
{
  "profilePictureURL": "https://avatars.githubusercontent.com/u/75525529?s=60&v=4",
  "roles": [
    {
      "name": "User"
    }
  ],
  "friends": [],
  "languages": [
    {
      "id": "33397a3b-46eb-424f-8e19-d88dbf3e953b"
    },
    {
      "id": "cea85a74-4820-42ff-b64f-61d7e9bfc696"
    }
  ],
  "technologies": [
    {
      "id": "907421d9-1b60-411b-b780-85e65a004b56"
    }
  ],
  "posts": [],
  "userName": "test",
  "email": "test1@bg.com",
  "firstName": "Tester",
  "lastName": "Tester"
}
```

## Delete User By Id

|||
|---|---|
|Description|Delete an existing user from the database `WARNING: THIS IS IRREVERSIBLE`|
|Type|DELETE|
|URL structure|`http://localhost:5000/api/User?Id=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 2](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|None|
|Returns|Nothing|
|||

***

# /api/Role

## Create Role

|||
|---|---|
|Description|Add a new Role to the DataBase|
|Type|POST|
|URL structure|`http://localhost:5000/api/Role`|
|Authentication|Bearer Token (JWT), [Authorization Type 3](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|JSON|
|Returns|JSON; The result role object, only with the role Id|
|||

Sample body:
```json
{
  "name": "string"
}
```
Sample response:
```json
{
  "id": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39"
}
```

## Get Role By Id

|||
|---|---|
|Description|Get an existing Role via it's Id|
|Type|GET|
|URL structure|`http://localhost:5000/api/Role?Id=1cc9773f-8d9a-4bfd-83ca-2099dc787a39`|
|Authentication|Bearer Token (JWT), [Authorization Type 1](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|None|
|Returns|JSON; The role object, only with it's name|
|||

Sample response:
```json
{
  "name": "Test"
}
```

## Update Role By Id

|||
|---|---|
|Description|Modify the values (name) of an existing role|
|Type|PUT|
|URL structure|`http://localhost:5000/api/Role?Id=1cc9773f-8d9a-4bfd-83ca-2099dc787a39`|
|Authentication|Bearer Token (JWT), [Authorization Type 3](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|JSON|
|Returns|Nothing|
|||

Sample body:
```json
{
  "name": "string"
}
```

## Delete Role By Id

|||
|---|---|
|Description|Remove an existing Role from the DataBase|
|Type|POST|
|URL structure|`http://localhost:5000/api/Role?Id=1cc9773f-8d9a-4bfd-83ca-2099dc787a39`|
|Authentication|Bearer Token (JWT), [Authorization Type 3](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|None|
|Returns|Nothing|
|||

***

# /api/Feed

## /GetPosts

|||
|---|---|
|Description|Get a certain amount of the latest posts of a User's friends|
|Type|POST|
|URL structure|`http://localhost:5000/api/Feed/GetPosts?UserId=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 1](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|JSON|
|Returns|JSON; An array with the selected posts|
|||

Sample body:
```json
{
  "pageNumber": 1,
  "firstPageTimeIssued": "2022-01-30T18:43:01.082Z",
  "pageSize": 5
}
```
Sample response:
```json
{
  "posts": [
    {
      "postId": "850d0655-72cb-4477-b69b-35e8645db266",
      "creatorFirstName": "test",
      "creatorLastName": "Test",
      "creatorUsername": "Test",
      "message": "A sample post",
      "timeCreated": "2021-02-03T10:52:38.271647",
      "comments": [],
      "fileUrls": []
    }
  ]
}
```

## /GetUserPosts

|||
|---|---|
|Description|Get a certain amount of the latest posts from a User|
|Type|POST|
|URL structure|`http://localhost:5000/api/GetUserPosts?UserName=test`|
|Authentication|None|
|Body|JSON|
|Returns|JSON; An array with the selected posts|
|||

Sample body:
```json
{
  "pageNumber": 1,
  "firstPageTimeIssued": "2022-01-30T18:43:01.082Z",
  "pageSize": 5
}
```
Sample response:
```json
{
  "posts": [
    {
      "postId": "850d0655-72cb-4477-b69b-35e8645db266",
      "creatorFirstName": "test",
      "creatorLastName": "Test",
      "creatorUsername": "Test",
      "message": "A sample post",
      "timeCreated": "2021-02-03T10:52:38.271647",
      "comments": [],
      "fileUrls": []
    }
  ]
}
```

***

# /api/Post

## Create Post

|||
|---|---|
|Description|Add a new Post to the DataBase|
|Type|POST|
|URL structure|`http://localhost:5000/api/Post?UserId=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 1](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|Multipart Form|
|Returns|JSON; The result Post object, only with the Post Id|
|||

Sample body:
|Name|Value|
|---|---|
|Message|The message of my post|
|Files|`attachment.txt` (that is the actual file)|
|Files|`attachment2.txt` (that is the actual file)|
|...||

Sample response:
```json
{
  "id": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39"
}
```

## Get Post By Id

|||
|---|---|
|Description|Get an existing Post from the DataBase|
|Type|GET|
|URL structure|`http://localhost:5000/api/Post?Id=1cc9773f-8d9a-4bfd-83ca-2099dc787a39`|
|Authentication|None|
|Body|None|
|Returns|JSON; The result Post object|
|||

Sample response:
```json
{
  "postId": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39",
  "creatorFirstName": "Test",
  "creatorLastName": "Test",
  "creatorUsername": "test",
  "message": "A sample post",
  "timeCreated": "2021-02-02T18:29:31.942772",
  "comments": [],
  "fileUrls": []
}
```

## Update Post By Id

|||
|---|---|
|Description|Update the values of an existing post|
|Type|PUT|
|URL structure|`http://localhost:5000/api/Post?UserId=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 2](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|Multipart Form|
|Returns|JSON; The result Post object, only with the Post Id|
|||

**Note:** When editing a post's files, they all get replaced, you cannot just add new files. After post is edited, it's "timeCreated" get's updated.

Sample body:
|Name|Value|
|---|---|
|PostId|1cc9773f-8d9a-4bfd-83ca-2099dc787a39|
|NewMessage|The new message of the post|
|Files|`attachment3.txt` (that is the actual file)|
|Files|`attachment4.txt` (that is the actual file)|
|...|

Sample response:
```json
{
  "id": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39"
}
```

## Delete Post By Id

|||
|---|---|
|Description|Remove an existing Post from the DataBase|
|Type|DELETE|
|URL structure|`http://localhost:5000/api/Post?Id=1cc9773f-8d9a-4bfd-83ca-2099dc787a39`|
|Authentication|Bearer Token (JWT), [Authorization Type 2](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|None|
|Returns|None|
|||

***

# /api/Comment

## Add comment

|||
|---|---|
|Description|Add a new Comment to an existing Post|
|Type|POST|
|URL structure|`http://localhost:5000/api/Comment?UserId=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 1](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|JSON|
|Returns|JSON; The result Comment object, only with the Comment Id|
|||

Sample body:
```json
{
  "postId": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39",
  "message": "First comment"
}
```
Sample response:
```json
{
  "id": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39"
}
```

## Get Comment By Id

|||
|---|---|
|Description|Get an existing Comment from the DataBase|
|Type|GET|
|URL structure|`http://localhost:5000/api/Comment?Id=1cc9773f-8d9a-4bfd-83ca-2099dc787a39`|
|Authentication|None|
|Body|None|
|Returns|JSON; The result Comment object|
|||

Sample response:
```json
{
  "commentId": "086d1a23-c977-4cdc-9bdf-dc81992b3a12",
  "postId": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39",
  "issuerFirstName": "Test",
  "issuerLastName": "Test",
  "issuerUsername": "test",
  "message": "First coment",
  "timeCreated": "2021-02-01T13:18:57.434512"
}
```

## Update Comment By Id

|||
|---|---|
|Description|Update the values of an existing comment|
|Type|PUT|
|URL structure|`http://localhost:5000/api/Comment?UserId=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 2](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|JSON|
|Returns|JSON; The result Comment object, only with the Comment Id|
|||

Sample body:
```json
{
  "commentId": "086d1a23-c977-4cdc-9bdf-dc81992b3a12",
  "postId": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39",
  "newMessage": "string"
}
```
Sample response:
```json
{
  "id": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39"
}
```

## Delete Comment By Id

|||
|---|---|
|Description|Remove an existing Comment from the DataBase|
|Type|DELETE|
|URL structure|`http://localhost:5000/api/Comment?Id=086d1a23-c977-4cdc-9bdf-dc81992b3a12`|
|Authentication|Bearer Token (JWT), [Authorization Type 2](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|None|
|Returns|None|
|||

***

# /api/Language

## /GetLanguages

|||
|---|---|
|Description|Get all available Languages from the DataBase|
|Type|GET|
|URL structure|`http://localhost:5000/api/Language/GetLanguages`|
|Authentication|Bearer Token (JWT), [Authorization Type 1](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|None|
|Returns|JSON; The result Language array object|
|||

Sample response:
```json
[
  {
    "id": "5286821a-1407-4daf-ac3e-49153c2d3f66",
    "name": "CSharp"
  },
  {
    "id": "cea85a74-4820-42ff-b64f-61d7e9bfc696",
    "name": "Perl"
  },
  {
    "id": "6dc1cb1a-1c4f-41af-8b44-86441cb60136",
    "name": "Java"
  }
]
```

## Create Language

|||
|---|---|
|Description|Add a new Language in the DataBase|
|Type|POST|
|URL structure|`http://localhost:5000/api/Comment`|
|Authentication|Bearer Token (JWT), [Authorization Type 3](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|JSON|
|Returns|JSON; The result Comment object, only with the Comment Id|
|||

Sample body:
```json
{
  "name": "Perl"
}
```
Sample response:
```json
{
  "id": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39"
}
```

## Get Language By Id

|||
|---|---|
|Description|Get an existing Language from the DataBase|
|Type|GET|
|URL structure|`http://localhost:5000/api/Language?Id=1cc9773f-8d9a-4bfd-83ca-2099dc787a39`|
|Authentication|None|
|Body|None|
|Returns|JSON; The result Language object|
|||

Sample response:
```json
{
  "id": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39",
  "name": "Perl"
}
```

## Update Language By Id

|||
|---|---|
|Description|Update the values of an existing Language|
|Type|PUT|
|URL structure|`http://localhost:5000/api/Language?Id=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 3](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|JSON|
|Returns|None|
|||

Sample body:
```json
{
  "name": "string"
}
```

## Delete Language By Id

|||
|---|---|
|Description|Remove an existing Language from the DataBase|
|Type|DELETE|
|URL structure|`http://localhost:5000/api/Language?Id=086d1a23-c977-4cdc-9bdf-dc81992b3a12`|
|Authentication|Bearer Token (JWT), [Authorization Type 3](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|None|
|Returns|None|
|||

***

# /api/Technology

## /GetTechnologies

|||
|---|---|
|Description|Get all available Technologies from the DataBase|
|Type|GET|
|URL structure|`http://localhost:5000/api/Language/GetTechnologies`|
|Authentication|Bearer Token (JWT), [Authorization Type 1](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|None|
|Returns|JSON; The result Technology array object|
|||

Sample response:
```json
[
  {
    "id": "5286821a-1407-4daf-ac3e-49153c2d3f66",
    "name": "ASP.NET"
  },
  {
    "id": "cea85a74-4820-42ff-b64f-61d7e9bfc696",
    "name": "Angular"
  }
]
```

## Create Technology

|||
|---|---|
|Description|Add a new Technology in the DataBase|
|Type|POST|
|URL structure|`http://localhost:5000/api/Technology`|
|Authentication|Bearer Token (JWT), [Authorization Type 3](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|JSON|
|Returns|JSON; The result Technology object, only with the Technology Id|
|||

Sample body:
```json
{
  "name": "Angular"
}
```
Sample response:
```json
{
  "id": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39"
}
```

## Get Technology By Id

|||
|---|---|
|Description|Get an existing Technology from the DataBase|
|Type|GET|
|URL structure|`http://localhost:5000/api/Technology?Id=1cc9773f-8d9a-4bfd-83ca-2099dc787a39`|
|Authentication|None|
|Body|None|
|Returns|JSON; The result Technology object|
|||

Sample response:
```json
{
  "id": "1cc9773f-8d9a-4bfd-83ca-2099dc787a39",
  "name": "Angular"
}
```

## Update Technology By Id

|||
|---|---|
|Description|Update the values of an existing Technology|
|Type|PUT|
|URL structure|`http://localhost:5000/api/Technology?Id=27e203bd-5312-4831-9334-cd3c20e5d672`|
|Authentication|Bearer Token (JWT), [Authorization Type 3](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|JSON|
|Returns|None|
|||

Sample body:
```json
{
  "name": "string"
}
```

## Delete Technology By Id

|||
|---|---|
|Description|Remove an existing Technology from the DataBase|
|Type|DELETE|
|URL structure|`http://localhost:5000/api/Technology?Id=086d1a23-c977-4cdc-9bdf-dc81992b3a12`|
|Authentication|Bearer Token (JWT), [Authorization Type 3](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication#token-validation)|
|Body|None|
|Returns|None|
|||