CREATE TABLE sentences_of_the_day(
    id INT PRIMARY KEY IDENTITY(1,1),
    sentence VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE partners(
	id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(50) NOT NULL UNIQUE,
    phone_number VARCHAR(16) NOT NULL UNIQUE,
    partner_address VARCHAR(50)
);

CREATE TABLE customer (
    id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    username VARCHAR(50) NOT NULL,
    customer_password VARCHAR(60) NOT NULL,
    email VARCHAR(30) NOT NULL UNIQUE,
    phone_number VARCHAR(16) UNIQUE,
    customer_address VARCHAR(50),
    fidelity_points INT NOT NULL
);

CREATE TABLE dress (
    id INT PRIMARY KEY IDENTITY(1,1),
    dress_name VARCHAR(50) NOT NULL,
    describe VARCHAR(50) NOT NULL,
    price DECIMAL NOT NULL,
    availible BIT NOT NULL,
    date_begin_available DATE NOT NULL,
    date_end_available DATE NOT NULL,
    partners_id INT NOT NULL,
    FOREIGN KEY (partners_id) REFERENCES partners(id)
);

CREATE TABLE dress_order (
    id INT PRIMARY KEY IDENTITY(1,1),
    billing_date DATE NOT NULL,
    delivery_date DATE NOT NULL,
    billing_address VARCHAR(50) NOT NULL,
    delivery_address  VARCHAR(50) NOT NULL,
    isValid BIT NOT NULL,
    customer_id INT NOT NULL,
    FOREIGN KEY (customer_id) REFERENCES customer(id)
);

CREATE TABLE order_line (
    id INT PRIMARY KEY IDENTITY(1,1),
    date_begin_location  DATE NOT NULL,
    date_end_location  DATE NOT NULL,
    final_price DECIMAL NOT NULL,
    customer_id INT NOT NULL,
    dress_order_id INT NOT NULL,
    dress_id INT NOT NULL,
    FOREIGN KEY (dress_order_id) REFERENCES dress_order(id),
    FOREIGN KEY (dress_id) REFERENCES dress(id)
);

CREATE TABLE favorites (
	id INT PRIMARY KEY IDENTITY(1,1),
	customer_id INT NOT NULL,
    dress_id INT NOT NULL,
    FOREIGN KEY (customer_id) REFERENCES customer(id),
    FOREIGN KEY (dress_id) REFERENCES dress(id)
)


insert into customer
values ('Florian', 'Janssens', 'flowgenz', '123456789', 'flowgenzyt@gmail.com', '470265668', 'chez moi', 100);
insert into customer
values ('Joris', 'Zonowatnik', 'warrior5060', '987654321', 'uneadresse@gmail.com', 'je connais pas', 'chez lui', 0);


DROP TABLE sentences_of_the_day;
DROP TABLE order_line;
DROP TABLE dress_order;
DROP TABLE dress;
DROP TABLE customer;
DROP TABLE partners;
DROP TABLE favorites;