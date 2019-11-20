CREATE TABLE sentences_of_the_day(
    id INT PRIMARY KEY IDENTITY(1,1),
    sentence VARCHAR NOT NULL UNIQUE
);

CREATE TABLE partners(
	id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR NOT NULL,
    last_name VARCHAR NOT NULL,
    email VARCHAR NOT NULL UNIQUE,
    phone_number VARCHAR NOT NULL UNIQUE,
    partner_address VARCHAR
);

CREATE TABLE customer (
    id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR NOT NULL,
    last_name VARCHAR NOT NULL,
    username VARCHAR NOT NULL,
    customer_password VARCHAR NOT NULL,
    email VARCHAR NOT NULL UNIQUE,
    phone_number VARCHAR UNIQUE,
    customer_address VARCHAR,
    fidelity_points INT NOT NULL
);

CREATE TABLE dress (
    id INT PRIMARY KEY IDENTITY(1,1),
    dress_name VARCHAR NOT NULL,
    describe VARCHAR NOT NULL,
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
    billing_address DATE NOT NULL,
    delivery_address  DATE NOT NULL,
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



DROP TABLE sentences_of_the_day;
DROP TABLE order_line;
DROP TABLE dress_order;
DROP TABLE dress;
DROP TABLE customer;
DROP TABLE partners;
DROP TABLE favorites;