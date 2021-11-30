 > As you complete each section you **must** remove the prompt text. Every *turnin* of this project includes points for formatting of this README so keep it clean and keep it up to date. 
 > Prompt text is any lines beginning with "\>"
 > Replace anything between \<...\> with your project specifics and remove angle brackets. For example, you need to name your project and replace the header right below this line with that title (no angle brackets). 
# Cardboard Stonks: Price Check Web Scraper

Authors:
* [Nathan Caridad](https://github.com/ncari002)
* [Maksim Kulbaev](https://github.com/unsafe4u)
* [Brian Porter](https://github.com/bporter11)
* [Paul Ullman](https://github.com/PaulU090)

## Project Description
 Why is it important or interesting to you?

* Web scrapping/data management is a useful skill to learn, so we're using this project to learn

 What languages/tools/technologies do you plan to use? (This list may change over the course of the project)

* [Web Scrape Target](cardrush.jp) - website with large database of prices for items we're interested in

* C# for web scraping

* Google sheets/csv file as database

What will be the input/output of your project? What are the features that the project provides?

* Input is a website link to a specific item or a .txt file with list of links to specific items

* Output is name, price, rarity, set code, set name, URL of the items processed
 >
 > ## Phase II
 > In addition to completing the "Class Diagram" section below, you will need to:
 > * Create an "Epic" (note) for each feature and each design pattern and assign them to the appropriate team member. Place these in the `Backlog` column
 > * Complete your first *sprint planning* meeting to plan out the next 7 days of work.
 >   * Create smaller actionable development tasks as issues and assign them to team members. Place these in the `TODO` column.
 >   * These cards should represent roughly 7 days worth of development time for your team, taking you until your first meeting with the TA
## Class Diagram
 > Include a class diagram(s) for your project and a description of the diagram(s). Your class diagram(s) should include all the main classes you plan for the project. This should be in sufficient detail that another group could pick up the project this point and successfully complete it. Use proper UML notation (as discussed in the course slides).
 
 > ## Phase III
 > You will need to schedule a check-in with the TA (during lab hours or office hours). Your entire team must be present. 
 > * Before the meeting you should perform a sprint plan like you did in Phase II.
 > * You should also update this README file by adding the following:
 >   * What design patterns did you use? For each design pattern you must explain in 4-5 sentences:
 * Null Object Pattern:
     * Provides default behavior in case data is not available. This is useful for our web scraper because different website might not have all the data we need, so certain functions are not implement exactly the same as others and instead return null values. An abstract class is first defined, and a class is defined for each website we use. Since the given websites contain different data, we can allow some return values to be assigned null if there is no data available, instead of just crashing our code.
 * Factory Pattern:
     * Provides the best method of creating object. The client need not know how creating the object works as all objects are referred to using a common interface that allows any object, despite it's differences, to be properly interacted with. This is great for our website scraper because the client only has to interact with the factory and the concrete object, the process of making the concrete object from the abstract object is abstracted away from the client/user.
 * Strategy Pattern:
     * #TODO (IExportBase implements this pattern, describe in what way)
 * Composite Pattern:
     * #TODO (ICardTactic implements this pattern, describe in what way)
 >   * How did the design pattern help you write better code?
 * This allowed us to write better code by defining clear guidelines as to how each website is handled, managing different use cases per website, and allowing things to inheret from an abstract class rather than hardcoding functionality. This allowed expandability in our code rather than having to restructure if we had new websites or export types in the future.
 >   * An updated class diagram that reflects the design patterns you used. You may combine multiple design patterns into one diagram if you'd like, but it needs to be clear which portion of the diagram represents which design pattern (either in the diagram or in the description).
 >   * Make sure your README file (and Project board) are up-to-date reflecting the current status of your project. Previous versions of the README file should still be visible through your commit history.
> 
> During the meeting with your TA you will discuss: 
 > * How effective your last sprint was (each member should talk about what they did)
 > * Any tasks that did not get completed last sprint, and how you took them into consideration for this sprint
 > * Any bugs you've identified and created issues for during the sprint. Do you plan on fixing them in the next sprint or are they lower priority?
 > * What tasks you are planning for this next sprint.

 
 > ## Final deliverable
 > All group members will give a demo to the TA during lab time. The TA will check the demo and the project GitHub repository and ask a few questions to all the team members. 
 > Before the demo, you should do the following:
 > * Complete the sections below (i.e. Screenshots, Installation/Usage, Testing)
 > * Plan one more sprint (that you will not necessarily complete before the end of the quarter). Your In-progress and In-testing columns should be empty (you are not doing more work currently) but your TODO column should have a full sprint plan in it as you have done before. This should include any known bugs (there should be some) or new features you would like to add. These should appear as issues/cards on your Project board.
 > * Make sure your README file and Project board are up-to-date reflecting the current status of your project (e.g. any changes that you have made during the project such as changes to your class diagram). Previous versions should still be visible through your commit history. 
 
 ## Screenshots
 > Screenshots of the input/output after running your application
 ## Installation/Usage
 > Instructions on installing and running your application
 ## Testing
 > How was your project tested/validated? If you used CI, you should have a "build passing" badge in this README.
 
