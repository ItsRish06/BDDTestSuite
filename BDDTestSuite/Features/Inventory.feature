Feature: Inventory

	As a user, I want to be able to look at products, sort them in a particular order or go to product details page by clicking on a product.


Background: 
	Given user navigates to the login page
	When user enters valid username and password
	Then user should be redirected to the inventory page

@Inventory @Regression
Rule: View details of a product by clicking on it 

	Scenario: View product details
		When user clicks on the title link for product '1'
		Then user should be redirected to inventory item detail page 
		And user should see the product details 

@Inventory @Regression
Rule: Sort the products on the inventory page 

	Scenario: Sort products in ascending order by name
		When user selects the option to sort the products by 'name' in 'ascending' order 
		Then the products on the inventory page should get sorted by 'name' in 'ascending' order

	Scenario: Sort products in descending order by name
		When user selects the option to sort the products by 'name' in 'descending' order 
		Then the products on the inventory page should get sorted by 'name' in 'descending' order

	Scenario: Sort products in ascending order by price
		When user selects the option to sort the products by 'price' in 'ascending' order 
		Then the products on the inventory page should get sorted by 'price' in 'ascending' order

	Scenario: Sort products in descending order by price
		When user selects the option to sort the products by 'price' in 'descending' order 
		Then the products on the inventory page should get sorted by 'price' in 'descending' order



