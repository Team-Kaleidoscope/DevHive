Certain actions with the API require User authentication. In DevHive, all authentication is done with [JSON Web Tokens](https://en.wikipedia.org/wiki/JSON_Web_Token). 

The JWTs must be sent as a [Bearer Token](https://www.oauth.com/oauth2-servers/differences-between-oauth-1-2/bearer-tokens/).

## Structure of tokens

The main contents of a User's token are the `UserName`, `ID` and `Roles`.

Sample token:
```
eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJRCI6IjI3ZTIwM2JkLTUzMTItNDgzMS05MzM0LWNkM2MyMGU1ZDY3MiIsIlVzZXJuYW1lIjoidGVzdCIsInJvbGUiOiJVc2VyIiwibmJmIjoxNjEyMzYxOTc1LCJleHAiOjE2MTI5MDgwMDAsImlhdCI6MTYxMjM2MTk3NX0.ZxhDSUsDf9cGig03QpzNgj3dkqbcfAoFXVIfixYGurzhd0l1_IO79UpE_Sb6ZU9hz3IT1XPrlrQ_Kd46L7xcQg
```
[Decoded](https://jwt.io/):

- Header
```json
{
  "alg": "HS512",
  "typ": "JWT"
}
```

- Data
```json
{
  "ID": "27e203bd-5312-4831-9334-cd3c20e5d672",
  "Username": "test",
  "role": "User",
  "nbf": 1612361975,
  "exp": 1612908000,
  "iat": 1612361975
}
```

- Signature
```
HMACSHA512(
  base64UrlEncode(header) + "." +
  base64UrlEncode(payload)
)
```

## Token validation

All token validations are done in the User Service. Depending on the situation, we can differentiate a couple types of authentication:

|||
|---|---|
|1|Has the role "User" or "Admin"|
|2|Has the role "User" and is the owner/author of the object or has the role "Admin"|
|3|Has the role "Admin"|
|||