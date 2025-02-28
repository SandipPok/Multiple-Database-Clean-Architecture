# **Project Setup and Database Queries**

## **Project Overview**

This project is built using **.NET 8**, and it supports both **MySQL** and **SQL Server** databases. The application is configured to work with either of the two databases based on the configuration set in the `appsettings.json` file. The connection strings for both MySQL and SQL Server are defined in the configuration, and the appropriate database provider is used at runtime.

---

## **Database Queries**

### **1. MySQL Query:**

For the MySQL database, the following query is used to create the `ItemDetailsBackup` table, which stores backup data for the items:

```sql
CREATE TABLE ItemDetailsBackup (
    detail_id INT PRIMARY KEY AUTO_INCREMENT,
    item_id INT,
    detail_description TEXT
);

```

### **2. SQL Server Query:**

For the SQL Server database, the following query is used to create the `Items` and `ItemDetails` table, which stores data for the items:

```sql
CREATE TABLE Items (
    item_id INT PRIMARY KEY IDENTITY,
    item_name VARCHAR(255),
    item_description TEXT
);

CREATE TABLE ItemDetails (
    detail_id INT PRIMARY KEY IDENTITY,
    item_id INT FOREIGN KEY REFERENCES Items(item_id) ON DELETE CASCADE,
    detail_description TEXT
);
```
