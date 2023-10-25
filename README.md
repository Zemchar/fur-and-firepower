# NOTICE
This project has been officaly postponed. Summer has ended and all contributors are busy. 

This is **NOT** a notice of abandonment, I still plan on completing this project someday. 

I still think a lot of the code and art here is wonderful, so I will release this publically until development restarts. This project is under copyright and this notice is not a license to redistribute, copy, sell, modify, or obtain patents for any part or component of this project that is original in any public way. Strictly private use is permitted.  Planning materials and design documents will remain private.

# Fur And Firepower
Fun little game

# Contributing
## Getting started Programming with [a Git Manager](https://github.com/spoiledcat/git-for-unity)
First, install git from their website or your package manager. 

Next, Navigate to your GitHub user settings and find "developer Settings" and click on "personal access tokens" within that menu.
Create a classic token with read/write access to reposotories at the minnimum and save it somewhere.

Finally run the following commands in terminal or Windows powershell (or cmd)

(Replace any brackets with the information requested)
```
git clone https://{YOUR_GITHUB_USERNAME}:{PERSONAL ACCESS TOKEN YOU JUST GENERATED}@github.com/Zemchar/fur-and-firepower.git
```
Next, open the project in unity. The github manager program should already be installed. If it is not, install it from the link at the title of this section

You are done! Remember to commit often and request locks when you do work on commonly accesed files
## Programming with raw git
First install git with your preffered method. Then run the following commands in terminal or windows powershell (cmd also works)
```
git clone https://github.com/Zemchar/fur-and-firepower.git
git config --global user.name "Your Name"
git config --global user.email "Your Email"
cd fur-and-firepower
```
Next go to your github settings, scroll all the way down to developer settings and click on personal access tokens.
Create a basic access token with repo read/write access and save it somewhere.
Next return to terminal and run these commands
```
git remote remove origin
git remote add origin "https://{YOUR_GITHUB_USERNAME}:{PERSONAL ACCESS TOKEN YOU JUST GENERATED}@github.com/Zemchar/fur-and-firepower.git"
git pull
```
If there are no errors, horray you are now ready to commit things!

To commit files from your machine to the repo the syntax is (from root unity directory)
```
git add .
git commit -m "{Description goes here}"
git push -u origin main
```
**Commit as often as possible**


### Always remember to git pull whenever you start programming! And comment your code pretty please

## Art
Please upload your files directly to this repo using github web. Place them in the appropriate folders so the developers have proper access to them.
If you are so inclined, attach a description to explain where devs should put it in the level etc. 
# Playtesting
Weekly builds will be released over in the releases tab. You may post feedback on the discord in the appropriate channel. Please include a video of your play session if possible! (google drive link or something works perfectly fine)
