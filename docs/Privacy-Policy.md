DevHive **doesn't collect any user data, that you haven't personally submitted** (there is no telemetry), and that won't ever change!

The only potentially sensitive that that could be stored is your profile (first and last name, email, ..) and your posts (if you've shared anything sensitive), but in both cases you've personally given this information.

## Data on the server

All data is stored in the database and isn't shared with anyone. The entity that is hosting an instance of the application could expose data to unknown third parties, but DevHive doesn't do anything of the sorts by itself!

## Data on your machine

On your computer, the only thing that is saved is your [authentication token](https://github.com/Team-Kaleidoscope/DevHive/wiki/Authentication) in session storage. This is done so you could stay logged in the website in the current tab, and after closing it, the data gets deleted.

In the future we could add a cookie to your computer storage, but that will still only hold the token for authentication purposes, so you can reopen your browser and still be logged in. **Tracking and third-party cookies are *never* going to be implemented!**

## Telemetry by tools

DevHive itself doesn't collect any type of telemetry, but that isn't the same for the tools it uses. 

The `dotnet` CLI tool is used to run the API, and `dotnet` does [`collect telemetry`](https://docs.microsoft.com/en-us/dotnet/core/tools/telemetry). [The same](https://angular.io/cli/usage-analytics-gathering) can be said for the Angular CLI (`ng`).

Thankfully in both cases, you can opt out. Ask the administrator(s) of the instance you're using whether they have disabled telemetry.

**Although**, it's important to mention that **this telemetry might not be collecting your data**, but the data of the server that uses it. Do your own research!