CREATE TABLE sentences_of_the_day(
    id INT PRIMARY KEY IDENTITY(1,1),
    sentence VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE _user (
    id INT PRIMARY KEY IDENTITY(1,1),
    username VARCHAR(50) NOT NULL UNIQUE,
    user_password VARCHAR(60) NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(30) NOT NULL UNIQUE,
    phone_number VARCHAR(16) UNIQUE,
    user_address VARCHAR(50),
    loyalty_points INT,
);

CREATE TABLE dress (
    id INT PRIMARY KEY IDENTITY(1,1),
    dress_name VARCHAR(50) NOT NULL,
    describe VARCHAR(50) NOT NULL,
    price DECIMAL(6,2) NOT NULL,
    available BIT NOT NULL,
    date_begin_available DATE NOT NULL,
    date_end_available DATE NOT NULL,
    urlImage VARCHAR(255) NOT NULL,
    user_id INT,
    FOREIGN KEY (user_id) REFERENCES _user(id)
);

CREATE TABLE dress_order (
    id INT PRIMARY KEY IDENTITY(1,1),
    billing_date DATE NOT NULL,
    delivery_date DATE NOT NULL,
    billing_address VARCHAR(50) NOT NULL,
    delivery_address  VARCHAR(50) NOT NULL,
    isValid BIT NOT NULL,
    user_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES _user(id)
    ON UPDATE CASCADE
    ON DELETE CASCADE
);

CREATE TABLE order_line (
    id INT PRIMARY KEY IDENTITY(1,1),
    date_begin_location  DATE NOT NULL,
    date_end_location  DATE NOT NULL,
    final_price DECIMAL(6,2) NOT NULL,
    user_id INT NOT NULL,
    dress_order_id INT NOT NULL,
    dress_id INT NOT NULL,
    FOREIGN KEY (dress_order_id) REFERENCES dress_order(id)
    ON UPDATE CASCADE
    ON DELETE CASCADE,
    FOREIGN KEY (dress_id) REFERENCES dress(id)
    ON UPDATE CASCADE
    ON DELETE CASCADE
);

CREATE TABLE favorites (
	id INT PRIMARY KEY IDENTITY(1,1),
	user_id INT NOT NULL,
    dress_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES _user(id)
    ON UPDATE CASCADE
    ON DELETE CASCADE,
    FOREIGN KEY (dress_id) REFERENCES dress(id)
    ON UPDATE CASCADE
    ON DELETE CASCADE
)

insert into _user 
values ('Flowgenz', '123456', 'Florian', 'Janssens', 'flowgenzyt@gmail.com', '470265668', 'Jemeppe', 100);
insert into _user 
values ('Warrior5060', '123456', 'Joris', 'Zonowatnik', 'joris.zono@gmail.com', '470265684', 'Auvelais', 0);
