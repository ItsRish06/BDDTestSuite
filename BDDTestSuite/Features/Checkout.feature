Feature: Checkout

As a user, I should be able to complete the checkout process to purchase products 

Background: 
	Given user navigates to the login page
	When user enters valid username and password
	Then user should be redirected to the inventory page

@Checkout @Regression
Rule: Checkout information(user form) and checkout overview page validation

	Scenario: Checkout form validation 
		When user adds a product to the cart
		And user navigates to the cart page 
		And user selects the checkout option
		Then the checkout form should validate all user entered inputs

	Scenario: Products added to cart should be visible on the checkout overview page
		When user adds multiple products to cart
		And user navigates to the cart page 
		And user selects the checkout option
		And user enters their information on the checkout form 
		And user navigates to checkout overview page
		Then user should see correct product information on the overview page 


@Checkout @Regression
Rule: Complete the checkout process and purchase single/multiple products 

	Scenario: Purchase single product 
		When user adds a product to the cart
		And user navigates to the cart page 
		And user selects the checkout option
		And user enters their information on the checkout form 
		And user navigates to checkout overview page
		And user finishes checkout process 
		Then user should be navigated to the thank you page

	Scenario: Purchase multiple products 
		When user adds multiple products to cart
		And user navigates to the cart page
		And user selects the checkout option
		And user enters their information on the checkout form 
		And user navigates to checkout overview page
		And user finishes checkout process 
		Then user should be navigated to the thank you page