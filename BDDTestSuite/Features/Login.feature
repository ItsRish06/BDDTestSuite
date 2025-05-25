Feature: Login
	
	As a user of the application, I want to be able to log in so that I can access my account and perform actions.

	@Login @Regression
Scenario: Login with correct credenials
	Given user navigates to the login page
	When user enters valid username and password
	Then user should be redirected to the inventory page

	@Login	@Regression
Scenario: Login with incorrect credentials
	Given user navigates to the login page
	When user enters invalid username and password
	Then user should see an error message for 'invalid' credentials
	And user should remain on the login page

	@Login	@Regression
Scenario: Login with empty credentials
	Given user navigates to the login page
	When user leaves username and password fields empty
	Then user should see an error message for 'empty' credentials
	And user should remain on the login page

	@Login @Regression
Scenario: Login with locked out user
	Given user navigates to the login page
	When user enters locked out username and password
	Then user should see an error message for 'locked out' credentials
	And user should remain on the login page

	