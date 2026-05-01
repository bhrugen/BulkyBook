# 📚 BulkyBook - E-Commerce Bookstore

A full-featured e-commerce web application built with **ASP.NET Core MVC** and **Entity Framework Core**. This project demonstrates enterprise-level architecture patterns, authentication, payment processing, and cloud deployment.

## 🎓 About This Project

This project is part of the comprehensive .NET Mastery course available at:  
**[Complete Guide to ASP.NET Core MVC (.NET 11)](https://www.dotnetmastery.com/Home/Details?courseId=9)**

Learn modern web development with ASP.NET Core, from fundamentals to advanced topics including:
- MVC Architecture Pattern
- Entity Framework Core & Repository Pattern
- Identity & Authentication
- Stripe Payment Integration
- Azure Deployment
- Email Services with Mailjet

---

## 🚀 Technologies & Features

### **Backend**
- **ASP.NET Core MVC** (.NET 11)
- **Entity Framework Core** with SQL Server
- **ASP.NET Core Identity** for authentication & authorization
- **Repository Pattern** for data access
- **Dependency Injection**
- **Areas** for modular organization

### **Frontend**
- **Razor Pages** with dynamic UI
- **Bootstrap 5** for responsive design
- **JavaScript** for interactive features

### **Third-Party Integrations**
- **Stripe** - Payment processing
- **Mailjet** - Email service
- **Azure SQL Database** - Cloud database
- **Azure App Service** - Hosting

### **Key Features**
✅ User authentication & authorization (Admin, Employee, Customer roles)  
✅ Product catalog with categories  
✅ Shopping cart functionality  
✅ Order management system  
✅ Stripe payment integration  
✅ Email notifications  
✅ Admin dashboard  
✅ Responsive design  
✅ Azure deployment ready

---

## 📁 Project Structure

```
BulkyBook/
├── BulkyBookWeb/              # Main web application
│   ├── Areas/
│   │   ├── Admin/             # Admin area (Products, Orders, Users, Dashboard)
│   │   ├── Customer/          # Customer area (Home, Cart)
│   │   └── Identity/          # Authentication (Login, Register)
│   ├── wwwroot/               # Static files (CSS, JS, images)
│   └── Views/                 # Razor views
├── BulkyBook.Business/        # Business logic & services
├── BulkyBook.DataAccess/      # Data access layer & repositories
├── BulkyBook.Models/          # Domain models & ViewModels
└── BulkyBook.Utility/         # Helper classes & constants
```

---

## 🛠️ Setup Instructions

### **Prerequisites**
- Visual Studio 2022 or later
- .NET 9 SDK or later
- SQL Server (LocalDB or full version)
- Azure account (for deployment)

### **1. Clone the Repository**
```bash
git clone https://github.com/bhrugen/BulkyBook.git
cd BulkyBook
```

### **2. Update Connection String**
Edit `BulkyBookWeb/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "SQLConnection": "Server=(localdb)\\mssqllocaldb;Database=BulkyBook;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### **3. Configure Third-Party Services**

#### **Stripe** (Payment Processing)
1. Create account at [stripe.com](https://stripe.com)
2. Get API keys from Dashboard
3. Add to `appsettings.json`:
```json
{
  "Stripe": {
    "SecretKey": "sk_test_your_key_here",
    "PublishableKey": "pk_test_your_key_here"
  }
}
```

#### **Mailjet** (Email Service)
1. Create account at [mailjet.com](https://www.mailjet.com)
2. Get API credentials
3. Add to `appsettings.json`:
```json
{
  "Mailjet": {
    "ApiKey": "your_api_key",
    "SecretKey": "your_secret_key",
    "SenderEmail": "your-email@example.com",
    "SenderName": "BulkyBook Store"
  }
}
```

### **4. Run Database Migrations**
```bash
# Open Package Manager Console in Visual Studio
Update-Database
```
Or via CLI:
```bash
dotnet ef database update --project BulkyBook.DataAccess --startup-project BulkyBookWeb
```

### **5. Run the Application**
```bash
dotnet run --project BulkyBookWeb
```
Or press `F5` in Visual Studio.

---

## 👤 Default Admin Account

After first run, use these credentials to login as admin:
- **Email**: `admin@dotnetmastery.com`
- **Password**: `Admin123*`

---

## 🌐 Deployment to Azure


### Quick Steps:
1. Create Azure SQL Database
2. Create Azure App Service
3. Configure firewall rules
4. Add connection strings & app settings in Azure Portal
5. Publish from Visual Studio

---

## 📸 Features Overview

### **Customer Features**
- Browse product catalog
- Add items to cart
- Secure checkout with Stripe
- Order history

### **Admin Features**
- Product management (CRUD)
- Category management
- Order management
- User management
- Dashboard analytics

### **Employee Features**
- View and manage orders
- Process shipments
- Update order status

---

## 📚 Learning Resources

- **Course**: [Complete ASP.NET Core MVC Course](https://www.dotnetmastery.com/Home/Details?courseId=9)
- **Instructor**: Bhrugen Patel
- **Website**: [DotNetMastery.com](https://www.dotnetmastery.com)

---

## 🤝 Contributing

This is an educational project. If you find issues or want to suggest improvements:
1. Fork the repository
2. Create a feature branch
3. Submit a pull request

---

## 📝 License

This project is created for educational purposes as part of the .NET Mastery course.

---

## 👨‍💻 Author

**Bhrugen Patel**
- Website: [DotNetMastery.com](https://www.dotnetmastery.com)
- GitHub: [@bhrugen](https://github.com/bhrugen)

---

## 🙏 Acknowledgments

- Thanks to all students of the .NET Mastery course
- ASP.NET Core Team for excellent documentation
- Community contributors

---

## 📞 Support

For questions and support:
- Visit [DotNetMastery.com](https://www.dotnetmastery.com)
- Check course Q&A section
- Review course videos

---

**⭐ If you found this project helpful, please star the repository!**
