-- Create Country table with auto-incrementing id
CREATE TABLE Country (
    id SERIAL PRIMARY KEY,
    name TEXT
);

-- Create Book table with a foreign key reference to the Country table
CREATE TABLE Book (
    id SERIAL PRIMARY KEY,
    title TEXT,
    country_id INT,
    CONSTRAINT fk_country
        FOREIGN KEY (country_id)
        REFERENCES Country (id)
        ON DELETE CASCADE
);