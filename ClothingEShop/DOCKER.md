# Docker Setup Guide

This guide explains how to run the Clothing E-Shop application using Docker and Docker Compose.

## ?? Prerequisites

- Docker Desktop (Windows/Mac) or Docker Engine (Linux)
- Docker Compose (usually included with Docker Desktop)

## ?? Quick Start

### 1. Environment Setup
Copy the Docker environment file:
```bash
cp .env.docker .env
```

Edit `.env` file with your preferred settings:
```env
DB_NAME=clothingeshop
DB_USER=postgres
DB_PASSWORD=postgres123
DB_PORT=5432
APP_PORT=8080
```

### 2. Build and Run
```bash
# Build and start all services
docker-compose up --build

# Or run in background
docker-compose up -d --build
```

### 3. Access the Application
- **Application**: http://localhost:8080
- **API**: http://localhost:8080/api/products
- **pgAdmin** (optional): http://localhost:8081

## ??? Development Mode

For development with hot reload:

```bash
# Start in development mode
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up --build

# Or just the database for local development
docker-compose up postgres pgadmin
```

## ?? Services Included

### Main Services
- **clothingeshop-app**: The main Blazor application
- **postgres**: PostgreSQL database
- **pgadmin**: Database management interface (optional)

### Service URLs
| Service | URL | Credentials |
|---------|-----|-------------|
| Application | http://localhost:8080 | - |
| API | http://localhost:8080/api/products | - |
| pgAdmin | http://localhost:8081 | admin@clothingeshop.com / admin123 |

## ?? Useful Commands

### Basic Operations
```bash
# Start services
docker-compose up

# Start in background
docker-compose up -d

# Stop services
docker-compose down

# Rebuild and start
docker-compose up --build

# View logs
docker-compose logs -f clothingeshop-app

# View all logs
docker-compose logs -f
```

### Database Operations
```bash
# Access PostgreSQL directly
docker-compose exec postgres psql -U postgres -d clothingeshop

# Backup database
docker-compose exec postgres pg_dump -U postgres clothingeshop > backup.sql

# Restore database
docker-compose exec -T postgres psql -U postgres -d clothingeshop < backup.sql
```

### Development Commands
```bash
# Rebuild only the app
docker-compose build clothingeshop-app

# Run app without cache
docker-compose build --no-cache clothingeshop-app

# Access app container
docker-compose exec clothingeshop-app /bin/bash
```

## ??? Volume Management

### Data Persistence
- `postgres_data`: Database files
- `pgadmin_data`: pgAdmin configuration
- `./logs`: Application logs

### Backup Volumes
```bash
# Backup PostgreSQL data
docker run --rm -v clothingeshop_postgres_data:/data -v $(pwd):/backup alpine tar czf /backup/postgres-backup.tar.gz /data

# Restore PostgreSQL data
docker run --rm -v clothingeshop_postgres_data:/data -v $(pwd):/backup alpine tar xzf /backup/postgres-backup.tar.gz -C /
```

## ?? Production Deployment

### Docker Production Setup
```bash
# Set production environment
export ASPNETCORE_ENVIRONMENT=Production

# Use production compose file
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

### Environment Variables for Production
```env
ASPNETCORE_ENVIRONMENT=Production
DB_PASSWORD=your_secure_password
PGADMIN_PASSWORD=your_secure_admin_password
```

## ?? Troubleshooting

### Common Issues

1. **Port conflicts**
   ```bash
   # Change ports in .env file
   APP_PORT=8090
   DB_PORT=5433
   PGADMIN_PORT=8082
   ```

2. **Database connection issues**
   ```bash
   # Check if PostgreSQL is healthy
   docker-compose ps
   docker-compose logs postgres
   ```

3. **Application won't start**
   ```bash
   # Check application logs
   docker-compose logs clothingeshop-app
   
   # Rebuild without cache
   docker-compose build --no-cache clothingeshop-app
   ```

4. **Permission issues on Linux**
   ```bash
   # Fix permissions for init scripts
   chmod +x init-scripts/*.sh
   ```

### Reset Everything
```bash
# Stop and remove all containers, networks, and volumes
docker-compose down -v --remove-orphans

# Remove all images
docker-compose down --rmi all

# Rebuild from scratch
docker-compose up --build
```

## ?? Development Workflow

### 1. Local Development
```bash
# Start only database
docker-compose up postgres

# Run app locally
dotnet run
```

### 2. Full Docker Development
```bash
# Development with hot reload
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up
```

### 3. Testing
```bash
# Run tests in container
docker-compose exec clothingeshop-app dotnet test
```

## ?? Security Notes

- Change default passwords in production
- Use environment variables for sensitive data
- Don't commit `.env` files to version control
- Use Docker secrets for production deployments

## ?? Monitoring

### Health Checks
```bash
# Check service health
docker-compose ps

# Detailed health status
docker inspect clothingeshop-postgres | grep Health -A 10
```

### Logs
```bash
# Real-time logs
docker-compose logs -f

# Specific service logs
docker-compose logs -f clothingeshop-app
docker-compose logs -f postgres
```

---

For additional help, check the main README.md or create an issue in the repository.