[![Watch the video](https://img.youtube.com/vi/24PmyKg43EY/hqdefault.jpg)](https://www.youtube.com/embed/24PmyKg43EY)
# Zamawianie Posiłków Online



Zamawianie Posiłków Online is a lightweight ASP.NET MVC mock-up for ordering meals from restaurants. It demonstrates a complete end-to-end flow: user registration and login, browsing restaurants and their menus, placing orders that persist to a SQL database, and a simple admin area for managing restaurants, meals, and viewing orders.


## Key features
- ### Customer accounts 
     - Register and log in with email and password.
     - Recover account if you have forgotten your password
     - Necessary for browsing and ordering


- ### Restaurant browsing and ordering

    - Browse restaurants and view each restaurant’s menu.

    - Select a restaurant and add specific meals from that restaurant to your cart.

    - Place orders that are saved to the database and available for later review.
    - View full order history and order details for the logged-in customer.

Orders persist in a local SQLite database by default, with possible configuration to connect to another database engine.

- ### Admin dashboard

    - Log in as an admin to manage the catalog.

    - Add, edit, and remove restaurants and meals, including image uploads for both restaurants and dishes.

    - See all placed orders from all customers.

- ### Interactive frontend

     - Client-side form validation and inline input error messages using JavaScript.

     - Small popups/toasts show success or failure after actions like ordering.

     - Simple dynamic UI elements (cart updates, confirmation modals) for a responsive feel.


## Tech Stack



**Backend:** ASP.NET MVC with C#

**Frontend:** HTML, CSS, JavaScript (vanilla)

**Database:** SQLite by default; configurable to external DB

**Architecture:** MVC design pattern with repository-like data access for clarity

**Project structure** Controllers for Account, Restaurants, Orders, Admin

**Views** for registration/login, restaurant list, menu, cart, order details, admin pages

**Models** for User, Restaurant, Meal, Order, OrderItem

**JavaScript** modules for validation, popups, and cart behavior

A simple image store for uploaded restaurant and meal pictures
## Deployment

To deploy this project Clone the repository.

Update connection string in appsettings.json to use SQLite (default, and file already in repo) or your preferred DB.

#### if connecting to a new db
- Run database migrations or use the provided seed to create sample restaurants and an admin user.

Build and run the app from Visual Studio or dotnet CLI.

Open the site, register a customer or sign in as the seeded admin account.

Usage notes
To place an order: choose a restaurant, add meals to the cart, review the cart and confirm. The order will be recorded in the database and visible in your order history.

Admin responsibilities: create restaurants, upload cover images, add meals with pictures and prices, and review all orders.

Client-side validation will highlight problems while typing; toast notifications confirm success or show errors.


Why this project
FoodOrderMock is perfect as a learning project or demo: it combines core web app concerns (authentication, file uploads, DB persistence, admin interfaces, and client-side UX) in a small, understandable codebase. Use it as a starting point for prototypes or teaching examples.

