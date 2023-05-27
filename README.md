# Fur And Firepower
Fun little game
# Contributing
## Programming
First install git with your preffered method. Then run the commands
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
git remote add origin "https://{YOUR_GITHUB_USERNAME}:{PERSONAL ACCESS TOKEN YOU JUST GENERATED}@github.com/Zemchar/fur-and-firepower.git"
git pull
```
If there are no errors, horray you are now ready to commit things!
**If you are not using command line git and instead plan on using a manager, skip the rest of this and figure out how your manager works**
To commit files from your machine to the repo the syntax is (from root unity directory)
```
git add .
git commit -m "{Description goes here}"
git push -u main
```
**Commit as often as possible**

### Always remember to git pull whenever you start programming! And comment your code pretty please

## Art
Please upload your files directly to this repo using github web. Place them in the appropriate folders so the developers have proper access to them.
If you are so inclined, attach a description to explain where devs should put it in the level etc. 
# Playtesting
Weekly builds will be released over in the releases tab. You may post feedback on the discord in the appropriate channel. Please include a video of your play session if possible! (google drive link or something works perfectly fine)
