Feature: Login
	
	As a user of the application, I want to be able to log in so that I can access my account and perform actions.

Background: 
	Given user navigates to the login page

@Login @Regression
Rule: Successful login

	Scenario: Login with correct credenials
		When user enters valid username and password
		Then user should be redirected to the inventory page


@Login @Regression
Rule: Unsuccessful login attemps

	Scenario: Login with incorrect credentials
		When user enters invalid username and password
		Then user should see an error message for 'invalid' credentials
		And user should remain on the login page

	Scenario: Login with empty credentials
		When user leaves username and password fields empty
		Then user should see an error message for 'empty' credentials
		And user should remain on the login page

	Scenario: Login with locked out user
		When user enters locked out username and password
		Then user should see an error message for 'locked out' credentials
		And user should remain on the login page

	