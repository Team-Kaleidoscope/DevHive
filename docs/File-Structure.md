DevHive has a lot of files and it can be hard to really see the whole structure of everything from a first glance. In this document, all of the important/notable files and folders are represented in a tree structure.

### Contents:
- [Common](#common)
- [Data](#data)
- [Service](#service)
- [Tests](#tests)
- [Web](#web)
- [Full Overview](#full-overview)

# Common

```
└── DevHive.Common
    └── Models
        ├── Identity
        │   └── TokenModel.cs
        └── Misc
            ├── IdModel.cs
            ├── PasswordModifications.cs
            └── Patch.cs
```

# Data

```
└── DevHive.Data
    ├── ConnectionString.json
    ├── DevHiveContext.cs
    ├── DevHiveContextFactory.cs
    ├── Interfaces
    |   ├── Models
    |   │   ├── IComment.cs
    │   |   ├── ILanguage.cs
    │   |   ├── IModel.cs
    │   |   ├── IPost.cs
    │   |   ├── IProfilePicture.cs
    │   |   ├── IRating.cs
    │   |   ├── IRole.cs
    │   |   ├── ITechnology.cs
    │   |   └── IUser.cs
    |   └── Repositories
    |       ├── ICommentRepository.cs
    |       ├── IFeedRepository.cs
    |       ├── ILanguageRepository.cs
    |       ├── IPostRepository.cs
    |       ├── IRatingRepository.cs
    |       ├── IRepository.cs
    |       ├── IRoleRepository.cs
    |       ├── ITechnologyRepository.cs
    |       └── IUserRepository.cs
    ├── Migrations
    ├── Models
    │   ├── Comment.cs
    │   ├── Language.cs
    │   ├── Post.cs
    │   ├── ProfilePicture.cs
    │   ├── Rating.cs
    │   ├── Role.cs
    │   ├── Technology.cs
    │   └── User.cs
    ├── RelationModels
    │   ├── RatedPost.cs
    │   ├── UserFriend.cs
    │   └── UserRate.cs
    └── Repositories
        ├── BaseRepository.cs
        ├── CommentRepository.cs
        ├── FeedRepository.cs
        ├── LanguageRepository.cs
        ├── PostRepository.cs
        ├── RatingRepository.cs
        ├── RoleRepository.cs
        ├── TechnologyRepository.cs
        └── UserRepository.cs
```

# Service

```
└── DevHive.Services
    ├── Configurations
    │   └── Mapping
    |       ├── CommentMappings.cs
    |       ├── FeedMappings.cs
    |       ├── LanguageMappings.cs
    |       ├── PostMappings.cs
    |       ├── RatingMappings.cs
    |       ├── RoleMapings.cs
    |       ├── TechnologyMappings.cs
    |       └── UserMappings.cs
    ├── Interfaces
    │   ├── ICloudService.cs
    │   ├── ICommentService.cs
    │   ├── IFeedService.cs
    │   ├── ILanguageService.cs
    │   ├── IPostService.cs
    │   ├── IRateService.cs
    │   ├── IRoleService.cs
    │   ├── ITechnologyService.cs
    │   └── IUserService.cs
    ├── Models
    |   ├── Cloud
    |   │   └── CloudinaryService.cs
    |   ├── Comment
    |   │   ├── CreateCommentServiceModel.cs
    |   │   ├── ReadCommentServiceModel.cs
    |   │   └── UpdateCommentServiceModel.cs
    |   ├── Feed
    |   │   ├── GetPageServiceModel.cs
    |   │   └── ReadPageServiceModel.cs
    |   ├── Identity
    |   │   ├── Role
    |   │   │   ├── CreateRoleServiceModel.cs
    |   │   │   ├── RoleServiceModel.cs
    |   │   │   └── UpdateRoleServiceModel.cs
    |   │   └── User
    |   │       ├── BaseUserServiceModel.cs
    |   │       ├── FriendServiceModel.cs
    |   │       ├── LoginServiceModel.cs
    |   │       ├── ProfilePictureServiceModel.cs
    |   │       ├── RegisterServiceModel.cs
    |   │       ├── UpdateFriendServiceModel.cs
    |   │       ├── UpdateProfilePictureServiceModel.cs
    |   │       ├── UpdateUserServiceModel.cs
    |   │       └── UserServiceModel.cs
    |   ├── Language
    |   │   ├── CreateLanguageServiceModel.cs
    |   │   ├── LanguageServiceModel.cs
    |   │   ├── ReadLanguageServiceModel.cs
    |   │   └── UpdateLanguageServiceModel.cs
    |   ├── Post
    |   |   ├── Rating
    |   |   │   ├── RatePostServiceModel.cs
    |   |   │   └── ReadPostRatingServiceModel.cs
    |   │   ├── CreatePostServiceModel.cs
    |   │   ├── ReadPostServiceModel.cs
    |   │   └── UpdatePostServiceModel.cs
    |   └── Technology
    |       ├── CreateTechnologyServiceModel.cs
    |       ├── ReadTechnologyServiceModel.cs
    |       ├── TechnologyServiceModel.cs
    |       └── UpdateTechnologyServiceModel.cs
    ├── Options
    │   └── JWTOptions.cs
    └── Services
        ├── CommentService.cs
        ├── FeedService.cs
        ├── LanguageService.cs
        ├── PostService.cs
        ├── RateService.cs
        ├── RoleService.cs
        ├── TechnologyService.cs
        └── UserService.cs
```

# Tests

```
└── DevHive.Tests
    ├── DevHive.Data.Tests
    │   ├── CommentRepository.Tests.cs
    │   ├── FeedRepository.Tests.cs
    │   ├── LenguageRepository.Tests.cs
    │   ├── PostRepository.Tests.cs
    │   ├── RoleRepository.Tests.cs
    │   ├── TechnologyRepository.Tests.cs
    │   └── UserRepositoryTests.cs
    ├── DevHive.Services.Tests
    │   ├── FeedService.Tests.cs
    │   ├── LanguageService.Tests.cs
    │   ├── PostService.Tests.cs
    │   ├── RoleService.Tests.cs
    │   ├── TechnologyServices.Tests.cs
    │   └── UserService.Tests.cs
    └── DevHive.Web.Tests
        ├── LanguageController.Tests.cs
        └── TechnologyController.Tests.cs
```

# Web

```
└── DevHive.Web
    ├── appsettings.json
    ├── Attributes
    │   ├── GoodPasswordModelValidation.cs
    │   └── OnlyLettersModelValidation.cs
    ├── Configurations
    |   ├── Extensions
    |   │   ├── ConfigureAutoMapper.cs
    |   │   ├── ConfigureDatabase.cs
    |   │   ├── ConfigureDependencyInjection.cs
    |   │   ├── ConfigureExceptionHandlerMiddleware.cs
    |   │   ├── ConfigureJWT.cs
    |   │   └── ConfigureSwagger.cs
    |   └── Mapping
    |       ├── CommentMappings.cs
    |       ├── FeedMappings.cs
    |       ├── LanguageMappings.cs
    |       ├── PostMappings.cs
    |       ├── RatingMappings.cs
    |       ├── RoleMappings.cs
    |       ├── TechnologyMappings.cs
    |       └── UserMappings.cs
    ├── Controllers
    │   ├── CommentController.cs
    │   ├── FeedController.cs
    │   ├── LanguageController.cs
    │   ├── PostController.cs
    │   ├── RateController.cs
    │   ├── RoleController.cs
    │   ├── TechnologyController.cs
    │   └── UserController.cs
    ├── Middleware
    │   └── ExceptionMiddleware.cs
    ├── Models
    |   ├── Comment
    |   │   ├── CreateCommentWebModel.cs
    |   │   ├── ReadCommentWebModel.cs
    |   │   └── UpdateCommentWebModel.cs
    |   ├── Feed
    |   │   ├── GetPageWebModel.cs
    |   │   └── ReadPageWebModel.cs
    |   ├── Identity
    |   │   ├── Role
    |   │   │   ├── CreateRoleWebModel.cs
    |   │   │   ├── RoleWebModel.cs
    |   │   │   └── UpdateRoleWebModel.cs
    |   │   └── User
    |   │       ├── BaseUserWebModel.cs
    |   │       ├── LoginWebModel.cs
    |   │       ├── ProfilePictureWebModel.cs
    |   │       ├── RegisterWebModel.cs
    |   │       ├── TokenWebModel.cs
    |   │       ├── UpdateProfilePictureWebModel.cs
    |   │       ├── UpdateUserWebModel.cs
    |   │       ├── UsernameWebModel.cs
    |   │       └── UserWebModel.cs
    |   ├── Language
    |   │   ├── CreateLanguageWebModel.cs
    |   │   ├── LanguageWebModel.cs
    |   │   ├── ReadLanguageWebModel.cs
    |   │   └── UpdateLanguageWebModel.cs
    |   ├── Post
    |   │   ├── Rating
    |   │   │   ├── RatePostWebModel.cs
    |   │   │   └── ReadPostRatingWebModel.cs
    |   │   ├── CreatePostWebModel.cs
    |   │   ├── ReadPostWebModel.cs
    |   │   └── UpdatePostWebModel.cs
    |   └── Technology
    |       ├── CreateTechnologyWebModel.cs
    |       ├── ReadTechnologyWebModel.cs
    |       ├── TechnologyWebModel.cs
    |       └── UpdateTechnologyWebModel.cs
    ├── Program.cs
    ├── Properties
    │   └── launchSettings.json
    └── Startup.cs
```

# Full overview

```
.
├── DevHive.code-workspace
├── DevHive.Common
|   └── Models
|       ├── Identity
|       │   └── TokenModel.cs
|       └── Misc
|           ├── IdModel.cs
|           ├── PasswordModifications.cs
|           └── Patch.cs
├── DevHive.Data
│   ├── ConnectionString.json
│   ├── DevHiveContext.cs
│   ├── DevHiveContextFactory.cs
|   ├── Interfaces
|   |   ├── Models
|   |   │   ├── IComment.cs
|   │   |   ├── ILanguage.cs
|   │   |   ├── IModel.cs
|   │   |   ├── IPost.cs
|   │   |   ├── IProfilePicture.cs
|   │   |   ├── IRating.cs
|   │   |   ├── IRole.cs
|   │   |   ├── ITechnology.cs
|   │   |   └── IUser.cs
|   |   └── Repositories
|   |       ├── ICommentRepository.cs
|   |       ├── IFeedRepository.cs
|   |       ├── ILanguageRepository.cs
|   |       ├── IPostRepository.cs
|   |       ├── IRatingRepository.cs
|   |       ├── IRepository.cs
|   |       ├── IRoleRepository.cs
|   |       ├── ITechnologyRepository.cs
|   |       └── IUserRepository.cs
│   ├── Migrations
│   ├── Models
│   │   ├── Comment.cs
│   │   ├── Language.cs
│   │   ├── Post.cs
│   │   ├── ProfilePicture.cs
│   │   ├── Rating.cs
│   │   ├── Role.cs
│   │   ├── Technology.cs
│   │   └── User.cs
│   ├── RelationModels
│   │   ├── RatedPost.cs
│   │   ├── UserFriend.cs
│   │   └── UserRate.cs
│   └── Repositories
│       ├── BaseRepository.cs
│       ├── CommentRepository.cs
│       ├── FeedRepository.cs
│       ├── LanguageRepository.cs
│       ├── PostRepository.cs
│       ├── RatingRepository.cs
│       ├── RoleRepository.cs
│       ├── TechnologyRepository.cs
│       └── UserRepository.cs
├── DevHive.Services
│   ├── Configurations
│   │   └── Mapping
|   |       ├── CommentMappings.cs
|   |       ├── FeedMappings.cs
|   |       ├── LanguageMappings.cs
|   |       ├── PostMappings.cs
|   |       ├── RatingMappings.cs
|   |       ├── RoleMapings.cs
|   |       ├── TechnologyMappings.cs
|   |       └── UserMappings.cs
│   ├── Interfaces
│   │   ├── ICloudService.cs
│   │   ├── ICommentService.cs
│   │   ├── IFeedService.cs
│   │   ├── ILanguageService.cs
│   │   ├── IPostService.cs
│   │   ├── IRateService.cs
│   │   ├── IRoleService.cs
│   │   ├── ITechnologyService.cs
│   │   └── IUserService.cs
│   ├── Models
|   |   ├── Cloud
|   |   │   └── CloudinaryService.cs
|   |   ├── Comment
|   |   │   ├── CreateCommentServiceModel.cs
|   |   │   ├── ReadCommentServiceModel.cs
|   |   │   └── UpdateCommentServiceModel.cs
|   |   ├── Feed
|   |   │   ├── GetPageServiceModel.cs
|   |   │   └── ReadPageServiceModel.cs
|   |   ├── Identity
|   |   │   ├── Role
|   |   │   │   ├── CreateRoleServiceModel.cs
|   |   │   │   ├── RoleServiceModel.cs
|   |   │   │   └── UpdateRoleServiceModel.cs
|   |   │   └── User
|   |   │       ├── BaseUserServiceModel.cs
|   |   │       ├── FriendServiceModel.cs
|   |   │       ├── LoginServiceModel.cs
|   |   │       ├── ProfilePictureServiceModel.cs
|   |   │       ├── RegisterServiceModel.cs
|   |   │       ├── UpdateFriendServiceModel.cs
|   |   │       ├── UpdateProfilePictureServiceModel.cs
|   |   │       ├── UpdateUserServiceModel.cs
|   |   │       └── UserServiceModel.cs
|   |   ├── Language
|   |   │   ├── CreateLanguageServiceModel.cs
|   |   │   ├── LanguageServiceModel.cs
|   |   │   ├── ReadLanguageServiceModel.cs
|   |   │   └── UpdateLanguageServiceModel.cs
|   |   ├── Post
|   |   |   ├── Rating
|   |   |   │   ├── RatePostServiceModel.cs
|   |   |   │   └── ReadPostRatingServiceModel.cs
|   |   │   ├── CreatePostServiceModel.cs
|   |   │   ├── ReadPostServiceModel.cs
|   |   │   └── UpdatePostServiceModel.cs
|   |   └── Technology
|   |       ├── CreateTechnologyServiceModel.cs
|   |       ├── ReadTechnologyServiceModel.cs
|   |       ├── TechnologyServiceModel.cs
|   |       └── UpdateTechnologyServiceModel.cs
│   ├── Options
│   │   └── JWTOptions.cs
│   └── Services
│       ├── CommentService.cs
│       ├── FeedService.cs
│       ├── LanguageService.cs
│       ├── PostService.cs
│       ├── RateService.cs
│       ├── RoleService.cs
│       ├── TechnologyService.cs
│       └── UserService.cs
├── DevHive.Tests
│   ├── DevHive.Data.Tests
│   │   ├── CommentRepository.Tests.cs
│   │   ├── FeedRepository.Tests.cs
│   │   ├── LenguageRepository.Tests.cs
│   │   ├── PostRepository.Tests.cs
│   │   ├── RoleRepository.Tests.cs
│   │   ├── TechnologyRepository.Tests.cs
│   │   └── UserRepositoryTests.cs
│   ├── DevHive.Services.Tests
│   │   ├── FeedService.Tests.cs
│   │   ├── LanguageService.Tests.cs
│   │   ├── PostService.Tests.cs
│   │   ├── RoleService.Tests.cs
│   │   ├── TechnologyServices.Tests.cs
│   │   └── UserService.Tests.cs
│   └── DevHive.Web.Tests
│       ├── LanguageController.Tests.cs
│       └── TechnologyController.Tests.cs
└── DevHive.Web
    ├── appsettings.json
    ├── Attributes
    │   ├── GoodPasswordModelValidation.cs
    │   └── OnlyLettersModelValidation.cs
    ├── Configurations
    |   ├── Extensions
    |   │   ├── ConfigureAutoMapper.cs
    |   │   ├── ConfigureDatabase.cs
    |   │   ├── ConfigureDependencyInjection.cs
    |   │   ├── ConfigureExceptionHandlerMiddleware.cs
    |   │   ├── ConfigureJWT.cs
    |   │   └── ConfigureSwagger.cs
    |   └── Mapping
    |       ├── CommentMappings.cs
    |       ├── FeedMappings.cs
    |       ├── LanguageMappings.cs
    |       ├── PostMappings.cs
    |       ├── RatingMappings.cs
    |       ├── RoleMappings.cs
    |       ├── TechnologyMappings.cs
    |       └── UserMappings.cs
    ├── Controllers
    │   ├── CommentController.cs
    │   ├── FeedController.cs
    │   ├── LanguageController.cs
    │   ├── PostController.cs
    │   ├── RateController.cs
    │   ├── RoleController.cs
    │   ├── TechnologyController.cs
    │   └── UserController.cs
    ├── Middleware
    │   └── ExceptionMiddleware.cs
    ├── Models
    |   ├── Comment
    |   │   ├── CreateCommentWebModel.cs
    |   │   ├── ReadCommentWebModel.cs
    |   │   └── UpdateCommentWebModel.cs
    |   ├── Feed
    |   │   ├── GetPageWebModel.cs
    |   │   └── ReadPageWebModel.cs
    |   ├── Identity
    |   │   ├── Role
    |   │   │   ├── CreateRoleWebModel.cs
    |   │   │   ├── RoleWebModel.cs
    |   │   │   └── UpdateRoleWebModel.cs
    |   │   └── User
    |   │       ├── BaseUserWebModel.cs
    |   │       ├── LoginWebModel.cs
    |   │       ├── ProfilePictureWebModel.cs
    |   │       ├── RegisterWebModel.cs
    |   │       ├── TokenWebModel.cs
    |   │       ├── UpdateProfilePictureWebModel.cs
    |   │       ├── UpdateUserWebModel.cs
    |   │       ├── UsernameWebModel.cs
    |   │       └── UserWebModel.cs
    |   ├── Language
    |   │   ├── CreateLanguageWebModel.cs
    |   │   ├── LanguageWebModel.cs
    |   │   ├── ReadLanguageWebModel.cs
    |   │   └── UpdateLanguageWebModel.cs
    |   ├── Post
    |   │   ├── Rating
    |   │   │   ├── RatePostWebModel.cs
    |   │   │   └── ReadPostRatingWebModel.cs
    |   │   ├── CreatePostWebModel.cs
    |   │   ├── ReadPostWebModel.cs
    |   │   └── UpdatePostWebModel.cs
    |   └── Technology
    |       ├── CreateTechnologyWebModel.cs
    |       ├── ReadTechnologyWebModel.cs
    |       ├── TechnologyWebModel.cs
    |       └── UpdateTechnologyWebModel.cs
    ├── Program.cs
    ├── Properties
    │   └── launchSettings.json
    └── Startup.cs
```
