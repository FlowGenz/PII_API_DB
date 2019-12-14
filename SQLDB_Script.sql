CREATE TABLE sentences_of_the_day(
    id INT PRIMARY KEY IDENTITY(1,1),
    sentence VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE users (
    username VARCHAR(50) PRIMARY KEY,
    user_password VARCHAR(60) NOT NULL,
    privilege VARCHAR(50) NOT NULL
)

CREATE TABLE partners(
	id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(50) NOT NULL UNIQUE,
    phone_number VARCHAR(16) NOT NULL UNIQUE,
    partner_address VARCHAR(50),
    username_user VARCHAR(50) NOT NULL,
    FOREIGN KEY (username_user) REFERENCES users(username)
    ON UPDATE CASCADE
    ON DELETE CASCADE
);

CREATE TABLE customer (
    id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(30) NOT NULL UNIQUE,
    phone_number VARCHAR(16) UNIQUE,
    customer_address VARCHAR(50),
    fidelity_points INT NOT NULL,
    username_user VARCHAR(50) NOT NULL,
    FOREIGN KEY (username_user) REFERENCES users(username)
);

CREATE TABLE dress (
    id INT PRIMARY KEY IDENTITY(1,1),
    dress_name VARCHAR(50) NOT NULL,
    describe VARCHAR(50) NOT NULL,
    price DECIMAL(6,2) NOT NULL,
    availible BIT NOT NULL,
    date_begin_available DATE NOT NULL,
    date_end_available DATE NOT NULL,
    urlImage VARCHAR(255) NOT NULL,
    partners_id INT NOT NULL,
    FOREIGN KEY (partners_id) REFERENCES partners(id)
    ON UPDATE CASCADE
    ON DELETE CASCADE
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
    ON UPDATE CASCADE
    ON DELETE CASCADE
);

CREATE TABLE order_line (
    id INT PRIMARY KEY IDENTITY(1,1),
    date_begin_location  DATE NOT NULL,
    date_end_location  DATE NOT NULL,
    final_price DECIMAL(6,2) NOT NULL,
    customer_id INT NOT NULL,
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
	customer_id INT NOT NULL,
    dress_id INT NOT NULL,
    FOREIGN KEY (customer_id) REFERENCES customer(id)
    ON UPDATE CASCADE
    ON DELETE CASCADE,
    FOREIGN KEY (dress_id) REFERENCES dress(id)
    ON UPDATE CASCADE
    ON DELETE CASCADE
)

insert into users
values ('flowgenz', '123456789', 'admin');
insert into users
values ('warrior5060', '987654321', 'admin');
insert into users
values ('partner1', '12456', 'partner');
insert into customer
values ('Florian', 'Janssens', 'flowgenzyt@gmail.com', '470265668', 'chez moi', 100, 'flowgenz');
insert into customer
values ('Joris', 'Zonowatnik', 'uneadresse@gmail.com', 'je connais pas', 'chez lui', 0, 'warrior5060');
insert into partners
values('Libelle', 'Jeannette', 'mail@mail.com', '045866212', 'Rue des partneaires', 'partner1');

select * from customer, users where customer.username_user = users.username;
select * from partners;

insert into dress
values('La robe de ouf', 'Une belle robe de ouf', 555.21, 1, '20191210', '20191225', 'url', 1);

select * from dress;

DROP TABLE sentences_of_the_day;
DROP TABLE order_line;
DROP TABLE dress_order;
DROP TABLE dress;
DROP TABLE customer;
DROP TABLE partners;
DROP TABLE favorites;