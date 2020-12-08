# Git commands with instructions

## git clone *url*

Clones a GitHub repository in the current directory, in a folder, the name of which is the repo's name

## git branch *option* *branch*

git branch - View a list of the branches

## git checkout *option* *branch*

git checkout *branch_name* - Switch to exiting branch
git checkout -b *branch_name* - Create a new branch

## git fetch

git fetch - Downloads commits, files and references from remote repo to local repo
 Fetching is what you do when you want to see what everybody else has been working on.
 Doesn't force a merge

## git pull *option*

git pull - Fetches changes and immediately merges them to local repo
 Combo of git fetch & git merge
git pull --rebase - "I want to put my changes on top of what everybody else has done in this branch."
git pull --rebase origin - This simply moves your local changes onto the top of what everybody else has already contributed.

## git add *files*

git add . - Add all files to track the changes made to them
git add *filename* - Add file with name *filename* to track the changes made to it

## git commit *options*

git commit - Opens a editor to write a message, documenting the changes made
git commit -m *Message* - Write the message inline, documenting the changes made
