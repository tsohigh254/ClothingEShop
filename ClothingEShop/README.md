# Clothing E-Shop - Assignment 1

A full-stack web application for managing clothing products with CRUD operations, built with Blazor Server (.NET 8) and PostgreSQL.

## ?? Features

### Core Features (Assignment Requirements)
- ? **Product Model** with name, description, price, and image URL
- ? **REST API Endpoints**:
  - `GET /api/products` - List all products
  - `GET /api/products/{id}` - Get single product
  - `POST /api/products` - Create new product
  - `PUT /api/products/{id}` - Update product
  - `DELETE /api/products/{id}` - Delete product
- ? **UI Components**:
  - Homepage with product list
  - Product detail page
  - Create/Edit product form
  - Delete functionality with confirmation
  - Navigation menu

### Bonus Features Implemented
- ? **Search/Filter products** - Search by name or description
- ? **Responsive design** - Works on mobile and desktop
- ? **Image preview** - Preview images when adding URLs
- ? **Pagination support** - Ready for implementation
- ? **Modern UI** - Bootstrap 5 with custom styling
- ? **Docker support** - Full containerization with Docker Compose
- ? **Render deployment** - Production-ready cloud deployment

## ??? Tech Stack

- **Frontend**: Blazor Server (.NET 8)
- **Backend**: ASP.NET Core Web API (.NET 8)
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **UI Framework**: Bootstrap 5
- **Icons**: Bootstrap Icons
- **Containerization**: Docker & Docker Compose
- **Deployment**: Render (Docker + PostgreSQL)

## ?? Prerequisites

- .NET 8 SDK
- PostgreSQL (local or cloud instance)
- Visual Studio 2022 or VS Code
- **OR** Docker Desktop (for containerized setup)

## ?? Setup Instructions

### Option 1: Docker Setup (Recommended)

#### Quick Start with Docker
```bash
# Clone the repository
git clone <your-repository-url>
cd ClothingEShop

# Copy environment file
cp .env.docker .env

# Build and run with Docker Compose
docker-compose up --build
```

**Access the application:**
- Application: http://localhost:8080
- API: http://localhost:8080/api/products
- pgAdmin (database management): http://localhost:8081

For detailed Docker instructions, see [DOCKER.md](DOCKER.md)

### Option 2: Local Development Setup

#### 1. Clone the Repository
```bash
git clone <your-repository-url>
cd ClothingEShop
```

#### 2. Database Setup

##### Option A: Local PostgreSQL
1. Install PostgreSQL locally
2. Create a database named `clothingeshop`
3. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=clothingeshop;Username=postgres;Password=your_password;Port=5432;"
  }
}
```

##### Option B: Cloud PostgreSQL (Recommended for deployment)
Use services like:
- **Neon** (Free tier): https://neon.tech/
- **Supabase** (Free tier): https://supabase.com/
- **Railway** (Free tier): https://railway.app/
- **ElephantSQL** (Free tier): https://www.elephantsql.com/

Update the connection string with your cloud database URL.

#### 3. Environment Configuration
1. Copy `.env.example` to `.env`
2. Update the values in `.env` file:
```env
DB_HOST=your_host
DB_NAME=clothingeshop
DB_USER=your_username
DB_PASSWORD=your_password
DB_PORT=5432
```

#### 4. Install Dependencies
```bash
dotnet restore
```

#### 5. Run the Application
```bash
dotnet run
```

The application will be available at:
- HTTPS: `https://localhost:7xxx`
- HTTP: `http://localhost:5xxx`

## ?? Docker Commands

```bash
# Start all services
docker-compose up

# Start in background
docker-compose up -d

# Rebuild and start
docker-compose up --build

# Stop services
docker-compose down

# Development mode with hot reload
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up

# View logs
docker-compose logs -f clothingeshop-app
```

## ?? API Endpoints

### Products API
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create new product |
| PUT | `/api/products/{id}` | Update product |
| DELETE | `/api/products/{id}` | Delete product |
| GET | `/api/products?search={term}` | Search products |

### Example API Usage

#### Create Product
```bash
curl -X POST http://localhost:8080/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Blue Jeans",
    "description": "Classic blue denim jeans",
    "price": 59.99,
    "imageUrl": "https://example.com/image.jpg"
  }'
```

## ?? Deployment

### Deploy to Render (Recommended)

This project is configured for easy deployment to Render with Docker and PostgreSQL.

**Quick Deploy Steps:**
1. Push your code to GitHub
2. Create a Render account at https://render.com
3. Create a new Blueprint from your repository
4. Render will automatically detect `render.yaml` and deploy both:
   - PostgreSQL Database
   - Blazor Web Application

**?? For detailed deployment instructions, see [RENDER_DEPLOYMENT.md](RENDER_DEPLOYMENT.md)**

### Other Hosting Options

#### 1. Railway (Full-stack deployment)
1. Connect your GitHub repository to Railway
2. Add PostgreSQL addon
3. Set environment variables
4. Deploy automatically on push

#### 2. Docker-based hosting
- **DigitalOcean App Platform**
- **Google Cloud Run**
- **AWS ECS**
- **Azure Container Instances**

### Environment Variables for Production
```env
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=your_production_connection_string
ASPNETCORE_URLS=http://+:10000
```

## ?? Project Structure

```
ClothingEShop/
??? Components/
?   ??? Layout/
?   ?   ??? MainLayout.razor
?   ?   ??? NavMenu.razor
?   ??? Pages/
?       ??? Home.razor
?       ??? Products.razor
?       ??? ProductDetail.razor
?       ??? ProductForm.razor
??? Controllers/
?   ??? ProductsController.cs
??? Data/
?   ??? ClothingEShopDbContext.cs
??? Models/
?   ??? Product.cs
??? Services/
?   ??? IProductService.cs
?   ??? ProductService.cs
??? init-scripts/
?   ??? 01-init.sh
??? docker-compose.yml
??? Dockerfile
??? render.yaml
??? Program.cs
??? appsettings.json
```

## ?? Assignment Deliverables

- ? **GitHub Repository**: [Your Repository URL]
- ? **Deployed Website**: [Your Deployment URL - e.g., https://clothingeshop-app.onrender.com]
- ? **Database**: PostgreSQL with online accessibility
- ? **API Documentation**: Available in this README
- ? **Security**: Environment variables not committed to Git
- ? **Containerization**: Docker support for easy deployment
- ? **Production Deployment**: Render configuration included

## ?? Testing the Application

### Manual Testing Checklist
- [ ] Create a new product
- [ ] View product details
- [ ] Edit existing product
- [ ] Delete a product
- [ ] Search for products
- [ ] Test API endpoints directly
- [ ] Verify responsive design on mobile

### Sample Products for Testing
Use these for initial testing:
1. **Classic White T-Shirt** - $19.99
2. **Blue Denim Jeans** - $59.99  
3. **Black Hoodie** - $39.99

## ?? Troubleshooting

### Common Issues

1. **Database Connection Failed**
   - Verify PostgreSQL is running
   - Check connection string format
   - Ensure database exists

2. **Build Errors**
   - Run `dotnet clean` then `dotnet restore`
   - Check .NET 8 SDK is installed

3. **API Not Working**
   - Check if controllers are properly registered
   - Verify route configurations

4. **Docker Issues**
   - Check if Docker Desktop is running
   - Verify port availability (8080, 5432, 8081)
   - See [DOCKER.md](DOCKER.md) for detailed troubleshooting

5. **Render Deployment Issues**
   - See [RENDER_DEPLOYMENT.md](RENDER_DEPLOYMENT.md) for comprehensive troubleshooting

## ?? Assignment Notes

### Security Implementation
- ? Database credentials in `.env` file (not committed)
- ? `.gitignore` properly configured
- ? Input validation on all forms
- ? SQL injection protection via EF Core
- ? Docker secrets support ready
- ? Production environment variables on Render

### UX Considerations
- ? Responsive design for mobile users
- ? Loading states and error handling
- ? Confirmation dialogs for destructive actions
- ? Breadcrumb navigation
- ? Search functionality for better product discovery

### DevOps Features
- ? Complete Docker containerization
- ? Development and production Docker configurations
- ? Database initialization scripts
- ? Comprehensive documentation
- ? Render deployment configuration
- ? Continuous deployment from GitHub

## ?? Documentation

- **[DOCKER.md](DOCKER.md)** - Complete Docker setup and usage guide
- **[RENDER_DEPLOYMENT.md](RENDER_DEPLOYMENT.md)** - Detailed Render deployment guide

## ????? Developer

**Student ID**: QE180214
**Assignment**: CRUD REST API + UI (Assignment 1)

---

For questions or issues, please check the troubleshooting section or create an issue in the repository.