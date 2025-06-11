Feature: Cart

As a user, I want to be able to add products to my cart or remove products from my cart

Background: 
	Given user navigates to the login page
	When user enters valid username and password
	Then user should be redirected to the inventory page

@Cart @Regression
Rule: Add/Remove products to/from cart 

	Scenario: Add to Cart button should change to Remove after its clicked 
		When user adds multiple products to cart 
		Then the [Add to Cart] button text for those products should change to [Remove]

	Scenario: Add one product to cart
		When user adds a product to the cart
		And user navigates to the cart page 
		Then user should see the added product(s) on the cart page

	Scenario: Add multiple products to cart
		When user adds multiple products to cart 
		And user navigates to the cart page 
		Then user should see the added product(s) on the cart page

	Scenario: Remove all products from cart via cart page 
		When user adds multiple products to cart 
		And user navigates to the cart page 
		And user removes the product(s) from the cart 
		Then there should not be any products on the cart page

	Scenario: Remove some products from cart page
		When user adds multiple products to cart 
		And user navigates to the cart page 
		And user removes some products from the cart page
		Then the cart should contain product(s) that are not removed

	Scenario: Remove product from cart via product page
		When user adds multiple products to cart 
		And user navigates to the cart page 
		Then user should see the added product(s) on the cart page
		When user navigates back to inventory page 
		And user removes the added products from inventory page 
		And user navigates to the cart page 
		Then there should not be any products on the cart page
